using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    RusherMovement _MovementScript;
    [SerializeField]
    ParticleSystem _Particle;

    public UnityEvent _HitSword;

    public static int _ScoreValue = 500;
    private void Start()
    {
        if(_MovementScript == null) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            
        }
        else if (other.CompareTag("Sword"))
        {
            _HitSword.Invoke();
            _MovementScript.SetModeRagdoll();
            StartCoroutine(DestroyThis());
            enabled = false;
        }
    }

    private IEnumerator DestroyThis()
    {
        _Particle.Play();
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(_Particle.main.startLifetimeMultiplier);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

        _HitSword.RemoveAllListeners();
    }
}
