using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class LegMover : MonoBehaviour
{
    [SerializeField] public float delay = 0.1f;
    public Transform limbSolverTarget;
    public Transform mainTarget;
    public float moveDistance;
    public LayerMask groundLayer;
    public float groundDistance;
    public bool up;
    private float cooldown;
    void Start()
    {
        
    }
    void Update()
    {
        CheckGround();
        if(cooldown >= 0)
        {
            limbSolverTarget.position = transform.position;
            cooldown -= Time.deltaTime;
        }
        if(Vector2.Distance(limbSolverTarget.position,transform.position) > moveDistance)
        {
            cooldown = Random.Range(0f,delay);   
        }
    }

    public void CheckGround(){
        Vector3 dir = Vector3.down;
        if(up) dir = Vector3.up;
        Vector3 startpoint = new Vector3(transform.position.x, mainTarget.position.y, transform.position.z);
        RaycastHit2D hit = Physics2D.Raycast(startpoint, dir, groundDistance, groundLayer);
        if(hit.collider != null)
        {
            Vector3 point = hit.point;
            transform.position = point;
        }
        else
        {
            transform.position = startpoint + Random.Range(0.8f,1f) * groundDistance * dir;
        }
    }
}