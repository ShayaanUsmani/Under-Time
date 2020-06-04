using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private bool aimingTeleport;
    private float teleportRange = 10f;
    private float teleportChannelTime = 1f;

    private string groundTag = "Ground";

    private float height;

    private bool channelingTeleport;
    private KeyCode teleportKey = KeyCode.LeftShift;
    public Camera cam;

    private void Awake()
    {
        height = gameObject.GetComponent<CapsuleCollider>().height;
    }
    void Update()
    {
        if (Input.GetKeyDown(teleportKey))
        {
            aimingTeleport = true;
            
        }

        if (aimingTeleport)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward * 10, out hit, teleportRange) &&
                hit.transform.tag == groundTag &&
                Input.GetMouseButtonDown(0))
            {
                Debug.Log("surface found");
                Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.black);
                StartCoroutine(TeleportAfterChanneling(hit.transform.position));
                /*
                RaycastHit hitBelow;
                if (Physics.Raycast(hit.transform.position, Vector3.down, out hitBelow, surfaceBelowRange))
                {
                    Debug.DrawRay(hit.transform.position, hitBelow.transform.position, Color.red);
                    Debug.Log("floor found");
                    StartCoroutine(TeleportAfterChanneling(hit.transform.position));
                }
                */
            }
        }
    }

    private IEnumerator TeleportAfterChanneling(Vector3 targetPos)
    {
        yield return new WaitForSeconds(teleportChannelTime);

        gameObject.transform.position = targetPos + Vector3.up * height;
        aimingTeleport = false;

    }
}
