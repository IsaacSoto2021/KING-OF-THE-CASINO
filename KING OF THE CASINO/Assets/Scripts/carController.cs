using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    //Car Controller
    public float acceleration = 700f;
    public float reverseAcceleration = -700f;

    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    private float currentAcceleration = 700f;
    public float currentTurnAngle = 15f;

    //Random Control
    private bool mouseControl = true;
    public float minSwitchTime = 5f;
    public float maxSwitchTime = 10f;
    public float switchTime;

    //Flip Car
    public float flipDetect = 0.5f;
    public float flipSpeed = 1f;
    public float flipDelay = 2f;
    private bool isFlipping = false;

    private void Start()
    {
        //setRandomTime();
    }

    private void FixedUpdate()
    {
        //reverse accelerate 
        if ( Input.GetAxisRaw("Vertical") > 0)
        {
            print("Go Forward");
            currentAcceleration = acceleration;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            print("Go Backward");
            currentAcceleration = reverseAcceleration;
        }


            //If mouse control is true, the player will use the mouse, else they use the keyboard
            if (mouseControl)
        {
            print("Mouse Time!");
            mouseButtons();
        }
        else
        {
            print("Keyboard Time!");
            keyboardButtons();
        }

        /*//if the timer runs out, it will switch keybinds
        switchTime -= Time.deltaTime;
        if (switchTime <=0)
        {
            print("Switcharoo!");
            mouseControl = !mouseControl;
            setRandomTime();
        }*/

        //apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        //apply sterring to front wheels
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        detectUpsideDown();

    }

    private void keyboardButtons()
    {
        //take care of steering
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
    }

    private void mouseButtons()
    {
        //currentTurnAngle = maxTurnAngle * Input.GetAxis("Mouse X");

        currentTurnAngle = 0f;

        //Left Button = Left Turn
        if (Input.GetMouseButton(0))
        {
            currentTurnAngle = -maxTurnAngle;
        }
        //Right Button = Right Turn
        else if (Input.GetMouseButton(1))
        {
            currentTurnAngle = maxTurnAngle;
        }
    }

    //detects if the car is upside down
    private void detectUpsideDown()
    {
        if (transform.up.y < flipDetect)
        {
            //if the car is upside down, the car flips
            StartCoroutine(flipsCar());
        }
    }

    //Flips the car
    IEnumerator flipsCar()
    {
        isFlipping = true;

        //delays the car for a few seconds
        yield return new WaitForSeconds(flipDelay);

        //flips the car to its normal position
        Quaternion rotateUpright = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateUpright, Time.deltaTime * flipSpeed);

        isFlipping = false;
    }

    //Switch between countdown from 5 to 10 seconds.
    /*private void setRandomTime()
    {
        switchTime = Random.Range(minSwitchTime, maxTurnAngle);
    }*/
}
