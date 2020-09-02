using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{


    Vector2 cursorPos;

    bool isPushing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMove(InputValue value)
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
                    drink.Pushing();
                    //isPushing = true;
                }

            }
        }

        cursorPos = newPos;


    }
}
