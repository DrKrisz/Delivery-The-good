using UnityEngine;

public class LightingController : MonoBehaviour
{
    [Header("References")]
    public Light sun;
    public GameClock gameClock;
    public Camera mainCamera; // Drag your Main Camera here

    [Header("Time Settings")]
    public float minIntensity = 0f;
    public float maxIntensity = 1f;
    public float sunriseHour = 6f;
    public float sunsetHour = 18f;

    [Header("Sky Colors")]
    public Color dayColor = new Color(0.5f, 0.8f, 1f); // light blue
    public Color nightColor = new Color(0.02f, 0.02f, 0.05f); // dark bluish black

    void Update()
    {
        float hour = gameClock.GetHour() + gameClock.GetMinute() / 60f;

        // Calculate light intensity
        float intensity = 0f;
        if (hour >= sunriseHour && hour <= sunsetHour)
        {
            float dayProgress = (hour - sunriseHour) / (sunsetHour - sunriseHour);
            intensity = Mathf.Sin(dayProgress * Mathf.PI);
        }

        sun.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensity);

        // Rotate the sun
        float sunAngle = (hour / 24f) * 360f - 90f;
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 170f, 0f));

        // Change camera background color
        float dayFactor = Mathf.Clamp01(intensity);
        if (mainCamera != null)
            mainCamera.backgroundColor = Color.Lerp(nightColor, dayColor, dayFactor);
    }
}
