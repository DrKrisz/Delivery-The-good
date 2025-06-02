using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Image staminaImage;
    public Sprite[] staminaSprites; // Drag 6 sprites here (0 = full, 5 = empty)
    public PlayerMovement playerMovement;

    void Update()
    {
        float energy = playerMovement.GetEnergy(); // 0–100
        int index = Mathf.Clamp(Mathf.FloorToInt((1f - (energy / 100f)) * staminaSprites.Length), 0, staminaSprites.Length - 1);
        staminaImage.sprite = staminaSprites[index];
    }
}
