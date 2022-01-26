using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public GameObject startSlime;
    public GameObject squashingSlime;
    public GameObject squashedSlime;
    public float jumpDelay;
    public float jumpForce;
    public float speed;
    public ParticleSystem landParticle;
    public bool isPeaceful;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        landParticle = landParticle.GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CallJump", 0.5f, jumpDelay + 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CallJump()
    {
        StartCoroutine(Jump());
    }

    public int GetMovingSide()
    {
        if (isPeaceful)
        {
            int i = Random.Range(-1, 1);
            if (i == 0)
            {
                i = 1;
                transform.localScale = new Vector3(-2, 2, 2);
            }
            else if(i <= -1)
            {
                transform.localScale = new Vector3(2, 2, 2);
            }
            else if(i >= 1)
            {
                transform.localScale = new Vector3(-2, 2, 2);
            }

            return i;
        }

        return 0;
    }

    public void ChangeSprites(bool startSlimeBool, bool squashingSlimeBool, bool sqashedSlimeBool)
    {
        startSlime.GetComponent<SpriteRenderer>().enabled = startSlimeBool;
        squashingSlime.GetComponent<SpriteRenderer>().enabled = squashingSlimeBool;
        squashedSlime.GetComponent<SpriteRenderer>().enabled = sqashedSlimeBool;
    }

    public IEnumerator Jump()
    {
        print("Jumping");
        //startSlime.transform.localScale = Vector3.one * -1;
        ChangeSprites(true, false, false);

        yield return new WaitForSeconds(1f);

        ChangeSprites(false, true, false);

        yield return new WaitForSeconds(0.5f);

        ChangeSprites(true, false, false);

        Vector2 movement = new Vector2(0, rb.velocity.y);

        movement.y = jumpForce;
        movement.x = GetMovingSide() * speed;

        rb.velocity = movement;

        yield return new WaitForSeconds(jumpForce / 5);

        ChangeSprites(false, false, true);
        //landParticle = Instantiate(landParticle, transform.position, new Quaternion(-90, 0, 0, 0));
        landParticle.emissionRate = 15;

        yield return new WaitForSeconds(0.3f);

        ChangeSprites(false, true, false);

        yield return new WaitForSeconds(0.3f);

        ChangeSprites(true, false, false);
        landParticle.emissionRate = 0;
    }
}