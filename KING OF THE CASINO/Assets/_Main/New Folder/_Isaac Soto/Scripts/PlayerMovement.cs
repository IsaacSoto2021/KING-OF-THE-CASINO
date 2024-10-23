using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Sharkey, Logan
/// 12/7/2023
/// Handles Things pertaining to the core necesseties of the player, such as movement, and important variables
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    //All variables are here

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private bool recentlyDamaged = false;

    private Vector3 moveDirection;
    private Rigidbody rb;
    public int money = 0;
    public int health = 100;
    public int maxHealth = 100;

    public GameObject PistolRef;
    public GameObject RifleRef;
    public GameObject UzoRef;
    private int currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        PistolRef.SetActive(true);
        RifleRef.SetActive(false);
        UzoRef.SetActive(false);
        currentWeapon = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        ChangeWeapon();

        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;

    }

    /// <summary>
    /// Function for dev testing weapon swapping
    /// </summary>
    public void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwapWeapon(3);
        }
    }

    /// <summary>
    /// BTS code to let the dev swaap weapons for testing purposes
    /// </summary>
    /// <param name="weaponNum"></param>
    private void SwapWeapon(int weaponNum)
    {
        if (weaponNum != currentWeapon)
        {
            if (weaponNum == 1)
            {
                PistolRef.SetActive(true);
                RifleRef.SetActive(false);
                UzoRef.SetActive(false);
            }
            if (weaponNum == 2)
            {
                PistolRef.SetActive(false);
                RifleRef.SetActive(true);
                UzoRef.SetActive(false);
            }
            if (weaponNum == 3)
            {
                PistolRef.SetActive(false);
                RifleRef.SetActive(false);
                UzoRef.SetActive(true);
            }
            currentWeapon = weaponNum;
        }
    }

    public void ZombieKill()
    {
        money += 500;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Code for mouse movement and jumping functionalities
    /// </summary>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    /// <summary>
    /// Code for player input for character movement
    /// </summary>
    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //when grounded
        if (grounded) rb.AddForce(moveDirection.normalized *  moveSpeed * 10f, ForceMode.Force);
        //in air
        else if (!grounded) rb.AddForce(moveDirection.normalized * airMultiplier * 10f, ForceMode.Force);
    }

    /// <summary>
    /// Limits the amount the player accelerates
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    /// <summary>
    /// Physics for Jumping
    /// </summary>
    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        //When the player collides with door, it will check and take payment
        if (collision.gameObject.tag == "Door")
        {
            if ( money >= collision.gameObject.GetComponent<Doors>().Payment)
            {
                collision.gameObject.SetActive(false);
                money -= collision.gameObject.GetComponent<Doors>().Payment;
                Debug.Log("Door Paid");
            }
        }

        //When the player gets hit, they will have a half second cooldown before they can be hit again
        if (collision.gameObject.tag == "Subject" && !recentlyDamaged)
        {
            health -= 5;
            if (health <= 0) SceneManager.LoadScene(3);
            DamageInvulnerability();
        }

        //Seperaate endgame teleporter version of the door above
        if (collision.gameObject.tag == "Door2")
        {
            if (money >= 10000)
            {
                collision.gameObject.SetActive(false);
                money -= 10000;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Checks and takes payment in exhange for a substantial boost in health
        if (other.gameObject.tag == "StrengthRum" && money >= 1500)
        {
            money -= 1500;
            maxHealth = 200;
            health = maxHealth;
            other.gameObject.SetActive(false);
        }

        //Checks and takes payment in exhange for a small boost in Fire Rate
        if (other.gameObject.tag == "FastNine" && money >= 1500)
        {
            money -= 1500;
            PistolRef.GetComponent<GunHandling>().fireRate += 4;
            RifleRef.GetComponent<GunHandling>().fireRate += 4;
            UzoRef.GetComponent<GunHandling>().fireRate += 4;
            other.gameObject.SetActive(false);
        }

        //Checks and takes payment in exhange for a small boost in damage
        if (other.gameObject.tag == "Concussion" && money >= 1500)
        {
            money -= 1500;
            PistolRef.GetComponent<GunHandling>().damage += 2;
            RifleRef.GetComponent<GunHandling>().damage += 2;
            UzoRef.GetComponent<GunHandling>().damage += 2;
            other.gameObject.SetActive(false);
        }

        //Loads the Portal Home Scene
        
        if (other.gameObject.tag == "PortalHome")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(0);
           
        }

        //Loads the Portal Menu Scene
        if (other.gameObject.tag == "PortalMenu")
        {
            SceneManager.LoadScene(2);
            this.transform.position = new Vector3(340, 10, -4);
        }
    }
    */
    /// <summary>
    /// Prevents Player from dieing in seconds to continuous hits every frame
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageInvulnerability()
    {
        recentlyDamaged = true;
        yield return new WaitForSeconds(0.5f);
        recentlyDamaged = false;
    }
}
