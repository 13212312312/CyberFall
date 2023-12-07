using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    List<EnemyMovement> list = new List<EnemyMovement>();
    int[] old = new int[30];
    int[] current = new int[30];
    public int number = 3;
    void Start()
    {
        for(int i = 0; i < 30; i++)
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
        return true;
    }
    // Update is called once per frame
    void Update()
    {
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
        int count = 0;
        for(int i = 0; i < 30; i++)
        {
            count += current[i];
        }
        Debug.Log(count);
    }
}
