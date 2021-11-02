using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 100f;
    public float damage = 10f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public Camera fpsCam;

    public ParticleSystem muzzleFlash;

    public GameObject impactEffect;

    float nextTimeToFire = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / fireRate);
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

         //   GameObject hitParticleEffect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            if (hit.collider.gameObject.tag == "Enviroment")
            {
                hit.collider.gameObject.GetComponent<ShaderControl>().Hitted(hit.point);
            }
        }

    }
}
