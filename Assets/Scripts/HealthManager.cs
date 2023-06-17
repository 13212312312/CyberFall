using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float currentHealth;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject coin;
    [SerializeField] int coinNumber = 4;
    [SerializeField] GameObject loseScreen;
    [SerializeField] public float invulnerabilityWindowDuration;
    private Upgrades upgradeManager;
    public float currentInvulnerability;
    private bool dead = false;
    Slider healthBar;

    void Start()
    {
        upgradeManager = FindObjectOfType<Upgrades>();
        if(upgradeManager.IncreasedHealth.owned && tag == "Player" )
        {
            health = health * 2;
        }
        if(upgradeManager.IncreasedInvulnerability.owned && tag == "Player" )
        {
            invulnerabilityWindowDuration = invulnerabilityWindowDuration * 2;
        }
        currentHealth = health;
        healthBar = canvas.transform.GetChild(0).GetComponent<Slider>();
        healthBar.maxValue = health;
        healthBar.value = currentHealth;
        canvas.SetActive(false);
        var cam = FindObjectOfType<Camera>();
        canvas.GetComponent<Canvas>().worldCamera = cam;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentInvulnerability > 0)
        {
            currentInvulnerability -= Time.deltaTime;
        }
        if(currentHealth != health)
        {
            canvas.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        if(tag == "Enemy")
        {
            currentHealth -= damage;
        }
        if(tag == "Player" && currentInvulnerability <= 0)
        {
            currentHealth -= damage;
            currentInvulnerability = invulnerabilityWindowDuration;
        }
        healthBar.value = currentHealth;
        if(currentHealth <= 0)
        {
            if(tag == "Enemy")
            {
                SpawnCoins();
                dead = true;
                Destroy(this.gameObject.transform.parent.gameObject);
            }
            if(tag == "Player")
            {
                loseScreen.SetActive(true);
                Debug.Log("You Lost!");
                Time.timeScale = 0;
            }
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    private void SpawnCoins()
    {
        if(dead) return;
        for(int i = 0; i <= coinNumber; i++)
        {
            GameObject spawnedCoint = Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
}
