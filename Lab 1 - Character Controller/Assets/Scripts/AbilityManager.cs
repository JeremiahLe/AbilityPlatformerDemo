using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    #region Skill Tree Stuff
    public int skillPoints = 0;
    
    public GameObject skillTree;
    public Image skillTreeBG;

    public bool skillTreeVisible = false;
    #endregion

    #region Ability Mechanics

    // Create List of ScriptableObjects "ScriptableAbilty"
    public List<ScriptableAbility> availableAbilities; // list of abilities to learned in skill tree window
    private List<ScriptableAbility> learnedAbilities; // list of abilities already learned and able to cycle thru

    private List<string> availableAbilityList; //  visible list of abilities available
    private List<string> learnedAbilityList; // visible list of learned abilities

    private int currentAbilityIndex = -1; // when cycling through available abilities, which one are we at?
    public ScriptableAbility currentAbility; // the reference to the ability we currently have selected

    private int currentLearnedAbilityIndex = -1; // for casting
    public ScriptableAbility currentActiveAbility; // for casting

    public GameObject abilityProjectile; // the ability projectile object

    public TextMeshProUGUI skillPointDisplay; // how many skill points do we have
    public TextMeshProUGUI currentAbilityDebug; // what is our current ability

    public TextMeshProUGUI debugAvailableAbilityList;
    public TextMeshProUGUI debugLearnedAbilityList;
    public TextMeshProUGUI debugCurrentAbilityIndex;

    //public GameObject AbilityHolder;
    #endregion

    void Start()
    {
        availableAbilities = new List<ScriptableAbility>();
        learnedAbilities = new List<ScriptableAbility>();

        availableAbilityList = new List<string>();
    }

    void Update()
    {
        UpdateUI();
        CycleAvailableAbilities();
        LearnAbility();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ability")
        {
            Destroy(collision.gameObject);
            ScriptableAbility testAbility = collision.gameObject.GetComponent<AbilityInitialization>().myAbility; // adding abilities to tree
            AddAbility(testAbility);
        }
    }

    public void AddAbility(ScriptableAbility ability)
    {
        availableAbilities.Add(ability); // add ability to list of abilities

        currentAbility = ability;

        availableAbilityList.Add(ability.abilityName.ToString() + "\n"); // update & display available ability list //  <<

        Debug.Log("Added " + ability.abilityName + " to skill tree.");
    }

    public void CycleAvailableAbilities()
    {
        if (availableAbilities.Count > 0 && currentAbility != null && currentAbilityIndex != -1 && gameObject.GetComponent<AbilityManager>().skillTreeVisible != false)
        {
            if (Input.GetButtonDown("LB") || (Input.GetKeyDown(KeyCode.Q))) // joystick 4
            {
                var index = currentAbilityIndex > 0 ? currentAbilityIndex - 1 : availableAbilities.Count - 1;
                SelectAbility(index);

                //Debug.Log("Hi, I worked! LB");
            }
            if (Input.GetButtonDown("RB") || (Input.GetKeyDown(KeyCode.E))) // joystick 5
            {
                var index = currentAbilityIndex == availableAbilities.Count - 1 ? 0 : currentAbilityIndex + 1;
                SelectAbility(index);

                //Debug.Log("Hi, I worked! RB");
            }
        }
    }

    public void LearnAbility()
    {
        if (Input.GetKeyDown(KeyCode.C) && gameObject.GetComponent<AbilityManager>().skillTreeVisible != false || Input.GetButtonDown("Fire2") && gameObject.GetComponent<AbilityManager>().skillTreeVisible != false) // Xbox button "Y"
        {
            if (currentAbility.isLearned != true && skillPoints < 0)
            {
                currentAbility.isLearned = true;
                skillPoints -= 1;
            }
        }
    }

    private void SelectAbility(int index)
    {
        if (index == currentAbilityIndex)
        {
            return;
        }

        currentAbility = availableAbilities[index];
        //currentItem.itemName = currentItem.itemName + " (Selected)";
        currentAbilityIndex = index;
        //currentItemDebug.text = "Current Item: " + currentItem.itemName;
    }

    void UpdateUI()
    {
        skillPointDisplay.text = "Skill Points: " + skillPoints;

        if (currentAbility != null)
        {
            debugAvailableAbilityList.text = "";
            foreach (ScriptableAbility ability in availableAbilities) // Add each ability to the text
                debugAvailableAbilityList.text += ability.abilityName.ToString() + "\n";
        }

        if (Input.GetKeyDown(KeyCode.P)) // or "Start" to open Skill Tree Window (Canvas)
        {
            if (skillTreeVisible == false)
            {
                skillTree.SetActive(true);
                skillTreeVisible = true;
                skillTreeBG.canvasRenderer.SetAlpha(0.5f);
            }
            else
            {
                skillTree.SetActive(false);
                skillTreeVisible = false;
            }
        } // opening Skill Tree

        // current ability

        if (currentAbility != null)
        {
            if (currentAbility.isLearned)
                currentAbilityDebug.text = "Current Ability: " + currentAbility.abilityName + "(Learned) " /*currentInventoryIndex*/;
            else
                currentAbilityDebug.text = "Current Ability: " + currentAbility.abilityName + "(Not Learned) " /*+ currentInventoryIndex*/;
        }
        else
        {
            currentAbilityDebug.text = "Current Ability: None ";
        }

        if (availableAbilityList.Count <= 0)
        {
            currentAbility = null;
        }

        debugCurrentAbilityIndex.text = "AbilityIndex: " + currentAbilityIndex;

        // keeping track of ability index

        if (currentAbilityIndex >= availableAbilities.Count)
        {
            currentAbilityIndex = 0;
        }

        if (currentAbilityIndex < 0)
        {
            currentAbilityIndex = availableAbilities.Count - 1;
        }
    }
}
