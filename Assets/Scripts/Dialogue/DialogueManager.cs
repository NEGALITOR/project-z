using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //<---------------------Variables----------------------->
    public GameObject TextBox;
    public TextAsset textFile;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI textDisplay;
    public string[] line;
    public int index;
    public int prevIndex;
    public int endLine;

    public float typingSpeed;

    public bool isActive;
    public bool startTalk;

    public Dialogue dialogue;

    public Animator playerNameAnimator;
    public Animator dialogueAnimator;
    public Animator textBoxAnimator;

    private void Start()
    {
        //startTalk = false;
        isEndOfLine();
        dialogue = FindObjectOfType<Dialogue>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        nextLine();
        
        if (startTalk)
        {
            enableText();
            playerNameAnimator.SetBool("startTalk", true);
            dialogueAnimator.SetBool("startTalk", true);
            textBoxAnimator.SetBool("startTalk", true);
        }
        else
        {
            playerNameAnimator.SetBool("startTalk", false);
            dialogueAnimator.SetBool("startTalk", false);
            textBoxAnimator.SetBool("startTalk", false);
            disableText();
        }
    }

    //<---------------------Typing Letter by Letter----------------------->
    IEnumerator Type()
    {
        startTalk = true;
        if (isActive)
            foreach (char letter in line[index].ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
    }
//<---------------------Press E to Initiate and Continue Dialogue----------------------->
    public void nextLine()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive)
        {
            startTalk = true;
            textDisplay.text = "";
            StopAllCoroutines();;
            StartCoroutine(Type());
            dialogue.PlayerName();
            //TypingSpeed();
            index++;
        }

        if (index > endLine)
        {
            --index;
            startTalk = false;
        }
    }

    //<---------------------Enable and Disable the Dialogue----------------------->

    public void enableText()
    {
        TextBox.SetActive(true);
        playerName.gameObject.SetActive(true);
        textDisplay.gameObject.SetActive(true);
    }

    public void disableText()
    {
        TextBox.SetActive(false);
        playerName.gameObject.SetActive(false);
        textDisplay.gameObject.SetActive(false);
    }

    //<---------------------Checks and Restarts Dialogue from Starting Line----------------------->
    public void reloadDialogue(TextAsset textFile)
    {
        if (textFile != null)
        {
            line = new string[1];
            line = (textFile.text.Split('\n'));

        }
    }

    public void isEndOfLine()
    {
        if (textFile != null)
        {
            line = (textFile.text.Split('\n'));
        }

        if (endLine == 0)
        {
            endLine = line.Length + 1;

        }
    }

    /*public void TypingSpeed()
    {
        if (line[index].Contains("*"))
        {
            typingSpeed = 0f;
        }
        else
        {
            typingSpeed = 0.08f;
        }
    } */
}

/*
 * string sentence = "The dog had a bone, a ball, and other toys.";
char[] charsToTrim = {',', '.', ' '};
string[] words = sentence.Split();
foreach (string word in words)
   Console.WriteLine(word.TrimEnd(charsToTrim));
 */
