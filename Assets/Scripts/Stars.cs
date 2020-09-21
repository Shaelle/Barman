using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Stars : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] GameObject noThanks;

    bool claimed = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
        noThanks.SetActive(false);
        StartCoroutine(ShowNoThanks());
    }

    IEnumerator ShowNoThanks()
    {
        yield return new WaitForSeconds(1.5f);
        noThanks.SetActive(true);
    }


    public void ClaimBonus()
    {
        if (!claimed) StartCoroutine(ClaimingBonus());
    }


    public void NoThanks()
    {
        StartCoroutine(LoadingScene(0));
    }


    IEnumerator ClaimingBonus()
    {

        claimed = true;

        // Some actions here

        LevelManager.money *= 2;

        PlayerPrefs.SetInt(LevelManager.moneySaveName, LevelManager.money);

        UpdateCoinsText();

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(LoadingScene(0));
           
    }

    public void Skip()
    {
        SceneManager.LoadScene(0);
    }

    void UpdateCoinsText()
    {
        coinsText.text = LevelManager.money.ToString();
    }



    IEnumerator LoadingScene(int sceneIndex) // Small pause before loading next scene, so click sound on the button can play
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }



}
