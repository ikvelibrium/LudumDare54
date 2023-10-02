using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Service;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _playerlayer;
    [SerializeField] private LayerMask _pSyst;
    [SerializeField] private float _timeBetwenAttacks;
    [SerializeField] private float _sightOfViewDistance;
    [SerializeField] private Transform _raycastStart;
    [SerializeField] private float _speed;
    [SerializeField] private float _stopTrackingDistance;
    [SerializeField] private GameObject _projectile;

    private bool _playerFound = false;
    private float _actualTimeBetwenAttacks;
    private Transform _target;
    private Rigidbody2D rb;
    private bool _isFaceRight = true;
    private float oldX;
    [SerializeField] private Transform[] _patrolpoints;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        _actualTimeBetwenAttacks = _timeBetwenAttacks;
        oldX = transform.position.x;
    }

    private void Update()
    {
        if (_playerFound == false)
        {
            DetectPlayer();
        }
        else
        {
            FollowPlayer(_target);
        }
        if (transform.position.x < oldX) _isFaceRight = false;
        else _isFaceRight = true;

        oldX = transform.position.x;
        _actualTimeBetwenAttacks -= Time.deltaTime;
        
    }
    

    private void DetectPlayer()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(_raycastStart.position, _raycastStart.right, _sightOfViewDistance);
        if (_isFaceRight == true)
        {
             hitInfo = Physics2D.Raycast(_raycastStart.position, _raycastStart.right, _sightOfViewDistance);
        } else
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

    private void FollowPlayer(Transform target)
    {
        if (Vector2.Distance(transform.position, target.position) <= _attackRange )
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, -_speed * Time.deltaTime);
        }   
        else if (Vector2.Distance(transform.position, target.position) <= _stopTrackingDistance)
        {
            _playerFound = false;
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

        Instantiate(_projectile, _attackPoint.position, Quaternion.identity);
        _actualTimeBetwenAttacks = _timeBetwenAttacks;
    }

    private void Flip()
    {
        if (_isFaceRight == true)
        {
            
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            _isFaceRight = false;
        }
    }
  
}
