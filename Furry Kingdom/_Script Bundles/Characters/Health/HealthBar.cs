using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image _healthBar;
    private void Awake()
    {
        _healthBar = GetComponent<Image>();
    }
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        _healthBar.fillAmount = currentHealth / maxHealth;
    }
}
