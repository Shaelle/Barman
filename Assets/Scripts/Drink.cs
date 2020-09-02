using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{

    [SerializeField] float speed = 5f;

    bool isMoving = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) transform.Translate(-speed * Time.deltaTime, 0, 0);
    }


    public void Pushing()
    {
        isMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
    }

}
