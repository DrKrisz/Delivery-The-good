using UnityEngine;
using TMPro; // For UI display

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 1.5f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    [Header("Energy System")]
    public float energy = 100f;
    public float energyLossRate = 0.1f;
    public TMP_Text energyText;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Respawn")]
    public Transform bedSpawnPoint;

    [Header("Crouch Settings")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1.2f;
    public float crouchSpeed = 3f;
    public float standingSpeed = 5f;
    public Transform cameraHolder; // Assign your Main Camera parent (optional)

    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        walkSpeed = standingSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateEnergyUI();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        bool isMoving = move.magnitude > 0.1f;
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Energy drain
        if (isMoving && energy > 0f)
        {
            energy -= energyLossRate * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, 100f);
            UpdateEnergyUI();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (energy <= 0f)
        {
            energy = 0f;

            PizzaManager manager = FindObjectOfType<PizzaManager>();
            if (manager != null && !manager.HasPizza()) // Only collapse if NOT holding pizza
            {
                GameOverManager go = FindObjectOfType<GameOverManager>();
                if (go != null)
                    go.TriggerGameOver();
            }
        }

        // Check if player fell into the void
        if (transform.position.y < -50f)
        {
            Debug.LogWarning("Player fell into the void! Respawning at bed.");
            controller.enabled = false;
            transform.position = bedSpawnPoint.position;
            controller.enabled = true;
        }

        // Toggle Crouch with 'C'
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            ApplyCrouch();
        }

        // Hold Crouch with Left Ctrl
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                ApplyCrouch();
            }
        }
        else if (isCrouching && !Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = false;
            ApplyCrouch();
        }

    }

    void UpdateEnergyUI()
    {
        if (energyText != null)
            energyText.text = "Energy: " + Mathf.FloorToInt(energy).ToString();
    }

    public void RestoreEnergy()
    {
        energy = 100f;
        UpdateEnergyUI();
    }

    public float GetEnergy()
    {
        return energy;
    }

    public void SetEnergy(float value)
    {
        energy = value;
        UpdateEnergyUI();
    }

    void ApplyCrouch()
    {
        controller.height = isCrouching ? crouchingHeight : standingHeight;
        walkSpeed = isCrouching ? crouchSpeed : standingSpeed;

        // Optional: adjust camera height too
        if (cameraHolder != null)
        {
            Vector3 camPos = cameraHolder.localPosition;
            camPos.y = isCrouching ? 0.4f : 0.7f;
            cameraHolder.localPosition = camPos;
        }
    }

}
