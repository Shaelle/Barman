using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{

    [SerializeField] float speed = 5f;

    public enum Directions { Stop, Forward, Left, Right}

    Directions direction = Directions.Stop;

    public bool correctOne = false;

    public enum Kinds { Manhattan, Margarita, Mojito, OldFashion, Vodka}

    public Kinds kind;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case Directions.Stop:
                break;

            case Directions.Forward:

                transform.Translate(-speed * Time.deltaTime, 0, 0);

                break;

            case Directions.Left:

                transform.Translate(0, 0, -speed * Time.deltaTime);

                break;

            case Directions.Right:

                transform.Translate(0, 0, speed * Time.deltaTime);

                break;

            default:

                Debug.Log("Unknown direction.");

                break;
        }

    }


    public void SetDirection(Directions moveDirection)
    {
        direction = moveDirection;
    }


}
