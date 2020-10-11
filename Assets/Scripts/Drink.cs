using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{

    public bool available = false;

    public bool starterPack = false;

    public int nom = 0;

    public float speed = 5f;
    [SerializeField] float sideSpeed = 5f;

    [SerializeField] Transform destinationLeft;
    [SerializeField] Transform destinationRight;


    [SerializeField] AudioSource broken;
    [SerializeField] AudioSource start;


    Rigidbody body;


    public bool correctOne = false;

    public enum Kinds { Manhattan, Margarita, Mojito, OldFashion, Vodka, Drink1, Drink2, Cola, Cocoa, Coffee}

    public Kinds kind;


    float currentSpeed;
    float acceleration = 0;


    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        if (body == null) Debug.LogError(name + " have no rigidbody.");
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


    public void EnableGravity()
    {
        body.useGravity = true;
    }


    public void DisableGravity()
    {
        body.useGravity = false;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
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
