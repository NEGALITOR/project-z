using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DT : MonoBehaviour
{

    public D dialogue;
    public DM dialogueM;

    public void TriggerDialogue()
    {
        FindObjectOfType<DM>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueM.isActive = true;

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueM.isActive = false;
        }

    }
}