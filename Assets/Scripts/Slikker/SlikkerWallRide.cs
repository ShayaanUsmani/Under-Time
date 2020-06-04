using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script requires the game object to have the player movement script
// as it will need it to function properly
[RequireComponent(typeof(PlayerMovement))]
public class SlikkerWallRide : MonoBehaviour
{
    private string environmentTag = "Environment";

    private PlayerMovement playerMovement;

    private Rigidbody rb;
    private float minWallRideVelMag = 0.1f;

    private float wallRideBonusVel = 25f;

    private GameObject ridableObject;

    private bool isRiding;

    private float gravity = 9.81f;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (!playerMovement.isGrounded 
            && ridableObject!=null 
            && Input.GetKey(playerMovement.jumpKey)
            && rb.velocity.magnitude > minWallRideVelMag)
        {
            Debug.Log("velocity mag:" + rb.velocity.magnitude); ///////////////////////////////////////////////
            isRiding = true;
        }
        else if (Input.GetKeyDown(playerMovement.jumpKey) || ridableObject == null)
        {
            isRiding = false;
        }
    }

    // if player is riding wall, make them have no movement in the y axis.
    private void FixedUpdate()
    {
        if (isRiding)
        {
            rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
            rb.AddForce(transform.forward * wallRideBonusVel, ForceMode.VelocityChange);
            //transform.Translate(transform.position.x, startRidingYPos, transform.position.z);

        }
        else
        {
            //rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == environmentTag)
        {
            ridableObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == environmentTag)
        {
            ridableObject = null;
        }
    }
}
