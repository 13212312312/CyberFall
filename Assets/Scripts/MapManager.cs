using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;
using UnityEngine;
using MapGenerator;
public class Level{
    public Tilemap Layout { get; set; }
    public Tilemap CompletedLayout { get; set; }
    public GameObject Enemies { get; set; }
    public bool Completed;
    public bool Discovered { get; set; }
    public int LayoutType{ get; set; }
    private int imageType;
    private int imageRotation;
    private bool enteredBattle;
    private EnemyHolder enemyHolderScript;
    // de facut kamikaze
    public Level(Tilemap layout, Tilemap completedLayout, int layoutType, GameObject enemies)
    {
        enteredBattle = false;
        Completed = true;
        Layout = layout;
        Discovered = false;
        CompletedLayout = completedLayout;
        LayoutType = layoutType;
        Enemies = enemies;
        Enemies.SetActive(false);
        enemyHolderScript = Enemies.GetComponent<EnemyHolder>();
    }
    public void Update()
    {
        if(Completed) return;
        Debug.Log(enemyHolderScript.GetEnemies());
        if(enemyHolderScript.GetEnemies() == 0)
        {
            Completed = true;
            Enemies.SetActive(false);
            CompletedLayout.gameObject.SetActive(true);
            Layout.gameObject.SetActive(false);
        }
    }

    public void EnterBattle()
    {
        if(enteredBattle) return;
        enteredBattle = true;
        Completed = false;
        CompletedLayout.gameObject.SetActive(false);
        Layout.gameObject.SetActive(true);
        Enemies.SetActive(true);
    }

    public Tilemap Getmap()
    {
        if(Completed)
        {
            return CompletedLayout;
        }
        return Layout;
    }
    public void Discover()
    {
        Discovered = true;
    }
    public void SetMapImage(bool[] NeighBour)
    {
        int counter = 0;
        foreach(var nb in NeighBour)
        {
            if(nb) counter++;
        }

        switch(counter)
        {
            case 1:
                imageType = 0;
                for(int i = 0; i < 4; i++)
                {
                    if(NeighBour[i])
                    {
                        imageRotation = 3 - i;
                        break;
                    }
                }
                break;
            case 2:
                if(NeighBour[0] == NeighBour[2] || NeighBour[1] == NeighBour[3])
                {
                    imageType = 2;
                    if(NeighBour[0] == true)
                    {
                        imageRotation = 1;
                    }
                    else
                    {
                        imageRotation = 0;
                    }
                }
                else
                {
                    imageType = 1;
                    imageRotation = 0;
                    for(int i = 1; i < 4; i++)
                    {
                        if(NeighBour[i] && NeighBour[i-1])
                        {
                            imageRotation = 3 - i;
                            break;
                        }
                    }
                }
                break;
            case 3:
                imageType = 3;
                if(!NeighBour[0]) 
                {
                    imageRotation = 4;
                }
                else
                {
                    for(int i = 0; i < 4; i++)
                    {
                        if(!NeighBour[i])
                        {
                            imageRotation = 4 - i;
                            break;
                        }
                    }
                }
                break;
            case 4:
                imageType = 4;
                imageRotation = 0;
                break;
        }
    }
    public int GetMapImage()
    {
        var value = imageType * 10 + imageRotation;
        if(LayoutType == 0) value += 100;
        if(LayoutType == 1) value += 200;
        return value;
    }
}
public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap[] templates;
    Map map;
    private Camera _cam;
    private PlayerMovement _player;
    private EnemyManager enemyManager;
    public int Width;
    public int Height;
    public int StartX;
    public int StartY;
    public int MaxDepth;
    public Vector2Int currentPosition;
    public System.Random random;
    private Level[,] Levels;
    public SceneData SceneData;
    private Color _currentForegroundColor;
    public float teleportCooldown = 0f;
    private float currentTeleportCooldown;
    private MinimapImageManager minimapManager;
    // Start is called before the first frame update
    void Awake(){
        minimapManager = FindObjectOfType<MinimapImageManager>();
        currentPosition = new Vector2Int(StartX,StartY);
        _cam = FindObjectOfType<Camera>();
        enemyManager = FindObjectOfType<EnemyManager>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        
        random = new System.Random();
        map = new Map(Width,Height,StartX,StartY,MaxDepth,templates.Length,random);
        map.GenerateMap();
        Levels = new Level[Height,Width];

        foreach(var template in templates)
        {
            template.gameObject.SetActive(false);
        }
        for(int i = 0; i < Height; i++)
        {
            for(int j = 0; j < Width; j++)
            {
                if(map.Matrix[i,j] != null)
                {
                    Tilemap layout = templates[map.Matrix[i,j].RoomType];
                    layout.color=SceneData.foregroundColor;
                    Tilemap completedLayout = RemoveTiles(layout,map.Matrix[i,j].HasNeighbours);
                    completedLayout.gameObject.SetActive(false);
                    completedLayout.transform.parent = GameObject.FindWithTag("Grid").transform;
                    Levels[i,j] = new Level(layout,completedLayout,map.Matrix[i,j].RoomType, enemyManager.GenerateEnemies());
                    Levels[i,j].SetMapImage(map.Matrix[i,j].HasNeighbours);
                }
            }
        }
        Levels[currentPosition.y,currentPosition.x].Getmap().gameObject.SetActive(true);
        Levels[currentPosition.y,currentPosition.x].Discover();
    }
    public Map GetFullMap()
    {
        return map;
    }
    void Start()
    {
        SetSceneData(SceneData);
        minimapManager.TriggerDiscover(StartY,StartX);
    }   

    public Tilemap RemoveTiles(Tilemap lvl, bool[] Neighbours)
    {
        Tilemap currentMap = Instantiate(lvl, lvl.transform.position, Quaternion.identity);
        var adjustVector =  new Vector3Int(currentMap.size.x/2 + 1, currentMap.size.y/2 - 1, 0);
        if(Neighbours[0]) currentMap = RemoveLeft(currentMap, adjustVector);
        if(Neighbours[1]) currentMap = RemoveDown(currentMap, adjustVector);
        if(Neighbours[2]) currentMap = RemoveRight(currentMap, adjustVector);
        if(Neighbours[3]) currentMap = RemoveUp(currentMap, adjustVector);
        
        return currentMap;
    }
    public Tilemap RemoveLeft(Tilemap currentMap, Vector3Int adjustVector)
    {
        for(int i = 2; i < currentMap.size.x / 2; i++)
        {
            currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2 - 1, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2 + 1, 0) - adjustVector, null);
            if(currentMap.size.y % 2 == 0)
            {
                currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2 + 2, 0) - adjustVector, null);
            }
        }
        return currentMap;
    }
    public Tilemap RemoveRight(Tilemap currentMap, Vector3Int adjustVector)
    {
        for(int i = currentMap.size.x / 2; i < currentMap.size.x; i++)
        {
            currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2 - 1, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2 + 1, 0) - adjustVector, null);
            if(currentMap.size.y % 2 == 0)
            {
                currentMap.SetTile(new Vector3Int(i, currentMap.size.y/2 + 2, 0) - adjustVector, null);
            }
        }
        return currentMap;
    }
    public Tilemap RemoveDown(Tilemap currentMap, Vector3Int adjustVector)
    {
        for(int i = 3; i < currentMap.size.y / 2; i++)
        {
            currentMap.SetTile(new Vector3Int(currentMap.size.x/2 - 1, i, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(currentMap.size.x/2, i, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(currentMap.size.x/2 + 1, i, 0) - adjustVector, null);
            if(currentMap.size.y % 2 == 0)
            {
                currentMap.SetTile(new Vector3Int(currentMap.size.x/2 + 2, i, 0) - adjustVector, null);
            }
        }
        return currentMap;
    }
    public Tilemap RemoveUp(Tilemap currentMap, Vector3Int adjustVector)
    {
        for(int i = currentMap.size.y / 2; i < currentMap.size.y; i++)
        {
            currentMap.SetTile(new Vector3Int(currentMap.size.x/2 - 1, i, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(currentMap.size.x/2, i, 0) - adjustVector, null);
            currentMap.SetTile(new Vector3Int(currentMap.size.x/2 + 1, i, 0) - adjustVector, null);
            if(currentMap.size.y % 2 == 0)
            {
                currentMap.SetTile(new Vector3Int(currentMap.size.x/2 + 2, i, 0) - adjustVector, null);
            }
        }
        return currentMap;
    }

    void Update()
    {
        currentTeleportCooldown -= Time.deltaTime;
        GetCurrentLevel().Update();
    }

    public Level GetCurrentLevel()
    {
        return Levels[currentPosition.y,currentPosition.x];
    }

    public Tilemap GetCurrentMap()
    {
        return GetCurrentLevel().Getmap();
    }

    public void SetSceneData(SceneData data)
    {
        SceneData = data;

        _cam.orthographicSize = data.camSize;
        _cam.backgroundColor = data.backgroundColor;
        GetCurrentMap().color = data.foregroundColor;

        _currentForegroundColor = data.foregroundColor;
    }

    public void EnteredFrom(int room)
    {
        if(currentTeleportCooldown > 0) return;
        currentTeleportCooldown = teleportCooldown;
        int nextRoomEntryPoint = Map.OpposingRoom[room];
        var lastPosition = currentPosition;
        currentPosition += new Vector2Int(Map.directionX[room],Map.directionY[room]);
        _player.transform.position = this.gameObject.transform.GetChild(nextRoomEntryPoint).transform.position;
        Levels[lastPosition.y,lastPosition.x].Getmap().gameObject.SetActive(false);
        Levels[lastPosition.y,lastPosition.x].Enemies.SetActive(false);
        Levels[currentPosition.y,currentPosition.x].Getmap().gameObject.SetActive(true);
        Levels[currentPosition.y,currentPosition.x].Getmap().color = SceneData.foregroundColor;
        Levels[currentPosition.y,currentPosition.x].Discover();
        minimapManager.TriggerDiscover(currentPosition.y,currentPosition.x);
        minimapManager.UpdatePoint();
    }

    public int GetMapImageFromIndex(int i,int j)
    {
        return Levels[i,j].GetMapImage();
    }

}
