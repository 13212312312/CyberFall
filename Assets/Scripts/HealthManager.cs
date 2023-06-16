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
    Slider healthBar;

    void Start()
    {
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
        if(currentHealth != health)
        {
            canvas.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if(currentHealth <= 0)
        {
            if(tag == "Enemy")
            {
                SpawnCoins();
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
        for(int i = 0; i <= coinNumber; i++)
        {
            GameObject spawnedCoint = Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
}
