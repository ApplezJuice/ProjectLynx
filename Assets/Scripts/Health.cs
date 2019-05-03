using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    public float health = 100f;
    
    public void TakeDamage(float dmg)
    {
        health = Mathf.Max(health - dmg, 0);
        
    }

    public void HealSelf(float amnt)
    {
        if (health+amnt >= 100)
        {
            health = 100;
        }
        else
        {
            health += amnt;
        }
    }
}
