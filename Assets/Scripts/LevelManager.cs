using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int money = 0;

    public static int level = 1;

    public static int updgrades = 0;
    public static int upgradeParts = 0;

    const int fullUpgradeParts = 5;

    Vector2 cursorPos;

    [Header("General")]

    [SerializeField] GameObject leftHand; //TODO: array of new prefabs
    [SerializeField] GameObject leftHand2;
    [SerializeField] GameObject rightHand;
    [SerializeField] GameObject rightHand2;


    [SerializeField] GameObject leftTrigger;
    [SerializeField] GameObject leftTrigger2;
    [SerializeField] GameObject rightTrigger;
    [SerializeField] GameObject rightTrigger2;

    [Header("Drinks")]

    [SerializeField] float drinkSpeed = 5f;

    [SerializeField] GameObject[] drinkSlots;

    [SerializeField] Drink[] drinks;
    [SerializeField] Sprite[] drinkIcons;

    Drink[] shuffledDrinks;


    [SerializeField] Rotation[] targetMarks;

    [Header("UI")]

    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] TextMeshProUGUI updgradesText;
    [SerializeField] TextMeshProUGUI updgradesPartsText;
    [SerializeField] GameObject getUpdgradeButton;

    [SerializeField] Image drinkHint;

    [SerializeField] GameObject caseButton;

    [SerializeField] GameObject nextlevelButton;
    TextMeshProUGUI nextLevelLabel;

    [SerializeField] GameObject finishedLabel;

    [Header("Particles")]

    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem wrongParticles;
    [SerializeField] ParticleSystem smileParticles;
    [SerializeField] ParticleSystem angryParticles;

    [Header("Audio")]

    [SerializeField] AudioSource startMelody;
    [SerializeField] AudioSource thanks;

    [Header("Coins")]

    [SerializeField] int minCoins = 10;
    [SerializeField] int maxCoins = 40;

    [SerializeField] int minLevelBonus = 60;
    [SerializeField] int maxLevelBonus = 150;

    [SerializeField] int bonusThreshold = 3;

    [Header("Other")]

    [SerializeField] [Range(1, 100)] int thanksChance = 40;

    [SerializeField] float sensitivity = 10;

    [SerializeField] Tracer[] tracers;


    int goodCount = 0;
    int badCount = 0;
    int brokenCount = 0;

    bool isPushing = false;
    bool pushPause = false;


    bool isPressed = false;
    Drink touchedDrink;
    Vector2 touchedStart;

    bool isGetUpdgrade = false;

    bool levelActive = false;
   

    [SerializeField] int minDrinksForNextLevel = 4;
    [SerializeField] int maxDrinksForNextLevel = 7;


    int drinksForNextLevel = 5;



    private void Awake()
    {
        nextLevelLabel = nextlevelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (drinkSlots.Length > drinks.Length) Debug.LogWarning("Number of drinks should be equal or bigger than the number of slots! Slots: " + drinkSlots.Length+", drinks: " +drinks.Length);

        Initlevel();

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void Initlevel()
    {

        levelActive = false;

        startMelody.gameObject.SetActive(true);


        goodCount = 0;
        badCount = 0;
        brokenCount = 0;
       
        caseButton.SetActive(true);
        finishedLabel.SetActive(false);


        if (nextLevelLabel == null) Debug.LogError("No text component for level label");

        nextLevelLabel.text = "Day " + level.ToString();
        nextlevelButton.SetActive(true);

        ResetLevel();

        moneyText.text = money.ToString();

        drinksForNextLevel = Random.Range(minDrinksForNextLevel, maxDrinksForNextLevel);


    }


    public void StartLevel()
    {

        levelActive = true;

        startMelody.gameObject.SetActive(false);


        //drinkHint.gameObject.SetActive(true);

        caseButton.SetActive(false);
        finishedLabel.SetActive(false);

        nextlevelButton.SetActive(false);

        isPushing = false;

        foreach (Drink drink in drinks)
        {
            drink.gameObject.SetActive(true);
        }


        ShuffleDrinks(drinks);
       
        UpdateTable();
        UpdateScore();
       
    }


    public void FinishLevel()
    {

        if (badCount + brokenCount <= bonusThreshold)
        {
            //money += Random.Range(minLevelBonus, maxLevelBonus);
            //UpdateScore();
        }

        levelActive = false;
        StartCoroutine(Finishinglevel());
    }



    IEnumerator Finishinglevel()
    {

        //caseButton.SetActive(true);

        ResetLevel();

        winParticles.Play();

        // some animations here

        yield return new WaitForSeconds(1.5f);
     
        finishedLabel.SetActive(true);

        isGetUpdgrade = true;

        updgradesText.gameObject.SetActive(true);
        updgradesPartsText.gameObject.SetActive(true);
        getUpdgradeButton.SetActive(true);

        updgradesText.text = updgrades.ToString();
        updgradesPartsText.text = upgradeParts.ToString() + " / " + fullUpgradeParts;

        yield return new WaitForSeconds(1.5f);

        winParticles.Stop();


        //Initlevel();

    }





    void ResetLevel()
    {

        isPushing = true;

        rightHand.SetActive(false);
        rightTrigger.SetActive(false);

        drinkHint.gameObject.SetActive(false);

        leftHand.SetActive(false);
        leftTrigger.SetActive(false);


        rightHand2.SetActive(false);
        rightTrigger2.SetActive(false);

        leftHand2.SetActive(false);
        leftTrigger2.SetActive(false);

        updgradesText.gameObject.SetActive(false);
        updgradesPartsText.gameObject.SetActive(false);
        getUpdgradeButton.SetActive(false);

        winParticles.Stop();
        wrongParticles.Stop();

        smileParticles.Stop();
        angryParticles.Stop();


        foreach (Drink drink in drinks)
        {
            drink.SetDirection(0, 0);
            drink.transform.position = Vector3.zero;
            drink.gameObject.SetActive(false);
        }


        foreach (Rotation mark in targetMarks)
        {
            mark.gameObject.SetActive(false);
        }


        foreach (Tracer tracer in tracers)
        {
            TracerReset(tracer);
        }

    }



    public void GoToShop()
    {
        SceneManager.LoadScene(1);
    }



    void OnPress(InputValue value)
    {
        isPressed = value.isPressed;

        if (!value.isPressed && touchedDrink != null)
        {

            

            if (!isPushing && cursorPos.y > touchedStart.y)
            {

                float delta = cursorPos.x - touchedStart.x;
                float distance = cursorPos.y - touchedStart.y;
                float acceleration = (delta / distance) / sensitivity;

                Time.timeScale = 1;

                
                //touchedDrink.SetDirection(drinkSpeed, delta);
                touchedDrink.SetDirection(drinkSpeed, acceleration);
                touchedDrink.StartMove();
                isPushing = true;

                foreach (Tracer tracer in tracers)
                {
                    TracerReset(tracer);
                }
                
                
            }

            touchedDrink = null;

        }
    }




    public void TracerReset(Tracer tracer)
    {


        if (touchedDrink != null)
        {
            tracer.GoToStart(touchedDrink.transform.position);
            tracer.lastStart = touchedDrink.transform.position;
        }

    }


    IEnumerator SendTracer()
    {

        while (touchedDrink != null)
        {

            for (int i = 0; i < tracers.Length; i++)
            {

                TracerReset(tracers[i]);

                float delta = cursorPos.x - touchedStart.x;
                float distance = cursorPos.y - touchedStart.y;
                float acceleration = (delta / distance) / sensitivity;
                tracers[i].SetDirection(drinkSpeed, acceleration);

                yield return new WaitForSeconds(0.4f);

                //TracerReset(tracers[i]);

            }
        }
    }


    void OnMove(InputValue value) // Drink being pushed
    {

        if (isPressed)
        {

            Vector2 newPos = value.Get<Vector2>();

            if (touchedDrink == null)
            {

                RaycastHit hit;

                Ray ray = Camera.main.ScreenPointToRay(newPos);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;

                    Drink drink = hitObject.GetComponent<Drink>();

                    if (drink != null)
                    {
                        touchedDrink = drink;
                        touchedStart = newPos;

                        StartCoroutine(SendTracer());
                    }

                }

            }

            cursorPos = newPos;
        }
    }



    void PlaceDrinks()
    {

        foreach (Drink drink in drinks)
        {
            drink.transform.position = Vector3.zero;
            drink.transform.rotation = Quaternion.identity;
            drink.SetDirection(0, 0);
        }

        
        for (int i = 0; i < drinkSlots.Length; i++)
        {
            shuffledDrinks[i].transform.position = drinkSlots[i].transform.position;
            shuffledDrinks[i].correctOne = false;
        }

        int n = Random.Range(0, drinkSlots.Length - 1); // Randomly selecting one drink as "ordered by client"

        shuffledDrinks[n].correctOne = true;

        drinkHint.sprite = drinkIcons[n];

        Drink.Kinds kind = shuffledDrinks[n].kind;

        foreach (Rotation target in targetMarks)
        {
            target.gameObject.SetActive(target.kind == kind);
        }



    }



    private void ShuffleDrinks(Drink[] drinks)
    {
        Drink[] newArray = drinks.Clone() as Drink[];

        for (int i = 0; i < newArray.Length; i++)
        {
            Drink tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        shuffledDrinks = newArray;
    }



    void SetHandDirection() // Randomly setting directions, from which a hand is coming
    {
        int rand = Random.Range(0, 100);


        if (rand < 25) // 0 - 25 far right
        {
            rightHand.SetActive(true); //TODO: simplify with new prefabs
            rightTrigger.SetActive(true);

            leftHand.SetActive(false);          
            leftTrigger.SetActive(false);

            rightHand2.SetActive(false);
            rightTrigger2.SetActive(false);

            leftHand2.SetActive(false);
            leftTrigger2.SetActive(false);

        }
        else if (rand < 50) // 25 - 50 far left
        {

            leftHand.SetActive(true);
            leftTrigger.SetActive(true);

            rightHand.SetActive(false);           
            rightTrigger.SetActive(false);

            rightHand2.SetActive(false);
            rightTrigger2.SetActive(false);

            leftHand2.SetActive(false);
            leftTrigger2.SetActive(false);
        }
        else if (rand < 75) // 50 - 75 close right
        {

            leftHand.SetActive(false);
            leftTrigger.SetActive(false);

            rightHand.SetActive(false);
            rightTrigger.SetActive(false);

            rightHand2.SetActive(true);
            rightTrigger2.SetActive(true);

            leftHand2.SetActive(false);
            leftTrigger2.SetActive(false);
        }
        else // rest - close left
        {
            leftHand.SetActive(false);
            leftTrigger.SetActive(false);

            rightHand.SetActive(false);
            rightTrigger.SetActive(false);

            rightHand2.SetActive(false);
            rightTrigger2.SetActive(false);

            leftHand2.SetActive(true);
            leftTrigger2.SetActive(true);
        }

    }


    public void DestinationReached(Drink drink) // Reached stop ("hand") trigger
    {
        Debug.Log("Destination Reached");

        StartCoroutine(ResetPushing());
        //isPushing = false;

        CheckHit(drink);
        UpdateScore();

        UpdateTable();

    }


    public void Miss()
    {
        if (isPushing) // prevent false alarms when drinks is reordering.
        {
            StartCoroutine(ResetPushing());
            //isPushing = false;
            //badCount++;
            brokenCount++;

            wrongParticles.Play();
            angryParticles.Play();

            UpdateScore();

            UpdateTable();
        }
    }


    public void UpdateTable()
    {
        if (levelActive)
        {
            SetHandDirection();
            PlaceDrinks();
        }
    }


    void UpdateScore()
    {

        progressText.text = goodCount.ToString() + " / " + drinksForNextLevel.ToString();
        moneyText.text = money.ToString();


        if (goodCount >= drinksForNextLevel)
        {


            level++;
            FinishLevel();

            //ShuffleDrinks(drinks); // New level - shuffling drinks.
        }


    }


    IEnumerator ResetPushing() // Pause after finishing pushing to prevent accidental activation
    {

        yield return new WaitForSeconds(0.1f);
        isPushing = false;
    }


    public void CheckHit(Drink drink)
    {
        Debug.Log("Check hit " + drink.name);

        if (drink.correctOne)
        {
            if (Random.Range(0, 100) <= thanksChance) thanks.Play(); 

            goodCount++;
            money += Random.Range(minCoins, maxCoins);

            smileParticles.Play();
            wrongParticles.Stop();
        }
        else
        {
            angryParticles.Play();
            wrongParticles.Play();


            badCount++;
        }
    }


    public void GetUpgrade()
    {

        if (isGetUpdgrade)
        {
            isGetUpdgrade = false;

            upgradeParts++;

            if (upgradeParts >= fullUpgradeParts)
            {
                upgradeParts = 0;
                updgrades++;
            }

            updgradesText.text = updgrades.ToString();
            updgradesPartsText.text = upgradeParts.ToString() + " / " + fullUpgradeParts;

            StartCoroutine(GetUpdgradeTimer());
        }
    }


    IEnumerator GetUpdgradeTimer()
    {

        yield return new WaitForSeconds(1.5f);

        Initlevel();

    }

}
