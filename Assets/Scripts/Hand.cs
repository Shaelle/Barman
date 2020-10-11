using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    public Transform target;
    //public Animator animator;

    [SerializeField] GameObject[] hands;
    [SerializeField] GameObject paw;

    [SerializeField] GameObject CenterMark;

    GameObject activeHand;
    Animator activeAnimator;

    Vector3 defaultPosition;

    public bool RemoveToRight = true;
    public bool isRemoving = false;

    public float removeSpeed = 10f;

    [SerializeField] [Range(1, 100)] int pawChance = 25;


    private void Awake()
    {
        defaultPosition = transform.position;
        CenterMark.SetActive(false);
    }



    private void OnEnable()
    {

        transform.position = defaultPosition;

        ChooseArm();
    }

    private void Update()
    {
        if (isRemoving)
        {
            if (RemoveToRight)
            {
                transform.Translate(0, 0, removeSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(0, 0, -removeSpeed * Time.deltaTime);
            }
        }
    }

    void ChooseArm()
    {

        foreach (GameObject hand in hands)
        {
            hand.SetActive(false);
        }


        paw.gameObject.SetActive(false);

        //int n = Random.Range(0, 100); // Disabled paw

        //if (n < pawChance)
        //{
        //    paw.gameObject.SetActive(true);
        //    activeHand = paw;
       // }
       // else
       // {
            int index = Random.Range(0, hands.Length);

            hands[index].gameObject.SetActive(true);

            activeHand = hands[index];
       // }


    }



    public void Grab(Vector3 drinkPos)
    {

        // Debug.Log("drink: " + drinkPos + " target: " + tar)

        transform.position = new Vector3(drinkPos.x, transform.position.y, drinkPos.z);

        activeAnimator = activeHand.GetComponent<Animator>();

        if (activeAnimator != null)
        {
            activeAnimator.Play("Clamp");
        }
    }


    public void ResestAnimation()
    {
        if (activeAnimator != null)
        {
            activeAnimator.Rebind();
        }
    }

}
