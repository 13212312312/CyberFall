using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    private bool inRange = false;
    private TcpLeaderboardClient server;
    void Awake()
    {
        server = FindObjectOfType<TcpLeaderboardClient>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && inRange)
		{
            server.SendMessage(0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
