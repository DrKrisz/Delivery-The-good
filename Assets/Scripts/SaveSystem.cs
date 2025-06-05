using UnityEngine;
using System.Reflection;

public class SaveSystem : MonoBehaviour
{
    public PizzaManager pizzaManager;
    public PlayerMovement playerMovement;
    public GameClock gameClock;

    private static SaveSystem instance;
    public static SaveSystem Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        if (pizzaManager != null)
            PlayerPrefs.SetFloat("Money", pizzaManager.GetCurrentMoney());

        if (playerMovement != null)
            PlayerPrefs.SetFloat("Energy", playerMovement.GetEnergy());

        if (gameClock != null)
        {
            var dayField = typeof(GameClock).GetField("currentDayIndex", BindingFlags.NonPublic | BindingFlags.Instance);
            var hourField = typeof(GameClock).GetField("currentHour", BindingFlags.NonPublic | BindingFlags.Instance);
            var minuteField = typeof(GameClock).GetField("currentMinute", BindingFlags.NonPublic | BindingFlags.Instance);

            if (dayField != null) PlayerPrefs.SetInt("Day", (int)dayField.GetValue(gameClock));
            if (hourField != null) PlayerPrefs.SetInt("Hour", (int)hourField.GetValue(gameClock));
            if (minuteField != null) PlayerPrefs.SetInt("Minute", (int)minuteField.GetValue(gameClock));
        }

        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (pizzaManager != null && PlayerPrefs.HasKey("Money"))
        {
            float money = PlayerPrefs.GetFloat("Money");
            var field = typeof(PizzaManager).GetField("money", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) field.SetValue(pizzaManager, money);
            pizzaManager.SendMessage("UpdateMoneyUI", SendMessageOptions.DontRequireReceiver);
        }

        if (playerMovement != null && PlayerPrefs.HasKey("Energy"))
        {
            playerMovement.SetEnergy(PlayerPrefs.GetFloat("Energy"));
        }

        if (gameClock != null && PlayerPrefs.HasKey("Day"))
        {
            int day = PlayerPrefs.GetInt("Day");
            int hour = PlayerPrefs.GetInt("Hour");
            int minute = PlayerPrefs.GetInt("Minute");
            gameClock.SetTime(day, hour, minute);
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}
