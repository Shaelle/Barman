using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miss : MonoBehaviour
{

    [SerializeField] LevelManager levelManager;

    private void OnTriggerExit(Collider other)
    {
        if (levelManager.isRemovingDrink) return;

        Drink drink = other.GetComponent<Drink>();

        if (drink != null) drink.Broken();

        levelManager.Miss();
    }
}
