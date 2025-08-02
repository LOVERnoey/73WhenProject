using UnityEngine;
using TMPro;

public class CountTest : MonoBehaviour, IDataPersistence
{
    public TextMeshProUGUI countText; // ลิงก์กับ TextMeshPro
    private int count = 0;

    void Start()
    {
        UpdateCountText(); // แสดงค่าเริ่มต้น
    }

    public void LoadData(GameData data)
    {
        this.count = data.countTest;
    }
    
    public void SaveData(GameData data)
    {
        data.countTest = this.count;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            count++;
            UpdateCountText();
        }
    }

    void UpdateCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}