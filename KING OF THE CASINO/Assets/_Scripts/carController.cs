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
    public AudioSource carBreakSFX;

    public float breakingForce = 1000f;
    public float maxTurnAngle = 15f;

    public float currentAcceleration = 1000f;
    public float currentTurnAngle = 15f;

    //Car Jump
    public float jumpPower = 100000;
    public bool grounded;
    private Rigidbody rigidBody;

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
    public float hazardReverseSpeed = -200f;
    public bool hazardEffect = false;

    //Speed Boost
    public float speedSwitch = 10000f;

    //Image
    public GameObject Image;

    //Score Count
    public int PlayerMoney = 10000;
    public bool isPowerUpActive = false;

    //Collision Handler, all collision based interactions will be handled here
    private void OnCollisionEnter(Collision collision)
    {
        // Breakable collisions and randomized value chance for each collision
        if (collision.gameObject.CompareTag("Breakable"))
        {
            int Value = Random.Range(0, 5);
            if (Value == 0)
            {
                PlayerMoney -= 1000;
                Debug.Log("You lose 1000 points");
            }
            else if (Value == 1)
            {
                PlayerMoney -= 2000;
                Debug.Log("You lose 2000 points");
            }
            else if (Value == 2)
            {
                PlayerMoney -= 3000;
                Debug.Log("You lose 3000 points");

            }
            else if (Value == 3)
            {
                PlayerMoney -= 4000;
                Debug.Log("You lose 4000 points");
            }
            else if (Value == 4)
            {
                PlayerMoney -= 5000;
                Debug.Log("You lose 5000 points");
            }
            else if (Value == 5)
            {
                PlayerMoney -= 6000;
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
        rigidBody = GetComponent<Rigidbody>();
        grounded = true;
    }

    private void FixedUpdate()
    {
        //Acceleration Gear Switch
        if ( Input.GetAxisRaw("Vertical") > 0)
        {
            //If there are no hazard effects, acceleration is kept
            if (hazardEffect == false)
            {
                print("Go Forward");
                currentAcceleration = acceleration;
            }
            //else acceleration is hazard speed
            else
            {
                currentAcceleration = hazardSpeed;
            }

        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            //If there are no hazard effects, reverse acceleration is kept
            if (hazardEffect == false)
            {
                print("Go Backward");
                currentAcceleration = -acceleration;
            }
            //else reverse acceleration is hazard reverse speed
            else
            {
                currentAcceleration = -hazardSpeed;
            }
        }

        print("Current Acceleration: " + currentAcceleration);

        //If player press space and grounded is true, start coroutine
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            StartCoroutine(jumpBoost());
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
        if (PlayerMoney <= 0)
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

    public void AddPoints(int pointsToAdd)
    {
        PlayerMoney += pointsToAdd; // Add the points to the player's total
        Debug.Log("Player points: " + PlayerMoney);
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
        ShowImage();
        Debug.Log("Boost On");
        currentAcceleration = speedSwitch;
        yield return new WaitForSeconds(1f);
        currentAcceleration = acceleration;
        Debug.Log("Boost Off");
    }

    private void ShowImage()
    {
        if (Image != null)
        {
            print("Image Shown");
            Image.SetActive(true);
            StartCoroutine(HideImageAfterDelay(10f));
        }
    }

    private IEnumerator HideImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Image != null)
        {
            Image.SetActive(false);
        }
    }

    //Jumps and have five seconds cool down to jump again
    private IEnumerator jumpBoost()
    {
        Debug.Log("Jump go!");
        rigidBody.AddForce(Vector3.up * jumpPower);
        grounded = false;
        yield return new WaitForSeconds(5f);
        grounded = true;
    }
}