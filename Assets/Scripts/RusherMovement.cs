using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RusherMovement : MonoBehaviour
{
    #region StateMachine

    delegate void RusherMovementDelegate();
    RusherMovementDelegate _Action;
    public void SetModeVoid()
    {
        _Action = DoVoid;
    }
    private void DoVoid() { }
    public void SetModeSeek(bool resetAction = true)
    {
        if (resetAction) { _Action = null; }
        _Action -= DoSeek;
        _Action += DoSeek;
    }
    [SerializeField]
    float _MaxSpeed = 10f;
    [SerializeField]
    float _MinSpeed = 1f;
    [SerializeField]
    float _SpeedModifier = 5f;
    [SerializeField]
    float _SlowDownDistance = 1f;
    float _Speed = 0f;
    [SerializeField]
    float _MaxRotationSpeed = 60f;

    private Vector3 _Direction;
    Transform _Target;
    private void DoSeek()
    {
        if (_Target != null)
        {
            Vector2 _TargetDirection = (_Target.position - transform.position).normalized;
            float angle = Mathf.Clamp(Vector2.SignedAngle(_Direction, _TargetDirection), -_MaxRotationSpeed, _MaxRotationSpeed);
            transform.Rotate(Vector3.forward, angle * Time.deltaTime);
            _Direction = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
        }
        if (Vector2.Distance(_Target.transform.position, transform.position) < _SlowDownDistance)
        {
            _Speed = Mathf.Max(_MinSpeed, _Speed - _SpeedModifier * Time.deltaTime);
        }
        else
        {
            _Speed = Mathf.Min(_MaxSpeed, _Speed + _SpeedModifier * Time.deltaTime);
        }
        transform.position += _Speed * Time.deltaTime * _Direction;
    }
    public void SetModeRagdoll(bool resetAction = true)
    {
        if (resetAction) { _Action = null; }
        _Action -= DoRagdoll;
        _Action += DoRagdoll;
    }
    private void DoRagdoll()
    {

    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
       _Target =  FindFirstObjectByType<Player>().GetComponent<Transform>();
       _Direction = Vector2.right;
       _Speed = _MinSpeed;
        SetModeSeek();
    }

    // Update is called once per frame
    void Update()
    {
        _Action();
    }
   


}
