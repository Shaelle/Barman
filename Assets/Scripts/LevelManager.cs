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
    public static int moneyLastLevel = 0;

    public static int diamonds = 0;

    public static int level = 1;


    public static int updgrades = 0;
    public static float upgradeProgress = 0;
    //public static int upgradeParts = 0;

    public static int rating = 1;


    public const string moneySaveName = "Money";
    public const string diamondsSaveName = "Diamonds";
    const string levelSaveName = "Level";
    const string upgradeSaveName = "Upgrades";
    const string upgradeProgressSaveName = "UpdgradeProgress";

    const int fullUpgradeParts = 5;

    Vector2 cursorPos;

    [Header("General")]

    [SerializeField] Hand leftHand;
    [SerializeField] Hand leftHand2;
    [SerializeField] Hand rightHand;
    [SerializeField] Hand rightHand2;


    [Header("Drinks")]

    [SerializeField] float drinkSpeed = 5f;

    [SerializeField] GameObject[] drinkSlots;

    [SerializeField] Drink[] allDrinks;

    [SerializeField] Drink[] drinks; // TODO: convert to list

    List<Drink> newDrinks;

    Drink[] shuffledDrinks;
    List<Drink> shuffledDrinksList;


    [SerializeField] Rotation[] targetMarks;


    [SerializeField] GameObject targetBackground;

    [Header("UI")]

    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI diamondsText;

    [SerializeField] TextMeshProUGUI updgradesText;
    [SerializeField] GameObject getUpdgradeButton;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject giftButton;
 

    [SerializeField] GameObject caseButton;
    [SerializeField] GameObject shop2Button; 

    [SerializeField] GameObject nextlevelButton;
    TextMeshProUGUI nextLevelLabel;

    [SerializeField] GameObject finishedLabel;

    [SerializeField] GameObject uiBackground;

    [Header("Particles")]


    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem winParticles2;
    [SerializeField] ParticleSystem wrongParticles;
    [SerializeField] ParticlesPlayer smileParticles;
    [SerializeField] ParticlesPlayer angryParticles;

    [SerializeField] [Range(1, 100)] int smilesChance = 45;

    [Header("Audio")]

    [SerializeField] AudioSource startMelody;
    [SerializeField] AudioSource thanks;
    [SerializeField] AudioSource dissapointed;
    [SerializeField] AudioSource excited;
    [SerializeField] AudioSource levelCompleted;

    [SerializeField] HandsSounds handSounds;

    [Header("Coins")]

    [SerializeField] int minCoins = 10;
    [SerializeField] int maxCoins = 40;

    [SerializeField] int minLevelBonus = 60;
    [SerializeField] int maxLevelBonus = 150;

    [Header("Other")]

    [SerializeField] [Range(1, 100)] int thanksChance = 40;
    [SerializeField] [Range(1, 100)] int wowChance = 60;

    [SerializeField] float sensitivity = 10;

    [SerializeField] Tracer[] tracers;
    [SerializeField] bool useTracers = false;

    [SerializeField] Tracer pointingHand;


    [SerializeField] float minHandDelay = 0;
    [SerializeField] float maxHandDelay = 1;


    int goodCount = 0;
    int badCount = 0;
    int brokenCount = 0;

    bool isPushing = false;
    bool pushPause = false;


    bool isPressed = false;
    Drink touchedDrink;
    Vector2 touchedStart;

    Vector3 start;
    Vector3 target;

    Hand activeHand;

    bool isGetUpdgrade = false;

    bool levelActive = false;

    bool training = false;
    bool needTraining = false;


    int targetNom;
   

    [SerializeField] int minDrinksForNextLevel = 4;
    [SerializeField] int maxDrinksForNextLevel = 7;


    int drinksForNextLevel = 5;


    Coroutine coTracers;


  
    private void Awake()
    {
        nextLevelLabel = nextlevelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        money =  PlayerPrefs.GetInt(moneySaveName, 0);
        moneyLastLevel = 0;

        diamonds = PlayerPrefs.GetInt(diamondsSaveName, 0);

        level = PlayerPrefs.GetInt(levelSaveName, 1);


        updgrades = PlayerPrefs.GetInt(upgradeSaveName, 0);
        upgradeProgress = PlayerPrefs.GetFloat(upgradeProgressSaveName, 0);


        Image upImage = getUpdgradeButton.GetComponent<Image>();

        if (upImage == null) Debug.LogError("Upgrade button's image not found.");


        upImage.fillAmount = upgradeProgress;


        newDrinks = new List<Drink>();
        

    }


    // Start is called before the first frame update
    void Start()
    {
        if (drinkSlots.Length > drinks.Length) Debug.LogWarning("Number of drinks should be equal or bigger than the number of slots! Slots: " + drinkSlots.Length+", drinks: " +drinks.Length);

        Initlevel();

        needTraining = (level == 1) ? true : false;
    }


    public void Initlevel() // Initializing level with default values
    {

        levelActive = false;


        foreach (Drink drink in allDrinks)
        {

            drink.transform.position = Vector3.zero;
            drink.SetDirection(0, 0);

            if (drink.starterPack)
            {
                drink.available = true;
            }
            else
            {
                drink.available = (PlayerPrefs.GetInt(drink.kind.ToString(), 0) == 1) ? true : false;
            }

            //if (drink.kind == Drink.Kinds.Drink1) drink.available = true;

            if (drink.available)
            {
                newDrinks.Add(drink);
            }

        }


        startMelody.gameObject.SetActive(true);

        goodCount = 0;
        badCount = 0;
        brokenCount = 0;
       
        caseButton.SetActive(true);
        shop2Button.SetActive(true);
        finishedLabel.SetActive(false);

        if (nextLevelLabel == null) Debug.LogError("No text component for level label");

        nextLevelLabel.text = "DAY " + level.ToString();
        nextlevelButton.SetActive(true);

        uiBackground.SetActive(true);

        //settingsButton.SetActive(true);

        giftButton.SetActive(true);

        ResetLevel();

        moneyText.text = money.ToString();

        diamondsText.text = diamonds.ToString();

        drinksForNextLevel = Random.Range(minDrinksForNextLevel, maxDrinksForNextLevel);
    }


    public void StartLevel() // Starting the level
    {

        levelActive = true;

        startMelody.gameObject.SetActive(false);

        caseButton.SetActive(false);
        shop2Button.SetActive(false);
        finishedLabel.SetActive(false);

        nextlevelButton.SetActive(false);
        //settingsButton.SetActive(false);
        giftButton.SetActive(false);

        uiBackground.SetActive(false);

        isPushing = false;

        foreach (Drink drink in drinks)
        {
            drink.gameObject.SetActive(true);
        }


        ShuffleDrinks(drinks);
        ShuffleDrinks(newDrinks);

       
        UpdateTable();
        UpdateScore();

    }


    public void FinishLevel()  // Finishing the level
    {
        levelActive = false;
        StartCoroutine(Finishinglevel());
    }



    IEnumerator Finishinglevel()
    {

        int bad = brokenCount + badCount;

        if (bad == 0)
        {
            rating = 3;
        }
        else if (bad < 3)
        {
            rating = 2;
        }
        else
        {
            rating = 1;
        }


        ResetLevel();

        levelCompleted.Play();

        //winParticles.Play();
        winParticles2.Play();

        yield return new WaitForSeconds(0.5f);

        // some animations here

        yield return new WaitForSeconds(2f);

  
        isGetUpdgrade = false;

              
        updgradesText.text = updgrades.ToString();

        yield return new WaitForSeconds(1.2f);

        winParticles.Stop();
        winParticles2.Stop();

        PlayerPrefs.Save();

        if (!isGetUpdgrade) SceneManager.LoadScene(2);

    }





    void ResetLevel() // Resetting the level
    {

        isPushing = true;

        targetBackground.SetActive(false);

        rightHand.gameObject.SetActive(false);

        leftHand.gameObject.SetActive(false);

        rightHand2.gameObject.SetActive(false);

        leftHand2.gameObject.SetActive(false);


        updgradesText.gameObject.SetActive(false);
        getUpdgradeButton.SetActive(false);

        winParticles.Stop();
        winParticles2.Stop();
        wrongParticles.Stop();

        smileParticles.Stop();
        angryParticles.Stop();



        if (shuffledDrinksList == null)
        {
            foreach (Drink drink in drinks)
            {
                drink.SetDirection(0, 0);
                drink.transform.position = Vector3.zero;
                drink.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Drink drink in newDrinks)
            {
                drink.SetDirection(0, 0);
                drink.transform.position = Vector3.zero;
                drink.gameObject.SetActive(false);
            }
        }


        foreach (Rotation mark in targetMarks)
        {
            mark.gameObject.SetActive(false);
        }


        foreach (Tracer tracer in tracers)
        {
            TracerReset(tracer);
        }

        pointingHand.GoToStart(Vector3.zero);
        pointingHand.SetDirection(0, 0);


    }



    public void GoToShop() // Opening the shop ("meta") screen
    {
        StartCoroutine(LoadingScene(1));
    }

    public void GoToShop2() // Opening the second shop
    {
        StartCoroutine(LoadingScene(4)); // TODO: switch to names
    }



    IEnumerator LoadingScene(int sceneIndex) // Small pause before loading next scene, so click sound on the button can play
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }



    void OnPress(InputValue value) // Event that happens when player presses / releases pointer (mouse, finger etc)
    {

        if (training) StopTraining();

        isPressed = value.isPressed;

        if (!value.isPressed && touchedDrink != null)
        {

            

            if (!isPushing && cursorPos.y > touchedStart.y)
            {

                float acceleration = CalcAcceleration();


                float delta = target.z - start.z;
                float distance = Mathf.Abs(target.x - start.x);

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





    public void TracerReset(Tracer tracer) // Retuning tracer to it's starting position
    {


        if (touchedDrink != null || training)
        {
            tracer.GoToStart(start);
            tracer.lastStart = start;
        }

    }


    IEnumerator SendTracer() // Sending tracer by drink's predicted trajectory
    {

        while (touchedDrink != null || training)
        {

            for (int i = 0; i < tracers.Length; i++)
            {

                TracerReset(tracers[i]);

                float acceleration = CalcAcceleration();

                tracers[i].SetDirection(drinkSpeed, acceleration);


                yield return new WaitForSeconds(0.4f);

            }

            yield return new WaitForSeconds(0.1f);

            if (training)
            {
                TracerReset(pointingHand);

                float acceleration = CalcAcceleration();

                pointingHand.SetDirection(drinkSpeed, acceleration);
            }

            yield return new WaitForSeconds(0.2f);

        }
    }


    void ActivateTraining() // Activating training when player starts the game in the first time: sending hand (and tracers) by recommended trajectory
    {
        needTraining = false;

        TracerReset(pointingHand);

        Debug.Log("activating training");
        foreach (Drink drink in drinks) // Search for the selected drink and set it as starting point
        {
            if (drink.correctOne)
            {
                start = drink.transform.position;
                break;
            }
        }

        target = activeHand.target.position;

        training = true;

       coTracers = StartCoroutine(SendTracer());

    }


    void StopTraining() // Stopping training after player presses the pointer
    {

        pointingHand.GoToStart(Vector3.zero);
        pointingHand.SetDirection(0, 0);

        Debug.Log("stoping training");
        training = false;
        StopCoroutine(coTracers);

        for (int i = 0; i < tracers.Length; i++)
        {
            TracerReset(tracers[i]);
        }
    }


    float CalcAcceleration() // Calculating speed and side acceleration for the drinks
    {

        float delta =  target.z - start.z;
        float distance = Mathf.Abs(target.x - start.x);

        drinkSpeed = 1.5f + distance * 0.7f; //  empirical formula for the speed

        return (delta / distance) / sensitivity;
    }


    void OnMove(InputValue value) // Player moving the pointer
    {

        if (isPressed)
        {

            Vector2 newPos = value.Get<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(newPos);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);


            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                GameObject hitObject = hit.transform.gameObject;

                if (touchedDrink == null)
                {
                    Drink drink = hitObject.GetComponent<Drink>();

                    if (drink != null)
                    {
                        touchedDrink = drink;
                        touchedStart = newPos;

                        start = hit.point;

                        if (useTracers) StartCoroutine(SendTracer());
                    }
                }


                if (hitObject.tag == "Targeting")
                {
                    target = hit.point;
                }
            }

            cursorPos = newPos;
        }
    }



    void PlaceDrinks() // Placing drinks on the table. Choosing the selected one.
    {

        if (shuffledDrinksList == null)
        {
            foreach (Drink drink in drinks)
            {
                drink.transform.position = Vector3.zero;
                drink.transform.rotation = Quaternion.identity;
                drink.SetDirection(0, 0);
            }
        }
        else
        {
            foreach (Drink drink in newDrinks)
            {
                drink.transform.position = Vector3.zero;
                drink.transform.rotation = Quaternion.identity;
                drink.SetDirection(0, 0);
            }
        }

        
        for (int i = 0; i < drinkSlots.Length; i++)
        {
            if (shuffledDrinksList == null)
            {
                shuffledDrinks[i].transform.position = drinkSlots[i].transform.position;
                shuffledDrinks[i].correctOne = false;
            }
            else
            {
                shuffledDrinksList[i].transform.position = drinkSlots[i].transform.position;
                shuffledDrinksList[i].correctOne = false;
            }
        }

        int n = Random.Range(0, drinkSlots.Length - 1); // Randomly selecting one drink as "ordered by client"

        if (shuffledDrinksList == null)
        {
            shuffledDrinks[n].correctOne = true;
        }
        else
        {
            shuffledDrinksList[n].correctOne = true;
        }

        targetNom = n;

        //Drink.Kinds kind = shuffledDrinks[n].kind;

        HideTargets();

    }


    void HideTargets() // Reset target marks for drinks by hiding them all
    {
        foreach (Rotation target in targetMarks)
        {
            target.gameObject.SetActive(false);
        }

        targetBackground.SetActive(false);
    }


    void ShowTarget() // Showing mark for the selected drink
    {

        Drink.Kinds kind;

        if (shuffledDrinksList == null)
        {
            kind = shuffledDrinks[targetNom].kind;
        }
        else
        {
            kind = shuffledDrinksList[targetNom].kind;
        }

        foreach (Rotation target in targetMarks)
        {
            target.gameObject.SetActive(target.kind == kind);
        }

        targetBackground.SetActive(true);
    }



    private void ShuffleDrinks(Drink[] drinks) // Shuffling drinks like a cards. The first (four) in the list will be go to the table
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


    private void ShuffleDrinks(List<Drink> drinks) // Shuffling drinks like a cards. The first (four) in the list will be go to the table
    {

        if (drinks == null)
        {
            Debug.LogWarning("List of drinks is null!");
            return;
        }

        List<Drink> newArray = new List<Drink>(drinks);

        for (int i = 0; i < newArray.Count; i++)
        {
            Drink tmp = newArray[i];
            int r = Random.Range(i, newArray.Count);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        shuffledDrinksList = new List<Drink>(newArray);
    }


    /*
             List<Drink> drinks2 = new List<Drink>(newDrinks);

        Debug.Log("New drinks: ");
        foreach (Drink drink in newDrinks)
        {
            Debug.Log(drink.kind.ToString());
        }

        Debug.Log("drinks2: ");
        foreach (Drink drink in drinks2)
        {
            Debug.Log(drink.kind.ToString());
        }
        */

    void SetHandDirection(float timer) // Randomly setting directions, from which a hand is coming
    {
        StartCoroutine(SettingDirection(timer));
    }



    IEnumerator SettingDirection(float timer) // Choosing randomely, from which direction "customer's hand" will appear
    {

        void SetRight(bool active) //TODO: simplify 
        {
            rightHand.gameObject.SetActive(active);
        }

        void SetLeft(bool active)
        {
            leftHand.gameObject.SetActive(active);
        }

        void SetRight2(bool active)
        {
            rightHand2.gameObject.SetActive(active);

        }

        void SetLeft2(bool active)
        {
            leftHand2.gameObject.SetActive(active);
        }

        SetRight(false); 
        SetLeft(false);

        SetRight2(false);
        SetLeft2(false);

        
        yield return new WaitForSeconds(timer);

        int rand = Random.Range(0, 100);


        if (rand < 25) // 0 - 25 far right
        {
            SetRight(true);

            handSounds.transform.position = rightHand.transform.position;
            activeHand = rightHand;
        }
        else if (rand < 50) // 25 - 50 far left
        {
            SetLeft(true);

            handSounds.transform.position = leftHand.transform.position;
            activeHand = leftHand;
        }
        else if (rand < 75) // 50 - 75 close right
        {
            SetRight2(true);

            handSounds.transform.position = rightHand2.transform.position;
            activeHand = rightHand2;
        }
        else // rest - close left
        {
            SetLeft2(true);

            handSounds.transform.position = leftHand2.transform.position;
            activeHand = leftHand2;
        }

        handSounds.Play();

        ShowTarget();
 
        StartCoroutine(ResetPushing());

        if (needTraining) ActivateTraining();

    }


    public void DestinationReached(Drink drink) // Reached stop ("hand") trigger
    {
        StartCoroutine(Reaching(drink));        
    }

    IEnumerator Reaching(Drink drink)
    {
        activeHand.Grab(drink.transform.position);

        CheckHit(drink);
        UpdateScore();

        yield return new WaitForSeconds(0.7f);


        UpdateTable(Random.Range(minHandDelay, maxHandDelay));

    }


    public void Miss() // Drink leaved the playing zone
    {
        if (isPushing) // prevent false alarms when drinks is reordering.
        {
            brokenCount++;

            Handheld.Vibrate();

            if (level == 1 && goodCount == 0) needTraining = true;

            if (Random.Range(0,100) <= smilesChance) angryParticles.Play();

            UpdateScore();

            UpdateTable(Random.Range(minHandDelay,maxHandDelay));
        }
    }


    public void UpdateTable(float timer = 0) // Updatig table
    {
        if (levelActive)
        {            
            PlaceDrinks();
            SetHandDirection(timer);
        }
    }


    void UpdateScore() // Show score on UI
    {

        moneyText.text = (money + moneyLastLevel).ToString(); // money.ToString();

        diamondsText.text = diamonds.ToString();



        if (goodCount >= drinksForNextLevel)
        {
            level++;
            PlayerPrefs.SetInt(levelSaveName, level);
            FinishLevel();
        }
    }


    IEnumerator ResetPushing() // Pause after finishing pushing to prevent accidental activation
    {
        yield return new WaitForSeconds(0.1f);
        isPushing = false;
    }


    public void CheckHit(Drink drink) // When drink reaches the hand, check if it the correct one
    {

        if (drink.correctOne)
        {


            if (Random.Range(0, 100) <= thanksChance)
            {
                thanks.Play();
            }
            else if (Random.Range(0, 100) <= wowChance)
            {
                excited.Play();
            }

            goodCount++;
            moneyLastLevel += Random.Range(minCoins, maxCoins);

            //PlayerPrefs.SetInt(moneySaveName, money);

            if (Random.Range(0,100) <= smilesChance) smileParticles.Play();

            wrongParticles.Stop();
        }
        else
        {

            Handheld.Vibrate();

            if (Random.Range(0,100) <= smilesChance) angryParticles.Play();

            dissapointed.Play();

            badCount++;

            if (level == 1 && goodCount == 0) needTraining = true;
        }
    }


    public void GetUpgrade() // Getting upgrade "currency" for the bar
    {

        if (isGetUpdgrade)
        {
            isGetUpdgrade = false;

            Image upImage = getUpdgradeButton.GetComponent<Image>();

            if (upImage == null) Debug.LogError("Upgrade button's image not found.");

            if (upImage.fillAmount != 1) return;
          
            updgrades++;
            PlayerPrefs.SetInt(upgradeSaveName, updgrades);

            updgradesText.text = updgrades.ToString();

            StartCoroutine(GetUpdgradeTimer(upImage));
        }
    }


    IEnumerator GetUpdgradeTimer(Image image)
    {

        yield return new WaitForSeconds(1.5f);

        image.fillAmount = 0;
        upgradeProgress = image.fillAmount;
        PlayerPrefs.SetFloat(upgradeProgressSaveName, upgradeProgress);

        var temp = image.color;
        temp.a = 0.5f;
        image.color = temp;

        SceneManager.LoadScene(2);
    }

}
