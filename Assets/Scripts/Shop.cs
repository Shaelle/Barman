using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coinsText;

    [SerializeField] ParticleSystem selectionParticles;

    [SerializeField] bool usingDiamonds = false;

    [SerializeField] Sprite drinkShop;
    [SerializeField] Sprite tableShop;

    [SerializeField] Image shopImage;

    [SerializeField] Product[] drinks;


    Vector2 pointerPos;

    public Product selectedProduct;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
    }


    void UpdateCoinsText()
    {
        if (usingDiamonds)
        {
            coinsText.text = LevelManager.diamonds.ToString();
        }
        else
        {
            coinsText.text = LevelManager.money.ToString();
        }
    }


    public void ReturnToBar()
    {
        StartCoroutine(LoadingScene(0));
    }


    public void SwitchToDrinks()
    {
        if (shopImage.sprite == drinkShop) return;

        shopImage.sprite = drinkShop;

        foreach (Product product in drinks)
        {
            product.Show();
        }
    }


    public void SwitchToTables()
    {
        if (shopImage.sprite == tableShop) return;

        shopImage.sprite = tableShop;

        foreach (Product product in drinks)
        {
            product.Hide();
        }

    }


    IEnumerator LoadingScene(int sceneIndex) // Small pause before loading next scene, so click sound on the button can play
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }


    void OnMove(InputValue value)
    {
        pointerPos = value.Get<Vector2>();
    }


    void OnPress(InputValue value) 
    {
        /*
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(pointerPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                Product product = hitObject.GetComponent<Product>();

                if (product != null)
                {
                    if (selectedProduct != null) selectedProduct.Deselect();

                    selectedProduct = product;
                    selectedProduct.Select();

                    //selectionParticles.transform.position = new Vector3(product.transform.position.x, product.transform.position.y + 0.3f, product.transform.position.z);
                    //if (!selectionParticles.gameObject.activeSelf) selectionParticles.gameObject.SetActive(true);
                    
                }
               
            }

        }
        */
    }


    public void Sell()
    {
        if (selectedProduct != null && LevelManager.money >= selectedProduct.price)
        {
            selectedProduct.Sell();
            UpdateCoinsText();
            selectedProduct = null;
            //selectionParticles.gameObject.SetActive(false);
        }
    }


    public void SellForDiamonds()
    {
        if (selectedProduct != null && LevelManager.diamonds >= selectedProduct.price)
        {
            selectedProduct.SellForDiamonds();
            UpdateCoinsText();
            selectedProduct = null;
            //selectionParticles.gameObject.SetActive(false);
        }
    }

}
