using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Button closeButton;
    public PlayerLook playerLook;
    public PlayerMovement playerMovement;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseInventory);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        playerLook.enabled = !isOpen;
        playerMovement.enabled = !isOpen;
    }

    void CloseInventory()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerLook.enabled = true;
        playerMovement.enabled = true;
    }
}
