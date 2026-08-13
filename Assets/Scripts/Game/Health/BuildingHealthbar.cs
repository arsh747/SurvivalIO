using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealthbar : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBarForegroundImage;
    public void UpdateHealthBar (buildingHealth hc)
    {
        _healthBarForegroundImage.fillAmount = hc.remHeath;
    }
}
