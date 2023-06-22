using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    Upgrades upgradeManager;
    CoinPicker coinManager;
    NameManager nameManager;
    Timer timeManager;
    WeaponManager weaponManager;
    void Awake()
    {
        upgradeManager = FindObjectOfType<Upgrades>();
        coinManager = FindObjectOfType<CoinPicker>();
        nameManager = FindObjectOfType<NameManager>();
        timeManager = FindObjectOfType<Timer>();
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        coinManager.Load();
        upgradeManager.Load();
        nameManager.Load();
        timeManager.Load();
        weaponManager.Load();
    }
    public void Save()
    {
        coinManager.Save();
        upgradeManager.Save();
        nameManager.Save();
        timeManager.Save();
        weaponManager.Save();
    }
}
