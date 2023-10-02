using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Service;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _damage;

    private PLayerCombat _playerCombat;
    private Rigidbody2D _rb;
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.AddForce(_rb.transform.right * _speed);
        Destroy(gameObject, 10);
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerChecker.CheckLayersEquality(collision.gameObject.layer, _playerLayer))
        {
            _playerCombat = collision.gameObject.GetComponent<PLayerCombat>();
            _playerCombat.GetDamage(_damage);
        }
        Destroy(gameObject);
    }
}
