using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Upgrades : MonoBehaviour
{
    public class Upgrade
    {
        public string name {get;set;}
        public bool owned {get;set;}
        public string description {get;set;}
        public float price {get; set;}
    }
    [SerializeField] public bool UpgradePiercing;
    [SerializeField] public bool UpgradeBulletSpeed;
    [SerializeField] public bool UpgradeMoreBullets;
    [SerializeField] public bool UpgradeIncreasedAttackSpeed;
    [SerializeField] public bool UpgradeIncreasedHealth;
    [SerializeField] public bool UpgradeIncreasedDamage;
    [SerializeField] public bool UpgradeIncreasedInvulnerability;
    // works
    public Upgrade BulletPiercing = new Upgrade{
        name = "Bullet Piercing",
        owned = false,
        description = "Allows your bullets to pass through more than one enemy"
    };
    // works
    public Upgrade IncreasedBulletSpeed = new Upgrade{
        name = "Increased Bullet Speed",
        owned = false,
        description = "Increases the speed of the bullet"
    };
    // works
    public Upgrade MoreBullets = new Upgrade{
        name = "More Bullets",
        owned = false,
        description = "Doubles the ammount of bullets you shoot"
    };
    // works
    public Upgrade IncreasedAttackSpeed = new Upgrade{
        name = "More Attack Speed",
        owned = false,
        description = "Doubles your fire rate"
    };
    // works
    public Upgrade IncreasedHealth = new Upgrade{
        name = "More Health",
        owned = false,
        description = "Doubles your health"
    };
    // works
    public Upgrade IncreasedDamage = new Upgrade{
        name = "Increased Bullet Damage",
        owned = false,
        description = "Doubles your bullet damage"
    };
    // works
    public Upgrade IncreasedInvulnerability = new Upgrade{
        name = "Increased Invulnerability Window",
        owned = false,
        description = "Doubles the duration you stay in invulnerability window"
    };

    void Awake()
    {
        BulletPiercing.owned = UpgradePiercing;
        IncreasedBulletSpeed.owned = UpgradeBulletSpeed;
        MoreBullets.owned = UpgradeMoreBullets;
        IncreasedAttackSpeed.owned = UpgradeIncreasedAttackSpeed;
        IncreasedHealth.owned = UpgradeIncreasedHealth;
        IncreasedDamage.owned = UpgradeIncreasedDamage;
        IncreasedInvulnerability.owned = UpgradeIncreasedInvulnerability;
    }
}
