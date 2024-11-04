using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public UnityEvent _HitPlayer;
    [SerializeField]
    float _GrowSpeed = 0.1f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Enemy"))
        {
            _HitPlayer.Invoke();
        }
    }

    public void _Grow()
    {
        transform.Find("Sword").localScale += Vector3.one * _GrowSpeed;
        Camera.main.orthographicSize += _GrowSpeed;
    }

    public void SetSize(float v)
    {
        transform.Find("Sword").localScale = Vector3.one;
        Camera.main.orthographicSize = 5f;
    }
}
