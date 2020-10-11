using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    [SerializeField] LevelManager manager;

    [SerializeField] GameObject menuPanel;

    [SerializeField] Sprite on;
    [SerializeField] Sprite off;

    [SerializeField] Image soundImage;
    [SerializeField] Image musicImage;
    [SerializeField] Image vibroImage;

    public static bool sound = true;
    public static bool music = true;
    public static bool vibration = true;

    // Start is called before the first frame update
    void Start()
    {
        sound = (PlayerPrefs.GetInt("Sound", 1) == 1) ? true : false;
        music = (PlayerPrefs.GetInt("Music", 1) == 1) ? true : false;
        vibration = (PlayerPrefs.GetInt("Vibration", 1) == 1) ? true : false;


        HideMenu();

        ShowIcon(soundImage, sound);
        ShowIcon(musicImage, music);
        ShowIcon(vibroImage, vibration);


        if (sound)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }


    void ShowIcon(Image image, bool active)
    {
        if (active)
        {
            image.sprite = on;
        }
        else
        {
            image.sprite = off;
        }
    }



    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0;
    }



    public void HideMenu()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }



    void Toggle(string saveName, Image image, ref bool param)
    {
        param = !param;

        if (param)
        {
            PlayerPrefs.SetInt(saveName, 1);
        }
        else
        {
            PlayerPrefs.SetInt(saveName, 0);
        }

        ShowIcon(image, param);
    }


    public void ToggleSound()
    {
        Toggle("Sound", soundImage, ref sound);

        if (sound)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }

    }


    public void ToggleMusic()
    {
        Toggle("Music", musicImage, ref music);

        if (music)
        {
            manager.StartMusic();
        }
        else
        {
            manager.StopMusic();
        }
    }


    public void ToggleVibration()
    {
        Toggle("Vibration", vibroImage, ref vibration);
    }


}
