using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public int price = 0;

    bool sold = false;

    Rotation drink;
    TextMesh priceTag;

    private void Awake()
    {
        drink = transform.GetChild(0).gameObject.GetComponent<Rotation>();
        priceTag = transform.GetChild(1).gameObject.GetComponent<TextMesh>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(drink.kind.ToString(), 0) == 1) // Drink already purchased - remove from list
        {
            sold = true;
            priceTag.text = "sold";
        }
        else
        {
            sold = false;
            priceTag.text = price.ToString();
        }
    }


    public void Sell()
    {
        PlayerPrefs.SetInt(drink.kind.ToString(), 1);
        sold = true;
        priceTag.text = "sold";
    }

}
