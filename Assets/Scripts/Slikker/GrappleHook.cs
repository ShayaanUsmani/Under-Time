using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class GrappleHook : MonoBehaviour
{
    private float grappleRange = 17f;
    private float ropeLength = 10f;
    private float strength = 9000f;
    private float speedIncrease = 105f;
    private float speedIncreaseRequirment = 6.5f;
    private float speedIncreaseCap = 17f;
    private float speedIncreaseReset = 5f;


    private Vector3 camOffset;
    private float camInitXGrappleRot = -45f;

    public PlayerMovement playerMovement;

    private float distance;

    private Canvas crosshairCanvas;

    private SpringJoint grappleRope;

    public Rigidbody playerRB;
    private GameObject player;

    public int exCamChildLayer = 8;

    public GameObject hookAnchor;
    private Rigidbody anchorRB;
    private Vector3 anchorOriginalPos;
    private Quaternion anchorOriginalRot;
    public GameObject anchorOriginalParent;

    public bool grappleHooked;
    private string hookableTag = "Environment";
    //private string hookTag = "Anchor";

    public Camera cam;

    public KeyCode hookKeyCode = KeyCode.LeftShift;

    // set the original parent to the parent of the hook before anything
    // that would cause it to change happens
    private void Awake()
    {
        player = playerRB.gameObject;
        playerMovement = player.GetComponent<PlayerMovement>();
        crosshairCanvas = cam.GetComponentInChildren<Canvas>();
        anchorOriginalPos = hookAnchor.transform.localPosition;
        anchorOriginalRot = hookAnchor.transform.rotation;
        anchorRB = hookAnchor.GetComponent<Rigidbody>();
        camOffset = new Vector3(0, 4f, -8f);
        anchorOriginalParent = hookAnchor.transform.parent.gameObject;

    }

    private void Update()
    {
        float velocity = playerRB.velocity.magnitude;
        if (Input.GetKey(hookKeyCode))
        {
            shootHook();            
            if (grappleHooked && 
                velocity <= speedIncreaseCap &&
                velocity >= speedIncreaseRequirment)
            {
                playerMovement.moveSpeed += speedIncrease;
            }
        }
        else if (Input.GetKeyUp(hookKeyCode) && grappleHooked)
        {
            returnHook();
            
        }
        if(velocity < speedIncreaseReset)
        {
            playerMovement.moveSpeed = playerMovement.baseSpeed;
        }
    }

    private void LateUpdate()
    {
        if (grappleHooked)
        {
            
        }
    }


    // Shoots the grapple hook at a hittable point, attaching the anchor to the hit point.
    // Since there is a spring joint attached from the anchor to the origin, the character
    // will be anchored to the hook's anchor which is shot.
    private void shootHook()
    {
        RaycastHit hit;
        if (!grappleHooked 
            && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, grappleRange) 
            && hit.transform.tag == hookableTag
            && grappleRope == null)
        {
            Debug.Log("Shot the hook and it landed!");

            crosshairCanvas.enabled = false;

            hookAnchor.transform.position = hit.transform.position;
            hookAnchor.transform.SetParent(hit.transform);
            
            grappleHooked = true;
            grappleRope = playerRB.gameObject.AddComponent<SpringJoint>();

            grappleRope.autoConfigureConnectedAnchor = false;
            grappleRope.connectedAnchor = anchorRB.transform.position;
            grappleRope.connectedBody = anchorRB;

            distance = Vector3.Distance(playerRB.transform.position, anchorRB.transform.position);

            grappleRope.minDistance = -ropeLength;
            grappleRope.maxDistance = ropeLength;
            grappleRope.spring = strength;
            grappleRope.enableCollision = true;
            cam.transform.localPosition += camOffset;
            cam.transform.rotation = Quaternion.Euler(Vector3.right * camInitXGrappleRot);
        }
    }

    // returns grapple hook to the origin.
    private void returnHook()
    {
        Debug.Log("Hook returned!");

        crosshairCanvas.enabled = true;

        grappleRope.gameObject.SetActive(true);
        //Vector3 direction = hookAnchor.transform.position - playerRB.transform.position;
        //playerRB.AddForce(direction * 100 * Time.deltaTime, ForceMode.Impulse);
        hookAnchor.transform.SetParent(gameObject.transform);
        hookAnchor.transform.localPosition = anchorOriginalPos;
        hookAnchor.transform.localRotation = anchorOriginalRot;
        Destroy(grappleRope);
        grappleHooked = false;
        cam.transform.localPosition -= camOffset;
        cam.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // makes all children of a gameobject its siblings
    private static void childrenToSiblings(GameObject parentTransform, string layerName)
    {
        foreach (Transform childTransform in parentTransform.transform)
        {            
            childTransform.SetParent(parentTransform.transform.parent);
            childTransform.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
    // makes all siblings (which were once its children) of a gameobject its siblings
    private static void siblingsToChildren(GameObject parentTransform, string layerName)
    {
        foreach (Transform childTransform in parentTransform.transform.parent)
        {
            if(childTransform.gameObject.layer == LayerMask.NameToLayer(layerName))
            childTransform.SetParent(parentTransform.transform);
            
        }
    }
    
}
