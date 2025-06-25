using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUiTrigger : MonoBehaviour
{
    public GameObject npcInteractUi;
    // Start is called before the first frame update
    void Awake()
    {
        npcInteractUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            npcInteractUi.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            npcInteractUi.SetActive(false);
        }
    }
}
