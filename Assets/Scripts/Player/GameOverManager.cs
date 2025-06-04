using UnityEngine;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup fadeCanvas;
    public TMP_Text messageText;
    public float fadeSpeed = 1.5f;

    [Header("References")]
    public PlayerMovement playerMovement;
    public PlayerLook playerLook;
    public PizzaManager pizzaManager;
    public GameClock gameClock;
    public Transform player;
    public Transform bedSpawnPoint;
    public GameObject scooter;

    private bool isGameOver = false;

    public void TriggerGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            StartCoroutine(GameOverSequence());
        }
    }

    IEnumerator GameOverSequence()
    {
        // Disable control
        playerMovement.enabled = false;
        playerLook.enabled = false;

        // Collapse animation (camera tilt)
        Transform cam = Camera.main.transform;
        Quaternion originalRot = cam.localRotation;
        Quaternion collapseRot = Quaternion.Euler(75f, originalRot.eulerAngles.y, 0f); // look downward

        float collapseTime = 1.2f;
        float t = 0f;
        while (t < collapseTime)
        {
            cam.localRotation = Quaternion.Slerp(originalRot, collapseRot, t / collapseTime);
            t += Time.deltaTime;
            yield return null;
        }

        // Fade to black
        while (fadeCanvas.alpha < 1f)
        {
            fadeCanvas.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Remove held pizza
        Transform heldPizza = player.Find("PizzaHoldPoint");
        if (heldPizza != null && heldPizza.childCount > 0)
        {
            Destroy(heldPizza.GetChild(0).gameObject);
            pizzaManager.ClearCurrentPizza(); // <--- Put it right here
        }


        // Clear pizza state from manager
        pizzaManager.ClearCurrentPizza(); // <-- use this if you added the method

        messageText.text = "You ran out of energy...\nEverything was stolen!";
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);

        messageText.gameObject.SetActive(false);

        // Reset player data
        pizzaManager.RemoveMoney(pizzaManager.GetCurrentMoney()); // $0
        playerMovement.RestoreEnergy(); // 100 energy

        // Reset time completely back to the initial starting day
        gameClock.ResetTime();

        // Respawn player at bed
        player.position = bedSpawnPoint.position + Vector3.up * 1f;
        player.rotation = bedSpawnPoint.rotation;

        // Remove scooter
        if (scooter != null)
            Destroy(scooter);

        yield return new WaitForSeconds(1f);

        // Fade back in
        while (fadeCanvas.alpha > 0f)
        {
            fadeCanvas.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Re-enable control
        playerMovement.enabled = true;
        playerLook.enabled = true;

        isGameOver = false;

        // Reset camera angle
        cam.localRotation = originalRot;

    }
}