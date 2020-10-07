using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Product : MonoBehaviour
{
    public int price = 0;

    bool sold = false;

    Rotation drink;
    [SerializeField] TextMeshProUGUI priceTag;

    [SerializeField] Shop shop;

    Vector3 defaultScale;

    

    public enum Kinds { Drink, Table, Theme}

    public Kinds kind = Kinds.Drink;

    private void Awake()
    {
        drink = transform.GetChild(0).gameObject.GetComponent<Rotation>();
        //priceTag = transform.GetChild(1).gameObject.GetComponent<TextMesh>();
        defaultScale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePriceTag();
    }


    void UpdatePriceTag()
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


    public void Select()
    {
        if (shop.selectedProduct != null) shop.selectedProduct.Deselect();
        transform.localScale *= 1.5f;

        shop.selectedProduct = this;
    }


    public void Deselect()
    {
        transform.localScale = defaultScale;
    }


    public void Show()
    {
        drink.gameObject.SetActive(true);
        UpdatePriceTag();
    }


    public void Hide()
    {
        drink.gameObject.SetActive(false);
        priceTag.text = "";
    }


    public void Sell()
    {
        PlayerPrefs.SetInt(drink.kind.ToString(), 1);
        sold = true;
        priceTag.text = "sold";
        LevelManager.money -= price;

        PlayerPrefs.SetInt(LevelManager.moneySaveName, LevelManager.money);

    }

    public void SellForDiamonds()
    {
        //PlayerPrefs.SetInt(drink.kind.ToString(), 1);
        sold = true;
        priceTag.text = "sold";
        LevelManager.diamonds -= price;

        PlayerPrefs.SetInt(LevelManager.diamondsSaveName, LevelManager.diamonds);
    }

}
