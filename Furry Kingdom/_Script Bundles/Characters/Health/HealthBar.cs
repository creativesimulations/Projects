using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    public Camera _carmeraToLookAt {get; private set;}

    private void Start()
    {
    }
    private void Update()
    {
        transform.LookAt(_carmeraToLookAt.transform.position);
    }
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        _healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void SetCamera(Camera cameraToSet)
    {
        
    }
}
