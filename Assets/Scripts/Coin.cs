using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    private float delay = 0;
    private float pasttime = 0;
    private float when = 1f;
    private Vector3 off;
    private SpriteRenderer sr;
    private CircleCollider2D cc2d;
    private CoinPicker script;
    private GameObject player;
    private bool magnetState = false;
    [SerializeField] float range = 1f;
    [SerializeField] float speed = 4f;

    private void Awake() 
    {
        off = new Vector3(Random.Range(-range,range), off.y, off.z);
        off = new Vector3(off.x, Random.Range(-range,range), off.z);
        sr = GetComponent<SpriteRenderer>();
        cc2d = GetComponent<CircleCollider2D>();
        player = GameObject.FindWithTag("Player");
        sr.enabled = false;
        cc2d.enabled = false;
    }
    public void TriggerMagnet(CoinPicker script)
    {
        this.script = script;
        magnetState = true;
    }
    public void Magnet()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if(Vector2.Distance(player.transform.position,transform.position) < 0.2f)
        {
            script.AddCoins(1);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (magnetState)
        {
            Magnet();
            return;
        }
        if (when >= delay)
        {
            pasttime = Time.deltaTime;
            transform.position += off * Time.deltaTime;
            delay += pasttime;
        }
        else
        {
            sr.enabled = true;
            cc2d.enabled = true;
        }  
    }
}
