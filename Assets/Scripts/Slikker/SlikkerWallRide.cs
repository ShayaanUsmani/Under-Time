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

    private GameObject ridableObject;

    private bool isRiding;

    private float startRidingYPos;

    private float gravity = 9.81f;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (!playerMovement.isGrounded && ridableObject!=null && Input.GetKey(playerMovement.jumpKey))
        {
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
            playerMovement.yVel = 0;
            //transform.Translate(transform.position.x, startRidingYPos, transform.position.z);
            Debug.Log(startRidingYPos);

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
            startRidingYPos = transform.position.y;
            Debug.Log(transform.position);
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
