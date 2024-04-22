using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f; 
    public float sprintSpeed = 12f;
    public float crouchSpeed = 2f;
    public float turnSpeed = 800;
    public float maxLookAngle = 90; 
    public float minLookAngle = -90; 
    public float gravityForce = 1; 
    public float jumpForce = 1;
    public float hyperJumpForce = 5;
    public float dashPower = 10f;
    public float dashCooldown = 3f;
    public float glideSpeed = 2f;
    public float groundPoundSpeed = 0.5f;
    public float groundPoundKnockback = 20f;
    public float groundPoundDemage = 10f;

    public GameObject myCamera;
    public Vector3 groundPoundHitbox;

    CharacterController controller;
    Vector3 currRot, movement;
    bool isCrouching = false;
    bool sliding = false;
    bool doubleJump = false;
    float speed;
    float speedMultiplyer = 1;
    float dashCooldownCheck = 3f;

    bool[] abylityUnlocks;
    // Start is called before the first frame update
    void Start()
    {
        //getting CharacterController
        controller = GetComponent<CharacterController>();

        //settin up cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //setting up nessesary variables
        currRot = new Vector3();
        speed = walkSpeed;

        //getting camera and setting it up to not render grass that if too far from player
        Camera camera = GetComponentInChildren<Camera>();
        float[] distances = new float[32];
        distances[7] = 50;
        camera.layerCullDistances = distances;

        //setting up SFX
        AudioManager.instance.Play("run");
        AudioManager.instance.Mute("run");
    }
    // Update is called once per frame
    void Update()
    {
        //if some menu is open, do nothing and return
        if (GameData.inMenu)
            return;
        // if player is sliding, loose some speed until start crouching
        if (sliding)
        {
            speed -= Time.deltaTime * 8;
            if(speed <= crouchSpeed)
            {
                speed = crouchSpeed;
                sliding = false;
            }
        }
        //check if crouch button is pressed
        if (Input.GetKeyDown(OptionsScript.keys[5]))
        {
            //shrink down the player
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
            controller.height = 1;
            isCrouching = true;

            //check if sprint button is pressed
            if (Input.GetKey(OptionsScript.keys[6]))
            {
                //start sliding
                speed += 5;
                sliding = true;
            }
            else
                //otherwise start crouching
                speed = crouchSpeed;
        }
        //if player stops crouching or sliding
        else if(Input.GetKeyUp(OptionsScript.keys[5]))
        {
            isCrouching = false;
            sliding = false;
            //scale up the player
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
            controller.height = 2;

            //check if sprint button is pressed
            if (Input.GetKey(OptionsScript.keys[6]))
                //set speed to sprinting speed
                speed = sprintSpeed;
            else
                //otherwise set speed to walking speed
                speed = walkSpeed;
        }
        //if the player is not crouching set speed acordimg to sprint input
        if (!isCrouching) {
            if (Input.GetKeyDown(OptionsScript.keys[6]))
                speed = sprintSpeed;
            else if (Input.GetKeyUp(OptionsScript.keys[6]))
                speed = walkSpeed; 
        }
        if (abylityUnlocks[0]) 
        {
            if (controller.isGrounded)
            {
                doubleJump = true;
            }
        }
        //if jump button gets pressed and the player is on ground, jump
        if (Input.GetKeyDown(OptionsScript.keys[4]))
        {
            if (isCrouching)
            {
                if(abylityUnlocks[2])
                    HyperJump();
            }
            else
            {
                Jump();
            }
        }
        //dash
        if (abylityUnlocks[1])
        {
            if (Input.GetKeyDown(OptionsScript.keys[13]))
                Dash();
        }
        //ground pound
        if (abylityUnlocks[4])
        {
            if (!controller.isGrounded)
            {
                if (Input.GetKeyDown(OptionsScript.keys[5]))
                    StartCoroutine(GroundPound());
            }
        }
        //gliding
        if (abylityUnlocks[3])
        {
            if (Input.GetKey(OptionsScript.keys[4]))
            {
                if (movement.y < -glideSpeed /100)
                    movement.y = -glideSpeed / 100;
            }
        }
        HandleMovement();
        HandleCamera();
    }
    //moves player acording to input and speed
    void HandleMovement()
    {
        //getting vertical input
        float vertical;
        if (Input.GetKey(OptionsScript.keys[0]))
            vertical = 1;
        else
            vertical = 0;

        if (Input.GetKey(OptionsScript.keys[1]))
            vertical += -1;
        //getting horizontal input
        float horizontal;
        if (Input.GetKey(OptionsScript.keys[2]))
            horizontal = -1;
        else
            horizontal = 0;

        if (Input.GetKey(OptionsScript.keys[3]))
            horizontal += 1;
        //moving the player
        movement = new Vector3(horizontal, movement.y - gravityForce * Time.deltaTime, vertical);
        movement.x *= Time.deltaTime * speed * speedMultiplyer;
        movement.z *= Time.deltaTime * speed * speedMultiplyer;
        movement = transform.right * movement.x + transform.forward * movement.z + transform.up * movement.y;
        controller.Move(movement);

        //playing run sfx
        AudioManager.instance.Mute("run");
        if (horizontal != 0)
            AudioManager.instance.Unmute("run");
        if(vertical != 0)
            AudioManager.instance.Unmute("run");
    }
    //rotates player acording to camera
    void HandleCamera()
    {
        //getting maouse position input
        currRot.x += Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        currRot.y += -Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
        currRot.x = Mathf.Repeat(currRot.x, 360);
        currRot.y = Mathf.Clamp(currRot.y, minLookAngle, maxLookAngle);
        //rotating the camera
        gameObject.transform.rotation = Quaternion.Euler(0, currRot.x, 0);
        myCamera.transform.rotation = Quaternion.Euler(currRot.y, currRot.x, 0);
    }
    //handle jumping
    public void Jump()
    {
        //normal jump
        if (controller.isGrounded)
        {
            movement.y = jumpForce / 100;
            AudioManager.instance.Play("jump");
        }
        //double-jump (if unlocked)
        else
        {
            if (doubleJump)
            {
                movement.y = jumpForce / 100;
                doubleJump = false;
                AudioManager.instance.Play("jump");
            }
        }
    }
    //Hyper jump
    public void HyperJump()
    {
        if (controller.isGrounded)
        {
            movement.y = hyperJumpForce / 100;
            AudioManager.instance.Play("hyperJump");
        }
    }
    //handle dashing
    public void Dash()
    {
        float checkTime = Time.time - dashCooldown;
        if (dashCooldownCheck > checkTime)
            return;
        else
        {
            dashCooldownCheck = Time.time;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 dirInput = new Vector2(horizontal, vertical);
            dirInput.Normalize();

            if (dirInput == Vector2.zero)
                dirInput.y = 1;

            Vector3 diraction = transform.right * dirInput.x + transform.forward * dirInput.y;
            Vector3 dashMovement = diraction * dashPower;

            controller.Move(dashMovement);
            AudioManager.instance.Play("dash");
        }
    }
    //handle GroundPound
    public IEnumerator GroundPound()
    {
        float startHeight = transform.position.y;
        movement.y = -groundPoundSpeed;
        yield return new WaitWhile(() => !controller.isGrounded);
        movement.y = 0;
        float distance = startHeight - transform.position.y;
        if (distance - transform.position.y > 5)
        {
            Vector3 center = transform.position;
            center.y -= 0.5f;
            Collider[] cols = Physics.OverlapBox(center, groundPoundHitbox / 2, transform.rotation);

            foreach (Collider col in cols)
            {
                //if its enemy, hit him
                if (col.CompareTag("Enemy") || col.CompareTag("Boss"))
                {
                    if (!col.isTrigger)
                    {
                        EnemyAi enemy = col.gameObject.GetComponent<EnemyAi>();
                        enemy.TakeDemage(groundPoundDemage * distance);
                        Vector3 dir = (enemy.transform.position - transform.position).normalized;
                        enemy.ApplyForce(dir * groundPoundKnockback * distance);
                    }
                }
            }
            AudioManager.instance.Play("groundPound");
        }
    }
    //update unlocked abylities so they can be used
    public void UpdateUnlocks()
    {
        abylityUnlocks = GameData.unlocks;
    }
    //gets a speed boost forgiven amount of time
    public IEnumerator BoostSpeed(int duration, float multipyer)
    {
        this.speedMultiplyer = multipyer;
        yield return new WaitForSeconds(duration);
        this.speedMultiplyer = 1;
    }
    //slows down time for given duration
    public IEnumerator Focus(float duration, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
}
