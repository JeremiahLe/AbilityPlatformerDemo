using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "ScriptableObjects/Ability", order = 1)]

public class ScriptableAbility : ScriptableObject
{
    public string abilityName; // the name of the ability
    public Sprite abilitySprite; // what does it look like?

    public float abilitySpeed; // how fast does it travel (projectile)
    public float abilityDamage; // if it does damage, how much?
    public float abilityCooldown; // what is the abilities cooldown (time before you can use it again)

    public bool offCooldown; // is the ability ready to use?
    public bool isLearned; // have you used a skill point to learn it?
}