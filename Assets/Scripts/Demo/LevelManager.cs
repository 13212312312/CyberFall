using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class LevelManager : MonoBehaviour
{
    /*
    private Camera _cam;
    private PlayerMovement _player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject enemies;
    [SerializeField] private Transform enemySpawnPoint;

    private int _currentPlayerTypeIndex;
    private int _currentTilemapIndex;
    private Color _currentForegroundColor;
    private Transform[] entities;

    public SceneData SceneData;

    private void Awake()
    {
        _cam = FindObjectOfType<Camera>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        int children = enemies.transform.childCount;
        Debug.Log(children);
        entities = new Transform[children];
        int i = 0;
        foreach (Transform child in enemies.transform)
        {
            entities[i] = child;
            i++;
        }
        copiedLevels = new Tilemap[levels.Length];
        i = 0;
        bool[] list = {true,true,true,true};
        foreach (Tilemap lvl in levels)
        {
            copiedLevels[i] = RemoveTiles(lvl,list);
            copiedLevels[i].gameObject.SetActive(false);
            copiedLevels[i].transform.parent = GameObject.FindWithTag("Grid").transform;
            i++;
        }
    }

    private void Start()
    {
        SetSceneData(SceneData);
    }

    public void SetSceneData(SceneData data)
    {
        SceneData = data;

        _cam.orthographicSize = data.camSize;
        _cam.backgroundColor = data.backgroundColor;
        levels[_currentTilemapIndex].color = data.foregroundColor;

        _currentForegroundColor = data.foregroundColor;
    }

    public void SwitchLevel(int index)
    {
        var lvl = levels;
        if(opened) lvl = copiedLevels;
        lvl[_currentTilemapIndex].gameObject.SetActive(false);
        lvl[index].gameObject.SetActive(true);
        lvl[index].color = _currentForegroundColor;
        lvl[_currentTilemapIndex] = lvl[index];
        foreach(var entity in entities)
        {
            var enemy = entity;
            enemy.transform.position = enemySpawnPoint.position;
            enemy.transform.Find("BodyBone").transform.position = enemySpawnPoint.position;
            enemy.transform.Find("BodyBone").GetComponent<EnemyMovement>().ResetCooldown();
        }

        _player.transform.position = spawnPoint.position;

        _currentTilemapIndex = index;
    }
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchLevel((_currentTilemapIndex == levels.Length - 1) ? 0 : _currentTilemapIndex + 1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {

        }
    }
    */
    public Tilemap GetLevel()
    {
        return new Tilemap();
    }
    
}
