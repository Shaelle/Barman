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


    bool isVisible = false;

    

    public enum Kinds { Drink, Table, Theme}

    public Kinds kind = Kinds.Drink;

    public RectTransform slot;

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

        if (slot != null)
        {

            //Vector3 slotPos = Camera.main.ScreenToWorldPoint(slot.transform.position);
            //transform.position = new Vector3(slotPos.x, slotPos.y, transform.position.z);

            transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, transform.position.z);
        }

    }


    void UpdatePriceTag()
    {
        switch (kind)
        {
            case Kinds.Drink:

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

                break;

            case Kinds.Table:

                sold = false;
                priceTag.text = price.ToString();

                break;

            case Kinds.Theme:
                break;
            default:
                break;
        }



    }


    public void Select()
    {
        if (isVisible)
        {
            if (shop.selectedProduct != null) shop.selectedProduct.Deselect();
            transform.localScale *= 1.5f;

            shop.selectedProduct = this;
        }
    }


    public void Deselect()
    {
        if (isVisible)
        {
            transform.localScale = defaultScale;
        }
    }


    public void Show()
    {
        isVisible = true;

        drink.gameObject.SetActive(true);
        UpdatePriceTag();
    }


    public void Hide()
    {

        isVisible = false;

        drink.gameObject.SetActive(false);
        priceTag.text = "";
    }


    public void Sell()
    {
        switch (kind)
        {
            case Kinds.Drink:

                PlayerPrefs.SetInt(drink.kind.ToString(), 1);
                sold = true;
                priceTag.text = "sold";
                LevelManager.money -= price;

                PlayerPrefs.SetInt(LevelManager.moneySaveName, LevelManager.money);

                break;
            case Kinds.Table:


                break;
            case Kinds.Theme:
                break;
            default:
                break;
        }


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
