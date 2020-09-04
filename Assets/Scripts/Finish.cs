using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    [SerializeField] LevelManager levelmanager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Finished");

        Drink drink = other.GetComponent<Drink>();

        if (drink != null) drink.SetDirection(Drink.Directions.Stop);

        levelmanager.UpdateTable();

        //SceneManager.LoadScene(0);
    }
}
