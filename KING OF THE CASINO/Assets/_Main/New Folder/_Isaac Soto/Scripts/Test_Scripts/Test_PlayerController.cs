using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerController : MonoBehaviour
{
    public float movespeed = 5f;
    public float mouseSensitivity = 100f;
    public Transform Player;

    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limits vertical rotation

       Player.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movement
        float horizontal = Input.GetAxis("Horizontal");  // A, D or Left, Right
        float vertical = Input.GetAxis("Vertical");      // W, S or Up, Down

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        transform.Translate(move * movespeed * Time.deltaTime, Space.World);

    }
}
