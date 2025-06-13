using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        //currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    private void Die()
    {
        // โหลดฉากใหม่ (เริ่มเกมใหม่)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}