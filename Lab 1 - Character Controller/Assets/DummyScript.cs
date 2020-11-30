using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DummyScript : MonoBehaviour
{
    public float hp = 100;
    public TextMeshProUGUI hpDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            hp -= collision.gameObject.GetComponent<ProjectileScript>().abilityDamage;
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpDisplay.text = "Health: " + hp;
    }
}
