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
        StartCoroutine(LoadingScene(0));
    }


    IEnumerator LoadingScene(int sceneIndex) // Small pause before loading next scene, so click sound on the button can play
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }
}
