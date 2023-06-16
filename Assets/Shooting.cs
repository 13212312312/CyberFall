using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] public GameObject Bullet;
    [SerializeField] public Transform gunTip;
    [SerializeField] public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Shoot()
    {
        var dir = (gunTip.position - transform.position).normalized;
    }
}
