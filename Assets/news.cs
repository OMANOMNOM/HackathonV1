using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class news : MonoBehaviour
{
    private float baseHeight = 2;
    private float curHeight;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        baseHeight = transform.position.y;   
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
            time = 0.0f;
        curHeight = baseHeight + Mathf.Sin(time);
        transform.position = new Vector3(transform.position.x, curHeight, transform.position.z);
    }
}
