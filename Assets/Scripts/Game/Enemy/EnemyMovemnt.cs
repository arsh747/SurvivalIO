using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovemnt : MonoBehaviour
{

    [SerializeField]private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private AudioSource jse;
    private float dirchnage;

    private Rigidbody2D _rigidbody;
    private PAC _pac;
    private Vector2 _targetDirection;
    // Start is called before the first frame update
    private void Awake()
    {
    _rigidbody = GetComponent<Rigidbody2D>();
    _pac = GetComponent <PAC>();
    _targetDirection = transform.up;
    jse.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
    }

    private void HandleRandomDirectionChange()
    {
        dirchnage -= Time.deltaTime;
        if (dirchnage <= 0)
        {
            float angleChange = Random.Range(-90f, 90f); 
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward); 
            _targetDirection = rotation * _targetDirection;
            dirchnage = Random.Range(1f, 5f);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (_pac.AwareOfPlayer)
        {
            _targetDirection = _pac.DirectionToPlayer;
        }
    }

    private void RotateTowardsTarget()
    {
       
        Quaternion targetRotation = Quaternion.LookRotation (transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
       
        
        _rigidbody.linearVelocity = transform.up * _speed;
        
    }
}
