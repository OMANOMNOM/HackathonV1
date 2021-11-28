using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationMovement : MonoBehaviour
{
    [Header("Movement")]
    public GameObject MoveToObject;
    public float TimeUntilNewPosition;
    private float OGTimeUntilNewPosition;
    public float speed;

    public GameObject[] PossibleLocations;

    public void Start()
    {
        OGTimeUntilNewPosition = TimeUntilNewPosition;
        GenerateNewPosition();
    }

    public void Update()
    {
        if(TimeUntilNewPosition <= 0)
        {
            TimeUntilNewPosition = OGTimeUntilNewPosition;
            GenerateNewPosition();
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(gameObject.transform.position, MoveToObject.transform.position, step);
            TimeUntilNewPosition -= Time.deltaTime;
        }
    }

    public void GenerateNewPosition()
    {
        int rng = Random.Range(0, 10);
        switch(rng)
        {
            case 0:
                MoveToObject.transform.position = PossibleLocations[0].transform.position;
                break;
            case 1:
                MoveToObject.transform.position = PossibleLocations[1].transform.position;
                break;
            case 2:
                MoveToObject.transform.position = PossibleLocations[2].transform.position;
                break;
            case 3:
                MoveToObject.transform.position = PossibleLocations[3].transform.position;
                break;
            case 4:
                MoveToObject.transform.position = PossibleLocations[4].transform.position;
                break;
            case 5:
                MoveToObject.transform.position = PossibleLocations[5].transform.position;
                break;
            case 6:
                MoveToObject.transform.position = PossibleLocations[6].transform.position;
                break;
            case 7:
                MoveToObject.transform.position = PossibleLocations[7].transform.position;
                break;
            case 8:
                MoveToObject.transform.position = PossibleLocations[8].transform.position;
                break;
            case 9:
                MoveToObject.transform.position = PossibleLocations[9].transform.position;
                break;
            case 10:
                MoveToObject.transform.position = PossibleLocations[10].transform.position;
                break;
        }
    }
}
