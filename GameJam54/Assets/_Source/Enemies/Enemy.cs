using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHp;
    public float Damage;
    public bool KnockFromRight;

    [SerializeField] private float _knockForce;
    [SerializeField] private float _timeInKnock;
    [SerializeField] private float _totalTimeInKnock;
    private float _currentHp;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        _currentHp = MaxHp;
    }

    private void Update()
    {
        if (_timeInKnock <= 0)
        {

        } 
    }

    public void GetDamage(float dmg)
    {
        if (_currentHp <= 0)
        {  
            Die();
        }

        KnockingBack();
        _currentHp -= dmg;
        Debug.Log($"Enemy hp = {_currentHp}");
    }

    public void KnockingBack()
    {
        if (KnockFromRight == true)
        {
            rb.velocity = new Vector2(-_knockForce, _knockForce);
        }
        else
        {
            rb.velocity = new Vector2(_knockForce, _knockForce);
        }
    }
    private void Die()
    {

        Debug.Log($"Enemy dead");
        Destroy(gameObject);
    }
}
