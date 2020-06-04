using UnityEngine;

// Base player controller script
// Controls the player, allowing them to move in the (x,z) plane and jump. 
public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed;
    public float moveSpeed;
    public float runSpeedBonus;
    public float crouchSpeedReduction;
    public bool canRun;

    private float crouchHeightDivider = 2f;
    private float jumpSpeed;


    private Rigidbody rb;
    private CapsuleCollider col;
    private float colOriginalHeight;

    public bool isGrounded;
    private bool isCrouching;

    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    private void Awake()
    {
        canRun = true;
        isGrounded = true;
        baseSpeed = 45f;
        runSpeedBonus = 10f;
        crouchSpeedReduction = 10f;
        moveSpeed = baseSpeed;
        jumpSpeed = 65f;
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

        if (Input.GetKey(runKey) && canRun)
        {
            moveSpeed = baseSpeed + runSpeedBonus;
        }
        else
        {
            moveSpeed = baseSpeed;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        rb.velocity = move * moveSpeed * Time.unscaledDeltaTime;

    }

    // makes the player jump upwards on input and decends smoothly with same velocity
    // as ascend (acceleration is 0 on both)
    // (since OnCollisionExit might not be accurate 100% of the time, causing inconsistent jumps,
    // we set isGrounded to false after jumping but still keep OnCollisionExit to prevent jumping
    // after walking off a ledge).
    private void JumpOnInput()
    {
            if (Input.GetKeyDown(jumpKey) && isGrounded)
            {
                if (isCrouching)
                {
                    UnCrouch();
                }
                rb.AddForce(Vector3.up * jumpSpeed * Time.unscaledDeltaTime, ForceMode.Impulse);
                isGrounded = false;
            Debug.Log("took off");
            }
        
    }
    
    // crouch if holding down crouch key
    private void CrouchOnInput()
    {
        if(Input.GetKey(crouchKey) && !isCrouching && canRun)
        {
            Crouch();
        }
        else if (Input.GetKeyUp(crouchKey) && isCrouching)
        {
            UnCrouch();
        }
    }

    private void Crouch()
    {
        canRun = false;
        baseSpeed -= crouchSpeedReduction;
        isCrouching = true;
        col.height = colOriginalHeight/crouchHeightDivider;
    }

    private void UnCrouch()
    {
        canRun = true;
        baseSpeed += crouchSpeedReduction;
        col.height = colOriginalHeight;
        isCrouching = false;
    }
}
