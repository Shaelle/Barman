using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Stars : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coinsText;

    bool claimed = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
    }


    public void ClaimBonus()
    {
        if (!claimed) StartCoroutine(ClaimingBonus());
    }


    IEnumerator ClaimingBonus()
    {

        claimed = true;

        // Some actions here

        LevelManager.money *= 2;

        PlayerPrefs.SetInt(LevelManager.moneySaveName, LevelManager.money);

        UpdateCoinsText();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(0);
           
    }

    public void Skip()
    {
        SceneManager.LoadScene(0);
    }

    void UpdateCoinsText()
    {
        coinsText.text = LevelManager.money.ToString();
    }



}
