using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 direction;
    private bool canMove = false;
    private float Damage;  
    private float Speed;
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private Vector2 _colliderCheckSize = new Vector2(0.25f, 0.25f);
    void Start()
    {

    }
    public void InitializeBullet(Vector3 dir,float damage,float speed) 
    {
        direction = dir;
        Damage = damage;
        Speed = speed;
        
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.position = transform.position + direction * Speed * Time.deltaTime;
        if(Physics2D.OverlapBox(transform.position, _colliderCheckSize, 0, _groundLayer))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<HealthManager>().TakeDamage(Damage);
            Destroy(this.gameObject);
        }
    }
}
