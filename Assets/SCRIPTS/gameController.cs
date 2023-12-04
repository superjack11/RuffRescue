using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.Animation;


public class gameController : dogClass
{

    public List<dog> dogs = new List<dog>();               // List of all the dogs the player currently owns.
    public List<item> items = new List<item>();    // List of all the items the player currently owns. #1,2,3,4 should be what is currently equipped.

    public List<GameObject> dogUIElements = new List<GameObject>();
    public List<GameObject> itemUIElements = new List<GameObject>();

    public GameObject dogUI;
    public GameObject itemUI;

    public GameObject dogContentParent;
    public GameObject itemContentParent;

    private string seperatorStr;
    private string endStr;

    public GameObject dogInfoBox;
    public SpriteRenderer dogInfoSprite;
    public TMP_InputField dogInfoNameBox;
    public TMP_InputField dogInfoDescriptionBox;

    private int selectedInfoDogIndex;

    public List<GameObject> dogsInDaYard = new List<GameObject>();

    public TMP_Text coinsTxt;

    public Tile wallTile;
    public Tile wallTileBottom;
    public Tile wallTileTop;

    public Tile floorTile;
    public Tile floorTileCorner;

    private Tilemap tilemap;

    private TouchScreenKeyboard keyboard;

    public GameObject dogFoodShelf;
    public GameObject tennisBallShelf;

    public GameObject dogPrefab;

    private dialogueController dc;

    // Start is called before the first frame update
    void Start()
    {
        dc = GetComponent<dialogueController>();

        PlayerPrefs.SetInt("coins", 999999999);
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();

        coinsTxt.text = "Coins: " + PlayerPrefs.GetInt("coins", 0);
        PlayerPrefs.SetString("dogs", "d1@|&%Fidor@|&%A rowdy rowdy dog@|&%4@|&%2@|&%1@|&%2@|&%4@|&%2@|&%}.<$d2@|&%Yabba@|&%A rowdy rowdy dog@|&%0@|&%2@|&%1@|&%2@|&%4@|&%2@|&%}.<$");
        // "collie0@|&%Fiddle Sticks@|&%A semi rowdy dog@|&%2@|&%2@|&%2@|&%1@|&%2@|&%0@|&%2@|&%}.<$");

        PlayerPrefs.SetString("items", "wt0@|&%Dingy Wall@|&%wall@|&%30@|&%}.<$ft0@|&%Dingy Floor@|&%floor@|&%30@|&%}.<$");

        dogInfoBox.SetActive(false);

        seperatorStr = "@|&%";
        endStr = "}.<$";

        loadData();

        updateDogs();

        foreach (dog doug in dogs) {
            //GameObject b = Instantiate(Resources.Load(doug.spriteName), new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(8f, 12f)), Quaternion.identity) as GameObject; //Instantiates Dogs Outside
            GameObject b = Instantiate(dogPrefab, new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(8f, 12f)), Quaternion.identity) as GameObject; //Instantiates Dogs Outside
            b.GetComponent<SpriteLibrary>().spriteLibraryAsset = Resources.Load(doug.spriteName) as SpriteLibraryAsset;
            b.GetComponent<dogScr>().dogInstance = doug;
            b.GetComponent<dogScr>().psuedoStart();
            dogsInDaYard.Add(b);
        }
        updateSpritesFromInventory();
    }

    public void updateDogs(bool withSelectBtns = true) {
        foreach (Transform l in dogContentParent.transform) {
            Destroy(l.gameObject);
        }

        foreach (dog d in dogs) {
            GameObject a = Instantiate(dogUI);
            a.GetComponent<dogUIElement>().dogInstance = d;
            a.transform.SetParent(dogContentParent.transform);
            a.transform.localScale = Vector2.one;
            if (!withSelectBtns) {
                a.GetComponent<dogUIElement>().selectBtn.gameObject.SetActive(false);
            }
            //a.transform.position = new Vector3(a.transform.position.x, a.transform.position.y, 0f);
            a.GetComponent<dogUIElement>().updateUI();
            dogUIElements.Add(a);
        }
    }

    void saveData() {
        //Converts dogs list to string, then saves to PlayerPrefs
        string strConverted = "";
        foreach (dog d in dogs) {
            strConverted += d.generateCode();
        }
        PlayerPrefs.SetString("dogs", strConverted);
        ///////////////


        //Converts items list to string, then saves to PlayerPrefs
        strConverted = "";
        foreach (item i in items)
        {
            strConverted += i.generateCode();
        }
        PlayerPrefs.SetString("items", strConverted);
        ///////////////
    }

    void loadData() {

        //     Load Dog Data

        string str = PlayerPrefs.GetString("dogs", "d1@|&%Fidor@|&%A rowdy rowdy dog@|&%2@|&%1@|&%2@|&%2@|&%0@|&%2@|&%}.<$");
        List<string> dogStrings = new List<string>();
        string a = "";
        string b = "";
        List<string> propertyStrings = new List<string>();


        for (int i = 0; i < str.Length; i++) { //Deconstructs playerprefs string into each dog
            if (str.Substring(i,4) != endStr) {
                a += str[i].ToString();
            } else {
                for (int o = 0; o < a.Length; o++) {
                    if (a.Substring(o, 4) != seperatorStr) {
                        b += a[o];
                    } else {
                        propertyStrings.Add(b);
                        b = "";
                        o += 3;
                    }
                    
                }

                dogs.Add(new dog(propertyStrings[0], propertyStrings[1], propertyStrings[2], Int32.Parse(propertyStrings[3]), Int32.Parse(propertyStrings[4]), Int32.Parse(propertyStrings[5]), Int32.Parse(propertyStrings[6]), Int32.Parse(propertyStrings[7]), Int32.Parse(propertyStrings[8])));
                propertyStrings.Clear();
                a = "";
                i += 3; //Adds 3 to the incrementor to account for the length of the seperatorString
            }
        }


        //     Load Item Data

        str = PlayerPrefs.GetString("items", "wallTile@|&%Dingy Wall@|&%wall@|&%30@|&%}.<$");
        List<string> itemStrings = new List<string>();
        a = "";
        b = "";
        propertyStrings = new List<string>();


        for (int i = 0; i < str.Length; i++) { //Deconstructs playerprefs string into each item
            if (str.Substring(i, 4) != endStr)
            {
                a += str[i].ToString();
            }
            else
            {
                for (int o = 0; o < a.Length; o++)
                {
                    if (a.Substring(o, 4) != seperatorStr)
                    {
                        b += a[o];
                    }
                    else
                    {
                        propertyStrings.Add(b);
                        b = "";
                        o += 3;
                    }

                }

                items.Add(new item(propertyStrings[0], propertyStrings[1], propertyStrings[2], Int32.Parse(propertyStrings[3])));
                propertyStrings.Clear();
                a = "";
                i += 3; //Adds 3 to the incrementor to account for the length of the seperatorString
            }
        }
    }

    public void openDogInfoBox(dog d) {
        foreach (dog a in dogs) {
            if (a == d) {
                selectedInfoDogIndex = dogs.IndexOf(a);
                break;
            }
        }

        dogInfoBox.SetActive(true);
        dogInfoSprite.sprite = (Resources.Load(d.spriteName) as SpriteLibraryAsset).GetSprite("MainSprite", "Main");
        dogInfoNameBox.text = d.dogName;
        dogInfoDescriptionBox.text = d.dogDescription;
    }

    public void closeDogInfoBox() {
        dogs[selectedInfoDogIndex].dogName = dogInfoNameBox.text;
        dogs[selectedInfoDogIndex].dogDescription = dogInfoDescriptionBox.text;

        foreach (Transform i in dogContentParent.transform) {
            i.gameObject.GetComponent<dogUIElement>().updateUI();
        }

        saveData();
        dogInfoBox.SetActive(false);
    }

    public void addDog(dog dig) {
        dogs.Add(new dog(dig.spriteName, dig.dogName, dig.dogDescription, dig.size, dig.energy, dig.dogSociability, dig.nonDogSociability, dig.vocality, dig.love));

        GameObject b = Instantiate(dogPrefab, new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(8f, 12f)), Quaternion.identity) as GameObject; //Instantiates Dogs Outside
        b.GetComponent<SpriteLibrary>().spriteLibraryAsset = Resources.Load(dig.spriteName) as SpriteLibraryAsset;
        b.GetComponent<dogScr>().dogInstance = dig;
        b.GetComponent<dogScr>().energy = dig.size;
        b.GetComponent<dogScr>().dogSociability = dig.dogSociability;
        b.GetComponent<dogScr>().vocality = dig.vocality;
        b.GetComponent<dogScr>().psuedoStart();
        dogsInDaYard.Add(b);
        saveData();

    }


    public void removeDog(dog dug) {
        int delIndexDogs = 0;
        int delIndexDogsInDaYard = 0;
        foreach (dog a in dogs) {
            if (a == dug) {
                delIndexDogs = dogs.IndexOf(a);
            }
        }
        foreach (GameObject a in dogsInDaYard) {
            if (a.GetComponent<dogScr>().dogInstance == dug) {
                delIndexDogsInDaYard = dogsInDaYard.IndexOf(a);
            }
        }
        dogs.Remove(dogs[delIndexDogs]);
        Destroy(dogsInDaYard[delIndexDogsInDaYard]);
        dogsInDaYard.Remove(dogsInDaYard[delIndexDogsInDaYard]);

        saveData();
    }
    
    public void writeChanges(dog dig) {
        int dogIndex = 0;
        foreach (dog a in dogs) {
            if (a == dig) {
                dogIndex = dogs.IndexOf(a);
            }
        }

        dogs[dogIndex] = dig;
    }

    public void addItem(item item) { // Adds item to inventory
        items.Add(new item(item.spriteName, item.itemName, item.itemType, item.value));
        saveData();
        UpdateInventory();
    }

    public void removeItem(item item) {
        int delIndex = 0;

        foreach (item i in items) {
            if (i == item) {
                delIndex = items.IndexOf(i);
            }
        }

        items.Remove(items[delIndex]);
        saveData();
        UpdateInventory();
    }

    public void UpdateInventory() { // Updates the inventory
        foreach (Transform l in itemContentParent.transform) {
            Destroy(l.gameObject);
        }

        foreach (item i in items) {
            GameObject a = Instantiate(itemUI);
            a.GetComponent<itemUIElement>().itemInstance = i;
            a.GetComponent<itemUIElement>().index = items.IndexOf(i);
            a.transform.SetParent(itemContentParent.transform);
            a.transform.localScale = Vector2.one;
            a.GetComponent<itemUIElement>().updateUI();
            itemUIElements.Add(a);
        }

        dc.updateVendorItemTxt();
    }

    public void updateSpritesFromInventory() {
        wallTile.sprite = Resources.Load<Sprite>("Items/" + items[0].spriteName);
        wallTileBottom.sprite = Resources.Load<Sprite>("Items/" + items[0].spriteName + "B"); //Denotes the Bottom variant of the wall sprite
        wallTileTop.sprite = Resources.Load<Sprite>("Items/" + items[0].spriteName + "T");   //Denotes the Top variant of the wall sprite

        floorTile.sprite = Resources.Load<Sprite>("Items/" + items[1].spriteName);
        floorTileCorner.sprite = Resources.Load<Sprite>("Items/" + items[1].spriteName + "C");  //Denotes the Corner variant of the floor sprite


        tilemap.RefreshAllTiles();
    }

    public void changeCoins(int amount) {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + amount);
        coinsTxt.text = "Coins: " + PlayerPrefs.GetInt("coins", 0);
    }

    public void swapInventoryItems(int index1, int index2) {
        item temp = items[index1];
        items[index1] = items[index2];
        items[index2] = temp;
        saveData();
        UpdateInventory();
        updateSpritesFromInventory();
    }

    public void pullUpKeyboard() {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }


}
