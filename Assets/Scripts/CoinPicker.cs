using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class CoinPicker : MonoBehaviour
{
    public string fileName = "coins.txt";
    [SerializeField] public int coins;
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Awake() 
    {
        Load();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = "     : " + coins;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin")
        {
            other.gameObject.GetComponent<Coin>().TriggerMagnet(this);
        }
    }
    public void AddCoins(int value)
    {
        coins += value;
    }

    public int GetCoins()
    {
        return coins;
    }

    public void SetCoins(int value)
    {
        coins = value;
    }
    public void Load()
    {
        if (File.Exists(fileName))
        {
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {

                    coins = reader.ReadInt32();
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
                writer.Write(coins);
            }
        }
    }
}
