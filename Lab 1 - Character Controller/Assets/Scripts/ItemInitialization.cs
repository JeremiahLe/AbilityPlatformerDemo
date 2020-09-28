using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInitialization : MonoBehaviour
{
    //[SerializeField]
    public ItemDefinitions myItem;
    public TextMeshProUGUI itemName;

    private float amplitude = 0.01f;
    public float frequency = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        itemName.text = myItem.itemName;
        myItem.isEquipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        //itemName.text = myItem.itemName;

        Vector3 newPos = transform.position;
        newPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = newPos;
    }
}
