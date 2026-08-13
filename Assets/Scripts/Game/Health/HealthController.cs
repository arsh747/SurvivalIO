using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] public float currHealth;
    [SerializeField] public float maxHealth;
    private Animator anim;

    // jse REMOVED — AudioManager handles hurt/death sounds now

    public GameObject deathMenuUI;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public float remHeath => currHealth / maxHealth;

    public UnityEvent OnDied;
    public UnityEvent OnDamage;
    public UnityEvent OnHealthChange;
    public UnityEvent StopEverything;
    public bool IsInvincible { get; set; }

    public void TakeDamage(float damageAmount)
    {
        if (currHealth == 0) return;
        if (IsInvincible) return;

        currHealth -= damageAmount;
        OnHealthChange.Invoke();

        // ✅ ADDED — plays hurt SFX through AudioManager
        AudioManager.Instance?.PlayPlayerHurt();

        anim.SetTrigger("Hit");

        if (currHealth < 0) currHealth = 0;

        if (currHealth == 0)
        {
            // ✅ ADDED — plays death SFX through AudioManager
            AudioManager.Instance?.PlayPlayerDeath();

            OnDied.Invoke();
            anim.SetTrigger("death");
            deathMenuUI.SetActive(true);
            StopEverything.Invoke();
        }
        else
        {
            OnDamage.Invoke();
        }
    }

    public void AddHealth(float amountToAdd)
    {
        if (currHealth == maxHealth) return;
        currHealth += amountToAdd;
        OnHealthChange.Invoke();
        if (currHealth > maxHealth) currHealth = maxHealth;
    }
}