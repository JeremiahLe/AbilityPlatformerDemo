using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemDefinitions : ScriptableObject
{
    public string itemName;
    public float jumpModifier;
    public float speedModifier;
    public bool isEquipped;
}
