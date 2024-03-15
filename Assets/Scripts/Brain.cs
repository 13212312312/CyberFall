using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Brain : MonoBehaviour
{
    List<EnemyMovement> list = new List<EnemyMovement>();
    GameObject Player;
    MapManager mapManager;
    private TcpClient socketConnection;
    float cooldown = 0.5f;
    float currentcooldown = 0;
    Byte[] bytes;
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
        currentcooldown = Math.Max(0, currentcooldown - Time.deltaTime);
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
        if (count == 0) return;
        if (currentcooldown != 0)
        {
            return;
        }
        currentcooldown = cooldown;
        var message = CreateMessage();
        var message2 = CreateResponse();
        foreach(var data in message2.enemies)
        {
            if (data.pozx != 0 && data.pozy != 0)
            {
                Debug.DrawLine(new Vector2(data.pozx - 0.2f, data.pozy - 0.2f), new Vector2(data.pozx + 0.2f, data.pozy + 0.2f), Color.red, 0.3f);
                Debug.DrawLine(new Vector2(data.pozx + 0.2f, data.pozy - 0.2f), new Vector2(data.pozx - 0.2f, data.pozy + 0.2f), Color.red, 0.3f);
            }
            //Debug.DrawLine(new Vector2(data.pozx + 1, data.pozy - 1), new Vector2(data.pozx + 1, data.pozy - 1), Color.red, 1);
        }
        var thrd = new Thread(() => { sendMessage(message, message2); });
        thrd.Start();
    }
    void sendMessage(Message m, Response r)
    {
        try
        {
            socketConnection = new TcpClient("localhost", 8053);
            bytes = new Byte[4096];
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
            return;
        }

        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string msg = "{ \"message\":" + m.SaveToString() + ",\"response\":" + r.SaveToString() + "}";
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(msg);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
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
    Response CreateResponse()
    {
        List<Vector2Int> poz = new List<Vector2Int>();
        foreach (var enemy in list)
        {
            Vector2Int pozEnemy;
            if(enemy.path == null || enemy.path.Count < 1)
            {
                pozEnemy = new Vector2Int((int)(enemy.gameObject.transform.position.x), (int)(enemy.gameObject.transform.position.x));
            }
            else
            {
                Vector3 target = enemy.path[UnityEngine.Random.Range(1, enemy.path.Count - 1)];
                pozEnemy = new Vector2Int((int)(target.x), (int)(target.y));
            }
            poz.Add(pozEnemy);
        }
        return new Response(poz);
    }
}
