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

    public Tables.Kind tableKind = Tables.Kind.Default;

    bool tableAviable = false;

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

        if (slot != null)
        {

            //Vector3 slotPos = Camera.main.ScreenToWorldPoint(slot.transform.position);
            //transform.position = new Vector3(slotPos.x, slotPos.y, transform.position.z);

            transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, transform.position.z);
        }


       if(kind == Kinds.Table)
        {
            tableAviable = (PlayerPrefs.GetInt("Table" + tableKind.ToString(), 0) == 1) ? true : false;

            if (tableKind == Tables.Kind.Default)
            {
                tableAviable = true;
            }


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
                    priceTag.text = "Sold";
                }
                else
                {
                    sold = false;
                    priceTag.text = price.ToString();
                }

                break;

            case Kinds.Table:


                if (tableAviable)
                {
                    sold = true;
                    priceTag.text = "Sold";
                }
                else
                {
                    sold = false;
                    priceTag.text = price.ToString();
                }

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

            if (kind == Kinds.Table && tableAviable)
            {
                LevelManager.selectedTable = tableKind;
                PlayerPrefs.SetInt(LevelManager.selectedTableName, (int)tableKind);
            }

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
                priceTag.text = "Sold";
                LevelManager.money -= price;

                break;
            case Kinds.Table:

                PlayerPrefs.SetInt("Table" + tableKind.ToString(), 1);
                tableAviable = true;
                priceTag.text = "Sold";
                LevelManager.money -= price;

                LevelManager.selectedTable = tableKind;
                PlayerPrefs.SetInt(LevelManager.selectedTableName, (int)tableKind);

                //tableAviable = (PlayerPrefs.GetInt("Table" + tableKind.ToString(), 0) == 1) ? true : false;


                break;

            case Kinds.Theme:
                break;
            default:
                break;
        }


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
