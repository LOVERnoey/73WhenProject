using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visuals Cue")]
    [SerializeField] private GameObject visualCue;
    
    [Header("Dialogue")]
    [SerializeField] private TextAsset inkJSON;
    
    private bool isPlayerInRange;

    private void Awake()
    {
        visualCue.SetActive(false);
        isPlayerInRange = false;
    }
    
    private void Update()
    {
        if (isPlayerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }
}
