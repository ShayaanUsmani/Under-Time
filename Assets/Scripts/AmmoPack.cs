using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    private int clipsAmount = 3;
    private string playerTag = "Player";

    public Canvas canvas;

    public Image availableVisual;

    public GameObject player;
    public PlayerAttack playerAttackManager;

    public Camera cam;
    void Update()
    {
        Vector3 lookAtPoint = cam.transform.position - canvas.transform.position;
        lookAtPoint.y = 0;
        canvas.transform.rotation = Quaternion.LookRotation(lookAtPoint);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == playerTag)
        {
            playerAttackManager.PickClips(clipsAmount);
            Destroy(gameObject);
        }
    }
}
