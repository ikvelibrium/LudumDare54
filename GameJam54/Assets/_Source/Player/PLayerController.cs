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

    private float _actualSpeed;
    private float _actualMana;
    private bool _isAbilityActive = false;
    private float _horizontalInput;
    private bool _facingRight = true;

    PLayerCombat _playerCombat;

    private void Start()
    {
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
        rb.velocity = new Vector2(_horizontalInput * _actualSpeed, rb.velocity.y);
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
