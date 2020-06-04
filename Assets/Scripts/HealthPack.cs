using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private string megaTag = "Mega";

    private float refreshTime;
    private float refreshTimer;
    private bool canHeal = true;

    private float healAmount;

    private string indicatorTag;

    private string playerTag = "Player";
    public GameObject player;
    private HealthManager healthManager;

    public Canvas canvas;
    public Image availableVisual;
    public Image refreshingVisual;


    public Camera cam;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        healthManager = player.GetComponent<HealthManager>();
        indicatorTag = gameObject.tag;
        refreshingVisual.enabled = false;
        if(indicatorTag == megaTag)
        {
            healAmount = 200f;
            refreshTime = 12f;
        }
        else
        {
            healAmount = 75f;
            refreshTime = 6f;
        }
    }
    void Update()
    {
        Vector3 lookAtPoint = cam.transform.position - canvas.transform.position;
        lookAtPoint.y = 0;
        canvas.transform.rotation = Quaternion.LookRotation(lookAtPoint);
        if (!canHeal)
        {
            availableVisual.enabled = false;
            refreshingVisual.enabled = true;
            refreshPack();
        }
    }

    private void HealPlayer()
    {
        healthManager.Heal(healAmount);
        canHeal = false;
    }

    private void refreshPack()
    {
        refreshTimer += Time.deltaTime;
        refreshingVisual.fillAmount = (refreshTimer / refreshTime);
        if (refreshTimer >= refreshTime)
        {
            refreshTimer = 0;
            canHeal = true;
            availableVisual.enabled = true;
            refreshingVisual.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canHeal && other.transform.tag == playerTag)
        {
            if (healthManager.currHealth + healthManager.currArmor < healthManager.combinedStartHealth)
            {
                HealPlayer();
            }
        }
    }
}
