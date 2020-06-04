using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyShoot : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    //private float shootForce = 5000f;
    private float shootCooldownTime = 0.45f;
    private float shootDmg = 5f;
    //private float bulletDieTime = 5f;
    private bool canShoot;

    public GameObject player;
    private HealthManager playerHealthManager;
    private NormalEnemyController enemyController;
    private bool isAggrod;

    private void Awake()
    {
        playerHealthManager = player.GetComponent<HealthManager>();
        enemyController = gameObject.GetComponent<NormalEnemyController>();
    }
    void Start()
    {
        canShoot = true;
    }

    void Update()
    {        
        isAggrod = enemyController.isAggrod;

        if (isAggrod && canShoot && enemyController.CanSeePlayer())
        {
            ShootPlayer();
        }
    }

    private void ShootPlayer()
    {

        canShoot = false;

        playerHealthManager.TakeDamage(shootDmg);

        //GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
        //bulletClone.transform.LookAt(playerTrans);
        //bulletClone.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shootForce * Time.deltaTime, ForceMode.Impulse);
        //Destroy(bulletClone, bulletDieTime);
        
        StartCoroutine(ShootCooldown(shootCooldownTime));
    }

    private IEnumerator ShootCooldown(float shootCooldownTime)
    {
        yield return new WaitForSeconds(shootCooldownTime);

        canShoot = true;
    }

    
}
