using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class carController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    //Car Controller
    public float acceleration = 1000f;
    public float reverseAcceleration = -1000f;
    public AudioSource carBreakSFX;

    public float breakingForce = 1000f;
    public float maxTurnAngle = 15f;

    public float currentAcceleration = 1000f;
    public float currentTurnAngle = 15f;

    //Random Control
    private bool mouseControl = false;
    public float minSwitchTime = 5f;
    public float maxSwitchTime = 10f;
    public float switchTime;

    //Flip Car
    public float flipDetect = 1f;
    public float flipSpeed = 1f;
    public float flipDelay = 2f;
    private bool isFlipping = false;

    //Variables for Hazard effects
    public float hazardSpeed = 200f;
    public bool hazardEffect = false;

    //Speed Boost
    public float speedSwitch = 10000f;


    //Score Count
    public int money = 10000;

    //Collision Handler, all collision based interactions will be handled here
    private void OnCollisionEnter(Collision collision)
    {
        // Breakable collisions and randomized value chance for each collision
        if (collision.gameObject.CompareTag("Breakable"))
        {
            int Value = Random.Range(0, 5);
            if (Value == 0)
            {
                money -= 1000;
                Debug.Log("You lose 1000 points");
            }
            else if (Value == 1)
            {
                money -= 2000;
                Debug.Log("You lose 2000 points");
            }
            else if (Value == 2)
            {
                money -= 3000;
                Debug.Log("You lose 3000 points");

            }
            else if (Value == 3)
            {
                money -= 4000;
                Debug.Log("You lose 4000 points");
            }
            else if (Value == 4)
            {
                money -= 5000;
                Debug.Log("You lose 5000 points");
            }
            else if (Value == 5)
            {
                money -= 6000;
                Debug.Log("You lose 6000 points");
            }

            Destroy(collision.gameObject);
        }

        //Hazard collision
        if (collision.gameObject.tag == "Hazard")
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(HazardSlow());

        }
    }


    private void Start()
    {
        setRandomTime();
    }

    private void FixedUpdate()
    {
        //reverse accelerate 
        if ( Input.GetAxisRaw("Vertical") > 0)
        {
            if (hazardEffect == false)
            {
                print("Go Forward");
                currentAcceleration = acceleration;
            }
            else
            {
                currentAcceleration = hazardSpeed;
            }

        }
        if (Input.GetAxisRaw("Vertical") < 0)
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

        //if the timer runs out, it will switch keybinds
        switchTime -= Time.deltaTime;
        if (switchTime <=0)
        {
            carBreakSFX.Play();
            print("Switcharoo!");
            mouseControl = !mouseControl;
            setRandomTime();
        }

        //apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        //apply sterring to front wheels
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        detectUpsideDown();

        //Loads win screen when player accomplishes objective
        if (money <= 0)
        {
            SceneManager.LoadScene(4);
        }
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
    private void setRandomTime()
    {
        switchTime = Random.Range(minSwitchTime, maxTurnAngle);
    }

    private IEnumerator HazardSlow()
    {
        hazardEffect = true;
        yield return new WaitForSeconds(4);
        hazardEffect = false;
    }

    public IEnumerator speedBoost()
    {
        Debug.Log("Boost On");
        currentAcceleration = speedSwitch;
        yield return new WaitForSeconds(1f);
        currentAcceleration = acceleration;
        Debug.Log("Boost Off");
    }

}
