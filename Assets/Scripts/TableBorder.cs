using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBorder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        Drink drink = other.GetComponent<Drink>();

        if (drink != null)
        {
            drink.SetDirection(0, 0);

              //Debug.Log("drink: " + drink.name);
        }
    }
}
