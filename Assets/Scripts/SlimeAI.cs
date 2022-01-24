using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public GameObject startSlime;
    public GameObject squashingSlime;
    public GameObject squashedSlime;
    public int jumpDelay;
    public float jumpForce;
    public ParticleSystem landParticle;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        landParticle = GetComponentInChildren<ParticleSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        landParticle.emissionRate = 0;
        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Jump()
    {
        print("Jumping");
        //startSlime.transform.localScale = Vector3.one * -1;
        startSlime.GetComponent<SpriteRenderer>().enabled = true;
        squashingSlime.GetComponent<SpriteRenderer>().enabled = false;
        squashedSlime.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.7f);

        startSlime.GetComponent<SpriteRenderer>().enabled = false;
        squashedSlime.GetComponent<SpriteRenderer>().enabled = false;
        squashingSlime.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        Vector2 movement = new Vector2(0, rb.velocity.y);

        movement.y = jumpForce;

        rb.velocity = movement;

        yield return new WaitForSeconds(jumpForce / 6);

        startSlime.GetComponent<SpriteRenderer>().enabled = false;
        squashedSlime.GetComponent<SpriteRenderer>().enabled = true;
        squashingSlime.GetComponent<SpriteRenderer>().enabled = false;
        landParticle.emissionRate = 25;

        yield return new WaitForSeconds(0.3f);

        startSlime.GetComponent<SpriteRenderer>().enabled = false;
        squashedSlime.GetComponent<SpriteRenderer>().enabled = false;
        squashingSlime.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.3f);

        startSlime.GetComponent<SpriteRenderer>().enabled = true;
        squashedSlime.GetComponent<SpriteRenderer>().enabled = false;
        squashingSlime.GetComponent<SpriteRenderer>().enabled = false;
        landParticle.emissionRate = 0;
    }
}
