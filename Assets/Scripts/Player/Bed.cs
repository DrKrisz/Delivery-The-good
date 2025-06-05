using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Bed : MonoBehaviour
{

    public Transform playerCamera; // Assign this in Inspector
    public float lookRange = 3f;   // How far the player can interact

    public float cost = 100f;
    public PlayerMovement playerMovement;
    public PizzaManager pizzaManager;
    public GameObject promptUI;

    [Header("Fade UI")]
    public CanvasGroup fadeCanvas;
    public float fadeSpeed = 1.5f;

    private bool playerInRange = false;

    void Update()
    {
        if (!playerInRange) return;

        bool isLooking = IsLookingAtBed();
        promptUI.SetActive(isLooking);

        if (isLooking && Input.GetKeyDown(KeyCode.E))
        {
            if (pizzaManager != null && playerMovement != null)
            {
                float money = pizzaManager.GetCurrentMoney();
                if (money >= cost)
                {
                    pizzaManager.RemoveMoney(cost);
                    StartCoroutine(SleepSequence());
                }
                else
                {
                    Debug.Log("Not enough money to sleep.");
                }
            }
        }
    }


    IEnumerator SleepSequence()
    {
        // Disable movement and look
        playerMovement.enabled = false;
        PlayerLook look = FindObjectOfType<PlayerLook>();
        if (look != null) look.enabled = false;

        // Fade to black
        while (fadeCanvas.alpha < 1f)
        {
            fadeCanvas.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Wait in black screen
        yield return new WaitForSeconds(1.5f);

        // Restore energy
        playerMovement.RestoreEnergy();

        // Advance time by 8 hours (16 x 30 minutes)
        GameClock clock = FindObjectOfType<GameClock>();
        if (clock != null)
        {
            for (int i = 0; i < 16; i++)
                clock.Advance30Minutes();
        }

        // Fade back in
        while (fadeCanvas.alpha > 0f)
        {
            fadeCanvas.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        SaveSystem system = FindObjectOfType<SaveSystem>();
        if (system != null)
            system.SaveGame();

        // Re-enable movement and look
        playerMovement.enabled = true;
        if (look != null) look.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    bool IsLookingAtBed()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, lookRange))
        {
            return hit.collider != null && hit.collider.gameObject == gameObject;
        }

        return false;
    }

}
