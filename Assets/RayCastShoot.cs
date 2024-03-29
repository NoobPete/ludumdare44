﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RayCastShoot : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = .07f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public int maxAmmo = 30;
    public int currentAmmo;
    public bool reloading = false;
    public float reloadTime = 1.4f;

    public TextMeshProUGUI ammoText;

    public Transform gunEnd;
    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(.05f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;

    public bool shotMade = false;
    public Vector3 shotPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();

        fpsCam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Reload") && !reloading)
        {
            reloading = true;
            currentAmmo = 0;
            StartCoroutine(ReloadWait());
        }

        if (Input.GetButton("Fire1") && Time.time > nextFire && currentAmmo > 0)
        {
            currentAmmo--;

            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;
            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
            RaycastHit hit;
            shotMade = true;
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange, layerMask))
            {
                shotPosition = hit.point;

                ShootableBox health = hit.collider.GetComponent<ShootableBox>();
                if (health != null)
                {
                    health.Damage(gunDamage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                shotPosition = fpsCam.transform.position + fpsCam.transform.forward * weaponRange;
            }
        }

        ammoText.text = currentAmmo + "/" + maxAmmo;
    }

    void LateUpdate()
    {
        if (shotMade)
        {
            laserLine.SetPosition(0, gunEnd.position);
            laserLine.SetPosition(1, shotPosition);
        }

        shotMade = false;
    }

    private IEnumerator ReloadWait()
    {
        yield return new WaitForSeconds(reloadTime);

        reloading = false;
        currentAmmo = maxAmmo;
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;

        yield return shotDuration;
        laserLine.enabled = false;
    }
}
