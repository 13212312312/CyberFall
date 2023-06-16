using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] int from;
    MapManager mapManager;
    
    void Awake() {
        mapManager = FindObjectOfType<MapManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            mapManager.EnteredFrom(from);
        }
    }
}
