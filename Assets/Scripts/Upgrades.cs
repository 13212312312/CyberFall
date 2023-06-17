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
    // works
    public Upgrade BulletPiercing = new Upgrade{
        name = "Bullet Piercing",
        owned = true,
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

    public Upgrade ReduceBulletSpread = new Upgrade{
        name = "Reduce Bullet Spread",
        owned = false,
        description = "Reduces the spread of the bullets"
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
}
