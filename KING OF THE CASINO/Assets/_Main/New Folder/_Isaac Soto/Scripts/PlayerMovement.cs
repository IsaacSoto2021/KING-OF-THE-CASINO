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
