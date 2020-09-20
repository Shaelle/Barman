using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coinsText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateCoinsText()
    {
        coinsText.text = LevelManager.money.ToString();
    }

    public void ReturnToBar()
    {
        SceneManager.LoadScene(0);
    }
}
