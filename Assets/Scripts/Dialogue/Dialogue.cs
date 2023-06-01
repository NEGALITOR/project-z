using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextAsset textFile;

    public int startLine;
    public int endLine;
    public float typingSpeed;

    public DialogueManager DM;

    // Start is called before the first frame update
    void Start()
    {
        DM = FindObjectOfType<DialogueManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //textBox.TypingSpeed();
    }

    //<---------------------NPC Collides with Player Tag and Initiates Dialogue with E----------------------->
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DM.typingSpeed = typingSpeed;
            DM.reloadDialogue(textFile);
            DM.index = startLine;
            DM.endLine = endLine;
            DM.isActive = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DM.isActive = false;
            DM.startTalk = false;
        }

    }

    public void PlayerName()
    {
        if (DM.startTalk)
        {
            DM.playerName.text = gameObject.name;
        }
    }
}
