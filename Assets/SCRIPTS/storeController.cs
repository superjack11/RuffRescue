using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class storeController : MonoBehaviour
{
    /*

    Playerprefs:
    dogFoodPrice
    dogFoodStocked   corresponds to the shelf sprite
    dogFoodInventory

    tennisBallPrice
    tennisBallStocked
    tennisBallInventory

    dogToyPrice
    dogToyStocked
    dogToyInventory

    dogTreatPrice
    dogTreatStocked
    dogTreatInventory
    
    once item goes onto shelf, thats that


    */

    public class customer {
        public string sprite;
        public string itemNeeded;

        public customer(string sprite, string itemNeeded) {
            this.sprite = sprite;
            this.itemNeeded = itemNeeded;
        }
    }

    private List<customer> possibleCustomers;

    public GameObject customerObj;

    public GameObject priceChangeUI;
    private TMP_Text titleText;
    private TMP_Text stockText;
    private TMP_Text priceText;
    private Button restockUIBtn;
    private Button plusBtn;
    private Button minusBtn;

    public GameObject dogFoodPriceChangeUI;
    public TMP_Text dfpcuiStockText;
    private TMP_Text dfpcuiPriceText;

    public GameObject tennisBallPriceChangeUI;
    public TMP_Text tbpcuiStockText;
    private TMP_Text tbpcuiPriceText;




    public GameObject dogFoodShelf;
    public Sprite[] dogFoodShelfSprites;
    private int maxShelfSpaceDogFood;

    public GameObject tennisBallShelf;
    public Sprite[] tennisBallShelfSprites;
    private int maxShelfSpaceTennisBall;

    public GameObject dogToyShelf;
    public Sprite[] dogToyShelfSprites;
    private int maxShelfSpaceDogToy;

    public GameObject dogTreatShelf;
    public Sprite[] dogTreatShelfSprites;
    private int maxShelfSpaceDogTreat;

    public GameObject dogBedShelf;
    public Sprite[] dogBedShelfSprites;
    private int maxShelfSpaceDogBed;


    private float customerTimer;

    private int numberOfPeople = 2;

    private string[] itemNeededList = new string[]{"dogFood", "tennisBall"};


    // Start is called before the first frame update
    void Start() {
        possibleCustomers = new List<customer>{
            new customer("p1", "dogFood"),
            new customer("p1", "tennisBall"),
            new customer("p0", "dogTreat"),
            new customer("p0", "dogToy"),
            new customer("p0", "dogBed")
            };

        titleText = priceChangeUI.transform.GetChild(4).GetComponent<TMP_Text>();
        stockText = priceChangeUI.transform.GetChild(6).GetComponent<TMP_Text>();
        priceText = priceChangeUI.transform.GetChild(3).GetComponent<TMP_Text>();
        restockUIBtn = priceChangeUI.transform.GetChild(5).GetComponent<Button>();
        minusBtn = priceChangeUI.transform.GetChild(2).GetComponent<Button>();
        plusBtn = priceChangeUI.transform.GetChild(1).GetComponent<Button>();

        dfpcuiPriceText = dogFoodPriceChangeUI.transform.GetChild(3).GetComponent<TMP_Text>();
        tbpcuiPriceText = tennisBallPriceChangeUI.transform.GetChild(3).GetComponent<TMP_Text>();

        dfpcuiPriceText.text = "$" + PlayerPrefs.GetInt("dogFoodPrice", 10);
        tbpcuiPriceText.text = "$" + PlayerPrefs.GetInt("tennisBallPrice", 10);

        tennisBallPriceChangeUI.SetActive(false);
        dogFoodPriceChangeUI.SetActive(false);

        PlayerPrefs.SetInt("dogFoodStocked", 3);
        PlayerPrefs.SetInt("dogToyStocked", 3);
        PlayerPrefs.SetInt("dogTreatStocked", 3);
        PlayerPrefs.SetInt("dogBedStocked", 3);
        PlayerPrefs.SetInt("tennisBallStocked", 2);

        PlayerPrefs.SetInt("dogFoodInventory", 4);
        PlayerPrefs.SetInt("dogToyInventory", 4);
        PlayerPrefs.SetInt("dogTreatInventory", 4);
        PlayerPrefs.SetInt("dogBedInventory", 4);
        PlayerPrefs.SetInt("tennisBallInventory", 6);



        updateShelfSprites();
        //updateStockTxt();
        customerTimer = Random.Range(0, 5f);

        maxShelfSpaceTennisBall = tennisBallShelfSprites.Length - 1;
        maxShelfSpaceDogFood = dogFoodShelfSprites.Length - 1;
        maxShelfSpaceDogToy = dogToyShelfSprites.Length - 1;
        maxShelfSpaceDogTreat = dogTreatShelfSprites.Length - 1;
        maxShelfSpaceDogBed = dogBedShelfSprites.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        customerTimer -= Time.deltaTime;

        if (customerTimer < 0) {
            spawnCustomer(new customer("p" + Random.Range(0,numberOfPeople), itemNeededList[Random.Range(0, itemNeededList.Length)]));
            customerTimer = Random.Range(3, 60f);

        }
    }

    /*
    public void plusBtn(int whichItem) {  //0 = dogFood,  1 = tennisBalls,   2 = dogToy,   3 = dogTreat,   4 = dogBed
        switch (whichItem) {
            case 0:
                PlayerPrefs.SetInt("dogFoodPrice", PlayerPrefs.GetInt("dogFoodPrice",10) + 1);
                dfpcuiPriceText.text = "$" + PlayerPrefs.GetInt("dogFoodPrice", 10);
                break;
            case 1:
                PlayerPrefs.SetInt("tennisBallPrice", PlayerPrefs.GetInt("tennisBallPrice", 10) + 1);
                tbpcuiPriceText.text = "$" + PlayerPrefs.GetInt("tennisBallPrice", 10);
                break;
            case 2:
                PlayerPrefs.SetInt("dogToyPrice", PlayerPrefs.GetInt("dogToyPrice", 10) + 1);
                priceText.text = "$" + PlayerPrefs.GetInt("dogToyPrice", 10);
                break;
            case 3:
                PlayerPrefs.SetInt("dogTreatPrice", PlayerPrefs.GetInt("dogTreatPrice", 10) + 1);
                priceText.text = "$" + PlayerPrefs.GetInt("dogTreatPrice", 10);
                break;
            case 4:
                PlayerPrefs.SetInt("dogBedPrice", PlayerPrefs.GetInt("dogBedPrice", 10) + 1);
                priceText.text = "$" + PlayerPrefs.GetInt("dogBedPrice", 10);
                break;
            default:
                Debug.Log("Unkown Input Plus Button");
                break;

        }
    }
    */

    public void plusMinusBtn(int whichItem, bool add) {  //0 = dogFood,  1 = tennisBalls,   2 = dogToy,   3 = dogTreat,   4 = dogBed
        int modifier = add ? 1 : -1;
        string item = "";

        switch (whichItem) {
            case 0:
                item = "dogFood";
                break;
            case 1:
                item = "tennisBall";
                break;
            case 2:
                item = "dogToy";
                break;
            case 3:
                item = "dogTreat";
                break;
            case 4:
                item = "dogBed";
                break;
            default:
                Debug.Log("Error in plusMinusBtn function");
                break;
        }

        PlayerPrefs.SetInt(item + "Price", PlayerPrefs.GetInt(item + "Price", 10) + modifier);
        priceText.text = "$" + PlayerPrefs.GetInt(item + "Price", 10);

        /*
        switch (whichItem)
        {
            case 0:
                PlayerPrefs.SetInt("dogFoodPrice", PlayerPrefs.GetInt("dogFoodPrice", 10) + modifier);
                priceText.text = "$" + PlayerPrefs.GetInt("dogFoodPrice", 10);
                break;
            case 1:
                PlayerPrefs.SetInt("tennisBallPrice", PlayerPrefs.GetInt("tennisBallPrice", 10) + modifier);
                priceText.text = "$" + PlayerPrefs.GetInt("tennisBallPrice", 10);
                break;
            case 2:
                PlayerPrefs.SetInt("dogToyPrice", PlayerPrefs.GetInt("dogToyPrice", 10) + modifier);
                priceText.text = "$" + PlayerPrefs.GetInt("dogToyPrice", 10);
                break;
            case 3:
                PlayerPrefs.SetInt("dogTreatPrice", PlayerPrefs.GetInt("dogTreatPrice", 10) + modifier);
                priceText.text = "$" + PlayerPrefs.GetInt("dogTreatPrice", 10);
                break;
            case 4:
                PlayerPrefs.SetInt("dogBedPrice", PlayerPrefs.GetInt("dogBedPrice", 10) + modifier);
                priceText.text = "$" + PlayerPrefs.GetInt("dogBedPrice", 10);
                break;
            default:
                Debug.Log("Unkown Input Plus Button");
                break;
        }
        */
    }

    /*
    public void minusBtn(int whichItem) {  //0 = dogFood,  1 = tennisBalls,   2 = dogToy,   3 = dogTreat,   4 = dogBed
        switch (whichItem) {
            case 0:
                PlayerPrefs.SetInt("dogFoodPrice", PlayerPrefs.GetInt("dogFoodPrice", 10) - 1);
                dfpcuiPriceText.text = "$" + PlayerPrefs.GetInt("dogFoodPrice", 10);
                break;
            case 1:
                PlayerPrefs.SetInt("tennisBallPrice", PlayerPrefs.GetInt("tennisBallPrice", 10) - 1);
                tbpcuiPriceText.text = "$" + PlayerPrefs.GetInt("tennisBallPrice", 10);
                break;
            default:
                Debug.Log("Unkown Input Minus Button");
                break;
        }
    }
    */

    public void restock (int whichItem) {  //0 = dogFood,  1 = tennisBall,   2 = dogToy,   3 = dogTreat,   4 = dogBed
        string item = "";
        int maxShelfSpace = -1;

        switch (whichItem) {
            case 0:
                item = "dogFood";
                maxShelfSpace = maxShelfSpaceDogFood;
                break;
            case 1:
                item = "tennisBall";
                maxShelfSpace = maxShelfSpaceTennisBall;
                break;
            case 2:
                item = "dogToy";
                maxShelfSpace = maxShelfSpaceDogToy;
                break;
            case 3:
                item = "dogTreat";
                maxShelfSpace = maxShelfSpaceDogTreat;
                break;
            case 4:
                item = "dogBed";
                maxShelfSpace = maxShelfSpaceDogBed;
                break;
            default:
                Debug.Log("Error in restock function");
                break;
        }

        if (PlayerPrefs.GetInt(item + "Stocked") < maxShelfSpace && PlayerPrefs.GetInt(item + "Inventory") > 0)
        {
            int emptyShelfSpace = maxShelfSpace - PlayerPrefs.GetInt(item + "Stocked");

            if (PlayerPrefs.GetInt(item + "Inventory") >= emptyShelfSpace) // Inventory is greater than or equal to the empty shelf space
            {
                PlayerPrefs.SetInt(item + "Stocked", maxShelfSpace);
                PlayerPrefs.SetInt(item + "Inventory", PlayerPrefs.GetInt(item + "Inventory") - emptyShelfSpace);
            }
            else // Inventory is less than the empty shelf space
            {
                PlayerPrefs.SetInt(item + "Stocked", PlayerPrefs.GetInt(item + "Stocked") + PlayerPrefs.GetInt(item + "Inventory"));
                PlayerPrefs.SetInt(item + "Inventory", 0);
            }
        }

        stockText.text = "Inventory: " + PlayerPrefs.GetInt(item + "Inventory") + "   Stock: " + PlayerPrefs.GetInt(item + "Stocked") + "/" + maxShelfSpace;


        updateShelfSprites();
        /*
        switch (whichItem) {
            case 0:
                if (PlayerPrefs.GetInt("dogFoodStocked") < maxShelfSpaceDogFood && PlayerPrefs.GetInt("dogFoodInventory") > 0) {
                    int emptyShelfSpace = maxShelfSpaceDogFood - PlayerPrefs.GetInt("dogFoodStocked");

                    if (PlayerPrefs.GetInt("dogFoodInventory") >= emptyShelfSpace) {
                        PlayerPrefs.SetInt("dogFoodStocked", maxShelfSpaceDogFood);
                        PlayerPrefs.SetInt("dogFoodInventory", PlayerPrefs.GetInt("dogFoodInventory") - emptyShelfSpace);
                    } else {
                        PlayerPrefs.SetInt("dogFoodStocked", PlayerPrefs.GetInt("dogFoodStocked") + PlayerPrefs.GetInt("dogFoodInventory"));
                        PlayerPrefs.SetInt("dogFoodInventory", PlayerPrefs.GetInt("dogFoodInventory") - emptyShelfSpace);
                    }
                }
                break;
            case 1:
                if (PlayerPrefs.GetInt("tennisBallStocked") < maxShelfSpaceTennisBall && PlayerPrefs.GetInt("tennisBallInventory") > 0)
                {
                    int emptyShelfSpace = maxShelfSpaceTennisBall - PlayerPrefs.GetInt("tennisBallStocked");

                    if (PlayerPrefs.GetInt("tennisBallInventory") >= emptyShelfSpace)
                    {
                        PlayerPrefs.SetInt("tennisBallStocked", maxShelfSpaceTennisBall);
                        PlayerPrefs.SetInt("tennisBallInventory", PlayerPrefs.GetInt("tennisBallInventory") - emptyShelfSpace);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("tennisBallStocked", PlayerPrefs.GetInt("tennisBallStocked") + PlayerPrefs.GetInt("tennisBallInventory"));
                        PlayerPrefs.SetInt("tennisBallInventory", PlayerPrefs.GetInt("tennisBallInventory") - emptyShelfSpace);
                    }
                }
                break;
            default:
                Debug.Log("Unrecognized input for restock button");
                break;
        }
        updateShelfSprites();
        //updateStockTxt();
        */
    }

    private void updateShelfSprites() {

        // dogFood
        dogFoodShelf.GetComponent<SpriteRenderer>().sprite = dogFoodShelfSprites[PlayerPrefs.GetInt("dogFoodStocked", 2)];

        //tennisBall
        tennisBallShelf.GetComponent<SpriteRenderer>().sprite = tennisBallShelfSprites[PlayerPrefs.GetInt("tennisBallStocked", 2)];

        //dogToy
        dogToyShelf.GetComponent<SpriteRenderer>().sprite = dogToyShelfSprites[PlayerPrefs.GetInt("dogToyStocked", 2)];

        //dogTreat
        dogTreatShelf.GetComponent<SpriteRenderer>().sprite = dogTreatShelfSprites[PlayerPrefs.GetInt("dogTreatStocked", 2)];

        //dogBed
        dogBedShelf.GetComponent<SpriteRenderer>().sprite = dogBedShelfSprites[PlayerPrefs.GetInt("dogBedStocked", 2)];

    }


    public void bringUpShelfUI (int whichItem) {  //0 = dogFood,  1 = tennisBall,   2 = dogToy,   3 = dogTreat,   4 = dogBed
        string item = "";
        int maxShelfSpace = -1;
        string title = "";

        switch (whichItem)
        {
            case 0:
                item = "dogFood";
                title = "Dog Food";
                maxShelfSpace = maxShelfSpaceDogFood;
                break;
            case 1:
                item = "tennisBall";
                title = "Tennis Balls";
                maxShelfSpace = maxShelfSpaceTennisBall;
                break;
            case 2:
                item = "dogToy";
                title = "Dog Toys";
                maxShelfSpace = maxShelfSpaceDogToy;
                break;
            case 3:
                item = "dogTreat";
                title = "Dog Treats";
                maxShelfSpace = maxShelfSpaceDogTreat;
                break;
            case 4:
                item = "dogBed";
                title = "Dog Beds";
                maxShelfSpace = maxShelfSpaceDogBed;
                break;
            default:
                Debug.Log("Error in bringUpShelfUI function");
                break;
        }

        titleText.text = title;
        priceText.text = "$" + PlayerPrefs.GetInt(item + "Price", 10);
        stockText.text = "Inventory: " + PlayerPrefs.GetInt(item + "Inventory") + "   Stock: " + PlayerPrefs.GetInt(item + "Stocked") + "/" + maxShelfSpace;

        /*
        //updateStockTxt();
        switch (whichItem) {
            case 0:
                priceText.text = "$" + PlayerPrefs.GetInt("dogFoodPrice", 10);
                stockText.text = "Inventory: " + PlayerPrefs.GetInt("dogFoodInventory") + "   Stock: " + PlayerPrefs.GetInt("dogFoodStocked") + "/" + maxShelfSpaceDogFood;

                //dfpcuiPriceText.text = "$" + PlayerPrefs.GetInt("dogFoodPrice", 10);
                //dogFoodPriceChangeUI.SetActive(true);
                break;
            case 1:
                priceText.text = "$" + PlayerPrefs.GetInt("tennisBallPrice", 10);
                stockText.text = "Inventory: " + PlayerPrefs.GetInt("tennisBallInventory") + "   Stock: " + PlayerPrefs.GetInt("tennisBallStocked") + "/" + maxShelfSpaceTennisBall;

                //tbpcuiPriceText.text = "$" + PlayerPrefs.GetInt("tennisBallPrice", 10);
                //tennisBallPriceChangeUI.SetActive(true);
                break;
            case 2:
                priceText.text = "$" + PlayerPrefs.GetInt("dogToyPrice", 10);
                stockText.text = "Inventory: " + PlayerPrefs.GetInt("dogToyInventory") + "   Stock: " + PlayerPrefs.GetInt("dogToyStocked") + "/" + maxShelfSpaceDogToy;
                break;
            case 3:
                priceText.text = "$" + PlayerPrefs.GetInt("dogTreatPrice", 10);
                stockText.text = "Inventory: " + PlayerPrefs.GetInt("dogTreatInventory") + "   Stock: " + PlayerPrefs.GetInt("dogTreatStocked") + "/" + maxShelfSpaceDogTreat;
                break;
            default:
                Debug.Log("Unkown input in opening shelf UI");
                break;
        }
        */

        restockUIBtn.onClick.RemoveAllListeners();
        plusBtn.onClick.RemoveAllListeners();
        minusBtn.onClick.RemoveAllListeners();

        restockUIBtn.onClick.AddListener( delegate { restock(whichItem); });
        plusBtn.onClick.AddListener( delegate { plusMinusBtn(whichItem, true); });
        minusBtn.onClick.AddListener(delegate { plusMinusBtn(whichItem, false); });
        priceChangeUI.SetActive(true);

    }

    public void closeUI () {
        priceChangeUI.SetActive(false);

        tennisBallPriceChangeUI.SetActive(false);
        dogFoodPriceChangeUI.SetActive(false);
    }

    public void sellItemToCustomer(string item) {

        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + PlayerPrefs.GetInt(item + "Price", 10));
        PlayerPrefs.SetInt(item + "Stocked", PlayerPrefs.GetInt(item + "Stocked") - 1);

        /*
        switch (item) {
            case "dogFood":
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + PlayerPrefs.GetInt("dogFoodPrice", 10));
                PlayerPrefs.SetInt("dogFoodStocked", PlayerPrefs.GetInt("dogFoodStocked") - 1);
                break;
            case "tennisBall":
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + PlayerPrefs.GetInt("tennisBallPrice", 10));
                PlayerPrefs.SetInt("tennisBallStocked", PlayerPrefs.GetInt("tennisBallStocked") - 1);
                break;
            case "dogToy":
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + PlayerPrefs.GetInt("dogToyPrice", 10));
                PlayerPrefs.SetInt("dogToyStocked", PlayerPrefs.GetInt("dogToyStocked") - 1);
                break;
            case "dogTreat":
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + PlayerPrefs.GetInt("dogTreatPrice", 10));
                PlayerPrefs.SetInt("dogTreatStocked", PlayerPrefs.GetInt("dogTreatStocked") - 1);
                break;
            case "dogBed":
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + PlayerPrefs.GetInt("dogBedPrice", 10));
                PlayerPrefs.SetInt("dogBedStocked", PlayerPrefs.GetInt("dogBedStocked") - 1);
                break;
        }
        */
        updateShelfSprites();
    }

    private void spawnCustomer(customer customer) {
        GameObject a = Instantiate(customerObj, new Vector2(17, Random.Range(-2f, 0f)), Quaternion.identity);
        a.GetComponent<SpriteLibrary>().spriteLibraryAsset = Resources.Load(customer.sprite) as SpriteLibraryAsset;
        a.GetComponent<customerScr>().psuedoStart(customer.itemNeeded);
    }

    public void testDebug() {
        Debug.Log("Works");
    }

    /*
    private void updateStockTxt () {
        dfpcuiStockText.text = "Inventory: " + PlayerPrefs.GetInt("dogFoodInventory") + "   Stock: " + PlayerPrefs.GetInt("dogFoodStocked") + "/" + maxShelfSpaceDogFood;
        tbpcuiStockText.text = "Inventory: " + PlayerPrefs.GetInt("tennisBallInventory") + "   Stock: " + PlayerPrefs.GetInt("tennisBallStocked") + "/" + maxShelfSpaceTennisBall;
    }
    */
}
