using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    List<EnemyMovement> list = new List<EnemyMovement>();
    GameObject Player;
    MapManager mapManager;
    int[] old = new int[30];
    int[] current = new int[30];
    public int number = 3;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        mapManager = GameObject.FindObjectOfType<MapManager>();
        for (int i = 0; i < 30; i++)
        {
            current[i] = 0;
        }
    }

    public void Add(EnemyMovement enemy)
    {
        list.Add(enemy);
        current[enemy.GetEnemyType()]++;
    }
    public bool Remove(EnemyMovement enemy,int type)
    {
        list.Remove(enemy);
        current[type]--;
        Destroy(enemy.transform.parent.gameObject);
        Debug.Log(CreateMessage().SaveToString());
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        //if (working) return;
        if(compareOldWithNew())
        {
            Start_Thinking();
        }
        else
        {
            replaceOld();
        }
    }
    bool compareOldWithNew()
    {
        for(int i = 0; i < 30; i++)
        {
            if(old[i] != current[i])
            {
                return false;
            }
        }
        return true;
    }
    void replaceOld()
    {
        for(int i = 0; i < 30; i++)
        {
            old[i] = current[i];
        }
    }
    void Start_Thinking()
    {
        //working = true;
        int count = 0;
        for(int i = 0; i < 30; i++)
        {
            count += current[i];
        }
    }

    Message CreateMessage()
    {
        return new Message(mapManager.GetCurrentLevel().LayoutType,
            Player.GetComponent<HealthManager>().currentHealth,
            Player.transform.position.x,
            Player.transform.position.y,
            list);
    }
}
