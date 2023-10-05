using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Service;

public class HealthSyst : MonoBehaviour
{
    [SerializeField] float _maxHp;
    [SerializeField] private float _knockForce;
    [SerializeField] private float _knockUpForce;
    [SerializeField] private LayerMask _smokeLayer;
    [SerializeField] private float _smokeDamage;
    [SerializeField] private float _smokeDamageKD;
    [SerializeField] private float _totalSmokeDamageTime;

    private ContDown _contDown;
    private BigEnemy _bigEnemy;
    private float _actualSmokeKd;
    private float _actualTotalSmokeDamageTime;
    private bool _isGetingDamageFromSmoke = false;
    private bool _isWating;
    private float _currentHp;
    private Rigidbody2D rb;
    private void Start()
    {
        _contDown = GameObject.FindGameObjectWithTag("Counter").GetComponent<ContDown>();
        gameObject.TryGetComponent<BigEnemy>(out _bigEnemy);
        _currentHp = _maxHp;
        rb = gameObject.GetComponent<Rigidbody2D>();
        _actualSmokeKd = 0;
        _actualTotalSmokeDamageTime = _totalSmokeDamageTime;
    }
    private void Update()
    {
        if (_isGetingDamageFromSmoke == true)
        {
            GetDamageWithSmoke();
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
        rb.AddForce(Vector2.up * _knockUpForce);
    }
    private void Die()
    {

        if (_bigEnemy != null)
        {
            _bigEnemy.SpawnGrave();
        }
        _contDown.ChangeEnmeysAmount();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerChecker.CheckLayersEquality(collision.gameObject.layer, _smokeLayer))
        {
            
            if (_isGetingDamageFromSmoke == false)
            {
                GetDamageWithSmoke();
                Debug.Log("d");
            }
           
            Destroy(collision.gameObject);
        }
    }

    private void GetDamageWithSmoke()
    {
        _isGetingDamageFromSmoke = true;
         _actualTotalSmokeDamageTime -= Time.deltaTime;
        _actualSmokeKd -= Time.deltaTime;
        if (_actualTotalSmokeDamageTime >= 0)
        {
            
            
            if (_actualSmokeKd <= 0)
            {
               
                GetDamage(_smokeDamage);
                _actualSmokeKd = _smokeDamageKD;
            }
        } else
        {
           
            _isGetingDamageFromSmoke = false;
            _actualTotalSmokeDamageTime = _totalSmokeDamageTime;
        }
      
    }
   
}
