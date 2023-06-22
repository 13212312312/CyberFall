using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    string fileName = "timer.txt";
    float hours, minutes, seconds;
    [SerializeField] Text text;
    void Start()
    {
        hours = 0;
        minutes = 0;
        seconds = 0;
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;

        minutes = minutes + ((int)seconds / 60);
        hours = hours + ((int)minutes / 60);
        seconds = seconds % 60;
        minutes = minutes % 60;
        Display();
    }

    public void Load()
    {
        if (File.Exists(fileName))
        {
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    hours = reader.ReadSingle();
                    minutes = reader.ReadSingle();
                    seconds = reader.ReadSingle();
                }
            }
        }
    }

    public void Save()
    {
        using (var stream = File.Open(fileName, FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(hours);
                writer.Write(minutes);
                writer.Write(seconds);
            }
        }
    }

    public string GetTimeAsString()
    {
        int hoursToDisplay;
        int minutesToDisplay;
        int secondsToDisplay;
        hoursToDisplay = (int)hours;
        minutesToDisplay = (int)minutes;
        secondsToDisplay = (int)seconds;

        return hoursToDisplay.ToString() + ":" + minutesToDisplay.ToString() + ":" + secondsToDisplay.ToString();

    }

    public void Display()
    {
        int hoursToDisplay;
        int minutesToDisplay;
        int secondsToDisplay;
        hoursToDisplay = (int)hours;
        minutesToDisplay = (int)minutes;
        secondsToDisplay = (int)seconds;
        text.text = "Time: " + GetTimeAsString();
    }
}
