using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask layerMask;
    
    public int selectedSlotIndex = 0;
    public GameObject hotbarSelector;
    public Inventory inventory;
    public bool inventoryShowing = false;

    public ItemClass selectedItem;
    public GameObject handHolder;

    public float moveSpeed;
    public float jumpForce;
    public bool onGround;
    public float breakTime;

    private Rigidbody2D rb;
    private Animator anim;
    
    private float horizontal;
    public bool hit;
    public int reachDistance;
    public bool place;

    [HideInInspector]
    public Vector2 spawnPos;

    public Vector2Int mousePos;
    public TerrainGeneration terrainGenerator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
    }

    public void Spawn()
    {
        GetComponent<Transform>().position = spawnPos;
    }

    private void FixedUpdate()
    {
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        // jumping
        if (jump > 0.1)
        {
            if(onGround)
                movement.y = jumpForce;
        }
        
        // autojumping
        // if (FootRayCast() && !HeadRayCast())
        // {
        //     if(onGround)
        //         movement.y = jumpForce * 0.7f;
        // }

        rb.velocity = movement;
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        hit = Input.GetMouseButtonDown(0);
        place = Input.GetMouseButtonDown(1);

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            // scroll up
            if (selectedSlotIndex < inventory.inventoryWidth)
                selectedSlotIndex += 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            // scroll down
            if (selectedSlotIndex > 0)
                selectedSlotIndex -= 1;
        }

        hotbarSelector.transform.position = inventory.hotbarUISlots[selectedSlotIndex].transform.position;
        if (selectedItem != null)
        {
            handHolder.GetComponent<SpriteRenderer>().sprite = selectedItem.sprite;
            if (selectedItem.itemType == ItemClass.ItemType.block)
            {
                handHolder.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            }
            else
            {
                handHolder.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            handHolder.GetComponent<SpriteRenderer>().sprite = null;
        }

        if (inventory.inventorySlots[selectedSlotIndex, inventory.inventoryHeight - 1] != null)
            selectedItem = inventory.inventorySlots[selectedSlotIndex, inventory.inventoryHeight - 1].item;
        else
        {
            selectedItem = null;
        }
        //Debug.Log(selectedItem);

        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryShowing = !inventoryShowing;
        }

        if (Vector2.Distance(transform.position, mousePos) <= reachDistance /*&&
            Vector2.Distance(transform.position, mousePos) > 1f*/)
        {
            if (hit)
            {
                //terrainGenerator.RemoveTile(mousePos.x, mousePos.y);
                terrainGenerator.BreakTile(mousePos.x, mousePos.y, selectedItem);
            }
            
            if (place)
            {
                if (selectedItem != null)
                {
                    if (selectedItem.itemType == ItemClass.ItemType.block)
                    {
                        if(terrainGenerator.CheckTile(selectedItem.tile, mousePos.x, mousePos.y, false))
                            inventory.Remove(selectedItem);
                    }
                }
            }

        }

        // if (hit && inventoryShowing)
        // {
        //     inventory.Drag();
        // }

        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        inventory.inventoryUI.SetActive(inventoryShowing);

        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("hit", hit || place);
    }

    // private void OnValidate()
    // {
    //     Debug.DrawRay(transform.position - (Vector3.up * 0.5f), -Vector2.right, Color.white, 10f);
    //     Debug.DrawRay(transform.position + (Vector3.up * 0.5f), -Vector2.right, Color.white, 10f);
    // }
    //
    // public bool FootRayCast()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position - (Vector3.up * 0.5f), -Vector2.right, 1f, 10);
    //     return hit;
    // }
    //
    // public bool HeadRayCast()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), -Vector2.right, 1f, 10);
    //     return hit;
    // }
}
