using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamagable
{
    public PlayerStat health;    
    public PlayerStat stamina;
    public PlayerStat mana;

    public event Action OnPlayerDamaged;

    public void TakeDamage(float amount)
    {
        health.Subtract(amount);
        OnPlayerDamaged?.Invoke();
    }

}