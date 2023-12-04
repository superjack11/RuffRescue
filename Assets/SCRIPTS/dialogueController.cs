using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.U2D.Animation;

public class dialogueController : dogClass
{
    public class newspaper {
        public string heading1;
        public string story1;
        public string heading2;
        public string story2;

        public newspaper(string heading1, string story1, string heading2, string story2) {
            this.heading1 = heading1;
            this.story1 = story1;
            this.heading2 = heading2;
            this.story2 = story2;
        }
    }

    public class adopter {
        public string sprite;
        public string dialogue;
        public int sizeWanted, energyWanted, dogSocWanted, nonDogSocWanted, vocalityWanted;


        public adopter(string sprite, string dialogue, int sizeWanted, int energyWanted, int dogSocWanted, int nonDogSocWanted, int vocalityWanted) {
            this.sprite = sprite;
            this.dialogue = dialogue;
            this.sizeWanted = sizeWanted;
            this.energyWanted = energyWanted;
            this.dogSocWanted = dogSocWanted;
            this.nonDogSocWanted = nonDogSocWanted;
            this.vocalityWanted = vocalityWanted;
        }

        public float compare(dog d) {
            return (Mathf.Abs(d.size - sizeWanted)
                + Mathf.Abs(d.energy - energyWanted)
                + Mathf.Abs(d.dogSociability - dogSocWanted)
                + Mathf.Abs(d.nonDogSociability - nonDogSocWanted)
                + Mathf.Abs(d.vocality - vocalityWanted)) / 5;
        }
    }

    public class donator {
        public string sprite;
        public dog donatedDog;
        public string dialogue;

        public donator(string sprite, dog donatedDog, string dialogue) {
            this.sprite = sprite;
            this.donatedDog = donatedDog;
            this.dialogue = dialogue;
        }

    }

    public class seller { // Sells items like floors and walls
        public string sprite;
        public List<item> itemsForSale;
        public string dialogue;

        public seller(string sprite, List<item> itemsForSale, string dialogue) {
            this.sprite = sprite;
            this.itemsForSale = itemsForSale;
            this.dialogue = dialogue;
        }

    }

    public class mailman {
        public string sprite;
        public string dialogue;
        public newspaper paper;

        public mailman(string sprite, string dialogue, newspaper paper) {
            this.sprite = sprite;
            this.dialogue = dialogue;
            this.paper = paper;
        }

    }


    public class genericPerson {
        public string sprite;
        public string dialogue;
        public string[] response1;
        public string[] response2;

        public genericPerson(string sprite, string dialogue, string[] response1, string[] response2) {
            this.sprite = sprite;
            this.dialogue = dialogue;
            this.response1 = response1;
            this.response2 = response2;
        }
    }

    public class vendor { // Sells items that can be sold by the player
        public string sprite;
        public List<string> itemsForSale;
        public string dialogue;

        public vendor(string sprite, List<string> itemsForSale, string dialogue)
        {
            this.sprite = sprite;
            this.itemsForSale = itemsForSale;
            this.dialogue = dialogue;
        }
    }

    public dog selectedDog;
    public Sprite[] letterSprites;

    public adopter TheAdopter;
    public donator TheDonator;
    public seller TheSeller;
    public mailman TheMailman;
    public genericPerson TheGenericPerson;
    public vendor TheVendor;

    public GameObject thePersonGameObj;
    public GameObject personPrefab;

    public bool occupied;

    public float pTimer;

    private gameController gameController;

    public GameObject allDialogueUI;
    public TMP_Text personDialogue;

    public TMP_Text sellerDialogue;

    public GameObject dogListPanel;
    public GameObject dogInfoPanel;
    public GameObject dialoguePanel;

    public GameObject shopPanel;
    public Transform shopContent;
    public GameObject itemUI;

    public GameObject inventoryPanel;
    public TMP_Text inventoryDogfoodTxt;
    public TMP_Text inventoryTennisBallTxt;
    public TMP_Text inventoryDogBedTxt;
    public TMP_Text inventoryDogToyTxt;
    public TMP_Text inventoryDogTreatTxt;

    public GameObject gradePanel;
    public TMP_Text gradeCoinGainTxt;
    public TMP_Text gradeGoodbyeTxt;
    public Image gradeLetterImage;

    public Button acceptOrGiveDogBtn;
    public Button denyDogBtn;

    public GameObject donatedDogUIParent;

    public GameObject dogUI;
    public List<GameObject> dogUIElements = new List<GameObject>();

    /*
    public List<dog> possibleDogs = new List<dog> {
        new dog("dog0", "Calvin", "", 2, 2, 2, 2, 2)
    };
    */

    private List<adopter> possibleAdopters;
    private List<donator> possibleDonators;
    private List<seller> possibleSellers;
    private List<vendor> possibleVendors;

    private List<string> possibleGiveDogLine;
    private List<string> possibleAcceptDogLine;
    private List<string> possibleDeclineDonor;
    private List<string> possibleDeclineAdopter;

    private List<string> possibleDogNames;

    DateTime currentDate;
    DateTime oldDate;


    private int numberOfDogsInFiles = 3;   // Change this when adding or removing dogs

    // Start is called before the first frame update
    void Start() {
        TheAdopter = null;
        TheDonator = null;
        TheSeller = null;
        TheMailman = null;
        TheVendor = null;

        occupied = false;
        pTimer = UnityEngine.Random.Range(1, 2);
        gameController = GameObject.FindWithTag("MainCamera").GetComponent<gameController>();

        //allDialogueUI.SetActive(false);

        dogListPanel.SetActive(false);
        dogInfoPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        shopPanel.SetActive(false);
        gradePanel.SetActive(false);


        //Store the current time when it starts
        currentDate = DateTime.Now;

        //Grab the old time from the player prefs as a long
        long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));

        //Convert the old time from binary to a DataTime variable
        DateTime oldDate = DateTime.FromBinary(temp);

        //Use the Subtract method and store the result as a timespan variable
        TimeSpan difference = currentDate.Subtract(oldDate);




        possibleDogNames = new List<string> {
            "Zippity",
            "Kachow",
            "Peabody",
            "Sherman",
            "Zaba",
            "Excalibur",
            "Georgia",
            "Atlantic",
            "Moose",
            "Zachary",
            "King",
            "Yapper",
            "Jack",
            "Cat",
            "Lion",
            "Danny",
            "Echo",
            "Tiger",
            "Kian",
            "Danny",
            "Athena",
            "Luna",
            "Guppy",
            "Stella",
            "Rocky",
            "Lincoln",
            "Sock",
            "Stockholm",
            "Olivia",
            "Fox",
            "Candle",
            "Ponder",
            "Sam",
            "Oatmilk",
            "Daryl",
            "Greg",
            "Pumpkin",
            "Gourd",
            "Cannoli",
            "Squash",
            "Cannelini"
        };

        possibleDeclineAdopter = new List<string> {
            "I don't think I have any good dogs for you",
            "Turn away",
            "No dog for you",
            "Exit the premises",
            "Sorry, no matches"
        };

        possibleDeclineDonor = new List<string> {
            "No thanks",
            "Sorry, no room",
            "No can do, pal.",
            "I'll pass",
            "Keep it moving"
        };

        possibleAcceptDogLine = new List<string> {
            "I'll take it!",
            "Sure, I can take it",
            "I'll find it a good home",
            "Yes please"
        };

        possibleGiveDogLine = new List<string> {
            "Sure, take this one!",
            "This is the dog for you!",
            "I've got the perfect match",
            "How about this one?",
            "This is a good one here"
        };

        possibleDonators = new List<donator> {
            new donator("p1", new dog("d" + UnityEngine.Random.Range(0,numberOfDogsInFiles), possibleDogNames[UnityEngine.Random.Range(0,possibleDogNames.Count)], "Average as they come", 2, 2, 2, 2, 2, 0), "I've got a real average dog for you. I'm moving and my new apartment said I couldn't have him with me."),
            new donator("p1", new dog("d" + UnityEngine.Random.Range(0,numberOfDogsInFiles), possibleDogNames[UnityEngine.Random.Range(0,possibleDogNames.Count)], "Lazy but loud", 2, 0, 2, 2, 4, 30), "This guy just lies around CONSTANTLY. I'd say it didn't do a thing if it didn't bark half the day."),
            new donator("p1", new dog("d" + UnityEngine.Random.Range(0,numberOfDogsInFiles), possibleDogNames[UnityEngine.Random.Range(0,possibleDogNames.Count)], "Sleeps all day", 2, 0, 2, 2, 1, 0), "'Sleeps all day' doesn't begin to describe it! I couldn't get this dog to go for a walk for the life of me!"),
            new donator("p1", new dog("d" + UnityEngine.Random.Range(0,numberOfDogsInFiles), possibleDogNames[UnityEngine.Random.Range(0,possibleDogNames.Count)], "Big dog who loves dogs and hates cats", 4, 2, 4, 0, 2, 0), "This is a dogs dog for sure. Not too social with cats"),
            new donator("p1", new dog("d" + UnityEngine.Random.Range(0,numberOfDogsInFiles), possibleDogNames[UnityEngine.Random.Range(0,possibleDogNames.Count)], "High Energy", 2, 5, 2, 2, 2, 0), "Too too too high energy for me. Can't handle this. I almost sprained my kankle chasing him down the street!")
        };

        possibleAdopters = new List<adopter> { // sprite, dialogue,  ///    sizeWanted, energyWanted, dogSocWanted, nonDogSocWanted, vocalityWanted
            new adopter("p1", "Hello! I want a really average dog", 2,2,2,2,2),
            new adopter("p1", "I'm lazy and have horribly annoying neighbors who will have my booty on a stick if they hear a dog bark! I don't have any other dogs",2,0,2,2,0),
            new adopter("p1", "I'm a lonely triathlete in need of a training partner. Do you have any high-energy dogs?",2,4,2,3,2),
            new adopter("p1", "Hi, I'm in the market for a dog. My house has a small dog door already, so something on the smaller side would be preferable.",1,2,2,2,2),
            new adopter("p1", "Hello there. I've got a small dog already, but he's too short to reach stuff around the house. I'm looking for a very large dog for him to ride around on.",4,2,3,2,2),
            new adopter("p1", "Dog please! I don't really have any strong preferences.",2,2,2,2,2)
        };

        possibleSellers = new List<seller> {
            new seller("p1", new List<item>{new item("ft1", "Diamond Tiles", "floor", 50), new item("ft2", "Checkered Floor", "floor", 50), new item("ft6", "Cobble Floor", "floor", 10) },"Take a look at my floors!"),
            new seller("p1", new List<item>{new item("wt0", "Wooden Wall", "wall", 25)}, "Take a look at this wall!")
        };

        possibleVendors = new List<vendor> {
            new vendor("p1", new List<string>{"dogBed", "tennisBall"}, "Fresh shipment from the dog bed and tennis ball store. Get em while they're hot!"),
            new vendor("p1", new List<string>{"dogTreat"}, "I've got the best dog treats in town. Buy! Buy! Buy!")

        };


    }

    // Update is called once per frame
    void Update()
    {
        if (pTimer < 0) {
            if (!occupied) {
                //int ranNum = UnityEngine.Random.Range(0, 4);
                int ranNum = 2;
                TheDonator = null;
                TheAdopter = null;
                TheSeller = null;
                TheVendor = null;
                if (ranNum == 0 && PlayerPrefs.GetString("dogs").Length > 10) {
                    TheAdopter = possibleAdopters[UnityEngine.Random.Range(0, possibleAdopters.Count)];
                    thePersonGameObj = Instantiate(personPrefab, new Vector2(17,0), Quaternion.identity);
                    thePersonGameObj.GetComponent<SpriteLibrary>().spriteLibraryAsset = Resources.Load(TheAdopter.sprite) as SpriteLibraryAsset;

                }
                else if (ranNum == 1) {
                    TheDonator = possibleDonators[UnityEngine.Random.Range(0, possibleDonators.Count)];
                    dog v = TheDonator.donatedDog;
                    TheDonator.donatedDog = new dog(v.spriteName, possibleDogNames[UnityEngine.Random.Range(0, possibleDogNames.Count)], v.dogDescription, v.size, v.energy, v.dogSociability, v.nonDogSociability, v.vocality, v.love);
                    thePersonGameObj = Instantiate(personPrefab, new Vector2(17, 0), Quaternion.identity);
                    thePersonGameObj.GetComponent<SpriteLibrary>().spriteLibraryAsset = Resources.Load(TheDonator.sprite) as SpriteLibraryAsset;

                }
                else if (ranNum == 2) {
                    TheSeller = possibleSellers[UnityEngine.Random.Range(0, possibleSellers.Count)];
                    thePersonGameObj = Instantiate(personPrefab, new Vector2(17, 0), Quaternion.identity);
                    thePersonGameObj.GetComponent<SpriteLibrary>().spriteLibraryAsset = Resources.Load(TheSeller.sprite) as SpriteLibraryAsset;
                } else {
                    TheVendor = possibleVendors[UnityEngine.Random.Range(0, possibleVendors.Count)];
                    thePersonGameObj = Instantiate(personPrefab, new Vector2(17, 0), Quaternion.identity);
                    thePersonGameObj.GetComponent<SpriteLibrary>().spriteLibraryAsset = Resources.Load(TheVendor.sprite) as SpriteLibraryAsset;
                }

                occupied = true;
            }
            //pTimer = UnityEngine.Random.Range(30, 120);
            pTimer = 1;
        } else {
            if (!occupied) {
                pTimer -= Time.deltaTime;
            }
        }
    }

    public void giveDog() {

        if (selectedDog != null) {
            thePersonGameObj.GetComponent<personScr>().leave();

            gameController.removeDog(selectedDog);
 
            //allDialogueUI.SetActive(false);
            dogListPanel.SetActive(false);
            dogInfoPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            gradeMatch(selectedDog, TheAdopter);

            selectedDog = null;
            occupied = false;
        }


    }

    public void acceptDog() {
        thePersonGameObj.GetComponent<personScr>().leave();
        gameController.addDog(TheDonator.donatedDog);
        occupied = false;
        //allDialogueUI.SetActive(false);
        dogListPanel.SetActive(false);
        dogInfoPanel.SetActive(false);
        dialoguePanel.SetActive(false);

    }

    public void declineDog() {
        thePersonGameObj.GetComponent<personScr>().leave();

        occupied = false;
        //allDialogueUI.SetActive(false);
        dogListPanel.SetActive(false);
        dogInfoPanel.SetActive(false);
        dialoguePanel.SetActive(false);

    }

    public void gradeMatch(dog d, adopter a) {
        float grade = a.compare(d);
        int letterIndex = 0;

        if (grade <= .75) {
            letterIndex = 0;
        } else if (grade > .75 && grade <= 1.5) {
            letterIndex = 1;
        } else if (grade > 1.5 && grade <= 2.25) {
            letterIndex = 2;
        } else if (grade > 2.25 && grade <= 3) {
            letterIndex = 3;
        } else if (grade > 3) {
            letterIndex = 4;
        } else {
            Debug.Log("Error Grading");
        }

        int earnedCoins = (int)(5 - grade) * d.love;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins",0) + earnedCoins);

        gradePanel.SetActive(true);
        gradeCoinGainTxt.text = "+ $" + earnedCoins;
        gradeGoodbyeTxt.text = "Goodbye, " + d.dogName + "!";
        gradeLetterImage.sprite = letterSprites[letterIndex];
    }

    public void pressPerson() {
        if (TheAdopter != null) { // Adopter
            personDialogue.text = TheAdopter.dialogue;
            acceptOrGiveDogBtn.GetComponentInChildren<TMP_Text>().text = possibleGiveDogLine[UnityEngine.Random.Range(0, possibleGiveDogLine.Count)];
            denyDogBtn.GetComponentInChildren<TMP_Text>().text = possibleDeclineAdopter[UnityEngine.Random.Range(0, possibleDeclineAdopter.Count)];
            acceptOrGiveDogBtn.onClick.RemoveAllListeners();
            acceptOrGiveDogBtn.onClick.AddListener(giveDog);
            denyDogBtn.onClick.RemoveAllListeners();
            denyDogBtn.onClick.AddListener(declineDog);

            foreach (Transform l in donatedDogUIParent.transform)
            {
                Destroy(l.gameObject);
            }
            gameController.updateDogs(true);
            dogListPanel.SetActive(true);
            dialoguePanel.SetActive(true);
        } else if (TheDonator != null) { // Donator
            personDialogue.text = TheDonator.dialogue;
            acceptOrGiveDogBtn.GetComponentInChildren<TMP_Text>().text = possibleAcceptDogLine[UnityEngine.Random.Range(0, possibleAcceptDogLine.Count)];
            denyDogBtn.GetComponentInChildren<TMP_Text>().text = possibleDeclineDonor[UnityEngine.Random.Range(0, possibleDeclineDonor.Count)];
            acceptOrGiveDogBtn.onClick.RemoveAllListeners();
            acceptOrGiveDogBtn.onClick.AddListener(acceptDog);
            denyDogBtn.onClick.RemoveAllListeners();
            denyDogBtn.onClick.AddListener(declineDog);

            foreach (Transform l in donatedDogUIParent.transform) {
                Destroy(l.gameObject);
            }
            GameObject a = Instantiate(dogUI);
            a.GetComponent<dogUIElement>().dogInstance = TheDonator.donatedDog;
            a.GetComponent<dogUIElement>().editBtn.gameObject.SetActive(false);
            a.GetComponent<dogUIElement>().selectBtn.gameObject.SetActive(false);
            a.transform.SetParent(donatedDogUIParent.transform);
            a.transform.localScale = Vector2.one;
            a.GetComponent<dogUIElement>().updateUI();
            dogUIElements.Add(a);
            gameController.updateDogs(false);
            dialoguePanel.SetActive(true);
        } else if (TheSeller != null) { // Seller
            sellerDialogue.text = TheSeller.dialogue;

            foreach (Transform l in shopContent.transform) {
                Destroy(l.gameObject);
            }

            foreach (item i in TheSeller.itemsForSale) {
                GameObject a = Instantiate(itemUI);
                a.GetComponent<itemUIElement>().itemInstance = i;
                a.transform.SetParent(shopContent);
                a.transform.localScale = Vector2.one;
                a.GetComponent<itemUIElement>().updateUI();
            }

            openInventory();
            shopPanel.SetActive(true);
        } else if (TheVendor != null) { // Vendor
            sellerDialogue.text = TheVendor.dialogue;

            foreach (Transform l in shopContent.transform)
            {
                Destroy(l.gameObject);
            }

            foreach (string i in TheVendor.itemsForSale)
            {
                GameObject a = Instantiate(itemUI);
                a.GetComponent<itemUIElement>().vendorItem = i;
                a.transform.SetParent(shopContent);
                a.transform.localScale = Vector2.one;
                a.GetComponent<itemUIElement>().updateUI();
            }

            openInventory();
            shopPanel.SetActive(true);
        }

    }

    public void openDogList() {
        gameController.updateDogs(false);
        dogListPanel.SetActive(true);
    }

    public void closeDogList() {
        dogListPanel.SetActive(false);
    }

    /*
    public void openShop () {
        shopPanel.SetActive(true);

        for(int i = 0; i < UnityEngine.Random.Range(3, 6); i ++) {
            GameObject a = Instantiate(itemUI);
            a.transform.SetParent(shopContent.transform);
            a.transform.localScale = Vector2.one;
            dogUIElements.Add(a);
        }
    }
    */

    public void openInventory () {
        gameController.UpdateInventory();

        updateVendorItemTxt();

        inventoryPanel.SetActive(true);
    }

    public void updateVendorItemTxt() {
        inventoryDogfoodTxt.text = ": " + PlayerPrefs.GetInt("dogFoodInventory");
        inventoryTennisBallTxt.text = ": " + PlayerPrefs.GetInt("tennisBallInventory");
        inventoryDogBedTxt.text = ": " + PlayerPrefs.GetInt("dogBedInventory");
        inventoryDogToyTxt.text = ": " + PlayerPrefs.GetInt("dogToyInventory");
        inventoryDogTreatTxt.text = ": " + PlayerPrefs.GetInt("dogTreatInventory");
    }

    public void closeInventory() {
        inventoryPanel.SetActive(false);
    }

    public void closeShopBox() {
        thePersonGameObj.GetComponent<personScr>().leave();
        occupied = false;
        shopPanel.SetActive(false);
    }

    public void closeGradeBox() {
        gradePanel.SetActive(false);
    }

    void OnApplicationQuit() {
        //Save the current system time as a string in the player prefs class
        PlayerPrefs.SetString("sysString", DateTime.Now.ToBinary().ToString());
    }

}
