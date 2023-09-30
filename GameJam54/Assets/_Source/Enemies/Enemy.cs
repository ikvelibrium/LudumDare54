using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Service;
using System;

public class Enemy : MonoBehaviour
{
    public float MaxHp;
    public float Damage;

    [SerializeField] private float _knockForce;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _playerlayer;
    [SerializeField] private float _timeBetwenAttacks;
    [SerializeField] private float _sightOfViewDistance;
    [SerializeField] private Transform _raycastStart;
    [SerializeField] private float _speed;
    
    private bool _playerFound = false;
    private float _actualTimeBetwenAttacks;
    private float _currentHp;
    private Transform _target;
    private Rigidbody2D rb;
    [SerializeField] private Transform[] _patrolpoints;

    private void Start()
    {
        
        rb = gameObject.GetComponent<Rigidbody2D>();
        _currentHp = MaxHp;
        _actualTimeBetwenAttacks = _timeBetwenAttacks;
        
    }
    
    private void Update()
    {
        if (_playerFound == false)
        {
            DetectPlayer();
        } else
        {
            FollowPlayer(_target);
        }
       
       
        _actualTimeBetwenAttacks -= Time.deltaTime;
    }

    private void DetectPlayer()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(_raycastStart.position, _raycastStart.right, _sightOfViewDistance);

        if (hitInfo.collider != null)
        {
            Debug.DrawLine(_raycastStart.position, hitInfo.point, Color.red);
            if (LayerChecker.CheckLayersEquality(hitInfo.collider.gameObject.layer, _playerlayer))
            {
                _target = hitInfo.collider.transform;
                _playerFound = true;
            }
        }
        else
        {
            Debug.DrawLine(_raycastStart.position, _raycastStart.position + _raycastStart.right * _sightOfViewDistance, Color.green);
        }
    }

    private void FollowPlayer(Transform target)
    {
        if (Vector2.Distance(transform.position,target.position ) >= _attackRange)
        {
            //Debug.Log(Vector2.Distance(transform.position, target.position));
            transform.position = Vector2.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        }
        else
        {
            if (_actualTimeBetwenAttacks <= 0)
            {
                Attack();
                
            }
            
        }
        
    }
    

    public void Attack()
    {
        Collider2D[] _hitEnemys = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerlayer);

        for (int i = 0; i < _hitEnemys.Length; i++)
        {

            _hitEnemys[i].GetComponent<PLayerCombat>().GetDamage(Damage);
            _hitEnemys[i].GetComponent<PLayerCombat>().KnockingBack(gameObject.transform);
        }
        _actualTimeBetwenAttacks = _timeBetwenAttacks;
    }
    public void GetDamage(float dmg)
    {
        if (_currentHp <= 0)
        {  
            Die();
        }

        _currentHp -= dmg;
        Debug.Log($"Enemy hp = {_currentHp}");
    }

    public void KnockingBack(Transform player)
    {
        Vector2 direction = (transform.position - player.position).normalized;
        rb.AddForce(direction * _knockForce);
        rb.AddForce(Vector2.up * _knockForce);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
