using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBarForegroundImage;

    public void UpdateHealthBar(HealthController hc)
    {
        _healthBarForegroundImage.fillAmount = hc.remHeath;
    }

    public void UpdateHealthBar(buildingHealth bh)
    {
        _healthBarForegroundImage.fillAmount = bh.remHeath;
    }
}