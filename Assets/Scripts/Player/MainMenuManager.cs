using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera menuCamera;
    public Transform playerCameraTarget; // Assign player head/camera position

    [Header("Menu UI")]
    public CanvasGroup menuCanvas;
    public Button playButton;
    public Button quitButton;

    [Header("Player")]
    public GameObject player;
    public MonoBehaviour[] playerScriptsToEnable; // Drag PlayerMovement, PlayerLook, etc.

    [Header("Transition")]
    public float flyDuration = 2f;
    public AnimationCurve flyCurve; // Optional: for easing

    private bool isTransitioning = false;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        menuCamera.enabled = true;
        player.SetActive(false); // Hide player control until game starts

        playButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(() => {
            SaveSystem system = FindObjectOfType<SaveSystem>();
            if (system != null)
                system.SaveGame();
            Application.Quit();
        });
    }

    public void StartGame()
    {
        if (!isTransitioning)
            StartCoroutine(FlyToPlayer());
    }

    IEnumerator FlyToPlayer()
    {
        isTransitioning = true;

        Vector3 startPos = menuCamera.transform.position;
        Quaternion startRot = menuCamera.transform.rotation;

        Vector3 endPos = playerCameraTarget.position;
        Quaternion endRot = playerCameraTarget.rotation;

        float t = 0f;

        // Fade out UI
        while (menuCanvas.alpha > 0f)
        {
            menuCanvas.alpha -= Time.deltaTime * 1.5f;
            yield return null;
        }

        menuCanvas.interactable = false;
        menuCanvas.blocksRaycasts = false;

        // Camera fly effect
        while (t < 1f)
        {
            float curvedT = flyCurve != null ? flyCurve.Evaluate(t) : t;

            menuCamera.transform.position = Vector3.Lerp(startPos, endPos, curvedT);
            menuCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, curvedT);

            t += Time.deltaTime / flyDuration;
            yield return null;
        }

        // Snap to player
        menuCamera.enabled = false;
        player.SetActive(true);

        foreach (var script in playerScriptsToEnable)
            script.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
