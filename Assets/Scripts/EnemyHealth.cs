using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float minHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private Image image;
    [Range(0f, 1f)]
    [SerializeField] private float potionChance;
    [SerializeField] private GameObject potion;
    [SerializeField] private GameObject deathParticle;
    private float health;
    private void Start()
    {
        health = Random.Range((int)minHealth, (int)maxHealth + 1);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateUI();
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        if (Random.Range(0f, 1f) <= potionChance)
        {
            Instantiate(potion, transform.position, Quaternion.identity);
        }
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void UpdateUI()
    {
        image.fillAmount = health / maxHealth;
    }
}
