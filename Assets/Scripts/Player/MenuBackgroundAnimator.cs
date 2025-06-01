using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundAnimator : MonoBehaviour
{
    public Sprite[] frames;         // Assign your 5 images here in the inspector
    public float frameRate = 0.5f;  // Time per frame

    private Image backgroundImage;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        backgroundImage = GetComponent<Image>();
        if (frames.Length > 0)
            backgroundImage.sprite = frames[0];
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            backgroundImage.sprite = frames[currentFrame];
            timer = 0f;
        }
    }
}
