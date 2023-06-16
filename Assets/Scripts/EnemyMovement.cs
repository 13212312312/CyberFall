using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Astar;

public class EnemyMovement : MonoBehaviour
{
    #region COMPONENTS
    private GameObject SpawnPoint;
    private MapManager mapManager;
    private GameObject Player;
    private static int EnemyCount;
    [SerializeField] private int id;
    List<int> colWithID;
    #endregion

    #region PARAMETERS
    private Vector2Int topLeft;
    private int width;
    private int height;
    Tilemap tilemap;
    [SerializeField] float speed;
    [SerializeField] float cooldown;
    float currentCooldown;
    List<Vector3Int> path;
    [SerializeField] bool canMove = true;
    [SerializeField] bool walking;
    [SerializeField] public float freezeDelay = 2f;
    private float moveCooldown = 0;
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
        SpawnPoint = GameObject.FindWithTag("EnemySpawnPoint");
    }
    void Start()
    {
        colWithID = new List<int>();
        moveCooldown = freezeDelay;
        EnemyCount+=1;
        id = EnemyCount;
        mapManager = FindObjectOfType<MapManager>();
        index = 0;
        currentCooldown = 0;
    }
    void OnEnable() {
        moveCooldown = freezeDelay;
        transform.position = SpawnPoint.transform.position;
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
        if(path.Count <= 1) return;
        Vector3 t1,t2;
        t1 = new Vector3(0,0);
        int last = -1;
        foreach(var point in path)
        {
            t2 = tilemap.GetCellCenterWorld(point);
            if (last == -1)
            {
                last = 1;
            }
            else
            {
                Debug.DrawLine (t1, t2, Color.green, 1);
            }
            t1 = t2;
        }
    }

    void GetPath()
    {
        GetMap();
        path = Program.Solve(tilemap,transform.position,Player.transform.position, walkableGround, walking);
    }

    void Update()
    {
        if(moveCooldown > 0)
        {
            moveCooldown -= Time.deltaTime;
            return;
        }
        if(!canMove)
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
        Move();
    }

    void GetNextPos()
    {
        if (index < path.Count - 1) 
        {
            index+=1;
        }
        else
        {
            currentCooldown = 0;
        }
        target = tilemap.GetCellCenterWorld(path[index]);
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

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            var otherID = col.gameObject.GetComponent<EnemyMovement>().GetID() ;
            if(otherID > id)
            {
                canMove = false;
                if(!colWithID.Contains(otherID))
                {
                    colWithID.Add(otherID);
                }
            }
        }
    }    

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            var otherID = col.gameObject.GetComponent<EnemyMovement>().GetID() ;
            if(otherID > id)
            {
                colWithID.Remove(otherID);
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
