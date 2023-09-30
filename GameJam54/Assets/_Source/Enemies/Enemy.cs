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
    public AIPath AiPath;
    

    [SerializeField] private float _knockForce;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _playerlayer;
    [SerializeField] private float _timeBetwenAttacks;

    [SerializeField] private Transform[] _patrolpoints;
    [SerializeField] private Transform _target;
    [SerializeField] private float _nextWayPointDistance;
    [SerializeField] private float _sightOfViewDistance;
    [SerializeField] private Transform _raycastStart;
    [SerializeField] private float _speed;
    [SerializeField] private AIDestinationSetter _setter;
    private Path _path;
    private Seeker _seeker;
    private int _currentWaypoint;
    private bool _reachedEndOfPath = true;
    

    private float _actualTimeBetwenAttacks;
    private float _currentHp;
    private Rigidbody2D rb; 
    

    private void Start()
    {
        _seeker = gameObject.GetComponent<Seeker>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        _currentHp = MaxHp;
        _actualTimeBetwenAttacks = _timeBetwenAttacks;

    }
    
    private void Update()
    {
        if (_reachedEndOfPath == true)
        {
            DetectPlayer();
        } else
        {
            Moove();
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
                GeneratePath(hitInfo.collider.gameObject.transform);
            }
        }
        else
        {
            Debug.DrawLine(_raycastStart.position, _raycastStart.position + _raycastStart.right * _sightOfViewDistance, Color.green);
        }
    }

    private void GeneratePath(Transform target)
    {
        _seeker.StartPath(rb.position, target.position, OnPathComplete);
        
      
        _reachedEndOfPath = false;
            
           
    }

    private void OnPathComplete(Path p)
    {
       if(!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    private void Moove()
    {
        
        if (_path == null)
            return;
        
        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            
            _reachedEndOfPath = true;
            return;
        }
        
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * _speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < _nextWayPointDistance)
        {
            _currentWaypoint++; 
        }
        
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

        Debug.Log($"Enemy dead");
        Destroy(gameObject);
    }
}
