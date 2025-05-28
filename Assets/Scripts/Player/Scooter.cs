using UnityEngine;
using TMPro;

public class Scooter : MonoBehaviour
{
    public float cost = 100f;
    public float scooterSpeed = 10f;
    public float energyLossMultiplier = 0.5f;

    public PlayerMovement playerMovement;
    public PizzaManager pizzaManager;
    public GameObject promptUI;
    public Transform scooterAttachPoint;
    public Transform playerCamera; // <-- NEW: assign the player camera here

    private bool playerInRange = false;
    private bool scooterOwned = false;
    private bool scooterActive = false;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Buying the scooter
        if (playerInRange && !scooterOwned && Input.GetKeyDown(KeyCode.F))
        {
            float money = pizzaManager.GetCurrentMoney();
            if (money >= cost)
            {
                pizzaManager.RemoveMoney(cost);
                scooterOwned = true;
                promptUI.GetComponent<TMP_Text>().text = "Press F to Use Scooter";
            }
        }

        // Using or exiting the scooter
        if (scooterOwned && Input.GetKeyDown(KeyCode.F))
        {
            if (!scooterActive && playerInRange)
            {
                ActivateScooter();
            }
            else if (scooterActive && IsLookingAtScooter())
            {
                DeactivateScooter();
            }
        }

        // Show/hide prompt only when looking at scooter
        if (scooterActive)
        {
            promptUI.SetActive(IsLookingAtScooter());
            if (promptUI.activeSelf)
                promptUI.GetComponent<TMP_Text>().text = "Press F to Stop Using Scooter";
        }
    }

    void ActivateScooter()
    {
        scooterActive = true;
        transform.SetParent(scooterAttachPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        playerMovement.walkSpeed = scooterSpeed;
        playerMovement.energyLossRate *= energyLossMultiplier;
    }

    void DeactivateScooter()
    {
        scooterActive = false;
        transform.SetParent(null);

        // Start just below the camera to avoid hitting player's head
        Vector3 rayOrigin = playerCamera.position + Vector3.down * 0.2f;
        Vector3 rayDirection = Vector3.down;

        RaycastHit hit;
        int groundLayerMask = LayerMask.GetMask("Default"); // Use whatever your ground is on

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 5f, groundLayerMask))
        {
           transform.position = hit.point + Vector3.down * 0.5f;
        }
        else
        {
            // fallback: place near feet without raycast
            transform.position = playerCamera.position + playerCamera.forward * -1f + Vector3.down * 1f;
        }

        // Rotate scooter to face same direction as camera
        transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);

        // Reset stats
        playerMovement.walkSpeed = 5f;
        playerMovement.energyLossRate /= energyLossMultiplier;

        promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!scooterOwned)
            {
                promptUI.SetActive(true);
                promptUI.GetComponent<TMP_Text>().text = "Press F to Buy Scooter ($100)";
            }
            else if (!scooterActive)
            {
                promptUI.SetActive(true);
                promptUI.GetComponent<TMP_Text>().text = "Press F to Use Scooter";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (!scooterActive)
                promptUI.SetActive(false);
        }
    }

    bool IsLookingAtScooter()
    {
        if (playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            return hit.collider.gameObject == this.gameObject;
        }

        return false;
    }
}
