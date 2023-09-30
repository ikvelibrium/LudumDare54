using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerCombat : MonoBehaviour
{
    [SerializeField] private float PlayerDamage;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemylayer;
    [SerializeField] private float _timeBetwenAttacks;
    
    

    public Animator Animatior;

    private  float _actualTimeBetwenAttack;
    private void Start()
    {
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
            
            _hitEnemys[i].GetComponent<Enemy>().GetDamage(PlayerDamage); 
            _hitEnemys[i].GetComponent<Enemy>().KnockingBack(gameObject.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)   
             return;
        
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
