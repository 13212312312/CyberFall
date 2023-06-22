using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class WeaponManager : MonoBehaviour
{
    private string fileName = "weapon.txt";
    [SerializeField] GameObject[] weapons;
    [SerializeField] int chosenWeapon;
    private bool inRange = false;

    void Awake()
    {
        Load();
        for(int i = 0; i < weapons.Length; i++)
        {
            if(i != chosenWeapon)
            {
                weapons[i].SetActive(false);
            }
            else
            {
                weapons[i].SetActive(true);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && inRange)
		{
            weapons[chosenWeapon].SetActive(false);
            chosenWeapon = (chosenWeapon + 1) % weapons.Length;
            weapons[chosenWeapon].SetActive(true);
		}
    }

    public void Load()
    {
        if (File.Exists(fileName))
        {
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    chosenWeapon = reader.ReadInt32();
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
                writer.Write(chosenWeapon);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
