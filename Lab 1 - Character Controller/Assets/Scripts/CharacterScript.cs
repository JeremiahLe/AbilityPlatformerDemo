using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
public class StatSystem : MonoBehaviour
    {
    enum Stats { speed, jumpForce, startingSpeed, startingJumpForce, startingSpeedNoItems}
}
*/

public class CharacterScript : MonoBehaviour
{
    #region Player Variables

    [SerializeField]
    private float speed = 30f;
    private float startingSpeed;
    private float startingSpeedNoItems;
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce = 13f;
    private float startingJumpForce;
    private float startingJumpForceNoItems;
    private float maxVelocity = 50f;
    private float startingMaxVelocity;

    #endregion

    #region Movement and Mechanics

    [SerializeField]
    private float groundCheckRadius = .504f;

    private bool canJump = true;
    private bool isCrouching = false;
    private bool canDash = true;

    public string direction;

    [SerializeField]
    private LayerMask platformsLayerMask;
    private Rigidbody2D rb;

    public Transform groundCheck;
    public Transform startingPosition;
    public Transform checkPoint;

    #endregion

    #region Other Components
    // Create Inventory of ScriptableObjects "ItemDefinitions"
    private List<ItemDefinitions> inventory;
    private List<string> itemList;

    private int currentInventoryIndex = -1;
    public ItemDefinitions currentItem;
    public GameObject droppedItem;

    public TextMeshProUGUI speedDebug;
    public TextMeshProUGUI jumpDebug;
    public TextMeshProUGUI inventoryList;
    public TextMeshProUGUI currentItemDebug;
    public TextMeshProUGUI itemIndexDebug;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startingSpeed = speed;
        startingSpeedNoItems = startingSpeed;
        startingJumpForce = jumpForce;
        startingJumpForceNoItems = startingJumpForce;

        maxVelocity = startingMaxVelocity;
        crouchSpeed = startingSpeed / 1.4f;

        inventory = new List<ItemDefinitions>();
        itemList = new List<string>();

        //currentItem.itemName = "none";
    }

    private void Update()
    {
        Movement();
        Jumping();
        Crouch();
        Dash();
        UpdateUI();
        //ClearInventory();
        ResetScene();
        CycleInventory();
        ToggleEquip();
        DropItem();
    }

    void Movement()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        rb.velocity += Vector2.right * move;

        if (move.x == 0 && rb.velocity != Vector2.zero)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        // Velocity can't exceed max Velocity
        if (rb.velocity.sqrMagnitude > maxVelocity)
        {
            rb.velocity *= 0.99f;
        }

        // Check player input to determine current direction for Dash
        if (move.x < 0)
        {
            direction = "Left";
        }
        else if (move.x > 0)
        {
            direction = "Right";
        }
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                canJump = false;
                rb.velocity += Vector2.up * jumpForce;
            }
        }

        canJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformsLayerMask);
    }

    void Crouch()
    {
        isCrouching = Input.GetKey(KeyCode.S) && canJump != false || Input.GetAxisRaw("Vertical") < 0 && canJump != false;

        if (isCrouching)
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            speed = crouchSpeed;
        }
        else
        {
            speed = startingSpeed;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) // Xbox "B"
        {
            if (canDash /*&& canJump*/)
            {
                if (direction == "Left")
                {
                    rb.AddForce(new Vector2(5.5f * -1, 0f), ForceMode2D.Impulse);
                    canDash = false;
                    Invoke("ResetDash", 0.6f);
                }
                else
                if (direction == "Right")
                {
                    rb.AddForce(new Vector2(5.5f * 1, 0f), ForceMode2D.Impulse);
                    canDash = false;
                    Invoke("ResetDash", 0.6f);
                }
            }
        }
    }

    // Reset Dash ability Cooldown
    void ResetDash(){
        speed = startingSpeed;
        maxVelocity = startingMaxVelocity;
        canDash = true;
    }

    // Collision check with Out of Bounds 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "ResetBounds"){
            transform.position = startingPosition.transform.position;
            rb.velocity = Vector2.zero;
        }
        else if (collision.gameObject.name == "ResetBoundsCheckpoint")
        {
            transform.position = checkPoint.transform.position;
            rb.velocity = Vector2.zero;
        }
        else if (collision.gameObject.tag == "Item")
        {
            Destroy(collision.gameObject);
            ItemDefinitions testItem = collision.gameObject.GetComponent<ItemInitialization>().myItem; // what item did we pick up?
            //ItemDefinitions item = collision.gameObject;
            //Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
            //inventory.AddItem(item);
            AddItem(testItem);
        }
    }

    public void AddItem(ItemDefinitions item)
    {
        inventory.Add(item); // Add picked up item to list of ItemDefintions (scriptable objects)

        currentItem = item; // Set current selected item to the item we just picked up

        currentInventoryIndex += 1;

        //startingSpeed += item.speedModifier; // modify player stats by item values
        //speed += item.speedModifier; // modify player stats by item values
        //jumpForce += item.jumpModifier; // modify player stats by item values

        itemList.Add(item.itemName.ToString() + "\n"); // update & display inventory list

        Debug.Log("Added " + item.itemName + " to inventory.");
    }

    public void ToggleEquip()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Fire2")) // Xbox button "Y"
        {
            if (currentItem != null)
            {
                if (currentItem.isEquipped == false)
                {
                    currentItem.isEquipped = true;
                    //currentItem.itemName = (currentItem.itemName + " (Equipped)");
                    speed += currentItem.speedModifier;
                    startingSpeed = speed;
                    jumpForce += currentItem.jumpModifier;
                    startingJumpForce = jumpForce;
                }
                else if (currentItem.isEquipped == true)
                {
                    currentItem.isEquipped = false;
                    //currentItem.itemName = (currentItem.itemName  " (Unequipped)");
                    speed -= currentItem.speedModifier;
                    startingSpeed = speed;
                    jumpForce -= currentItem.jumpModifier;
                    startingJumpForce = jumpForce;
                }
            }
        }
    }

    public void DropItem()
    {
        if (Input.GetButtonDown("Drop") || (Input.GetKeyDown(KeyCode.X)))
        {
            if (currentItem != null)
            {
                // Repeat of unequip logic
                if (currentItem.isEquipped == true)
                {
                    //currentItem.isEquipped = false;
                    speed -= currentItem.speedModifier;
                    startingSpeed = speed;
                    jumpForce -= currentItem.jumpModifier;
                    startingJumpForce = jumpForce;
                    currentItem.isEquipped = false;
                }

                // Instantiate Dropped Item
                var go = Instantiate(droppedItem, new Vector2(this.transform.position.x + Random.Range(1.0f, 1.5f), this.transform.position.y + Random.Range(-0.1f, 0.8f)) , Quaternion.identity);

                go.GetComponent<ItemInitialization>().myItem = currentItem;
                go.GetComponent<ItemInitialization>().myItem.itemName = currentItem.itemName;

                go.GetComponent<ItemInitialization>().myItem.speedModifier = currentItem.speedModifier;
                go.GetComponent<ItemInitialization>().myItem.jumpModifier = currentItem.jumpModifier;

                go.GetComponent<ItemInitialization>().myItem.isEquipped = false;

                // Update inventory
                inventory.Remove(currentItem);
                currentInventoryIndex -= 1;
                currentItem = inventory[inventory.Count - 1];

                /*
                if (inventory.Count >= 0)
                {
                    currentItem = inventory[currentInventoryIndex - 1];
                    currentInventoryIndex -= 1;
                }
                else
                {
                    currentItem = inventory[currentInventoryIndex + 1];
                    currentInventoryIndex += 1;
                }
                */
                //currentItemDebug.text = "Current Item: " + currentItem.itemName;
            }
        }
    }

    public void CycleInventory()
    {
        if (inventory.Count > 0 && currentItem != null && currentInventoryIndex != -1)
        {
            if (Input.GetButtonDown("LB") || (Input.GetKeyDown(KeyCode.Q))) // joystick 4
            {
                var index = currentInventoryIndex > 0 ? currentInventoryIndex - 1 : inventory.Count - 1;
                SelectItem(index);

                //Debug.Log("Hi, I worked! LB");
            }
            if (Input.GetButtonDown("RB") || (Input.GetKeyDown(KeyCode.E))) // joystick 5
            {
                var index = currentInventoryIndex == inventory.Count - 1 ? 0 : currentInventoryIndex + 1;
                SelectItem(index);

                //Debug.Log("Hi, I worked! RB");
            }
        }
    }

    private void SelectItem(int index)
    {
        if (index == currentInventoryIndex)
        {
            return;
        }

        currentItem = inventory[index];
        //currentItem.itemName = currentItem.itemName + " (Selected)";
        currentInventoryIndex = index;
        //currentItemDebug.text = "Current Item: " + currentItem.itemName;
    }

    void UpdateUI()
    {
        speedDebug.text = "Speed: " + speed;
        jumpDebug.text = "Jumpforce: " + jumpForce;

        if (currentItem != null)
        {
            inventoryList.text = ""; //Clear the text
            foreach (ItemDefinitions item in inventory) //Add each item to the text
                inventoryList.text += item.itemName.ToString() + "\n";
        }

        /*
        if (currentItem == null)
            currentItemDebug.text = "Current Item: None";
        else 
            currentItemDebug.text = "Current Item: " + currentItem.itemName;
            */

        // Update Current Item display

        if (currentItem != null)
        {
            if (currentItem.isEquipped)
                currentItemDebug.text = "Current Item: " + currentItem.itemName + "(Equipped) " /*currentInventoryIndex*/;
            else
                currentItemDebug.text = "Current Item: " + currentItem.itemName + "(Unequipped) " /*+ currentInventoryIndex*/;
        }
        else
        {
            currentItemDebug.text = "Current Item: None ";
        }
        

        if (inventory.Count <= 0)
        {
            currentItem = null;
        }

        itemIndexDebug.text = "Index: " + currentInventoryIndex;


        // Keeping track of inventory index

        if (currentInventoryIndex >= inventory.Count)
        {
            currentInventoryIndex = 0;
        }

        if (currentInventoryIndex < 0)
        {
            currentInventoryIndex = inventory.Count - 1;
        }

        //currentItemDebug.text = "Current Item: " + currentItem.itemName;

        //inventory[currentInventoryIndex].itemName = currentItem.itemName + " (Selected)";

        /*
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryList.text = inventory[i].itemName + "\n";
            //itemList[i] = inventory[i].itemName;
        }
        */
    }

    /*
    void ClearInventory()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Fire2")) // Xbox button "Y"
        {
            inventory.Clear();
            inventoryList.text = "Items: ";
            currentItem = null;

            startingSpeed = startingSpeedNoItems;
            speed = startingSpeedNoItems;
            jumpForce = startingJumpForce;
        }
    }
    */

    void ResetScene()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Fire3")) // Xbox button "Share?"
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    // Stats Milestone 1

    float ReturnStatValue(string returnStat)
    {
        if (returnStat == "Speed")
            return speed;
        else if (returnStat == "jumpForce")
            return jumpForce;
        else
            return 0;
    }

    void AlterStatValue(string statToChange, float value)
    {
        if (statToChange == "Speed")
        {
            speed += value;
            startingSpeed = speed;
        }
        else if (statToChange == "jumpForce")
        {
            jumpForce += value;
            startingJumpForce = jumpForce;
        }
    }

    void ResetStatValue(string statToReset)
    {
        if (statToReset == "Speed")
            speed = startingSpeedNoItems;
        else if (statToReset == "jumpForce")
            jumpForce = startingJumpForceNoItems;
    }

    // Stats Milestone 1
}
