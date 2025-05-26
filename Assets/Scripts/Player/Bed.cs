using UnityEngine;
using TMPro;

public class Bed : MonoBehaviour
{
    public float cost = 100f;
    public PlayerMovement playerMovement;
    public PizzaManager pizzaManager;
    public GameObject promptUI; // assign your SleepPromptText object here

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (pizzaManager != null && playerMovement != null)
            {
                float money = pizzaManager.GetCurrentMoney();
                if (money >= cost)
                {
                    pizzaManager.RemoveMoney(cost);
                    playerMovement.RestoreEnergy();
                    Debug.Log("Slept and restored energy!");
                }
                else
                {
                    Debug.Log("Not enough money to sleep.");
                }
            }
        }
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
}
