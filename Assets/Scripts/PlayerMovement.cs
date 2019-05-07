using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3;
    public float wallSitckTime = .25f;
    float timeToWallUnstick;
    int wallJumpMaxCount = 2;
    int currentWallJumpCount;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    public Texture2D cursorMain;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public bool isFacingRight;

    Controller2D controller;

    public Character myChar;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorMain, hotSpot, cursorMode);
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        myChar = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        //print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    private void Update()
    {

        // DEBUG TESTING
        if (Input.GetKeyDown(KeyCode.K))
        {
            myChar.TakeDamage(10);
            myChar.hpNeedsUpdateing = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            myChar.HealSelf(10);
            myChar.hpNeedsUpdateing = true;
        }
        // Mana
        if (Input.GetKeyDown(KeyCode.L))
        {
            myChar.UseMana(10);
            myChar.manaNeedsUpdating = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            myChar.GetMana(10);
            myChar.manaNeedsUpdating = true;
        }

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    StatModifier strMod = new StatModifier(10f, StatModType.Flat);
        //    myChar.Strength.AddModifier(strMod);
        //    uiManager.charSheetNeedsUpdating = true;
        //}
        // END DEBUG TESTING

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        isFacingRight = (velocity.x >= 0) ? true : false;

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocity.x = 0;
                velocityXSmoothing = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallSitckTime;
                }
            }
            else
            {
                timeToWallUnstick = wallSitckTime;
            }

        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding && currentWallJumpCount < wallJumpMaxCount)
            {
                currentWallJumpCount++;

                if (wallDirX == input.x)
                {
                    // trying to move in the same dir as the wall we are facing
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }else if (input.x == 0)
                {
                    // jumping off wall with no input
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    // input opp of wall dir
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                currentWallJumpCount = 0;
                velocity.y = jumpVelocity;
            }
        }


        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
