using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyPosition
{
    public float pozx, pozy;
    public EnemyPosition(float x,float y)
    {
        pozx = x;
        pozy = y;
    }
}
[Serializable]
public class EnemyData
{
    public EnemyPosition poz;
    public int type;
    public EnemyData(float x,float y,int tp)
    {
        poz = new EnemyPosition(x, y);
        type = tp;
    }
}
[Serializable]
public class Message
{
    public float player_health;
    public float player_x;
    public float player_y;
    public int mapType;
    int maxEnemies = 7;
    public EnemyData[] enemies;

    public Message(int layoutType, float health, float pozx, float pozy, List<EnemyMovement> enemyList)
    {
        enemies = new EnemyData[maxEnemies];
        player_health = health;
        player_x = pozx;
        player_y = pozy;
        mapType = layoutType;

        int count = Math.Min(maxEnemies, enemyList.Count);
        for(int i = 0; i < count; i++)
        {
            GameObject gameObject = enemyList[i].gameObject;
            var enemyMovementScript = gameObject.GetComponent<EnemyMovement>();
            enemies[i] = new EnemyData(gameObject.transform.position.x, gameObject.transform.position.y, enemyMovementScript.GetEnemyType());
        }
        for(int i = count; i < maxEnemies; i++)
        {
            enemies[i] = new EnemyData(0.0f, 0.0f, 0);
        }
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
[Serializable]
public class Response
{
    public EnemyPosition[] enemies;
}

