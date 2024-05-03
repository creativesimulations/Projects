using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Tooltip("Health bar to be updated.")]
    [SerializeField] private Image _healthBar;
    public Camera _carmeraToLookAt { get; private set; }

    private void Start()
    {
    }
    private void Update()
    {
        transform.LookAt(_carmeraToLookAt.transform.position);
    }

    /// <summary>
    /// Update health bar.
    /// </summary>
    /// <param name="currentHealth"></param> Current health amount.
    /// <param name="maxHealth"></param> Maximum health amount.
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        _healthBar.fillAmount = currentHealth / maxHealth;
    }

    /// <summary>
    /// Set the camera that will view the health bar.
    /// </summary>
    /// <param name="cameraToSet"></param> Camera to view the health bar.
    public void SetCamera(Camera cameraToSet)
    {
        // NEED TO IMPLEMENT A DIFFERENT APPROACH TO SHOW THE HEALTH BARS IN ALL CAMERAS IF THEY ARE IN VIEW.  ***
    }
}
