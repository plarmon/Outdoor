using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnchor : MonoBehaviour
{
    private Vector3 startingPosition;
    [SerializeField]
    private float maxDistanceTravel;
    [SerializeField] [Range(0,1)]
    private float travelSpeed;
    private float distance;

    private GameManager gm;

    private void Start() {
        startingPosition = transform.position;
        distance = 0;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    private void FixedUpdate()
    {
        if(!gm.GetPaused()) {
            transform.position = new Vector3 (startingPosition.x + (Mathf.Sin(distance) * maxDistanceTravel), startingPosition.y, startingPosition.z);  
            distance += travelSpeed * Time.deltaTime; 
        }
    }
}
