using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContDown : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemysArray;
    [SerializeField] private GameObject WinScrean;
    [SerializeField] private TMP_Text _enemys;
    private int _enemysCount;
    private void Start()
    {
        _enemysCount = _enemysArray.Length;
    }
    private void Update()
    {
        _enemys.text = $"Enemies left: {_enemysCount}/{_enemysArray.Length} " ;
        if (_enemysCount <= 0)
        {
            WinScrean.SetActive(true);
        }
    }
    public void ChangeEnmeysAmount()
    {
        _enemysCount--;
    }
    
}
