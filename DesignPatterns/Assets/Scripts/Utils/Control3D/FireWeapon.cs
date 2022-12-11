using CommandPattern;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Control3D
{
    public class FireWeapon : Command
    {
        private GameObject bullet;
        private float speed = 5000f;

        public FireWeapon(GameObject bulletPrefab)
        {
            bullet = bulletPrefab;
        }
        
        public override void Execute(Transform objectTransform, bool modifierApplied = false)
        {
            GameObject instBullet = GameObject.Instantiate(bullet, objectTransform.transform.position, Quaternion.identity);
            Transform bulletBody = instBullet.transform.Find("bulletBody");
			Rigidbody instBulletRBody = bulletBody.GetComponent<Rigidbody>();

			instBulletRBody.AddForce(objectTransform.transform.forward * speed);
            GameObject.Destroy(instBullet, 2f);
        }
    }
}