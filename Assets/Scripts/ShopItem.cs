using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] int upgrade;
    [SerializeField] Text titleText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text priceText;
    [SerializeField] GameObject buyButton;
    private Upgrades upgradeManager;
    private CoinPicker coinManager;
    // Start is called before the first frame update

    void Awake()
    {
        upgradeManager = FindObjectOfType<Upgrades>();
        coinManager = FindObjectOfType<CoinPicker>();
    }

    void Start()
    {
        var currentUpgrade = upgradeManager.getUpgrade(upgrade);
        titleText.text = currentUpgrade.name;
        descriptionText.text = currentUpgrade.description;
        priceText.text = "Price: " + currentUpgrade.price;
        buyButton.SetActive(!currentUpgrade.owned);
    }

    public void TryToBuy()
    {
        var currentUpgrade = upgradeManager.getUpgrade(upgrade);
        if(coinManager.coins >= currentUpgrade.price)
        {
            currentUpgrade.owned = true;
            coinManager.coins -= (int)currentUpgrade.price;
            buyButton.SetActive(false);
            FindObjectOfType<UpgradesShop>().callChange();
        }
    }
}
