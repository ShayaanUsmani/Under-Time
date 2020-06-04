using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private string playerTag = "Player";
    private float damage = 15f;
    private HealthManager playerHealthManager;
    private void Awake()
    {        
        playerHealthManager = GameObject.FindGameObjectWithTag(playerTag).GetComponent<HealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == playerTag)
        {
            playerHealthManager.TakeDamage(damage);
            Debug.Log("I did it for the boys");
            Destroy(gameObject);
        }
    }
}
