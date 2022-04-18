using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EnemyStatistics : MonoBehaviour
{
    [SerializeField] private int healthLevel = 10;
    [SerializeField] private Animator animator;
    
    private int _maxHealth;
    private int _currentHealth;
    
    private void Start()
    {
        _currentHealth = SetMaxHealthFromHealthLevel();
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = healthLevel * 10;
        return _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        animator.Play("TakeDamage01");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            animator.Play("Death01");
        }
    }
}
