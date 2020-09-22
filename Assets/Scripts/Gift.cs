using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Gift : MonoBehaviour
{

    static float fillPercent = 0;

    const string fillSaveName = "GiftFillPercent";

    const int nextScene = 3;
    const float pause = 0.5f;

    [SerializeField] GameObject getGift;

    [SerializeField] AudioSource sound;

    [SerializeField] TextMeshProUGUI day;

    Image image;

    bool adding;


    private void Awake()
    {
        image = getGift.GetComponent<Image>();

        if (image == null) Debug.LogError("Gift's image not found");

        fillPercent = PlayerPrefs.GetFloat(fillSaveName, 0);
        image.fillAmount = fillPercent;


        if (image.fillAmount < 1)
        {
            var temp = image.color;
            //temp.a = 0.9f;
            image.color = temp;
        }


        adding = false;

        sound.Stop();

        day.text = "DAY " + LevelManager.level;

        StartCoroutine(AddingGift());

    }



    IEnumerator AddingGift()
    {

        adding = true;

        // Extra conditions here

        sound.Play();

        if (fillPercent < 1)
        {
            for (int i = 0; i <= 20; i++)
            {
                fillPercent = Mathf.Clamp(image.fillAmount + 0.01f, 0, 1);
                image.fillAmount = fillPercent;

                yield return new WaitForSeconds(0.1f);

                if (image.fillAmount == 1)
                {
                    var temp = image.color;
                    temp.a = 1f;
                    image.color = temp;

                    break;
                }
            }
        }

        sound.Stop();

        PlayerPrefs.SetFloat(fillSaveName, fillPercent);

        adding = false;

        //yield return new WaitForSeconds(pause);

        //if (fillPercent < 1) SceneManager.LoadScene(nextScene);

    }



    public void GetGift()
    {
        if (fillPercent == 1) StartCoroutine(GettingGift());
    }

    IEnumerator GettingGift()
    {
        adding = true;

        fillPercent = 0;

        // Getting gift here

        image.fillAmount = 0;
        PlayerPrefs.SetFloat(fillSaveName, fillPercent);

        var temp = image.color;
        temp.a = 0.5f;
        image.color = temp;

        yield return new WaitForSeconds(pause);

        adding = false;
     
        StartCoroutine(LoadingScene(nextScene));
    }


    public void Skip()
    {
       if (!adding) StartCoroutine(LoadingScene(nextScene));
    }


    IEnumerator LoadingScene(int sceneIndex) // Small pause before loading next scene, so click sound on the button can play
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }


}
