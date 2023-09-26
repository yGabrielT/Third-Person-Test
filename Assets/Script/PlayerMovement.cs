using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerMovement : MonoBehaviour
{
    
    public float Velocity = 2f;
    public float LerpSpeed = .25f;
    public float PlayerRotateLerpValue = 5f;
    Vector3 _inputRefined;
    Vector2 _inputVec;
    CharacterController _char;
    public float gravity = 10f;
    public Transform springArm;

    private void Start()
    {
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

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 relativeinputy = inputy * forward;
        Vector3 relativeinputx = inputx * right;


        Vector3 relativeinput = relativeinputy + relativeinputx;


        _inputVec = new Vector2(relativeinput.x, relativeinput.z).normalized * Velocity;
        

        _inputRefined = Vector3.Lerp(_inputRefined, new Vector3(_inputVec.x, 0, _inputVec.y),  LerpSpeed * Time.deltaTime);
        _inputRefined.y = -gravity;

        Quaternion newRot = Quaternion.Euler(transform.rotation.x, springArm.localEulerAngles.y, transform.rotation.z);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * PlayerRotateLerpValue);



        _char.Move(_inputRefined * Time.deltaTime);
    }
}
