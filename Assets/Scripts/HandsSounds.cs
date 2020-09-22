using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsSounds : MonoBehaviour
{

    [SerializeField] AudioSource ding;
    [SerializeField] AudioSource hello1;
    [SerializeField] AudioSource hello2;

    //[SerializeField] [Range(0, 100)] int hello1Chance;


    public void Play()
    {
        int n = Random.Range(0, 100);

        if (n < 40)
        {
            hello1.Play();
        }
        else if (n < 70)
        {
            hello2.Play();
        }
        else
        {
            ding.Play();
        }
    }


}
