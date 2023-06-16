using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject Player;
    [SerializeField] public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + offset;
    }

    public void Flip()
    {
        offset.x = -offset.x;
    }
}
