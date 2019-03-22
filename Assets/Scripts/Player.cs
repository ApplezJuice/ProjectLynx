using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1;

    [SerializeField]
    private Transform[] _groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private float jumpForce;

    private bool facingRight;
    private bool isGrounded;
    private bool isJumping;

    private Rigidbody2D _rigidBody;
    private Animator _playerAnim;
    

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        // sets the component of the rigidbody to this variable
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        
        // looks at the input axis and returns a value between 0->1
        float horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
        Flip(horizontal);

        //ResetValues();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
    }

    private void ResetValues()
    {
        isJumping = false;
    }

    private void HandleMovement(float horizontal)
    {
        _rigidBody.velocity = new Vector2(horizontal * movementSpeed, _rigidBody.velocity.y);
        // need to clamp horizontal
        _playerAnim.SetFloat("movementSpeed", Mathf.Abs(horizontal));

        if (isGrounded && isJumping)
        {
            isGrounded = false;
            _rigidBody.AddForce(new Vector2(_rigidBody.velocity.x, jumpForce));
            isJumping = false;
        }

    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            Vector3 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
            // swap the bool
            facingRight = !facingRight;
        }
    }

    private bool CheckIfGrounded()
    {
        // if falling or on the ground
        if (_rigidBody.velocity.y <= 0)
        {
            // creates colliders around the feet
            foreach (Transform point in _groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i =0; i < colliders.Length; i++)
                {
                    // if the current collider isn't the player
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
