using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Astar;

public class EnemyMovement : MonoBehaviour
{
    #region COMPONENTS
    private GameObject[] SpawnPoints;
    private MapManager mapManager;
    private GameObject Player;
    private Brain brain;
    private static int EnemyCount;
    [SerializeField] private int id;
    List<GameObject> colWithID;
    #endregion

    #region PARAMETERS
    private Vector2Int topLeft;
    private int width;
    private int height;
    Tilemap tilemap;
    [SerializeField] float CollisionDamage;
    [SerializeField] float speed;
    [SerializeField] float cooldown;
    float currentCooldown;
    public List<Vector3Int> path;
    public List<Vector3> pathBezier;
    List<int> pascal;
    int bezierPoints = 8;
    [SerializeField] bool canMove = true;
    [SerializeField] bool walking;
    [SerializeField] public float freezeDelay = 2f;
    [SerializeField] bool shooter = false;
    [SerializeField] public float shootingRange = 3f;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public float shootingCooldown = 4f;
    [SerializeField] public GameObject enemyBullet;
    [SerializeField] public float bulletDamage;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public bool bomber;
    [SerializeField] public float explosionRadiusTrigger;
    [SerializeField] public float explosionRadiusRange;
    [SerializeField] public float explosionDamage;
    [SerializeField] public float explosionDetonationDuration;
    private float currentExplosionTime;
    private bool exploding;
    private int Type = 0;
    
    private bool stopToShoot = false;
    private Camera cam;
    private float moveCooldown = 0;
    private float currentShootingCooldown;
    int index;
    bool IsFacingRight = true;   
    Vector3 target;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
	[SerializeField] private string walkableGround;
	#endregion

    void Awake() {
        Player = GameObject.FindWithTag("Player");
        SpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        brain = GameObject.FindObjectOfType<Brain>();
        cam = FindObjectOfType<Camera>();
        Type = 3;
        if(bomber)
            Type = 0;
        if(shooter)
            Type = 1;
        if(walking) 
            Type = 2;
        
    }
    void Start()
    {
        colWithID = new List<GameObject>();
        moveCooldown = freezeDelay;
        brain.Add(this);
        EnemyCount+=1;
        id = EnemyCount;
        mapManager = FindObjectOfType<MapManager>();
        gameObject.GetComponent<HealthManager>().currentInvulnerability = freezeDelay;
        index = 0;
        currentCooldown = 0;
        pascal = getPascalRow(bezierPoints);
    }
    public static List<int> getPascalRow(int rowIndex)
    {   
        List<int> currow = new List<int>();
        currow.Add(1);
        if (rowIndex == 0)
        {
            return currow;
        }
        List<int> prev = getPascalRow(rowIndex - 1);

        for(int i = 1; i < prev.Count; i++)
        {
            int curr = prev[i - 1] + prev[i];
            currow.Add(curr);
        }
        currow.Add(1);
        return currow;
    }
    void OnEnable() {
        moveCooldown = freezeDelay;
        transform.position = SpawnPoints[Random.Range(0,SpawnPoints.Length)].transform.position;
    }

    void GetMap()
    {
        tilemap = mapManager.GetCurrentMap();
        BoundsInt bounds = tilemap.cellBounds;
        height = bounds.size.y;
        width = bounds.size.x;
    }

    void DrawPath()
    {
        if(pathBezier.Count <= 1) return;
        Vector3 t1,t2;
        t1 = new Vector3(0,0);
        int last = -1;
        foreach(var point in pathBezier)
        {
            t2 = point;
            if (last == -1)
            {
                last = 1;
            }
            else
            {
                Debug.DrawLine (t1, t2, Color.green, cooldown);
            }
            t1 = t2;
        }
    }

    void GetPath()
    {
        GetMap();
        path = Program.Solve(tilemap,transform.position,Player.transform.position, walkableGround, walking);
        Bezier();
    }

    void Bezier()
    {
        pathBezier = new List<Vector3>();
        List<Vector3> points = new List<Vector3>();
        foreach(var point in path)
        {
            points.Add(tilemap.GetCellCenterWorld(point));
        }
        int i;
        for(i = 0; i < points.Count - bezierPoints; i += bezierPoints) {
            addBezierPoints(points, i, bezierPoints, pascal);
        }
        int localBezierPoints = points.Count - i - 1;
        List<int> localPascal = getPascalRow(localBezierPoints);
        
        addBezierPoints(points, i, localBezierPoints,localPascal);
    }
    private void addBezierPoints(List<Vector3> points, int index, int localBezierPoints, List<int> localPascal)
    {
        for(float u = 0; u < 1; u += 0.1f) {
            Vector3 point = new Vector3(0,0,0);
            for(int j = 0; j <= localBezierPoints; j++) {
                point += localPascal[j] * points[index + j] * Mathf.Pow(u, j) * Mathf.Pow(1.0f - u, localBezierPoints - j);
            }
            pathBezier.Add(point);
        }
    }
    private bool CanSeePlayer()
    {
        var dir = (Player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, shootingRange, playerLayer);
        if(hit.collider != null)
        {
            return true;
        }
        return false;
    }
    void Shoot()
    {
        if(currentShootingCooldown > 0) return;
        
        var dir = (Player.transform.position - transform.position).normalized;
        var InstantiatedBullet = Instantiate(enemyBullet, transform.position, Quaternion.identity);
        var script = InstantiatedBullet.GetComponent<EnemyBulletScript>();
        script.InitializeBullet((new Vector3(dir.x, dir.y, transform.position.z)).normalized,bulletDamage,bulletSpeed);
        
        currentShootingCooldown = shootingCooldown;
    }
    void Update()
    {
        if(moveCooldown > 0)
        {
            moveCooldown -= Time.deltaTime;
            return;
        }
        if(shooter)
        {
            if(currentShootingCooldown > 0)
            {
                currentShootingCooldown -= Time.deltaTime;
            }
            if(Vector2.Distance(Player.transform.position,transform.position) < shootingRange && CanSeePlayer())
            {
                stopToShoot = true;
                Shoot();
            }
            else
            {
                stopToShoot = false;
            }
        }
        if(bomber)
        {
            if(Vector2.Distance(Player.transform.position,transform.position) < explosionRadiusTrigger)
            {
                exploding = true;
            }
        }
        if(exploding)
        {
            currentExplosionTime += Time.deltaTime;
            if(currentExplosionTime >= explosionDetonationDuration)
            {
                var scrpt = GetComponent<HealthManager>();
                if (Vector2.Distance(Player.transform.position,transform.position) < explosionRadiusRange)
                {
                    Player.GetComponent<HealthManager>().TakeDamage(explosionDamage);
                }
                scrpt.TakeDamage(9999);
                return;
            }
        }
        colWithID.RemoveAll(item => item == null);
        if(colWithID.Count == 0)
        {
            canMove = true;
        }
        if(!canMove || exploding || stopToShoot)
        {
            return;
        }
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
        {
            currentCooldown = cooldown;
            GetPath();
            DrawPath();
            index = 0;
            GetNextPos();
        }
        if(pathBezier.Count > 0)
        {
            Move();
        }
    }

    void GetNextPos()
    {
        if (index < pathBezier.Count - 1) 
        {
            index += 1;
        }
        else
        {
            currentCooldown = 0;
        }
        if(pathBezier.Count > 0)
        {
            target = pathBezier[index];
        }
    }

	private void TurnSide()
	{
		Vector3 scale = transform.localScale; 
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}

    void Move()
    {
        if(target.x < transform.position.x && IsFacingRight || 
           target.x > transform.position.x && !IsFacingRight)
        {
            TurnSide();
        } 
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(transform.position == target)
        {
            GetNextPos();
        }
    }
    public int GetEnemyType()
    {
        return Type;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (this.gameObject == null || col == null)
            return;
        if(col.gameObject.CompareTag("Enemy"))
        {
            var other = col.gameObject.GetComponent<EnemyMovement>();
            if (other == null) return;
            if(other.GetID() > id && other.GetEnemyType() <= Type)
            {
                canMove = false;
                if(!colWithID.Contains(other.gameObject))
                {
                    colWithID.Add(other.gameObject);
                }
            }
        }
        if(col.gameObject.CompareTag("Player") && ! shooter && ! bomber && canMove)
        {
            col.gameObject.GetComponent<HealthManager>().TakeDamage(CollisionDamage);
        }
    }    

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            var other = col.gameObject.GetComponent<EnemyMovement>();
            if(other.GetID() > id && other.GetEnemyType() <= Type)
            {
                colWithID.Remove(other.gameObject);
                if(colWithID.Count == 0)
                {
                    canMove = true;
                }
            }
        }
    }

    void SetMove(bool status)
    {
        canMove = status;
    }

    public int GetID()
    {
        return id;
    }

    public void ResetCooldown()
    {
        currentCooldown = 0;
    }

}
