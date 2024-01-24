using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float speed = 5.0f;
    public Rigidbody bombRb;
    public float throwForce = 10f;
    public float torqueForce = 5f;
    public bool canThrowBomb = false;
    public GameObject bombPrefab;
    private int count;
    private Animator playerAnim;
    public AudioClip bombSound;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    private Queue<GameObject> bombsQueue = new Queue<GameObject>();
    public bool hasLife = false;
    public bool isDead = false;
    private int life;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        bombRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rightInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.D)){
            transform.Translate(Vector3.right * rightInput * Time.deltaTime * speed);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.forward * forwardInput * Time.deltaTime * speed);
        }
        if (Input.GetKeyDown(KeyCode.Space) && canThrowBomb)
        {
            ThrowBomb();
        }
    }

    void ThrowBomb()
    {
        if (bombsQueue.Count > 0)
        {
            GameObject bomb = bombsQueue.Dequeue();
            if (bombRb != null)
            {
                bombRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
                bombRb.AddTorque(Vector3.up * torqueForce, ForceMode.Impulse);
            }

            Destroy(bomb, 10f);
            StartCoroutine(WaitForNextBomb());
        }
        //else
        //{
        //    canThrowBomb = false;
        //}
    }
    private IEnumerator WaitForNextBomb()
    {
        yield return new WaitForSeconds(5f); // Adjust the delay as needed
        canThrowBomb = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MultiBomb"))
        {
            Destroy(other.gameObject);
            for (int i = 0; i < 3; i++)
            {
                Vector3 bombPosition = new Vector3(transform.position.x, bombPrefab.transform.position.y, transform.position.z);
                GameObject bomb = Instantiate(bombPrefab, bombPosition, transform.rotation);
                bombsQueue.Enqueue(bomb);
            }
            canThrowBomb = true;
            ThrowBomb();
        }
        else if (other.gameObject.CompareTag("Lives"))
        {
            Destroy(other.gameObject);
            hasLife = true;
            life++;
        }
        else if (other.gameObject.CompareTag("Bomb"))
        {
            Destroy(other.gameObject);
            Vector3 bombPosition = new Vector3(transform.position.x, bombPrefab.transform.position.y, transform.position.z);
            GameObject bomb = Instantiate(bombPrefab, bombPosition, transform.rotation);
            bombsQueue.Enqueue(bomb);

            if (!canThrowBomb)
            {
                canThrowBomb = true;
                ThrowBomb();
            }
        }
        else if (other.gameObject.CompareTag("Mines"))
        {
            count++;
            Debug.Log("Score: " + count);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("RealBomb"))
        {
            explosionParticle.Play();
            Destroy(other.gameObject);
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            playerAudio.PlayOneShot(bombSound, 1.0f);
            isDead = true;

            if (hasLife)
            {
                playerAnim.SetBool("Death_b", false); // Set back to walk animation
                life--;
                isDead = false;
            }
        }
        else
        {
            Debug.Log("not colliding");
        }
    }
}
