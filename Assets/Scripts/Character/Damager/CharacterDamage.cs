using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Character))]
public class CharacterDamage : MonoBehaviour
{
    [Serializable]
    public class CharacterDamageChangeEvent : UnityEvent<int> {}


    [SerializeField] 
    int baseDamage = 1;

    [SerializeField] //for debug
    int damageBonus = 0;

    [SerializeField] //for debug
    float damageMultiplier = 1;

    public int CurrentDamage { get{return currentDamage;}}
    [SerializeField] //for debug
    int currentDamage;


    public CharacterDamageChangeEvent onDamageChange;


    public void AddDamageBonus(int value)
    {
        damageBonus += value;
        UpdateCurrentDamage();
    }

    public void AddDamageMultiplier(float value)
    {
        damageMultiplier += value;
        UpdateCurrentDamage();
    }


    void UpdateCurrentDamage()
    {
       
        int totalDamageValue = baseDamage + damageBonus;
        if (totalDamageValue <= 0)
        {
            currentDamage = 0;
        }
        else if (damageMultiplier > 0)
        {
            currentDamage = Mathf.RoundToInt(totalDamageValue * damageMultiplier);
        }
        else
        {
            // From 0, -1, onwards we have 1/2, 1/4
            double multiplier = Math.Pow(2, damageMultiplier - 1);
            currentDamage = Mathf.RoundToInt(totalDamageValue * damageMultiplier);
        }

        onDamageChange.Invoke(currentDamage);
        
    }

}