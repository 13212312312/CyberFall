using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class Upgrades : MonoBehaviour
{
    public class Upgrade
    {
        public string name {get;set;}
        public bool owned {get;set;}
        public string description {get;set;}
        public float price {get; set;}
    }
    public string fileName = "upgrades.txt";
    // works
    public Upgrade BulletPiercing = new Upgrade{
        name = "Bullet Piercing",
        owned = false,
        description = "Allows your bullets to pass through more than one enemy",
        price = 201
    };
    // works
    public Upgrade IncreasedBulletSpeed = new Upgrade{
        name = "Increased Bullet Speed",
        owned = false,
        description = "Increases the speed of the bullet",
        price = 202
    };
    // works
    public Upgrade MoreBullets = new Upgrade{
        name = "More Bullets",
        owned = false,
        description = "Doubles the ammount of bullets you shoot",
        price = 203
    };
    // works
    public Upgrade IncreasedAttackSpeed = new Upgrade{
        name = "More Attack Speed",
        owned = false,
        description = "Doubles your fire rate",
        price = 204
    };
    // works
    public Upgrade IncreasedHealth = new Upgrade{
        name = "More Health",
        owned = false,
        description = "Doubles your health",
        price = 205
    };
    // works
    public Upgrade IncreasedDamage = new Upgrade{
        name = "Increased Bullet Damage",
        owned = false,
        description = "Doubles your bullet damage",
        price = 206
    };
    // works
    public Upgrade IncreasedInvulnerability = new Upgrade{
        name = "Increased Invulnerability Window",
        owned = false,
        description = "Doubles the duration you stay in invulnerability window",
        price = 207
    };

    public void Load()
    {
        if (File.Exists(fileName))
        {
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {

                    BulletPiercing.owned = reader.ReadBoolean();
                    IncreasedBulletSpeed.owned = reader.ReadBoolean();
                    MoreBullets.owned = reader.ReadBoolean();
                    IncreasedAttackSpeed.owned = reader.ReadBoolean();
                    IncreasedHealth.owned = reader.ReadBoolean();
                    IncreasedDamage.owned = reader.ReadBoolean();
                    IncreasedInvulnerability.owned = reader.ReadBoolean();
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
                writer.Write(BulletPiercing.owned);
                writer.Write(IncreasedBulletSpeed.owned);
                writer.Write(MoreBullets.owned);
                writer.Write(IncreasedAttackSpeed.owned);
                writer.Write(IncreasedHealth.owned);
                writer.Write(IncreasedDamage.owned);
                writer.Write(IncreasedInvulnerability.owned);
            }
        }
    }

    void Awake()
    {
        Load();
        /*
        BulletPiercing.owned = false;
        IncreasedBulletSpeed.owned = false;
        MoreBullets.owned = false;
        IncreasedAttackSpeed.owned = false;
        IncreasedHealth.owned = false;
        IncreasedDamage.owned = false;
        IncreasedInvulnerability.owned = false;
        Save();
        */
    }

    public Upgrade getUpgrade(int which)
    {
        switch(which)
        {
            case 0:
                return BulletPiercing;
            case 1:
                return IncreasedBulletSpeed;
            case 2:
                return MoreBullets;
            case 3:
                return IncreasedAttackSpeed;
            case 4:
                return IncreasedHealth;
            case 5:
                return IncreasedDamage;
            case 6:
                return IncreasedInvulnerability;
        }
        return null;
    }

}
