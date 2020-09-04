using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LevelManager : MonoBehaviour
{


    Vector2 cursorPos;

    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;


    [SerializeField] GameObject[] drinkSlots;
    [SerializeField] Drink[] drinks;
    [SerializeField] Rotation[] targetMarks;

    [SerializeField] TextMeshProUGUI goodText;
    [SerializeField] TextMeshProUGUI badText;


    int goodCount = 0;
    int badCount = 0;

    bool isPushing = false;

    public enum HandDirections { Left, Right}

    public HandDirections handDirection;



    // Start is called before the first frame update
    void Start()
    {
        UpdateTable();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMove(InputValue value) // Drink being pushed
    {
     
        Vector2 newPos = value.Get<Vector2>();

        if (newPos.y > cursorPos.y)
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(newPos);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                Drink drink = hitObject.GetComponent<Drink>();

                if (drink != null && !isPushing)
                {
                    drink.SetDirection(Drink.Directions.Forward);
                    isPushing = true;
                }

            }
        }

        cursorPos = newPos;
    }


    void PlaceDrinks()
    {
        Drink[] shuffledDrinks = ShuffleDrinks(drinks); // Shuffling drink on the table

        for (int i = 0; i < drinkSlots.Length; i++)
        {
            shuffledDrinks[i].transform.position = drinkSlots[i].transform.position;
            shuffledDrinks[i].correctOne = false;
        }

        int n = Random.Range(0, shuffledDrinks.Length - 1); // Randomly selecting one drink as "ordered by client"

        shuffledDrinks[n].correctOne = true;

        Drink.Kinds kind = shuffledDrinks[n].kind;

        foreach (Rotation target in targetMarks)
        {
            target.gameObject.SetActive(target.kind == kind);
        }



    }



    private Drink[] ShuffleDrinks(Drink[] drinks)
    {
        Drink[] newArray = drinks.Clone() as Drink[];

        for (int i = 0; i < newArray.Length; i++)
        {
            Drink tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }



    void SetHandDirection() // Randomly setting directions, from which a hand is coming
    {
        int rand = Random.Range(0, 100);

        if (rand > 50)
        {
            handDirection = HandDirections.Right;
            leftHand.SetActive(false);
            rightHand.SetActive(true);
        }
        else
        {
            handDirection = HandDirections.Left;
            leftHand.SetActive(true);
            rightHand.SetActive(false);
        }

    }


    public void DestinationReached(Drink drink) // Reached stop ("hand") trigger
    {
        isPushing = false;
        CheckHit(drink);
        UpdateScore();
    }


    public void UpdateTable()
    {
        SetHandDirection();
        PlaceDrinks();
    }


    void UpdateScore()
    {
        goodText.text = goodCount.ToString();
        badText.text = badCount.ToString();
    }


    public void CheckHit(Drink drink)
    {
        if (drink.correctOne)
        {
            goodCount++;
        }
        else
        {
            badCount++;
        }
    }

}
