using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class buildingHealth : MonoBehaviour
{
    [SerializeField] public float currHealth;
    [SerializeField] public float maxHealth;
    [SerializeField] private HealthBar healthBar;   // <-- Inspector se apni HB drag karni hai
    private Animator anim;
    public GameObject deathMenuUI;

    private void Start()
    {
        anim = GetComponent<Animator>();
        UpdateHealthUI();   // start pe bar full dikhani hai
    }

    public float remHeath
    {
        get
        {
            return currHealth / maxHealth;
        }
    }

    public UnityEvent OnDied;
    public UnityEvent OnDamage;
    public UnityEvent StopEverything;
    public bool IsInvincible { get; set; }

    public void TakeDamage(float damageAmount)
    {
        if (currHealth == 0)
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }

        currHealth -= damageAmount;
        anim.SetTrigger("hit");
        if (currHealth < 0)
        {
            currHealth = 0;
        }

        UpdateHealthUI();   // <-- har damage pe sprite/bar choti hogi

        if (currHealth == 0)
        {
            OnDied.Invoke();
            deathMenuUI.SetActive(true);
            StopEverything.Invoke();
            // GameOver();   // agar alag se game over logic chahiye to neeche dekho
        }
        else
        {
            OnDamage.Invoke();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(this);
        }
    }
}