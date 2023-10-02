using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BigEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _gravePref;
    
    public void SpawnGrave()
    {
        Debug.Log("Vrode vse ok");
        Instantiate(_gravePref, gameObject.transform.position, Quaternion.identity);
    }
}

