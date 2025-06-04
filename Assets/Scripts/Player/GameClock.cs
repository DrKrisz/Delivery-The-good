using System.Collections;
using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    public TMP_Text timeDisplay;

    private string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    private int currentDayIndex = 0; // Monday
    private int currentHour = 7;     // Start at 07:00
    private int currentMinute = 0;


    private float interval = 5f; // Real-time seconds per in-game time tick

    void Start()
    {
        UpdateTimeDisplay();
        StartCoroutine(AdvanceTime());
    }

    IEnumerator AdvanceTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Advance30Minutes();
            UpdateTimeDisplay();
        }
    }

    public void Advance30Minutes()
    {
        currentMinute += 30;
        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;
            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDayIndex = (currentDayIndex + 1) % daysOfWeek.Length;
            }
        }
    }

    // Resets the clock back to the initial starting time
    public void ResetTime()
    {
        currentDayIndex = 0; // Monday
        currentHour = 7;
        currentMinute = 0;
        UpdateTimeDisplay();
    }

    void UpdateTimeDisplay()
    {
        string formattedTime = string.Format("{0} {1:00}:{2:00}", daysOfWeek[currentDayIndex], currentHour, currentMinute);
        timeDisplay.text = formattedTime;
    }

    public int GetHour()
    {
        return currentHour;
    }

    public int GetMinute()
    {
        return currentMinute;
    }

}
