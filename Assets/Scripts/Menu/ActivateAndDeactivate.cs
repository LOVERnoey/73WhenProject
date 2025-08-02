using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAndDeactivate : MonoBehaviour
{
    [SerializeField] private GameObject objectsToActivate;
    [SerializeField] private GameObject objectsToDeactivate;
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(SimulateEscapePress());
    }   
    
    IEnumerator SimulateEscapePress()
    {
        yield return new WaitForSeconds(0.5f); // รอ 2 วินาที
        Activate(); // เรียกเหมือนกด ESC
        yield return new WaitForSeconds(0.2f); // รอ 2 วินาที       
        Deactivate();
    }
    // Update is called once per frame
    private void Activate()
    {
        objectsToActivate.SetActive(true);
    }
    private void Deactivate()
    {
        objectsToDeactivate.SetActive(false);
    }
}
