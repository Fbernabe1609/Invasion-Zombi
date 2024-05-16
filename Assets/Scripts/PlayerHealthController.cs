using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBar;

    private int currentHealth;
    private PlayerMovementController movementController;
    private bool isInvincible = false;

    private void Start()
    {
        movementController = GetComponent<PlayerMovementController>();

        currentHealth = maxHealth;
        healthBar.fillAmount = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)

        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            healthBar.fillAmount = (float)currentHealth / maxHealth;
            movementController.PlayHitSound();

            if (currentHealth <= 0)
            {
                Die();
            }

            StartCoroutine(InvincibleCooldown());
        }
    }

    private void Die()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().RecordPlayingTime();
        SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator InvincibleCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }
}