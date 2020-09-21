using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{

    public float speed = 5f;
    [SerializeField] float sideSpeed = 5f;

    float currentSpeed;
    float acceleration = 0;


    TrailRenderer trailRenderer;


    public Vector3 lastStart;


    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = sideSpeed;


    }


    void FixedUpdate()
    {
        if (speed > 0)
        {
            currentSpeed -= acceleration;
            transform.Translate(-speed * Time.deltaTime, 0, currentSpeed * Time.deltaTime);
        }
    }



    public void GoToStart(Vector3 start)
    {

        //trailRenderer.Clear();

        SetDirection(0, 0);
        gameObject.transform.position = start;
        if (trailRenderer != null) trailRenderer.Clear();

    }



    public void SetDirection(float initialspeed, float sideAcceleration)
    {
        if (initialspeed == 0)
        {
            currentSpeed = sideSpeed;
        }
 
        speed = initialspeed;

        acceleration = -sideAcceleration;
    }





}
