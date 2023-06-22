using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class MenuManager : MonoBehaviour
{
    NameManager nameManager;
    TcpLeaderboardClient tcpClient;
    [SerializeField] GameObject Menu1;
    [SerializeField] GameObject Menu2;
    void Awake()
    {
        nameManager = FindObjectOfType<NameManager>();
        Menu1.SetActive(true);
        Menu2.SetActive(false);
        tcpClient = FindObjectOfType<TcpLeaderboardClient>();
        nameManager.gameObject.SetActive(false);
    }

    public void ConfirmButton()
    {
        using (var stream = File.Open("coins.txt", FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(10000);
            }
        }
        using (var stream = File.Open("upgrades.txt", FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                for(int i = 0; i < 7; i++)
                    writer.Write(false);
            }
        }
        using (var stream = File.Open("timer.txt", FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                for(int i = 0; i < 3; i++)
                writer.Write(0);
            }
        }
        nameManager.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartNewGame()
    {
        Menu2.SetActive(true);
        nameManager.gameObject.SetActive(true);
        Menu1.SetActive(false);

    }

    public void Continue()
    {
        if (File.Exists("coins.txt"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Leaderboard()
    {
        tcpClient.SendMessage(1);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void Cancel()
    {
        nameManager.Load();
        Menu2.SetActive(false);
        nameManager.gameObject.SetActive(false);
        Menu1.SetActive(true);
    }
}
