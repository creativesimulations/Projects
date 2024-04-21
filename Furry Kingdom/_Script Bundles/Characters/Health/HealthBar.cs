using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Camera _carmeraToLookAt;

    private void Awake()
    {
        _carmeraToLookAt = GameManager.Instance.Camera;
        GetComponent<Canvas>().worldCamera = _carmeraToLookAt;
    }

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
}
