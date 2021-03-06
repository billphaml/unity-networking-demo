/******************************************************************************
*  This is the DialogueManager class.
 * The purpose of this class is to manage the dialoguescene scriptable object 
 * and pass it dialogue object through the queue to display to the player
 * Currently missing the ability to trigger events, apply buffs and change
 * values.
 * 
 * Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public GameObject textGroup;
    public CanvasGroup dialogueCanvasGroup = null;
    public CanvasGroup userResponseCanvas = null;
    public CanvasGroup continueCanvas = null;
    public TextMeshProUGUI dialogueDisplay;
    public TextMeshProUGUI placeholderDisplay;
    public TextMeshProUGUI activeDisplay;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI timeDisplay;

    public Queue<Dialogue> queueDialogue; // Shows the actual dialogue

    private int index;
    private int currentIndex;
    private float textSpeed = 0.01f;
    private bool activeType;
    private bool canEnter;
    private float endTimer = 0.5f;
    private bool canNext;
    private bool isEndDialogue;
    private bool isActive;
    [SerializeField] private float dialogueTime;
    [SerializeField] private bool dialogueTimeActive;
    [SerializeField] private Dialogue activeDialogue;
    private DialogueScene currentDialogueScene;


    void Start()
    {
        queueDialogue = new Queue<Dialogue>();
    }

    void Update()
    {
        DialogueController();
        UpdateDialogueTimer();
        UpdateEndDialogue();
    }

    // This IEnumerator is used to give the textDialogue  a typing effect.
    IEnumerator DialogueTyping()
    {
        DequeueDisplayText();

        foreach (char letter in activeDialogue.dialogueText.ToCharArray())
        {
            dialogueDisplay.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        canEnter = true;
        turnOnContinue();
    }

    // This method is used in order to display the placeholder text for the players to type in
    public void DisplayText()
    {
        DequeueDisplayText();

        foreach (char letter in activeDialogue.dialogueText.ToCharArray())
        {
            placeholderDisplay.text += letter;
        }
    }

    public void DequeueDisplayText()
    {
        canEnter = false;
        turnOffContinue();
        activeDialogue = queueDialogue.Dequeue();
        activeType = activeDialogue.canType;
        nameDisplay.text = activeDialogue.speakerName;
    }

    // This method is used in order to help the player control the dialogue of the game.
    public void DialogueController()
    {
        if (!activeType)
        {
            if (canEnter == true && Input.GetKeyDown(KeyCode.Space) && isActive)
            {
                ResponseHandler();
                NextDialogue();
            }
        }
        else
        {
            if (canEnter == true && Input.GetKeyDown(KeyCode.Space) && isActive)
            {
                // Trigger The Event Tag 
                TurnOffTimer();
                hideResponseHelpText();
                hideUserResponse();
                InsertNextDialogue(activeDialogue.branchNext);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && isActive)
            {
                upButtonDialogue();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && isActive)
            {
                downButtonDialogue();
            }

            char[] currentChar = activeDialogue.dialogueText.ToCharArray();


            if (canEnter == false && (currentChar[currentIndex] == ' ' || currentChar[currentIndex] == ',' || currentChar[currentIndex] == '.'))
            {
                moveNextChar();
            }

            foreach (char letter in Input.inputString)
            {
              UserInput(letter);
            }


            if (EndDialogue())
            {
                canEnter = true;
                turnOnContinue();
            }
            else
            {
                canEnter = false;
                turnOffContinue();
            }
        }
    }

    public void UserInput(char uInput)
    {
        string activeWord = activeDialogue.dialogueText;
        Debug.Log(uInput);
        foreach (char letter in activeWord.ToCharArray())
        {
            if (canEnter == false)
            {

                if (getLowerLetter() == uInput || getUpperLetter() == uInput)
                {
                    // Play typing sound
                    // Highlight letter and move onto the next letter
                    moveNextChar();
                    break;
                }
            }
        }
    }

    private void moveNextChar()
    {
        activeDisplay.text += getLetter();
        currentIndex++;
    }

    // This method is used to check for player lowercase input
    public char getLowerLetter()
    {
        string activeWord = activeDialogue.dialogueText.ToLower();

        char[] theChar = activeWord.ToCharArray();

        return theChar[currentIndex];
    }

    // This method is used to check for player uppercase input
    public char getUpperLetter()
    {
        string activeWord = activeDialogue.dialogueText.ToUpper();

        char[] theChar = activeWord.ToCharArray();

        return theChar[currentIndex];
    }

    // This method is used to get the letter at the current index that the user is 
    public string getLetter()
    {
        string activeWord = activeDialogue.dialogueText;

        char[] theChar = activeWord.ToCharArray();

        return theChar[currentIndex].ToString();
    }


    public void ResetTextBox()
    {
        currentIndex = 0;
        dialogueDisplay.text = "";
        placeholderDisplay.text = "";
        activeDisplay.text = "";
        nameDisplay.text = "";
    }

    // This method is used to start the dialogue system in the game.
    public void StartDialogue(DialogueScene startScene)
    {
        // Find the dialogue set corresponding with the scene
        DialogueSystem.theLocalGameManager.TurnOffPlayerMovement();
        currentDialogueScene = startScene;
        TurnOnDialogue();

        for(int i = 0; i < currentDialogueScene.sceneDialogue.Length; i++)
        {
            if(currentDialogueScene.sceneDialogue[i].branchNum == 0)
            {
                queueDialogue.Enqueue(currentDialogueScene.sceneDialogue[i]);
            }
        }

        NextDialogue();
    }

    /// <summary>
    /// This method is used to move onto the next dialogue in the array.
    /// </summary>
    public void NextDialogue()
    {
        if (queueDialogue.Count > 0)
        {
            ResetTextBox();

            if (queueDialogue.Peek().canType == true)
            {
                
                DisplayText();
            }
            else
            {
                StartCoroutine(DialogueTyping());
            }

        }
        else
        {
            DialogueSystem.theLocalGameManager.TurnOnPlayerMovement();
            ResetTextBox();
            EndOfDialogue();
            // Animate and hide dialogue box
        }
    }


    /// <summary>
    /// This method is used to help insert dialogue following a player's choice
    /// </summary>
    /// <param name="iBranchNum"></param>
    public void InsertNextDialogue(int iBranchNum)
    {
        queueDialogue.Clear();
        
        for(int i = 0; i < currentDialogueScene.sceneDialogue.Length; i++)
        {
            if(currentDialogueScene.sceneDialogue[i].branchNum == iBranchNum)
            {
                queueDialogue.Enqueue(currentDialogueScene.sceneDialogue[i]);
            }
        }

        NextDialogue();
    }

    public void ResponseHandler()
    {
        if (activeDialogue.dialogueResponse.Length > 0)
        {
            QuestHandler();
            showUserResponse();
            for (int i = 0; i < activeDialogue.dialogueResponse.Length; i++)
            {
                queueDialogue.Enqueue(activeDialogue.dialogueResponse[i]);
            }
            if(activeDialogue.typeTime >= 0)
            {
                TurnOnTimer();
            }
            else
            {
                showResponseHelpText();
            }
        }
    }

    /// <summary>
    /// This method is used to return whether the player is at the end of the dialogue string.
    /// </summary>
    /// <returns></returns>
    public bool EndDialogue()
    {
        bool endType = (currentIndex >= activeDialogue.dialogueText.Length);
        return endType;
    }

    public void upButtonDialogue()
    {
        for (int i = 0; i < queueDialogue.Count; i++)
        {
            queueDialogue.Enqueue(activeDialogue);
            NextDialogue();
        }
    }

    public void downButtonDialogue()
    {
        queueDialogue.Enqueue(activeDialogue);
        NextDialogue();
    }

    // This method is used to turn on the dialogue, showing the textbox
    private void TurnOnDialogue()
    {
        //textGroup.SetActive(true);
        dialogueCanvasGroup.alpha = 1;
        dialogueCanvasGroup.interactable = true;
        dialogueCanvasGroup.blocksRaycasts = true;
        isActive = true;
        DialogueSystem.theLocalGameManager.TurnOnDialogue();
    }

    // This method is used to turn off the dialogue, hiding the textbox
    private void TurnOffDialogue()
    {
        //textGroup.SetActive(false);
        dialogueCanvasGroup.alpha = 0;
        dialogueCanvasGroup.interactable = false;
        dialogueCanvasGroup.blocksRaycasts = false;
        isActive = false;
        GiveQuest.theGiveQuest.closeQuest();
        DialogueSystem.theLocalGameManager.TurnOffDialogue();
    }

    private void TurnOnTimer()
    {
        dialogueTime = activeDialogue.typeTime;
        dialogueTimeActive = true;
    }

    private void TurnOffTimer()
    {
        dialogueTime = activeDialogue.typeTime;
        dialogueTimeActive = false;
    }

    private void turnOnContinue()
    {
        continueCanvas.alpha = 1;
    }

    private void turnOffContinue()
    {
        continueCanvas.alpha = 0;
    }

    public bool GetActive()
    {
        return isActive;
    }

    private void EndOfDialogue()
    {
        isEndDialogue = true;
        endTimer = 0.5f;
    }

    private void QuestHandler()
    {
        if (activeDialogue.theQuest != null)
        {
            GiveQuest.theGiveQuest.setQuest(activeDialogue.theQuest);
            GiveQuest.theGiveQuest.openQuest();
        }
    }

    private void showUserResponse()
    {
        userResponseCanvas.alpha = 1;
        userResponseCanvas.interactable = true;
        userResponseCanvas.blocksRaycasts = true;
    }

    private void hideUserResponse()
    {
        userResponseCanvas.alpha = 0;
        userResponseCanvas.interactable = false;
        userResponseCanvas.blocksRaycasts = false;
    }

    private void showResponseHelpText()
    {
        timeDisplay.text = "Type your response!";
    }

    private void hideResponseHelpText()
    {
        timeDisplay.text = "";
    }
    
    private void UpdateEndDialogue()
    {
        if (isEndDialogue == true)
        {
            endTimer -= Time.deltaTime;
            if (endTimer <= 0)
            {
                GameEvent.theGameEvent.OnEndOfDialogue(currentDialogueScene, activeDialogue.branchNum);
                TurnOffDialogue();
                TurnOffTimer();
                hideUserResponse();
                isEndDialogue = false;
            }
        }
    }

    private void UpdateDialogueTimer()
    {
        if (dialogueTimeActive == true)
        {
            dialogueTime -= Time.deltaTime;
            int intTime = (int)dialogueTime + 1;
            if(intTime == 1)
            {
                timeDisplay.text = intTime.ToString() + " second, type your response!";
            }
            else
            {
                timeDisplay.text = intTime.ToString() + " seconds, type your response!";
            }

            if (dialogueTime <= 0)
            {
                TurnOffTimer();
                hideUserResponse();
                InsertNextDialogue(-1);
            }
        }
    }
}
