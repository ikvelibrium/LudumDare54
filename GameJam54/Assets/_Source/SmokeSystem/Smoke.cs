using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Service;

public class Smoke : MonoBehaviour
{
   
    public List<ParticleCollisionEvent> collisionEvents;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private LayerMask _smokeCheackerLayer;
    [SerializeField] private GameObject _checker;

    private Collision2D _collision;
    
    void Start()
    {
        _particleSystem = gameObject.GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    private void OnParticleTrigger( )
    {
        
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i] ; 
            Instantiate(_checker, p.position, Quaternion.identity);
            enter[i] = p;
        }
        _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
       
      
    }

}




