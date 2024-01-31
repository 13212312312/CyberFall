using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] public float currentHealth;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject coin;
    [SerializeField] int coinNumber;
    [SerializeField] GameObject loseScreen;
    [SerializeField] public float invulnerabilityWindowDuration;
    Brain brain;
    private Upgrades upgradeManager;
    public float currentInvulnerability;
    private bool dead = false;
    Slider healthBar;

    void Start()
    {
        brain = GameObject.FindObjectOfType<Brain>();
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
        if(tag == "Enemy" && currentInvulnerability <= 0)
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
            if(tag == "Enemy" && !dead)
            {
                SpawnCoins();
                dead = true;
                EnemyMovement script = GetComponent<EnemyMovement>();
                int type = script.GetEnemyType();
                bool x = brain.Remove(script,type);
                Destroy(this.gameObject.transform.parent.gameObject);
            }
            if(tag == "Player")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
