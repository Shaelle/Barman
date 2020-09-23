using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] GameObject noThanks;

    [SerializeField] Image star1;
    [SerializeField] Image star2;
    [SerializeField] Image star3;

    [SerializeField] TextMeshProUGUI day;

    bool claimed = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
        noThanks.SetActive(false);

        star1.fillAmount = 0;
        star2.fillAmount = 0;
        star3.fillAmount = 0;

        day.text = "DAY " + (LevelManager.level - 1);

        StartCoroutine(ShowRating());

        StartCoroutine(ShowNoThanks());

    }


    IEnumerator ShowRating()
    {
        StartCoroutine(FillStar(star1));

        yield return new WaitForSeconds(0.2f);

        if (LevelManager.rating >= 2) StartCoroutine(FillStar(star2));

        yield return new WaitForSeconds(0.2f);

        if (LevelManager.rating == 3) StartCoroutine(FillStar(star3));
    }


    IEnumerator FillStar(Image star)
    {
        for (int i = 0; i <= 100; i++)
        {

            float fillPercent = Mathf.Clamp(star.fillAmount + 0.01f, 0, 1);
            star.fillAmount = fillPercent;

            yield return new WaitForSeconds(0.01f);
        }

        star.fillAmount = 1;
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
        SaveMoney();
        StartCoroutine(LoadingScene(0));
    }


    IEnumerator ClaimingBonus()
    {

        claimed = true;

        // Some actions here

        LevelManager.moneyLastLevel *= 2;

        SaveMoney();

        UpdateCoinsText();

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(LoadingScene(0));
           
    }

    public void Skip()
    {
        SaveMoney();
        SceneManager.LoadScene(0);
    }


    void SaveMoney()
    {
        LevelManager.money += LevelManager.moneyLastLevel;
        PlayerPrefs.SetInt(LevelManager.moneySaveName, LevelManager.money);
    }

    void UpdateCoinsText()
    {
        coinsText.text = LevelManager.moneyLastLevel.ToString();
    }



    IEnumerator LoadingScene(int sceneIndex) // Small pause before loading next scene, so click sound on the button can play
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }



}
