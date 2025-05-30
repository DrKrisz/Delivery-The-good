using UnityEngine;
using TMPro;

public class Car : MonoBehaviour
{
    public float cost = 1f;
    public float carSpeed = 15f;
    public float energyLossMultiplier = 0.3f;

    [Header("References")]
    public PlayerMovement playerMovement;
    public PizzaManager pizzaManager;
    public GameObject promptUI;
    public Transform carAttachPoint;
    public Transform playerCamera;
    public Transform carCameraAnchor;
    public GameObject playerObject; // Hide when driving
    public GameObject thirdPersonCamera; // Enable while in car

    private bool playerInRange = false;
    private bool carOwned = false;
    private bool carActive = false;

    private Vector3 originalCamPosition;
    private Quaternion originalCamRotation;

    void Update()
    {
        if (playerInRange && !carOwned && Input.GetKeyDown(KeyCode.F))
        {
            float money = pizzaManager.GetCurrentMoney();
            if (money >= cost)
            {
                pizzaManager.RemoveMoney(cost);
                carOwned = true;
                promptUI.GetComponent<TMP_Text>().text = "Press F to Use Car";
            }
        }

        if (carOwned && Input.GetKeyDown(KeyCode.F))
        {
            if (!carActive && playerInRange)
                ActivateCar();
            else if (carActive && IsLookingAtCar())
                DeactivateCar();
        }

        if (carActive)
        {
            promptUI.SetActive(IsLookingAtCar());
            if (promptUI.activeSelf)
                promptUI.GetComponent<TMP_Text>().text = "Press F to Exit Car";
        }
    }

    void ActivateCar()
    {
        carActive = true;

        // Move car to player
        transform.SetParent(carAttachPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Stats
        playerMovement.walkSpeed = carSpeed;
        playerMovement.energyLossRate *= energyLossMultiplier;

        // Hide player
        playerObject.SetActive(false);

        // Enable 3rd person camera
        originalCamPosition = playerCamera.position;
        originalCamRotation = playerCamera.rotation;
        thirdPersonCamera.transform.position = carCameraAnchor.position;
        thirdPersonCamera.transform.rotation = carCameraAnchor.rotation;
        thirdPersonCamera.SetActive(true);
        playerCamera.gameObject.SetActive(false);
    }

    void DeactivateCar()
    {
        carActive = false;
        transform.SetParent(null);

        // Drop car to ground
        Vector3 rayOrigin = playerCamera.position + Vector3.down * 0.2f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 5f))
            transform.position = hit.point + Vector3.down * 0.5f;
        else
            transform.position = playerCamera.position + playerCamera.forward * -1f + Vector3.down * 1f;

        transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);

        // Reset stats
        playerMovement.walkSpeed = 5f;
        playerMovement.energyLossRate /= energyLossMultiplier;

        // Show player
        playerObject.SetActive(true);

        // Reset camera
        thirdPersonCamera.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptUI.SetActive(true);

            if (!carOwned)
                promptUI.GetComponent<TMP_Text>().text = "Press F to Buy Car ($1000)";
            else if (!carActive)
                promptUI.GetComponent<TMP_Text>().text = "Press F to Use Car";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (!carActive)
                promptUI.SetActive(false);
        }
    }

    bool IsLookingAtCar()
    {
        if (playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            return hit.collider.gameObject == this.gameObject;

        return false;
    }
}
