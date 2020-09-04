using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    [SerializeField] float speed = 5f;
    public Drink.Kinds kind;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
