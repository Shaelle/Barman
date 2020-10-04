using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    public Transform target;
    //public Animator animator;

    [SerializeField] GameObject hand;
    [SerializeField] GameObject paw;

    GameObject activeHand;

    [SerializeField] [Range(1, 100)] int pawChance = 25;
    


    // Start is called before the first frame update
    void Start()
    {
        ChooseArm();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChooseArm()
    {
        int n = Random.Range(0, 100);

        if (n < pawChance)
        {
            hand.gameObject.SetActive(false);
            paw.gameObject.SetActive(true);

            activeHand = paw;
        }
        else
        {
            hand.gameObject.SetActive(true);
            paw.gameObject.SetActive(false);

            activeHand = hand;
        }
    }

    public void Grab()
    {
        Animator animator = activeHand.GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("Clamp");
            animator.Rebind();
        }
    }
}
