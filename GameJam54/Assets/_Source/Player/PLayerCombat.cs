using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerCombat : MonoBehaviour
{
    [SerializeField] private float _maxHp;
    [SerializeField] private float _playerDamage;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemylayer;
    [SerializeField] private float _timeBetwenAttacks;
    [SerializeField] private float _knockForce;
    [SerializeField] private float _manaRegen;

    private PLayerController _pLayerController;
    private Rigidbody2D rb;
    public Animator Animatior;
    private float _currentHp;
    private  float _actualTimeBetwenAttack;
    private bool _isAbilityActive = false;
    private void Start()
    {
        _currentHp = _maxHp;
        rb = gameObject.GetComponent<Rigidbody2D>();
        _pLayerController = gameObject.GetComponent<PLayerController>();
        _actualTimeBetwenAttack = _timeBetwenAttacks;
    }
    void Update()
    {
        _actualTimeBetwenAttack -= Time.deltaTime;
        if (_actualTimeBetwenAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                _actualTimeBetwenAttack = _timeBetwenAttacks;
            }
        }
       
    }

    private void Attack()
    {

        Collider2D[] _hitEnemys = Physics2D.OverlapCircleAll(_attackPoint.position,_attackRange,_enemylayer);
        
        for (int i = 0; i < _hitEnemys.Length; i++)
        {
            
            _hitEnemys[i].GetComponent<HealthSyst>().GetDamage(_playerDamage); 
            _hitEnemys[i].GetComponent<HealthSyst>().KnockingBack(gameObject.transform);
            _pLayerController.ManaRegen(_manaRegen);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)   
             return;
        
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
    public void KnockingBack(Transform enemy)
    {
        if (_isAbilityActive == false)
        {
            Vector2 direction = (transform.position - enemy.position).normalized;
            rb.AddForce(direction * _knockForce);
            rb.AddForce(Vector2.up * _knockForce);
        } 
    }
    public void ChangeAbolotyBool( bool ability)
    {
        _isAbilityActive = ability;
    }
    public void Heal(float healAmount)
    {
        _currentHp += healAmount;
    }
    public void GetDamage(float dmg)
    {
        if (_isAbilityActive == false)
        {
            if (_currentHp <= 0)
            {
                Die();
            }

            _currentHp -= dmg;
            Debug.Log($"Player hp = {_currentHp}");
        } 
        
    }

    private void Die()
    {

    }
}
