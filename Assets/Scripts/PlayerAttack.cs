using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int shootMouseButton = 0;
    public KeyCode reloadKey = KeyCode.R;

    public float basicShortRangeAtkDmg = 50f;
    public float basicLongRangeAtkDmg = 25f;
        
    public float fullAmmo = 25f;
    public float ammoCount;
    public float clipCount = 6f;
    public float reloadTime = 1f;

    public float shootCooldownTime = 0.3f;
    public bool canShoot;

    private bool reloaded;
    public bool reloading;
    public TextMeshProUGUI noClipText;

    public TextMeshProUGUI ammoCountText;
    public TextMeshProUGUI clipCountText;

    private float shortRange = 30f;

    private string enemyLayerName = "Enemy";

    public Camera cam;

    private void Awake()
    {
        ammoCount = fullAmmo;
        noClipText.enabled = false;
        canShoot = true;
    }
    void Update()
    {
        DisplayAmmoAndClipCount();
        if ((Input.GetMouseButton(shootMouseButton) || Input.GetMouseButtonDown(shootMouseButton)) && canShoot)
        {
            Shoot();
        }
        if (Input.GetKey(reloadKey))
        {
            Reload();
        }
    }

    private void DisplayAmmoAndClipCount()
    {
        ammoCountText.SetText(ammoCount.ToString());
        clipCountText.SetText(clipCount.ToString());
    }

    private void Shoot()
    {
        canShoot = false;
        if(ammoCount <= 0 && !reloading)
        {
            Reload();
            return;
        }

        if (reloading)
        {
            return;
        }

        ammoCount--;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit) &&
            hit.transform.gameObject.layer == LayerMask.NameToLayer(enemyLayerName))
        { 
            if(hit.distance <= shortRange)
            {
                hit.transform.GetComponent<EnemyHealth>().TakeDamage(basicShortRangeAtkDmg);
            }
            else
            {
                hit.transform.GetComponent<EnemyHealth>().TakeDamage(basicLongRangeAtkDmg);
            }
        }
        StartCoroutine(ShootCooldown(shootCooldownTime));
    }

    public void PickClips(int clips)
    {
        clipCount += clips;
    }

    public void Reload()
    {
        if(ammoCount == fullAmmo)
        {
            return;
        }

        if (clipCount > 0)
        {
            reloading = true;
            StartCoroutine(ReloadCoroutine(reloadTime));
            noClipText.enabled = false;
        }
        else
        {
            noClipText.enabled = true;
        }
    }

    private IEnumerator ReloadCoroutine(float reloadTime)
    {
        reloaded = false;

        yield return new WaitForSeconds(reloadTime);

        if (!reloaded)
        {
            ammoCount = fullAmmo;
            clipCount--;
            reloaded = true;
        }
        reloading = false;
        canShoot = true;
    }

    private IEnumerator ShootCooldown(float shootCooldownTime)
    {
        yield return new WaitForSeconds(shootCooldownTime);

        canShoot = true;
    }
}
