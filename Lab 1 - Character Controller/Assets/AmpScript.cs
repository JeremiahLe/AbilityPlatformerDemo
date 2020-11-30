using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpScript : MonoBehaviour
{
    public float frequency;
    private float amplitude = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = newPos;
    }
}
