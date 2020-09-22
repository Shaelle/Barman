using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coinsText;

    Vector2 pointerPos;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
    }


    void UpdateCoinsText()
    {
        coinsText.text = LevelManager.money.ToString();
    }

    public void ReturnToBar()
    {
        StartCoroutine(LoadingScene(0));
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
                    if (LevelManager.money >= product.price)
                    {
                        LevelManager.money -= product.price;
                        product.Sell();
                        UpdateCoinsText();
                    }
                    
                }
               
            }

        }
    }
}
