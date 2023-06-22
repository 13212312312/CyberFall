using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class NameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string playerName;
    string fileName = "name.txt";
    [SerializeField] bool show = true;

    void Awake() 
    {
        Load();
    }

    public void Load()
    {
        if (File.Exists(fileName))
        {
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    playerName = reader.ReadString();
                }
            }
        }
    }

    public void Save()
    {
        using (var stream = File.Open(fileName, FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(playerName);
            }
        }
    }

    void OnGUI()
    {
        if(show)
            playerName = GUI.TextField(new Rect(600, 500, 400, 40), playerName, 25);
    }
}
