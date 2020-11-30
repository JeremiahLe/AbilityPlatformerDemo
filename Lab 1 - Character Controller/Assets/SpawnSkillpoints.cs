using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkillpoints : MonoBehaviour
{
    public GameObject Skillpoint;
    // Start is called before the first frame update
    void Start()
    {
        var go = Instantiate(Skillpoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
