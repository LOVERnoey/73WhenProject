using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject menu;
    public AudioClip healSoundFX;

    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        //currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void Update()
    {
        if (menu.activeSelf)
        {
            healthText.text = "";
        }
        else
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        SoundFXManager.instance.PlaySoundFXClip(healSoundFX, transform, 1f);
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