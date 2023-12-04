using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D.Animation;

public class dogUIElement : dogClass
{
    public dog dogInstance;

    public Image dogSprite;
    public Image heartImage;

    public TMP_Text dogName;
    public TMP_Text dogDescription;
    public Button editBtn;
    
    private gameController gameController;
    private dialogueController dialogueController;


    private GameObject contentContainer;
    public Button selectBtn;

    public Sprite[] heartSprites;

    private void Start() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        GameObject cam = GameObject.FindWithTag("MainCamera");
        gameController = cam.GetComponent<gameController>();
        dialogueController = cam.GetComponent<dialogueController>();

        contentContainer = GameObject.Find("selectedDogContentContainer");

        if (editBtn != null && transform.parent == contentContainer) {
            editBtn.gameObject.SetActive(false);
        }

    }

    public void updateUI() {

        dogSprite.sprite = (Resources.Load(dogInstance.spriteName) as SpriteLibraryAsset).GetSprite("MainSprite", "Main");
        dogName.text = dogInstance.dogName;

        if (dogDescription != null) {
            dogDescription.text = dogInstance.dogDescription;
        }
        if (editBtn != null) {
            editBtn.onClick.AddListener(onBtnClick);
        }

        if (heartImage != null) {
            int love = dogInstance.love;
            int heartSpriteIndex = love switch {
                int i when i > 95 => 0,
                int i when i > 85 && i <= 95 => 1,
                int i when i > 75 && i <= 85 => 2,
                int i when i > 65 && i <= 75 => 3,
                int i when i > 55 && i <= 65 => 4,
                int i when i > 45 && i <= 55 => 5,
                int i when i > 35 && i <= 45 => 6,
                int i when i > 25 && i <= 35 => 7,
                int i when i <= 25 => 8,
                _ => 0
            };

            heartImage.sprite = heartSprites[heartSpriteIndex];
        }

    }

    public void onBtnClick() {
        gameController.openDogInfoBox(dogInstance);

    }

    public void onSelectBtnClick() {
        foreach (Transform l in contentContainer.transform) {
            Destroy(l.gameObject);
        }
        GameObject a = Instantiate(this.gameObject);
        a.GetComponent<dogUIElement>().selectBtn.gameObject.SetActive(false);
        a.GetComponent<dogUIElement>().dogInstance = dogInstance;
        a.transform.SetParent(contentContainer.transform);
        a.transform.localScale = Vector2.one;
        a.GetComponent<dogUIElement>().updateUI();
        dialogueController.selectedDog = dogInstance;
    }
}
