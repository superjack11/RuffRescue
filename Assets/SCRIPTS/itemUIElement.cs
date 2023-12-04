using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class itemUIElement : dogClass
{
    public item itemInstance;

    public Image itemSprite;
    public TMP_Text itemNameTxt;
    public TMP_Text itemPriceTxt;
    public Button buyOrEquipBtn;

    [SerializeField] private TMP_Text stockTxt;

    private gameController gameController;
    private dialogueController dialogueController;

    private bool inInventory; // Whether or not the player already owns the item. If true, the buy button becomes the equip button
    public string vendorItem;

    private int price;
    private int maxStock;
    private int stock;


    public int index;

    [SerializeField] private Sprite dogBedSprite;
    [SerializeField] private Sprite dogTreatSprite;
    [SerializeField] private Sprite dogToySprite;
    [SerializeField] private Sprite tennisBallSprite;
    [SerializeField] private Sprite dogFoodSprite;

    // Start is called before the first frame update
    void Start() {

        if (vendorItem.Length < 1) {
            stockTxt.gameObject.SetActive(false);
            price = itemInstance.value + Random.Range(-5, 5);

            Transform[] siblings = transform.parent.GetComponentsInChildren<Transform>();
            /*
            index = 0;

            for (int i = 0; i < siblings.Length; i++) {
                if (siblings[i].gameObject == this) {
                    index = i;
                    break;
                }
            }
            */


            inInventory = transform.parent.gameObject.name == "shopContent" ? false : true;

            if (inInventory && index <= 1) {  // Disables price and button becuase its already owned and equipped
                itemPriceTxt.enabled = false;
                buyOrEquipBtn.gameObject.SetActive(false);
            } else if (inInventory) {        // Disables price and enables the equip button
                itemPriceTxt.enabled = false;
                buyOrEquipBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "Equip";
            } else {      // Enables price and buy button
                itemPriceTxt.text = "$" + price;
                buyOrEquipBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "Buy";
            }

        } else {
            maxStock = Random.Range(2, 7);
            stock = maxStock;
            stockTxt.text = "Stock: " + maxStock + "/" + maxStock;
            stockTxt.gameObject.SetActive(true);
            buyOrEquipBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "Buy";

            /*
            int basePrice = 5;

            switch (vendorItem)
            {
                case "dogBed":
                    itemSprite.sprite = dogBedSprite;
                    itemNameTxt.text = "Dog Bed";
                    basePrice = 15;
                    break;
                case "dogTreat":
                    itemSprite.sprite = dogTreatSprite;
                    itemNameTxt.text = "Dog Treat";
                    basePrice = 4;
                    break;
                case "dogToy":
                    itemSprite.sprite = dogToySprite;
                    itemNameTxt.text = "Dog Toy";
                    basePrice = 3;
                    break;
                case "dogFood":
                    itemSprite.sprite = dogFoodSprite;
                    itemNameTxt.text = "Dog Food";
                    basePrice = 7;
                    break;
                case "tennisBall":
                    itemSprite.sprite = tennisBallSprite;
                    itemNameTxt.text = "Tennis Ball";
                    basePrice = 3;
                    break;
                default:
                    break;
            }

            price = basePrice + Random.Range(-2, 2);
            itemPriceTxt.text = "$" + price;
            */
            updateUI();
        }



        GameObject cam = GameObject.FindWithTag("MainCamera");
        gameController = cam.GetComponent<gameController>();
        dialogueController = cam.GetComponent<dialogueController>();
    }

    // Update is called once per frame
    public void updateUI() {
        int basePrice = 5;

        if (vendorItem.Length < 1) {
            itemSprite.sprite = Resources.Load<Sprite>("Items/" + itemInstance.spriteName);
            itemNameTxt.text = itemInstance.itemName;
        } else {
            switch(vendorItem){
                case "dogBed":
                    itemSprite.sprite = dogBedSprite;
                    itemNameTxt.text = "Dog Bed";
                    basePrice = 15;
                    break;
                case "dogTreat":
                    itemSprite.sprite = dogTreatSprite;
                    itemNameTxt.text = "Dog Treat";
                    basePrice = 4;
                    break;
                case "dogToy":
                    itemSprite.sprite = dogToySprite;
                    itemNameTxt.text = "Dog Toy";
                    basePrice = 3;
                    break;
                case "dogFood":
                    itemSprite.sprite = dogFoodSprite;
                    itemNameTxt.text = "Dog Food";
                    basePrice = 7;
                    break;
                case "tennisBall":
                    itemSprite.sprite = tennisBallSprite;
                    itemNameTxt.text = "Tennis Ball";
                    basePrice = 3;
                    break;
                default:
                    break;
            }

            price = basePrice + Random.Range(-2, 2);
            itemPriceTxt.text = "$" + price;
        }
        
    }

    public void onBtnClick() {
        if (vendorItem.Length < 1) {
            if (inInventory) { // Equips the item
                switch (itemInstance.itemType) {
                    case "wall":
                        gameController.swapInventoryItems(0,index);
                        break;
                    case "floor":
                        gameController.swapInventoryItems(1, index);
                        break;
                    case "rug":
                        gameController.swapInventoryItems(3, index);
                        break;
                    default:
                        gameController.swapInventoryItems(0, index);
                        break;
                }
            
            } else { // Buys the item
                if (PlayerPrefs.GetInt("coins", 0) >= price) {
                    gameController.addItem(itemInstance);
                    gameController.changeCoins(-price);
                    gameController.UpdateInventory();
                    Destroy(this.gameObject);
                }
            }
        } else { // Buys a vendor item
            if (PlayerPrefs.GetInt("coins", 0) >= price && stock > 0) {
                stock--;
                stockTxt.text = "Stock: " + stock + "/" + maxStock;
                gameController.changeCoins(-price);
                PlayerPrefs.SetInt(vendorItem + "Inventory", PlayerPrefs.GetInt(vendorItem + "Inventory") + 1);
                gameController.UpdateInventory();
                //Destroy(this.gameObject);
                if (stock == 0) {
                    Destroy(gameObject);
                }
            }
        }
    }

}
