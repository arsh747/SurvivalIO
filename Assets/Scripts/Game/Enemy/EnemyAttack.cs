using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _damageAmount;

    // Throttle attack sound — OnCollisionStay2D fires every physics frame
    // without this you'd get hundreds of attack sounds per second
    float nextAttackSoundTime = 0f;
    const float attackSoundInterval = 0.6f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        var healthController = collision.gameObject.GetComponent<HealthController>();
        var hcBuilding = collision.gameObject.GetComponent<buildingHealth>();

        if (healthController != null)
        {
            healthController.TakeDamage(_damageAmount);

            // ✅ ADDED — throttled so attack sound isn't spammed every frame
            if (Time.time >= nextAttackSoundTime)
            {
                AudioManager.Instance?.PlayZombieAttack();
                nextAttackSoundTime = Time.time + attackSoundInterval;
            }
        }

        if (hcBuilding != null)
        {
            hcBuilding.TakeDamage(_damageAmount);

            if (Time.time >= nextAttackSoundTime)
            {
                AudioManager.Instance?.PlayZombieAttack();
                nextAttackSoundTime = Time.time + attackSoundInterval;
            }
        }
    }
}