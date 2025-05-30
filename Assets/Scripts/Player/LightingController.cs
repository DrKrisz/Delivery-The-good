using UnityEngine;

public class LightingController : MonoBehaviour
{
    [Header("References")]
    public Light sun;
    public GameClock gameClock; // Drag your GameClock GameObject here

    [Header("Time Settings")]
    public float minIntensity = 0f;
    public float maxIntensity = 1f;
    public float sunriseHour = 6f;
    public float sunsetHour = 18f;

    void Update()
    {
        float hour = gameClock.GetHour() + gameClock.GetMinute() / 60f;

        // Calculate intensity curve based on time
        float intensity = 0f;

        if (hour >= sunriseHour && hour <= sunsetHour)
        {
            float dayProgress = (hour - sunriseHour) / (sunsetHour - sunriseHour);
            intensity = Mathf.Sin(dayProgress * Mathf.PI); // Smooth up and down
        }

        sun.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensity);

        // Optional: rotate sun over time
        float sunAngle = (hour / 24f) * 360f - 90f; // offset so 12:00 is overhead
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 170f, 0f));
    }
}
