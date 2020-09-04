using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{

    [SerializeField] float speed = 5f;
    [SerializeField] float sideSpeed = 5f;

    [SerializeField] Transform destinationLeft;
    [SerializeField] Transform destinationRight;

    public enum Directions { Stop, Forward, Left, Right}

    Directions direction = Directions.Stop;

    public bool correctOne = false;

    public enum Kinds { Manhattan, Margarita, Mojito, OldFashion, Vodka}

    public Kinds kind;


    float currentSpeed;
    float acceleration = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = sideSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        
        float drag;

        float CalculateDrag()
        {
            currentSpeed -= acceleration;

            //if (currentSpeed < 0) currentSpeed = 0.01f;

            return currentSpeed * Time.deltaTime;
        }

        switch (direction)
        {
            case Directions.Stop:

                currentSpeed = sideSpeed;

                break;

            case Directions.Forward:

                transform.Translate(-speed * Time.deltaTime, 0, 0);

                break;

            case Directions.Left:

                drag = CalculateDrag();
                transform.Translate(-speed * Time.deltaTime, 0, +drag);

                break;

            case Directions.Right:

                drag = CalculateDrag();
                transform.Translate(-speed * Time.deltaTime, 0, -drag);

                break;

            default:

                Debug.Log("Unknown direction.");

                break;
        }

    }


    public void SetDirection(Directions moveDirection)
    {

        float CalculateAcceleration(Transform destination)
        {
            float dist;
            dist = Mathf.Abs(transform.position.z - destination.position.z) / 15;
            return Mathf.Clamp(dist,0,0.08f);
        }

        switch (moveDirection)
        {

            case Directions.Left:

                acceleration =  CalculateAcceleration(destinationLeft);

                //Debug.Log("left; acceleration: " + acceleration);
                break;

            case Directions.Right:

                acceleration =  CalculateAcceleration(destinationRight);

               // Debug.Log("right; acceleration: " + acceleration);
                break;

            default:
                acceleration = 0;
                break;
        }


        direction = moveDirection;
    }


}
