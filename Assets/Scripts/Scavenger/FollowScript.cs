using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FollowScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    private NavMeshAgent nma;
    private Animator anim;
    private Rigidbody rb;
    public Rigidbody playerRb;
    public float maxViewDistance = 5;
    public float enemyrange;
    public float Radius;
    public float speed;
    public float fov;
    public float timer;
    public float wandertimer;
    public bool ischasing;
    // vars for AI detection
    private Transform enemyTransform;
    private Transform playerPos;
    public bool playerDetected = false;
    public int detectionTimer;

    //Follow player when in a distance/seen
    //Forget where player is after x time not seeing
    //when player gets close - health or kill


    void Start()
    {
        wandertimer = timer;
        anim = GetComponent<Animator>();
        nma = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemyTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 distance = rb.position - playerRb.position;
        if (distance.magnitude < maxViewDistance && distance.magnitude > 1f)
        {
            transform.LookAt(playerRb.position, Vector3.up);
            //transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation

            //move if distance from target is greater than 1
            rb.AddRelativeForce(new Vector3(0, 0, speed * Time.deltaTime));
        }

        float angle = Vector3.Angle((playerRb.position - transform.position), playerRb.transform.forward);
        if (angle < fov)
        {
            Debug.Log(angle);
        }

        // get player position
        playerPos = GameObject.FindWithTag("Player").transform;

        */
        // enemy range
        var dis = Vector3.Distance(enemyTransform.position, player.transform.position);    // grab distance between enemy and player

        // check is player is within the enemies range
        if (dis <= enemyrange)
        {
            ischasing = true;
        }
        else
        {
            ischasing = false;
            anim.SetBool("FoundPlayer", false);
        }

        if (ischasing == true)
        {
            followPlayer();
        }

        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = wandertimer;
            Vector3 newPos = RandomNavSphere(transform.position, Radius, -1);
            nma.SetDestination(newPos);
        }

    }

    // move enemy
    void followPlayer()
    {
        /*
        transform.LookAt(player.transform.position, Vector3.up);
        enemyTransform.position += enemyTransform.forward * speed * Time.deltaTime;
        */
        ischasing = true;
        anim.SetBool("FoundPlayer", true);
        nma.SetDestination(player.transform.position);
    }

    // kill player if collides with enemy
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.Damage(player.Health);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }


}
