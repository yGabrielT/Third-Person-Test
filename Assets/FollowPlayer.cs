using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Vector2 rawMouseInput;
    Vector2 mouseInput;
    Vector2 smoothMouseInput;
    public float smoothLerpMouseValue = 10f;
    public float TopClampValue = 80;
    public float BottomClampValue = 80;
    public float Sensivity = 1.0f;
    public Transform playerTransform;
    public float follorLerpSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, follorLerpSpeed * Time.deltaTime);
        RotateCamera();
    }

    void RotateCamera()
    {
        rawMouseInput = new Vector2(Input.GetAxisRaw("Mouse X") * Sensivity, Input.GetAxisRaw("Mouse Y") * Sensivity);
        
        smoothMouseInput = Vector2.Lerp(smoothMouseInput, rawMouseInput, Time.deltaTime * smoothLerpMouseValue);
        mouseInput += smoothMouseInput;
        mouseInput.y = Mathf.Clamp(mouseInput.y, TopClampValue, BottomClampValue);
        transform.rotation = Quaternion.Euler(-mouseInput.y, mouseInput.x, transform.rotation.eulerAngles.z);
    }
}
