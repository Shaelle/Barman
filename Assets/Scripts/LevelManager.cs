using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{


    Vector2 cursorPos;

    bool isPushing = false;

    public enum HandDirections { Left, Right}

    public HandDirections handDirection;

    // Start is called before the first frame update
    void Start()
    {
        SetHandDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMove(InputValue value) // Drink being pushed
    {
     
        Vector2 newPos = value.Get<Vector2>();

        if (newPos.y > cursorPos.y)
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(newPos);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                Drink drink = hitObject.GetComponent<Drink>();

                if (drink != null && !isPushing)
                {
                    drink.SetDirection(Drink.Directions.Forward);
                    isPushing = true;
                }

            }
        }

        cursorPos = newPos;
    }



    void SetHandDirection() // Randomly setting directions, from which a hand is coming
    {
        int rand = Random.Range(0, 100);

        if (rand > 50) handDirection = HandDirections.Right;
        else handDirection = HandDirections.Left;

    }


    public void DestinationReached(Drink drink) // Reached stop ("hand") trigger
    {
        SetHandDirection();
        isPushing = false;
    }
}
