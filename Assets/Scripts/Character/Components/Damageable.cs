using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [Serializable]
    public class HealthSetEvent : UnityEvent<Damageable>
    { }

    [Serializable]
    public class HealEvent : UnityEvent<int, Damageable>
    { }

    [Serializable]
    public class OnHitEvent : UnityEvent<object, Damageable>
    { }

    [Serializable]
    public class DeathEvent : UnityEvent<Damageable>
    { }


    [Serializable]
    public class AddedImmunityEvent : UnityEvent<Damageable>
    { }

    [Serializable]
    public class EndedImmunityEvent : UnityEvent<Damageable>
    { }

    public float onHitImmunityTime = 2f;

    
    [SerializeField] int baseHp = 6;
    [SerializeField] int baseHpBonus = 0;
    [SerializeField] int currentBaseHp = 6;

    public int CurrentHealth {get {return currentHp;}}
    [SerializeField] protected int currentHp = 6;


    [NonSerialized]
    public bool isDead = false;

    // Track different sources of immunity
    private HashSet<object> _immunitySources = new();

    
    public HealthSetEvent onHealthSet;
    public HealEvent onHeal;
    public OnHitEvent onHit;
    public DeathEvent onDeath;
    public AddedImmunityEvent addedImmunity;
    public EndedImmunityEvent endedImmunity; 


    void Start()
    {
        currentHp = baseHp;
        RecalculateBaseHealth();
    }

    public void IncreaseBaseHealthBonus(int value)
    {
        baseHpBonus += value;
        currentHp += value;
        RecalculateBaseHealth();
    }

    public void RecalculateBaseHealth()
    {
        int newBaseHp = baseHp + baseHpBonus;

        if (newBaseHp <= 0)
        {
            currentBaseHp = 1;        
        }

        currentBaseHp = newBaseHp;
    }

    
    // Health setters
    public void ResetHealth()
    {
        currentHp = currentBaseHp;
    }

    
    // Methods for handling immunity
    public bool IsImmune()
    {
        return _immunitySources.Count > 0;
    } 

    IEnumerator OnHitImmunity(object damager)
    {
        AddImmunity(damager);
        
        yield return new WaitForSeconds(onHitImmunityTime);

        RemoveImmunity(damager);
    }

    public void AddImmunity(object source)
    {
        if (source == null)
        {
            Debug.LogError("Immunity source cannot be null!");
            return;
        }

        _immunitySources.Add(source);
        addedImmunity.Invoke(this);
    }

    public void RemoveImmunity(object source)
    {
        if (source == null)
        {
            Debug.LogError("Immunity source cannot be null!");
            return;
        }

        if (_immunitySources.Contains(source))
        {
            _immunitySources.Remove(source);

            // Removed last stack of immunity
            if (!IsImmune()) endedImmunity.Invoke(this); 
        }
    }

    public void ClearAllImmunity()
    {
        _immunitySources.Clear();
    }


    // Main TakeDamage (calle from Damager scripts)
    public void TakeDamage(object damagerObject, int damage) 
    {
        if (!IsImmune())
        {
            currentHp -= damage;
            onHit.Invoke(damagerObject, this);

            FindObjectOfType<GameplayUI>().UpdateHearts();

            if (currentHp <= 0)
            {
                Death();
            }

            // Use object as unique hash identifier
            StartCoroutine(OnHitImmunity(damagerObject));
        }
        
    }

    public void Heal(int value)
    {
        currentHp = Math.Min(baseHp, currentHp + value);
        onHeal.Invoke(value, this);

        FindObjectOfType<GameplayUI>().UpdateHearts();
    }

    public void Death()
    {
        isDead = true;
        onDeath.Invoke(this);
        AddImmunity(this);
    }


}