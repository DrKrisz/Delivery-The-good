using NUnit.Framework;
using UnityEngine;
using System.Reflection;

public class GameClockTests
{
    [Test]
    public void Advance30MinutesTwice_UpdatesHourAndDay()
    {
        var go = new GameObject();
        var clock = go.AddComponent<GameClock>();

        // Set time to 23:30 on Monday
        var hourField = typeof(GameClock).GetField("currentHour", BindingFlags.NonPublic | BindingFlags.Instance);
        var minuteField = typeof(GameClock).GetField("currentMinute", BindingFlags.NonPublic | BindingFlags.Instance);
        var dayField = typeof(GameClock).GetField("currentDayIndex", BindingFlags.NonPublic | BindingFlags.Instance);

        hourField.SetValue(clock, 23);
        minuteField.SetValue(clock, 30);
        dayField.SetValue(clock, 0);

        // Act
        clock.Advance30Minutes();
        clock.Advance30Minutes();

        // Assert
        Assert.AreEqual(0, clock.GetHour(), "Hour should wrap to 0 after midnight.");
        Assert.AreEqual(30, clock.GetMinute(), "Minute should be 30 after two advances.");
        Assert.AreEqual(1, (int)dayField.GetValue(clock), "Day should advance by one after crossing midnight.");

        Object.DestroyImmediate(go);
    }
}
