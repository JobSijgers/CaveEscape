using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEngine.ParticleSystem;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private GameObject healParticle;
    private int health;

    private void Start()
    {
        health = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (health <= 0)
            return;
        health -= damage;
        healthUI.UpdateHealthUI(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        GameManager.instance.LostGame();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pot"))
        {
            health += 2;
            healthUI.UpdateHealthUI(health, maxHealth);
            GameObject particle = Instantiate(healParticle, collision.transform.position, Quaternion.identity);
            particle.transform.parent = null;
            Destroy(collision.gameObject);
        }
    }
}
