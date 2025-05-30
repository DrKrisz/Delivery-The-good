using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight; // assign in Inspector
    public KeyCode toggleKey = KeyCode.G;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
