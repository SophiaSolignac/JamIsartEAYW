using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class SwordMovement : MonoBehaviour
{
    [SerializeField]
    Transform _Center;
    private Vector3 mouseWorldPos;
    float distFromCenter = 0.1f;
    private Vector3 oldPosition;
    private Vector3 _Direction;

    public Vector3 newPosition { get; private set; }

    private Vector3 translatio;
    private float _CurrentRotationSpeed = 0f;
    [SerializeField]
    private float _AngularAcceleration = 2f; 
    [SerializeField]
    private float _AngularReversedAcceleration = 20f;
    [SerializeField]
    float _MaxRotationSpeed = 60f;


    // Start is called before the first frame update
    void Start()
    {
        distFromCenter = (transform.position - _Center.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector2 _TargetDirection = (mouseWorldPos - transform.position).normalized;
        float angle = Vector2.SignedAngle(_Direction, _TargetDirection);

        HandleRotation(angle);

        transform.Rotate(Vector3.forward, _CurrentRotationSpeed * Time.deltaTime);

        //oldPosition = transform.position;
        _Direction = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
        
        transform.position = _Center.position + _Direction * distFromCenter;

        //newPosition = (_Center.position + _Direction * distFromCenter);
        //translatio = newPosition - oldPosition;
        //if (translatio.magnitude > .10f)
        //{
        //    print(translatio);
        //    return;
        //}
        //transform.Translate(((_Center.position + _Direction * distFromCenter ) - oldPosition));
    }

    private void HandleRotation(float angle)
    {
        float speedNextStep = _CurrentRotationSpeed + _AngularAcceleration * Time.deltaTime * MathF.Sign(angle);
        if (MathF.Sign(angle) != Mathf.Sign(_CurrentRotationSpeed))
        {
            speedNextStep += _AngularReversedAcceleration * Time.deltaTime * MathF.Sign(angle) ;
        }
        _CurrentRotationSpeed = Mathf.Clamp(speedNextStep, -_MaxRotationSpeed, _MaxRotationSpeed);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(oldPosition,.1f);
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(newPosition, .1f);
        //Gizmos.DrawLine(oldPosition, newPosition);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(translatio,.1f);
    }
}
