using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Service;
using System;

public class Enemy : MonoBehaviour
{

    public float Damage;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _playerlayer;
    [SerializeField] private LayerMask _pSyst;
    [SerializeField] private float _timeBetwenAttacks;
    [SerializeField] private float _sightOfViewDistance;
    [SerializeField] private Transform _raycastStart;

    [SerializeField] private float _stopTrackingDistance;
    [SerializeField] private float _stayOnPointTime;

    private int _currentPatrolPoint = 0;
    private bool _playerFound = false;
    private bool _isCorutineCalled = false;
    private bool _isFaceRight = true;
    private float _actualTimeBetwenAttacks;
    private float _currentHp;
    private Transform _target;
    private Rigidbody2D rb;
    
    [SerializeField] private Transform[] _patrolpoints;

    private void Start()
    { 
        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }
    
    private void Update()
    {
       
        if (_playerFound == false)
        {
            DetectPlayer();
            
        }
       
        _actualTimeBetwenAttacks -= Time.deltaTime;
    }

    private void DetectPlayer()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(_raycastStart.position, _raycastStart.right, _sightOfViewDistance);
        if (_isFaceRight == true)
        {
            hitInfo = Physics2D.Raycast(_raycastStart.position, _raycastStart.right, _sightOfViewDistance);
        }
        else
        {
            hitInfo = Physics2D.Raycast(_raycastStart.position, -_raycastStart.right, _sightOfViewDistance);
        }

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

   
    

    public void Attack()
    {
        Collider2D[] _hitEnemys = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerlayer);
        Debug.Log("aaa");
        for (int i = 0; i < _hitEnemys.Length; i++)
        {

            _hitEnemys[i].GetComponent<PLayerCombat>().GetDamage(Damage);
            _hitEnemys[i].GetComponent<PLayerCombat>().KnockingBack(gameObject.transform);
        }
        _actualTimeBetwenAttacks = _timeBetwenAttacks;
    }

    
    
    
    
   
    
    
}
