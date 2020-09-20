using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gift : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadStars());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator LoadStars()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(3);
    }
}
