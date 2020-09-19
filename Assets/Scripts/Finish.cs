using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    [SerializeField] LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {

        Drink drink = other.GetComponent<Drink>();

        if (drink != null)
        {
            drink.SetDirection(0, 0);

            levelManager.DestinationReached(drink);

            //Debug.Log("drink: " + drink.name);
        }
        else
        {
            Tracer tracer = other.GetComponent<Tracer>();

            if (tracer != null) tracer.GoToStart(tracer.lastStart);
        }     
    }
}
