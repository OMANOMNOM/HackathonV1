using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public Rigidbody playerRb;
    public float maxViewDistance = 5;
    public float speed;
    public float fov;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = rb.position - playerRb.position;
        if (distance.magnitude < maxViewDistance && distance.magnitude > 1f)
        {
            transform.LookAt(playerRb.position, Vector3.up);
            //transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation

            //move if distance from target is greater than 1
            rb.AddRelativeForce(new Vector3(0, 0, speed * Time.deltaTime));
        }

        float angle = Vector3.Angle((playerRb.position - rb.position), playerRb.transform.forward);
        
            Debug.Log(angle);
        
    }
}
