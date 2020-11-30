using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float velX = 0f;
    public float velY = 0f;

    public float speed = 0f;
    public float abilityDamage = 0f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyProjectile", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velX * speed, velY * speed);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
