using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapImageManager : MonoBehaviour
{
    [SerializeField] public GameObject point;
    [SerializeField] public GameObject[] spriteList;
    [SerializeField] public float spriteSize = 0.5f;
    [SerializeField] public GameObject minimapObject;
    [SerializeField] public Material IntranceMaterial;
    [SerializeField] public Material ExitMaterial;
    private GameObject[,] minimap;
    private GameObject instantiatedPoint;
    private MapManager mapManager;
    private Vector3 adjust;

    void Awake() {
        mapManager = FindObjectOfType<MapManager>();
        minimap = new GameObject[mapManager.Height,mapManager.Width];
        adjust = new Vector3(spriteSize * (mapManager.Width / 2),spriteSize * (mapManager.Height / 2),0);
        minimapObject.SetActive(false);
        for(int i = 0; i < mapManager.Height; i++)
        {
            for(int j = 0; j < mapManager.Width; j++)
            {
                if(mapManager.GetFullMap().Matrix[i,j] != null)
                {
                    int img = mapManager.GetMapImageFromIndex(i,j);
                    int imgType = img / 10 % 10;
                    int imgRot = img % 10;
                    int roomType = img / 100 % 10;
                    float zRotation = -90.0f * imgRot;
                    minimap[i,j] = Instantiate(spriteList[imgType],transform.position + new Vector3(spriteSize * j, spriteSize * (mapManager.Height - i), 0) - adjust, Quaternion.identity);
                    minimap[i,j].transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
                    minimap[i,j].transform.parent = minimapObject.transform;
                    minimap[i,j].SetActive(false);
                    
                    if(roomType == 1)
                        minimap[i,j].GetComponent<Renderer>().material.color = IntranceMaterial.color;
                    if(roomType == 2)
                        minimap[i,j].GetComponent<Renderer>().material.color = ExitMaterial.color;
                    
                }
            }
        }
        instantiatedPoint = Instantiate(point,transform.position + new Vector3(spriteSize * mapManager.Width/2 - spriteSize/2, spriteSize * (mapManager.Height/2 + 1)   , 0) - adjust, Quaternion.identity);
        instantiatedPoint.transform.parent = minimapObject.transform;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            minimapObject.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            minimapObject.SetActive(false);
        }
    }

    public void UpdatePoint()
    {
        float pozX,pozY;
        pozX = mapManager.currentPosition.x;
        pozY = mapManager.Height - mapManager.currentPosition.y;
        pozX = pozX * spriteSize;
        pozY = pozY * spriteSize;
        instantiatedPoint.transform.position = transform.position + new Vector3(pozX, pozY, 0) - adjust;
    }
    public void TriggerDiscover(int i,int j)
    {
        minimap[i,j].SetActive(true);
    }
}
