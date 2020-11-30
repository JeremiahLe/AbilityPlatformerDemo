using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInitialization : MonoBehaviour
{
    public ScriptableAbility myAbility;
    public Sprite abilitySprite;

    void Start()
    {
        myAbility.isLearned = false;
        myAbility.offCooldown = true;
    }

    void Update()
    {
        
    }
}
