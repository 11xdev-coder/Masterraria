using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;

    private Rigidbody2D rb;
    private Animator anim;
    
    private float horizontal;
    public bool hit;
    public int reachDistance;

    [HideInInspector]
    public Vector2 spawnPos;
    public Vector2Int mousePos;
    public TerrainGeneration terrainGenerator;

    public void Spawn()
    {
        GetComponent<Transform>().position = spawnPos;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    private void FixedUpdate()
    {
        float reach = Vector2.Distance(mousePos, this.transform.position);
        reach = Mathf.RoundToInt(reach);

        horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");
        hit = Input.GetMouseButton(0);

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (hit && reach <= reachDistance)
        {
            terrainGenerator.RemoveTile(mousePos.x, mousePos.y);
        }

        if (horizontal > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (jump > 0.1)
        {
            if(onGround)
                movement.y = jumpForce;
        }

        rb.velocity = movement;
    }

    private void Update()
    {
        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("hit", hit);
    }
}
