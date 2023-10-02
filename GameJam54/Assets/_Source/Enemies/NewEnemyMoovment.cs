using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NewEnemyMoovment : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _activateDistance = 50f;
    [SerializeField] private float _pathUpdateSeconds = 0.5f;


    [SerializeField] private float _speed = 2f, _jumpForce = 5f;
    [SerializeField] private  float _nextWaypointDistance = 3f;
    [SerializeField] private  float _jumpNodeHeightRequirement = 0.8f;
    [SerializeField] private float _jumpModifier = 0.3f;
    [SerializeField] private  float _jumpCheckOffset = 0.1f;


    [SerializeField] private bool _followEnabled = true;
    [SerializeField] private  bool _jumpEnabled = true, _isJumping, _isInAir;
    [SerializeField] private bool _directionLookEnabled = true;
    [SerializeField] private float _stayOnPointTime;

    [SerializeField] private Vector3 startOffset;

    private Path _path;
    private int _currentWaypoint = 0;
    [SerializeField] public RaycastHit2D _isGrounded;
    Seeker _seeker;
    Rigidbody2D _rb;
    private bool _isOnCoolDown;
    private bool _isCorutineCalled = false;
    private int _currentPatrolPoint = 1;

    [SerializeField] private Transform[] _patrolPoints;

    public void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _isJumping = false;
        _isInAir = false;
        _isOnCoolDown = false;

        InvokeRepeating("UpdatePath", 0f, _pathUpdateSeconds);
    }

    private void Update()
    {
        Patrol();
        if (TargetInDistance() && _followEnabled)
        {
            
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (_followEnabled && TargetInDistance() && _seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, _target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (_path == null)
        {
            return;
        }

        // Reached end of path
        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            StartCoroutine(Wait());
            _isCorutineCalled = true;
            return;
        }

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + _jumpCheckOffset, transform.position.z);
        _isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * _speed;

        
        if (_jumpEnabled && _isGrounded && !_isInAir && !_isOnCoolDown)
        {
            if (direction.y > _jumpNodeHeightRequirement)
            {
                if (_isInAir) return;
                _isJumping = true;
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                StartCoroutine(JumpCoolDown());

            }
        }
        if (_isGrounded)
        {
            _isJumping = false;
            _isInAir = false;
        }
        else
        {
            _isInAir = true;
        }

        // Movement
        _rb.velocity = new Vector2(force.x, _rb.velocity.y);

      
        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if (distance < _nextWaypointDistance)
        {
            _currentWaypoint++;
        }

        // Direction Graphics Handling
        if (_directionLookEnabled)
        {
            if (_rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (_rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, _target.transform.position) < _activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        _isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        _isOnCoolDown = false;
    }

    private void Patrol()
    {
        if (transform.position != _patrolPoints[_currentPatrolPoint].position)
        {
            
            _target = _patrolPoints[_currentPatrolPoint];
        }
        else
        {
            if (_isCorutineCalled == false)
            {
               
                StartCoroutine(Wait());
                _isCorutineCalled = true;
            }

        }
    }
    private IEnumerator Wait()
    {
        Debug.Log("asdasd");
        yield return new WaitForSeconds(_stayOnPointTime);
        if (_currentPatrolPoint + 1 < _patrolPoints.Length)
        {
            _currentPatrolPoint++;
        }
        else
        {
            _currentPatrolPoint = 0;
        }
        _isCorutineCalled = false;
    }
}
