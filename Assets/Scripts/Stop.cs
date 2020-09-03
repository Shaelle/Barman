using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Drink drink = other.GetComponent<Drink>();

        if (drink != null)
        {

            if (levelManager.handDirection == LevelManager.HandDirections.Left) drink.SetDirection(Drink.Directions.Left);
            else drink.SetDirection(Drink.Directions.Right);

            //drink.SetDirection(Drink.Directions.Stop);
            levelManager.DestinationReached(drink);
                
        }
    }
}
