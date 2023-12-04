using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using TMPro;
using UnityEngine.UI;

public class customerScr : MonoBehaviour
{
    public string itemNeeded = "dogFood";
    //private Vector2 shelfOfInterest;
    public Vector2 destination = new Vector2(-1.1f, 2.5f);

    private bool reachedShelf;

    private storeController storeController;

    private int basePrice = 10;

    public GameObject speechBubble;

    private Animator animator;

    public Image emojiImage;

    [SerializeField] private Sprite happyFace;
    [SerializeField] private Sprite sadFace;
    [SerializeField] private Sprite mehFace;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        storeController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<storeController>();
        reachedShelf = false;
        speechBubble.SetActive(false);

        if (Random.Range(0,2) == 0) {
            StartCoroutine("initialMove");
        } else {
            StartCoroutine("wander");
        }
        
    }

    IEnumerator initialMove() {
        animator.SetBool("walking", true);

        float lerpDuration = Vector2.Distance(transform.position, destination) / 2;


        Vector2 pos = transform.position;

        if (transform.position.x < destination.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(pos, destination, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;

        animator.SetBool("walking", false);
        reachedShelf = true;
    }

    public void psuedoStart(string item) {
        itemNeeded = item;
        switch (itemNeeded) {
            case "dogFood":
                destination = new Vector2(-1.1f, 2.5f);
                basePrice = 10;
                break;
            case "tennisBall":
                destination = new Vector2(-7f, -0.75f);
                basePrice = 5;
                break;
            case "dogToy":
                destination = new Vector2(1f, -0.75f);
                basePrice = 5;
                break;
            case "dogTreat":
                destination = new Vector2(-3f, -0.75f);
                basePrice = 7;
                break;
            case "dogBed":
                destination = new Vector2(-6.75f, 2f);
                basePrice = 20;
                break;
            default:
                break;
        }
    }

    

    private void FixedUpdate() {
        if (reachedShelf) {
            reachedShelf = false;
            StartCoroutine(judge(PlayerPrefs.GetInt(itemNeeded + "Price", 20)));
        }
    }

    IEnumerator move (Vector2 destination, bool die = false) {
        animator.SetBool("walking", true);

        float lerpDuration = Vector2.Distance(transform.position, destination) / 2;
        

        Vector2 pos = transform.position;

        if (transform.position.x < destination.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(pos, destination, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;

        animator.SetBool("walking", false);
        reachedShelf = true;

        if (die) {
            Destroy(this.gameObject);
        }
    }

    IEnumerator judge (int price) {
        int ranRange = Random.Range(-6, 5);
        Debug.Log("Base price: " + basePrice + ".  ranRange: " + ranRange + ".   price: " + price);

        if (PlayerPrefs.GetInt(itemNeeded + "Stocked") > 0) {

            if (price > basePrice + ranRange) {
                // Too Pricey
                emojiImage.sprite = sadFace;
                speechBubble.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                speechBubble.SetActive(false);
            } else if (price > basePrice + ranRange - ranRange / 2) {
                // Purchase, but its still a little pricey
                emojiImage.sprite = mehFace;
                speechBubble.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                speechBubble.SetActive(false);

                if (Random.Range(0,100) > 30) {
                    storeController.sellItemToCustomer(itemNeeded);
                    GetComponent<ParticleSystem>().Play();
                }
            } else {
                // Purchase item
                storeController.sellItemToCustomer(itemNeeded);
                emojiImage.sprite = happyFace;
                speechBubble.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                speechBubble.SetActive(false);
                GetComponent<ParticleSystem>().Play();
            }
        } else {
            emojiImage.sprite = sadFace;
            speechBubble.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            speechBubble.SetActive(false);
        }

        StartCoroutine(move(new Vector2(17,0), true));
    }

    IEnumerator wander () {
        Vector2 target = new Vector2(-1.1f + Random.Range(-0.1f,0.1f), 2.5f + Random.Range(-0.1f, 0.1f));

        animator.SetBool("walking", true);

        float lerpDuration = Vector2.Distance(transform.position, target) / 2;


        Vector2 pos = transform.position;

        if (transform.position.x < target.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(pos, target, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;

        animator.SetBool("walking", false);

        yield return new WaitForSeconds(.5f);

        if (Random.Range(0,2) == 0) {
            target = new Vector2(-7f + Random.Range(-0.1f, 0.1f), 2.5f + Random.Range(-0.1f, 0.1f));

            animator.SetBool("walking", true);

            lerpDuration = Vector2.Distance(transform.position, target) / 2;


            pos = transform.position;

            if (transform.position.x < target.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            timeElapsed = 0;

            while (timeElapsed < lerpDuration)
            {
                float t = timeElapsed / lerpDuration;
                t = t * t * (3f - 2f * t);
                transform.position = Vector2.Lerp(pos, target, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = target;

            animator.SetBool("walking", false);

            yield return new WaitForSeconds(.5f);

            StartCoroutine(move(new Vector2(17, 0), true));
        }
        else {
            target = new Vector2(-8f + Random.Range(-0.1f, 0.1f), -1f + Random.Range(-0.1f, 0.1f));

            animator.SetBool("walking", true);

            lerpDuration = Vector2.Distance(transform.position, target) / 2;


            pos = transform.position;

            if (transform.position.x < target.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            timeElapsed = 0;

            while (timeElapsed < lerpDuration)
            {
                float t = timeElapsed / lerpDuration;
                t = t * t * (3f - 2f * t);
                transform.position = Vector2.Lerp(pos, target, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = target;

            animator.SetBool("walking", false);

            yield return new WaitForSeconds(.5f);

            StartCoroutine(move(new Vector2(17, 0), true));
        }

    }
}
