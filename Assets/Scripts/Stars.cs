using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stars : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadBar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator LoadBar()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }
}
