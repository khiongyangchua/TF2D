﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Defines a basic rocket launched from a rocket launcher.
 * A rocket explodes upon impact with a body, causing explosion damage and knockback.
 */
public class Rocket : MonoBehaviour
{
    public GameObject shooter;
    private Rigidbody2D rgbd;

    public float radius;
    public float velocity;
    public int directDamage;
    public float knockback;
    public float effectiveExplosiveDistance;

    public GameObject explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        // Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), shooter.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        // Explode on collision

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] affectedTargets = Physics2D.OverlapCircleAll(this.transform.position, radius);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            Explode(affectedTargets, collision);
        }

        else if(collision.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            // Deal contact damage, then explode.
            DirectlyDamage(collision.gameObject);
            Explode(affectedTargets, collision);
        }
    }

    void DirectlyDamage(GameObject collidedObject)
    {
        // collidedObject.dealDamage();
    }

    void Explode(Collider2D[] affectedTargets, Collision2D collision)
    {
        // Apply knockback to enemies and self in the vicinity
        // The angle is the angle between floor, and explosion to affectedTarget center of mass
        foreach (Collider2D affectedTarget in affectedTargets)
        {

            float dist = Vector3.Distance(affectedTarget.transform.position, this.transform.position);
            Vector3 dir = (affectedTarget.transform.position - this.transform.position).normalized;

            // TODO: Add damage and knockback scaling to distance.
            if (affectedTarget.CompareTag("Players"))
            {
                affectedTarget.GetComponent<Rigidbody2D>().AddForce(dir * knockback);
            }
        }

        // DIE
        GameObject generatedParticles = GameObject.Instantiate(explosionParticles);
        generatedParticles.transform.position = this.transform.position;

        // Rotate particle to correct angle.
        generatedParticles.transform.rotation = Quaternion.Euler(Vector2.Reflect(this.GetComponent<Rigidbody2D>().velocity, collision.contacts[0].normal));

        Destroy(generatedParticles.gameObject, 2f);

        Destroy(this.gameObject);
    }
}
