using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradesShop : MonoBehaviour
{
    bool inRange = false;
    [SerializeField] GameObject shopMenu;
    SaveManager saveManager;
    bool somethingChanged = false;
    // Start is called before the first frame update
    void Awake()
    {
        shopMenu.SetActive(false);
        saveManager = FindObjectOfType<SaveManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.F) && inRange)
		{
            if(shopMenu.activeSelf)
            {
                CloseShop();
            }
            else
            {
			    shopMenu.SetActive(true);
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
            CloseShop();
        }
    }

    public void callChange()
    {
        somethingChanged = true;
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        if(somethingChanged)
        {
            saveManager.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
