using UnityEngine;
using UnityEngine.UI;

public class LookAtItem : MonoBehaviour
{
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private GameObject fPromptUI; // UI ปุ่ม F เช่น Text “Press F to pick up”

    private Item currentItem;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Item item = hit.collider.GetComponent<Item>();

            if (item != null)
            {
                fPromptUI.SetActive(true);
                item.SetPlayerLooking(true);
                currentItem = item;
            }
            else
            {
                HidePrompt();
            }
        }
        else
        {
            HidePrompt();
        }
    }

    void HidePrompt()
    {
        if (currentItem != null)
        {
            currentItem.SetPlayerLooking(false);
            currentItem = null;
        }

        fPromptUI.SetActive(false);
    }
}