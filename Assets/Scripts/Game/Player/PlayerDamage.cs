using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private float InDur;

    private InvinController ic;

    private void Awake()
    {
        ic = GetComponent<InvinController>();
    }

    public void StInvin()
    {
        ic.StInvin(InDur);
    }
}
