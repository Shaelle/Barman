using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{

    public float speed = 5f;
    [SerializeField] float sideSpeed = 5f;

    [SerializeField] Transform destinationLeft;
    [SerializeField] Transform destinationRight;


    [SerializeField] AudioSource broken;
    [SerializeField] AudioSource start;


    public bool correctOne = false;

    public enum Kinds { Manhattan, Margarita, Mojito, OldFashion, Vodka, Drink1, Drink2, Cola}

    public Kinds kind;


    float currentSpeed;
    float acceleration = 0;

 
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


    public void SetDirection(float initialspeed, float sideAcceleration)
    {
        if (initialspeed == 0) currentSpeed = sideSpeed;

        speed = initialspeed;

        /*if (sideAcceleration != 0)
        {
            Debug.Log(sideAcceleration);


            sideAcceleration = (sideAcceleration > 0) ? 1 : -1;
        }

        acceleration = -sideAcceleration / 20;*/

        acceleration = -sideAcceleration;
    }


    public void Broken()
    {
        broken.Play();
    }

    public void StartMove()
    {
        start.Play();
    }





}
