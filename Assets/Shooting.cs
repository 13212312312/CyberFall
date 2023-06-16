using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] public GameObject Bullet;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletDamage;
    [SerializeField] public float attackSpeed = 1f;
    [SerializeField] public int numberOfBullets;
    [SerializeField] public float spreadDistance;
    private Camera cam;
    private float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0)
        {
            cooldown = cooldown - Time.deltaTime;
            return;
        }
        if(Input.GetAxis ("Fire1") > 0)
        {
            Shoot();
        } 
    }
    void Shoot()
    {
        Vector3 mousePos = Input.mousePosition;
        var aim = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane)) - transform.position;
        for(int i = 0; i < numberOfBullets; i++)
        {
            var dir = (Random.insideUnitSphere * spreadDistance + aim).normalized;
            var InstantiatedBulled = Instantiate(Bullet, transform.position, Quaternion.identity);
            InstantiatedBulled.GetComponent<BulletScript>().InitializeBullet((new Vector3(dir.x, dir.y, transform.position.z)).normalized,bulletDamage,bulletSpeed);
        }
        cooldown = attackSpeed;
    }
}
