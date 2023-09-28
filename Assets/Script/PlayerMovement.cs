using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerMovement : MonoBehaviour
{
    
    public float Velocity = 2f;
    public float SprintSpeed = 12f;
    public float LerpSpeed = .25f;
    public float PlayerRotateLerpValue = 5f;
    Vector3 _inputRefined;
    Vector2 _inputVec;
    CharacterController _char;
    public float speed;
    public float gravity = 10f;
    public Animator _anim;
    public Transform springArm;
    private Quaternion lastRot;
    private bool stoppedMoving;

    private float baseVelocity;

    private void Start()
    {
        baseVelocity = Velocity;
        _char = GetComponent<CharacterController>();
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        float inputx = Input.GetAxisRaw("Horizontal");
        float inputy = Input.GetAxisRaw("Vertical");
        bool inputSprint = Input.GetKey(KeyCode.LeftShift);
        if (inputSprint)
        {
            Velocity = SprintSpeed;
        }
        else
        {
            Velocity = baseVelocity;
        }
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 relativeinputy = inputy * forward;
        Vector3 relativeinputx = inputx * right;
        
        _anim.SetFloat("Speed", speed);

        Vector3 relativeinput = relativeinputy + relativeinputx;

        
        _inputVec = new Vector2(relativeinput.x, relativeinput.z).normalized * Velocity;
        
        float speedMag = (new Vector2(inputy, inputx).normalized).magnitude;
        speed = Mathf.Lerp(speed, speedMag * Velocity, LerpSpeed * Time.deltaTime);

        _inputRefined = Vector3.Lerp(_inputRefined, new Vector3(_inputVec.x, 0, _inputVec.y),  LerpSpeed * Time.deltaTime);
        _inputRefined.y = -gravity;

        Quaternion newRot = Quaternion.Euler(transform.rotation.x, springArm.localEulerAngles.y, transform.rotation.z);
        
        if (_inputVec != Vector2.zero)
        {
            stoppedMoving = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativeinput, Vector3.up), Time.deltaTime * PlayerRotateLerpValue);
            lastRot = transform.rotation;


        }
        else
        {
            if (stoppedMoving)
            {
                stoppedMoving = false;
                transform.rotation = Quaternion.Slerp(transform.rotation, lastRot, Time.deltaTime * PlayerRotateLerpValue);
            }
            
        }
        



        _char.Move(_inputRefined * Time.deltaTime);
    }
}
