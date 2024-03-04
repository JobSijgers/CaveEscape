using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    public void UpdateHealthUI(float newHealth, float maxHealth)
    {
        healthBar.fillAmount = newHealth / maxHealth;   
    }
}
