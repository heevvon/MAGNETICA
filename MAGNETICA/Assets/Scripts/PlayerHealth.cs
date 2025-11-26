using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 5f;   // 5칸
    public float currentHealth = 5f;
    public float healAmount = 0.5f;   // 회복량
    public float healInterval = 10f;   // 10초마다 회복
    float healTimer = 0f;

    public System.Action<float> OnHealthChanged;  // UI 업데이트용

    void Start()
    {
        OnHealthChanged += UIManager.Instance.UpdateHeartUI;
    }

    void Update()
    {
        healTimer += Time.deltaTime;

        if (healTimer >= healInterval)
        {
            Heal(healAmount);
            healTimer = 0f;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    void Die()
    {
        SceneManager.LoadScene("GameOver");
    }
}
