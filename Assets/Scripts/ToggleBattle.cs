using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBattle : MonoBehaviour
{
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
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player")
        {
            mapManager.GetCurrentLevel().EnterBattle();
        }
    }
}
