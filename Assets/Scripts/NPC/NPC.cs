using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private bool isPlayerLooking;

    public void SetPlayerLooking(bool state)
    {
        isPlayerLooking = state;
        // ตัวอย่าง: เปิด UI สนทนา, แสดงชื่อ ฯลฯ
    }
}
