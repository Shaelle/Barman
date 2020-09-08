﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{

    public float speed = 5f;
    [SerializeField] float sideSpeed = 5f;

    [SerializeField] Transform destinationLeft;
    [SerializeField] Transform destinationRight;


    [SerializeField] GameObject broken;


    public bool correctOne = false;

    public enum Kinds { Manhattan, Margarita, Mojito, OldFashion, Vodka}

    public Kinds kind;


    float currentSpeed;
    float acceleration = 0;

 
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = sideSpeed;
        broken.SetActive(false);
    }


    // Update is called once per frame
    void Update()
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

        acceleration = -sideAcceleration / 20;
    }


    public void Broken()
    {
        broken.SetActive(true);
        broken.GetComponent<AudioSource>().Play();
    }



}
