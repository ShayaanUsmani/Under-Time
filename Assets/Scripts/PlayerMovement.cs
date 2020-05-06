using UnityEngine;

// Base player controller script
// Controls the player, allowing them to move in the (x,z) plane and jump. 
public class PlayerMovement : MonoBehaviour
{
    private float baseSpeed { get; set; }

    [SerializeField]
    private float moveSpeed { get; set; }

    [SerializeField]
    private float jumpSpeed { get; set; }

    public float yVel;
    public float downVel = 10f;


    private Rigidbody rb;
    private CapsuleCollider col;
    private float colOriginalHeight;

    private string groundTag = "Ground";
    public bool isGrounded;
    private bool isCrouching;

    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    private void Start()
    {
        baseSpeed = 10f;
        moveSpeed = baseSpeed;
        jumpSpeed = 35f;
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<CapsuleCollider>();
        colOriginalHeight = col.height;
    }

 
    private void Update()
    {
        MoveOnInput();
        JumpOnInput();
        CrouchOnInput();
        
    }

    // based on input in x and z dimensions, move the rb in that direction.
    private void MoveOnInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        rb.velocity = move*moveSpeed;
    }

    // makes the player jump upwards on input and decends smoothly with same velocity
    // as ascend (acceleration is 0 on both)
    private void JumpOnInput()
    {
        if (isGrounded)
        {
            yVel = -downVel * Time.deltaTime;
            if (Input.GetKeyDown(jumpKey))
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                if (isCrouching)
                {
                    UnCrouch();
                }
            }
        }
        else
        {
            yVel -= downVel * Time.deltaTime;
        }

        rb.AddForce(Vector3.up * yVel, ForceMode.VelocityChange);
    }

    // Check if player is colliding with the ground, if so, they are grounded
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    // Check if player stopped colliding with the ground, if so, they are not grounded
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == groundTag)
        {
            isGrounded = false;
        }
    }
    
    // crouch if holding down crouch key
    private void CrouchOnInput()
    {
        if(Input.GetKey(crouchKey))
        {
            Crouch();
        }
        else
        {
            UnCrouch();
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        col.height = colOriginalHeight/2;
    }

    private void UnCrouch()
    {
        col.height = colOriginalHeight;
        isCrouching = false;
    }
}
