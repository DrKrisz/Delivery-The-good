using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text titleText;
    public TMP_Text dialogueText;
    public Button option1Button;
    public Button option2Button;
    public GameObject talkPromptUI;

    [Header("Player Control")]
    public PlayerMovement playerMovement;
    public PlayerLook playerLook;

    private bool playerInRange = false;
    private DialogueState currentState;

    private enum DialogueState
    {
        Greeting,
        PlayerResponse,
        NPCFollowup,
        End
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        talkPromptUI.SetActive(false);

        option1Button.onClick.AddListener(OnOption1);
        option2Button.onClick.AddListener(OnOption2);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !dialoguePanel.activeSelf)
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        talkPromptUI.SetActive(false);
        currentState = DialogueState.Greeting;
        ShowGreeting();

        // Lock control
        playerMovement.enabled = false;
        playerLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        // Restore control
        playerMovement.enabled = true;
        playerLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ShowGreeting()
    {
        titleText.text = "NPC";
        dialogueText.text = "How is your day?";

        option1Button.GetComponentInChildren<TMP_Text>().text = "It's good, how about you?";
        option2Button.GetComponentInChildren<TMP_Text>().text = "Goodbye";

        option1Button.gameObject.SetActive(true);
        option2Button.gameObject.SetActive(true);
    }

    void ShowPlayerResponse()
    {
        titleText.text = "Player";
        dialogueText.text = "It's good, how about you?";

        option1Button.GetComponentInChildren<TMP_Text>().text = "Click Next";
        option1Button.gameObject.SetActive(true);
        option2Button.gameObject.SetActive(false);

        currentState = DialogueState.NPCFollowup;
    }

    void ShowNPCFollowup()
    {
        titleText.text = "NPC";
        dialogueText.text = "That's sooo cool!!";

        option1Button.GetComponentInChildren<TMP_Text>().text = "Goodbye";
        option1Button.gameObject.SetActive(true);
        option2Button.gameObject.SetActive(false);

        currentState = DialogueState.End;
    }

    void OnOption1()
    {
        switch (currentState)
        {
            case DialogueState.Greeting:
                ShowPlayerResponse();
                currentState = DialogueState.NPCFollowup;
                break;

            case DialogueState.NPCFollowup:
                ShowNPCFollowup();
                currentState = DialogueState.End;
                break;

            case DialogueState.End:
                EndDialogue();
                break;
        }
    }

    void OnOption2()
    {
        EndDialogue(); // If player says Goodbye early
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            talkPromptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            talkPromptUI.SetActive(false);
            EndDialogue();
        }
    }
}
