using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinController : MonoBehaviour
{
    private HealthController hc;

    private void Awake()
    {
        hc = GetComponent<HealthController>();
    }

    public void StInvin(float InDur)
    {
        StartCoroutine(InvinCoroutine(InDur));
    }

    private IEnumerator InvinCoroutine( float InDur)
    {
        hc.IsInvincible = true;
        yield return new WaitForSeconds(InDur);
        hc.IsInvincible = false; 
    }
}
