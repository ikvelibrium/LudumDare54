using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerController : MonoBehaviour
{
    public float Speed;
    public float JumpForce;


    [SerializeField] private float _mana;
    [SerializeField] private KeyCode _abilityKey;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject _playerBody;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _speedInSmokeMode;
    [SerializeField] private float _smokeManacostPerSecond;
    [SerializeField] private float _hpRegen; 
    [SerializeField] private float _slopeCheckDistance;

    private float _slopeDownAngle;
    private bool _isOnSloap;
    private float _actualSpeed;
    private float _slopeDownAngleOld;
    private float _actualMana;
    private bool _isAbilityActive = false;
    private float _horizontalInput;
    private bool _facingRight = true;
    private CapsuleCollider2D _cc;

    private Vector2 _normalPerep;
    private Vector2 _colliderSize;

    PLayerCombat _playerCombat;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        _cc = gameObject.GetComponent<CapsuleCollider2D>();
        _colliderSize = _cc.size;
        _particleSystem.Stop();
        _actualSpeed = Speed;
        _actualMana = _mana;
        _playerCombat = gameObject.GetComponent<PLayerCombat>();
    }
    void Update()
    {
       
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(_abilityKey) && _actualMana >= _mana && _isAbilityActive == false)
        {
            TurnIntoSmoke();
        } else if (Input.GetKeyDown(_abilityKey) && _isAbilityActive == true)
        {
            TurnOutOfSmoke();
        }
        if (_isAbilityActive == true)
        {
            if (_actualMana <= _smokeManacostPerSecond)
            {
                TurnOutOfSmoke();
            }
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce); 
            }
            _actualMana -= Time.deltaTime;
        }
        
        Flip();
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, _colliderSize.y / 2));
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

    }
    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(checkPos, Vector2.down,_slopeCheckDistance, groundLayer);
        if (_hit)
        {
            Debug.DrawRay(_hit.point, _hit.normal, Color.green);
            _normalPerep = Vector2.Perpendicular(_hit.normal);
            _slopeDownAngle = Vector2.Angle(_hit.normal, Vector2.up);
            Debug.DrawRay(_hit.point, _normalPerep, Color.red);
            if (_slopeDownAngle != _slopeDownAngleOld)
            {
                _isOnSloap = true;
            }
            _slopeDownAngleOld = _slopeDownAngle;
        }
    }
    public void ManaRegen(float mana)
    {
        if (_isAbilityActive == false && _actualMana <= _mana)
        {
            _actualMana += mana;
            if (_actualMana > _mana)
            {
                _actualMana = _mana;
            }
        } 
    
    }
    private void FixedUpdate()
    {
        SlopeCheck();
        if (groundCheck && !_isOnSloap)
        {

            rb.velocity = new Vector2(_horizontalInput * _actualSpeed, 0f);
        } else if (groundCheck && _isOnSloap)
        {
            rb.velocity = new Vector2(Speed * _normalPerep.x * _horizontalInput, Speed * _normalPerep.y * - _horizontalInput);
        } else if (!groundCheck)
        {
            rb.velocity = new Vector2(Speed * _horizontalInput, rb.velocity.y);
        }
        
        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (_facingRight && _horizontalInput < 0f || !_facingRight && _horizontalInput > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    } 
    private void TurnIntoSmoke()
    {
        _playerCombat.Heal(_hpRegen);
        _isAbilityActive = true;
        _playerBody.SetActive(false);
        _actualSpeed = _speedInSmokeMode;
        _particleSystem.Play();
        _playerCombat.ChangeAbolotyBool(_isAbilityActive);
    }
    private void TurnOutOfSmoke()
    {
        _isAbilityActive = false;
        _playerBody.SetActive(true);
        _actualSpeed = Speed;
        _particleSystem.Stop();
        _playerCombat.ChangeAbolotyBool(_isAbilityActive);
    }

    
    
}
