﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    [SerializeField] LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        Drink drink = other.GetComponent<Drink>();

        if (drink != null) drink.SetDirection(Drink.Directions.Stop);

        levelManager.DestinationReached(drink);

        levelManager.UpdateTable();

        //SceneManager.LoadScene(0);
    }
}