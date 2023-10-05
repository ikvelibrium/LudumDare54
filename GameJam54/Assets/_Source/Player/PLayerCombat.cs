using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _hpBar;
    [SerializeField] private float _hpPlus;

    private bool _isHealing = false;
    private PlayerMover _playerMover;
    private Rigidbody2D rb;
    private float _currentHp;
    private  float _actualTimeBetwenAttack;
    private bool _isAbilityActive = false;
    private void Start()
    {
        _currentHp = _maxHp;
        rb = gameObject.GetComponent<Rigidbody2D>();
        _playerMover = gameObject.GetComponent<PlayerMover>();
        _actualTimeBetwenAttack = _timeBetwenAttacks;
    }
    void Update()
    {
        _hpBar.fillAmount = _currentHp / _maxHp;
        _actualTimeBetwenAttack -= Time.deltaTime;
        if (_actualTimeBetwenAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                _actualTimeBetwenAttack = _timeBetwenAttacks;
            }
        }
        if (_isAbilityActive == true && _currentHp <= _maxHp )
        {
            _currentHp += Time.deltaTime * _hpPlus;
        } else if(_currentHp > _maxHp)
        {
            _currentHp = _maxHp;
        }
       
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
        Collider2D[] _hitEnemys = Physics2D.OverlapCircleAll(_attackPoint.position,_attackRange,_enemylayer);
        
        for (int i = 0; i < _hitEnemys.Length; i++)
        {
            
            _hitEnemys[i].GetComponent<HealthSyst>().GetDamage(_playerDamage); 
            _hitEnemys[i].GetComponent<HealthSyst>().KnockingBack(gameObject.transform);
            _playerMover.ManaRegen(_manaRegen);
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
    
    public void GetDamage(float dmg)
    {
        if (_isAbilityActive == false)
        {
            if (_currentHp <= 0)
            {
                Die();
            }
            _animator.SetTrigger("GetDamage");
            _currentHp -= dmg;
            Debug.Log($"Player hp = {_currentHp}");
        } 
        
    }

    private void Die()
    {
        LvlManger.LoadLvl();
    }
}
