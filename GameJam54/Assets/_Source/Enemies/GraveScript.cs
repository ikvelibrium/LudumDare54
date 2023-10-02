using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveScript : MonoBehaviour
{
    [SerializeField] private float _spawnEnemyInSeconds;
    [SerializeField] GameObject _bigEnemyPref;

   
    void Update()
    {
        _spawnEnemyInSeconds -= Time.deltaTime;
        if (_spawnEnemyInSeconds <= 0)
        {
            Instantiate(_bigEnemyPref, gameObject.transform.position, _bigEnemyPref.transform.rotation);
            Destroy(gameObject);
        }
    }
}
