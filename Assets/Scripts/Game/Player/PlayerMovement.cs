using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PinePie.SimpleJoystick;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private JoystickController joystick;
    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 smoothMovement;
    private Vector2 smoothVelocity;
    private Animator anim;
    private bool isJoystickActive = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (joystick != null) joystick.OnTouchRemoved += HandleJoystickReleased;
    }

    private void OnDisable()
    {
        if (joystick != null) joystick.OnTouchRemoved -= HandleJoystickReleased;
    }

    private void HandleJoystickReleased()
    {
        isJoystickActive = false;
        _movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (joystick != null && joystick.InputDirection != Vector2.zero)
        {
            _movementInput = joystick.InputDirection;
            isJoystickActive = true;
        }

        smoothMovement = Vector2.SmoothDamp(smoothMovement, _movementInput, ref smoothVelocity, 0.1f);
        _rigidbody.linearVelocity = smoothMovement * speed;
        RotateInDirectionOfInput();
        SetAnimation();

        // ✅ ADDED — plays footstep sound when moving
        // AudioManager internally throttles this so it won't spam
        //if (_movementInput != Vector2.zero)
        //{
        //    AudioManager.Instance?.PlayWalk();
        //}
    }

    private void SetAnimation()
    {
        bool IsMoving = _movementInput != Vector2.zero;
        anim.SetBool("IsMoving", IsMoving);
    }

    private void OnMove(InputValue inputValue)
    {
        if (!isJoystickActive)
            _movementInput = inputValue.Get<Vector2>();
    }

    private void RotateInDirectionOfInput()
    {
        if (_movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, smoothMovement);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _rigidbody.MoveRotation(rotation);
        }
    }
}