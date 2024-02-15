using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class EnemyMessageData
{
    public static int maxEnemies = 7;
}
[Serializable]
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
    public EnemyData[] enemies;

    public Message(int layoutType, float health, float pozx, float pozy, List<EnemyMovement> enemyList)
    {
        enemies = new EnemyData[EnemyMessageData.maxEnemies];
        player_health = health;
        player_x = pozx;
        player_y = pozy;
        mapType = layoutType;

        int count = Math.Min(EnemyMessageData.maxEnemies, enemyList.Count);
        for(int i = 0; i < count; i++)
        {
            GameObject gameObject = enemyList[i].gameObject;
            var enemyMovementScript = gameObject.GetComponent<EnemyMovement>();
            enemies[i] = new EnemyData(gameObject.transform.position.x, gameObject.transform.position.y, enemyMovementScript.GetEnemyType());
        }
        for(int i = count; i < EnemyMessageData.maxEnemies; i++)
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
    public Response()
    {
        enemies = new EnemyPosition[EnemyMessageData.maxEnemies];
    }
    public Response(List<Vector2Int> positions)
    {
        enemies = new EnemyPosition[EnemyMessageData.maxEnemies];

        int count = Math.Min(EnemyMessageData.maxEnemies, positions.Count);
        for (int i = 0; i < count; i++)
        {
            enemies[i] = new EnemyPosition(positions[i].x, positions[i].y);
        }
        for (int i = count; i < EnemyMessageData.maxEnemies; i++)
        {
            enemies[i] = new EnemyPosition(0.0f, 0.0f);
        }

    }
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}

