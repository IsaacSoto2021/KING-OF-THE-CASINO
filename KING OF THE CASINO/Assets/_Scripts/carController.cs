using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class carController : MonoBehaviour
{

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;
    //Car Controller
    public float acceleration = 1000f;
    public float carSpeed = 1000f;
    public float maxSpeed = 1000f;

    public AudioSource carBreakSFX;

    public float breakingForce = 1000f;
    public float currentBreakingForce = 0f;
    public float maxTurnAngle = 15f;

    public float currentAcceleration = 0f;
    public float currentTurnAngle = 15f;

    public bool goBack;

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

    //Variables for Ghost Ability
    public bool GhostActive = false;
    public int GhostDuration = 10;
    public GameObject carBody;
    public bool GhostCooldown = false;

    //Game reference for Objective UI
    public GameObject objectives;

    //
    [SerializeField] Text scoreCounter;

    //Speed Boost
    public float doubleAcceleration = 10000f;
    public float doubleSpeed = 10000f;
    public float doubleMax = 10000f;

    //Image
    public GameObject Image;

    //Score Count
    public int PlayerMoney = 0;
    public bool isPowerUpActive = false;

    //Image
    public GameObject GhostImage;
    public GameObject JumpImage;


    private void OnTriggerEnter(Collider other)
    {
        /*if (gameObject.CompareTag ("Breakable"))
        {
            PlayerMoney -= 25000;
            Destroy(other.gameObject);
            Debug.Log("Hit");
        }*/
    }

    //Collision Handler, all collision based interactions will be handled here
    private void OnCollisionEnter(Collision collision)
    {
        // Breakable collisions and randomized value chance for each collision
        if (collision.gameObject.CompareTag("Breakable"))
        {
            PlayerMoney -= 25000;

            Destroy(collision.gameObject);
        }

        //Hazard collision
        if (collision.gameObject.tag == "Hazard")
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(HazardSlow());

        }

        //Obstacle Collision
        if (collision.gameObject.tag == "Obstacle")
        {
            collision.gameObject.SetActive(false);
        }


        //Enemy Collision, checks if collision is with enemy, then checks if ghost mode is active and either ends game or has a pass through
        if (collision.gameObject.tag == "Enemy")
        {
            if (GhostActive == false)
            {
                SceneManager.LoadScene(3);
                Cursor.lockState = CursorLockMode.None;

            }
            else if (GhostActive == true)
            {
               // Debug.Log("Enemy Pass Through");
            }
        }


    }

    private void Start()
    {
        //setRandomTime();
        rigidBody = GetComponent<Rigidbody>();
        grounded = true;
        StartCoroutine(ObjectivesVisi());
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody.centerOfMass = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        rigidBody.AddForce(transform.forward * currentAcceleration);
        //Updates Score UI
        scoreCounter.text = "Money: " + PlayerMoney.ToString();

        //Acceleration Gear Switch
        if (Input.GetKeyDown(KeyCode.W))
        {
            goBack = !goBack;
        }

        if (!goBack)
        {
            //If there are no hazard effects, acceleration is kept
            if (hazardEffect == false)
            {
               // print("Go Forward");
                currentAcceleration = Mathf.MoveTowards(currentAcceleration, acceleration, carSpeed * Time.deltaTime);
            }
            //else acceleration is hazard speed
            else
            {
                currentAcceleration = Mathf.MoveTowards(currentAcceleration, hazardSpeed, carSpeed * Time.deltaTime);
            }
        }
        else
        {
            //If there are no hazard effects, reverse acceleration is kept
            if (hazardEffect == false)
            {
              //  print("Go Backward");
                currentAcceleration = Mathf.MoveTowards(currentAcceleration, -acceleration, carSpeed * Time.deltaTime);
            }
            //else reverse acceleration is hazard reverse speed
            else
            {
                currentAcceleration = Mathf.MoveTowards(currentAcceleration, hazardSpeed, carSpeed * Time.deltaTime);
            }
        }

        // Limit the speed
        float currentSpeed = rigidBody.velocity.magnitude;
        if (currentSpeed > maxSpeed)
        {
            currentAcceleration = 0f; // Stop accelerating if max speed is reached
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
           // print("Mouse Time!");
            mouseButtons();
        }
        else
        {
           // print("Keyboard Time!");
            keyboardButtons();
        }

        //if the timer runs out, it will switch keybinds
        /*switchTime -= Time.deltaTime;
        if (switchTime <=0)
        {
            carBreakSFX.Play();
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

        //apply breakforce to four wheels
        frontLeft.brakeTorque = currentBreakingForce;
        frontRight.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;

        detectUpsideDown();

        //Loads win screen when player accomplishes objective
        if (PlayerMoney <= 0)
        {
            SceneManager.LoadScene(4);
            Cursor.lockState = CursorLockMode.None;
        }

        //Activates ghost mode on button press
        if (Input.GetKeyDown(KeyCode.R) && GhostCooldown == false)
        {
            StartCoroutine(GhostMode());
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

    public void AddPoints(int amount)
    {
        PlayerMoney += amount;  // Add or subtract points (based on the amount)
        UpdateUI();
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
        currentAcceleration = doubleAcceleration;
        carSpeed = doubleSpeed;
        maxSpeed = doubleMax;
        yield return new WaitForSeconds(5f);
        currentAcceleration = acceleration;
        carSpeed = 1000f;
        maxSpeed = 1000f;
        Debug.Log("Boost Off");
    }

    private void ShowImage()
    {
        if (Image != null)
        {
            print("Image Shown");
            Image.SetActive(true);
            StartCoroutine(HideImageAfterDelay(5f));
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

        JumpImage.SetActive(false);
        grounded = false;
        yield return new WaitForSeconds(5f);
        grounded = true;
        JumpImage.SetActive(true);
    }

    //Coroutine for the ghost ability. Sets ghost mode true for short period, dictating what happens in OnCollisionEnter
    private IEnumerator GhostMode()
    {
        Debug.Log("Ghost Mode is Active)");
        GhostActive = true;
        carBody.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(10);
        GhostActive = false;
        carBody.GetComponent<Renderer>().material.color = Color.green;
        GhostCooldown = true;

        GhostImage.SetActive(false);
        yield return new WaitForSeconds(5);
        GhostCooldown = false;
        GhostImage.SetActive(true);

    }

    //Coroutine for objective ui, setting it so its only present on screen for 10 seconds.
    private IEnumerator ObjectivesVisi()
    {
        objectives.active = true;
        yield return new WaitForSeconds(10);
        objectives.active = false;

    }
    private void UpdateUI()
    {
        scoreCounter.text = "Money: " + PlayerMoney.ToString(); // Update UI text with PlayerMoney value
    }
}
