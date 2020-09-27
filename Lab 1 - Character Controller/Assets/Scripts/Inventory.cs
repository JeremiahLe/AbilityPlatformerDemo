using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ScriptableObject {

    private List<ItemDefinitions> inventory;

    public Inventory() {
        inventory = new List<ItemDefinitions>();

        //AddItem( );
        Debug.Log("Inventory");
    }

    public void AddItem(ItemDefinitions item){
        inventory.Add(item);
    }

}
