using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    private string playerTag = "Player";
    private string groundTag = "Ground";
    private GameObject player;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == groundTag)
        {
            playerMovement.isGrounded = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == groundTag)
        {
            playerMovement.isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == groundTag)
        {
            playerMovement.isGrounded = false;
            Debug.Log("Airborne");
        }
    }
}
