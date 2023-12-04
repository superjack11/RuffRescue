using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.Animation;

public class dogScr : dogClass
{
    public dog dogInstance;

    public float love;
    public int energy;
    public int dogSociability;
    //public int nonDogSociability;
    public int vocality;

    //        Range of Values:      0  1  2  3  4

    public float walkRange = 5f;

    private float idleActionTimer;
    private float activeActionTimer;
    private float barkTimer;
    private float sleepTimer;
    private float eatTimer;

    public GameObject cam;

    public bool canAct;


    private float hunger;
    private float thirst;

    //public Transform tennisBall;

    public GameObject speechBubble;

    private GameObject contentContainer;
    public GameObject outsideDogUIPref;

    private float startTime;
    private float petTimer;

    private ParticleSystem ps;

    private gameController gameController;

    private GameObject textInBubble;

    private Animator animator;

    private float scale;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        textInBubble = GetComponentInChildren<TMP_Text>().gameObject;
        petTimer = 0;

        speechBubble.SetActive(false);
        canAct = true;
        cam = GameObject.Find("Main Camera");
        gameController = cam.GetComponent<gameController>();

        //idleActions();
        //StartCoroutine("runAroundRand");
        //runAroundRand();
        //StartCoroutine("runBackandForth");

        idleActionTimer = 20f;
        activeActionTimer = Random.Range(1f,5f);
        barkTimer = 10f;
        sleepTimer = 10f;
        eatTimer = Random.Range(1f, 5f);

        contentContainer = GameObject.Find("selectedDogContentContainerOutside");

        
    }

    public void psuedoStart() {
        scale = 1 + ((dogInstance.size - 2) * .1f);
        transform.localScale = new Vector2(scale, scale);
        energy = dogInstance.energy;
        dogSociability = dogInstance.dogSociability;
        vocality = dogInstance.vocality;
    }

    // Update is called once per frame
    void Update()
    {

        textInBubble.transform.localScale = new Vector3(-1 * transform.localScale.x,1,1);

        if (petTimer >= 0) {
            petTimer -= Time.deltaTime;
        }

        if (canAct) {
            if (idleActionTimer < 0) {
                idleActions();
                idleActionTimer = Random.Range(30 - energy * 5, 50 - energy * 5);
            } else {
                idleActionTimer -= Time.deltaTime;
            }

            if (energy >= 2) {
                if (activeActionTimer < 0) {
                    activeActions();
                    activeActionTimer = Random.Range(30 - energy * 5, 50 - energy * 5);
                } else {
                    activeActionTimer -= Time.deltaTime;
                }
            }

            if (vocality >= 2) {
                if (barkTimer < 0) {
                    StartCoroutine(speak("Arf"));
                    barkTimer = Random.Range(50 - vocality * 10, 70 - vocality * 10);
                } else {
                    barkTimer -= Time.deltaTime;
                }
            }

            if (energy <= 2) {
                if (sleepTimer < 0) {
                    StartCoroutine("sleep");
                    sleepTimer = Random.Range(50f, 100f);
                } else {
                    sleepTimer -= Time.deltaTime;
                }
            }

            if (eatTimer < 0) {
                StartCoroutine("eat");
                eatTimer = Random.Range(30f, 50f);
            } else {
                eatTimer -= Time.deltaTime;
            }
        }

        if (Input.touchCount > 0) {  // Touch Input
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && Vector2.Distance(Camera.main.ScreenToWorldPoint(touch.position), transform.position) < 1) {
                float startTime = Time.time;
                foreach (Transform l in contentContainer.transform)
                {
                    Destroy(l.gameObject);
                }
                GameObject a = Instantiate(outsideDogUIPref);
                a.GetComponent<dogUIElement>().dogInstance = dogInstance;
                a.transform.SetParent(contentContainer.transform);
                a.transform.localScale = Vector2.one;
                a.GetComponent<dogUIElement>().updateUI();
            }

            if (touch.phase == TouchPhase.Moved && Vector2.Distance(Camera.main.ScreenToWorldPoint(touch.position), transform.position) < 1 && Time.time - startTime > .25f && petTimer <= 0) {
                petActions();
            }
        }

        if (Input.GetMouseButton(0)) { // Mouse Input
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(clickPos, transform.position) < 1) {
                foreach (Transform l in contentContainer.transform) {
                    Destroy(l.gameObject);
                }
                GameObject a = Instantiate(outsideDogUIPref);
                a.GetComponent<dogUIElement>().dogInstance = dogInstance;
                a.transform.SetParent(contentContainer.transform);
                a.transform.localScale = Vector2.one;
                a.GetComponent<dogUIElement>().updateUI();
            }

            if (Vector2.Distance(clickPos, transform.position) < 1 && petTimer <= 0) {
                petActions();
            }



        }

    }

    void petActions () {
        Debug.Log("petActions");

        petTimer = 2f;
        loveIncrease(1);

        if (Random.Range(0, 10 * vocality) > 20) {
            StartCoroutine(speak("Arf"));
        } else if (Random.Range(0,2) == 1) {
            StartCoroutine("wag");
        } else {
            StartCoroutine("sitWag");
        }

    }

    IEnumerator eat() {
        canAct = false;

        animator.SetBool("walking", true);

        Vector2 targ = new Vector2(3,14.3f);
        float lerpDuration = Random.Range(1f - energy / 5, 2f - energy / 5);

        Vector2 pos = transform.position;

        transform.localScale = new Vector2(scale, scale);

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(pos, targ, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targ;

        animator.SetBool("walking", true);

        animator.SetBool("eating", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("eating", false);

        walkRange = Random.Range(1f, 8f);
        Vector2 target = new Vector2(transform.position.x + Random.Range(-walkRange, walkRange), transform.position.y + Random.Range(-walkRange, walkRange));

        while (target.x > 6.3f || target.x < -6.3f || target.y > 14.3f || target.y < 7.5f)
        {
            target = new Vector2(transform.position.x + Random.Range(-walkRange, walkRange), transform.position.y + Random.Range(-walkRange, walkRange));
        }

        StartCoroutine(moveDogSub(target));
        yield return new WaitForSeconds(1f);

        canAct = true;
    }

    IEnumerator sitWag()
    {
        Debug.Log("sitWag");

        canAct = false;
        animator.SetBool("sitWagging", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("sitWagging", false);
        canAct = true;
    }

    IEnumerator wag () {
        Debug.Log("wag");

        canAct = false;
        animator.SetBool("wagging", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("wagging", false);
        canAct = true;
    }

    void loveIncrease(int amount) {
        Debug.Log("loveIncrease");

        dogInstance.love += dogInstance.love >= 100 ? 0 : amount;
        gameController.writeChanges(dogInstance);
        var psem = ps.emission;
        psem.rateOverTime = amount + 2;
        ps.Play();

    }

    void idleActions () {  //Picks idle action for dog
        int rng = Random.Range(0,3);

        switch(rng) {
            case 0:
                StartCoroutine("idleWalk");
                break;
            default:
                StartCoroutine("idleWalk");
                break;
        }
    }

    void activeActions() {  //Picks active action for dog
        int rng = Random.Range(0, 3000);
        switch (rng) {
            case 0:
                StartCoroutine("runBackandForth");
                break;
            case 1:
                if (gameController.dogsInDaYard.Count > 1) {
                    StartCoroutine("interact");
                }
                break;
            default:
                if (gameController.dogsInDaYard.Count > 1) {
                    StartCoroutine("interact");
                }
                break;
        }
        


    }

    public void chaseBall(Vector2 target) {
        Debug.Log("chaseBall");

        StartCoroutine(moveDogSub(target));
    }

    IEnumerator moveDogSub(Vector2 targ, float lerpDuration = -1) {
        Debug.Log("moveDogSub");

        canAct = false;
        animator.SetBool("walking", true);

        if (lerpDuration == -1) {
            lerpDuration = Random.Range(1f - energy / 5, 2f - energy / 5);
        }

        Vector2 pos = transform.position;

        if (transform.position.x < targ.x) {
            transform.localScale = new Vector3(scale, scale, scale);
        } else {
            transform.localScale = new Vector3(-scale, scale, scale);
        }

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(pos, targ, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targ;

        animator.SetBool("walking", false);
        canAct = true;
    }

    IEnumerator idleWalk() {   //Dog walks to a random position close by
        Debug.Log("idleWalk");

        canAct = false;
        walkRange = Random.Range(1f,8f);
        Vector2 target = new Vector2(transform.position.x + Random.Range(-walkRange, walkRange), transform.position.y + Random.Range(-walkRange, walkRange));

        while (target.x > 6.3f || target.x < -6.3f || target.y > 14.3f || target.y < 7.5f) {
            target = new Vector2(transform.position.x + Random.Range(-walkRange, walkRange), transform.position.y + Random.Range(-walkRange, walkRange));
        }

        StartCoroutine(moveDogSub(target));
        yield return new WaitForSeconds(1f);
        canAct = true;
    }

    IEnumerator runBackandForth () {
        Debug.Log("runBackandForth");

        canAct = false;
        Vector2 target = new Vector2(-6, Random.Range(8, 13));

        float initialLerpDuration = Vector2.Distance(transform.position, target) / 8;

        Vector2 pos = transform.position;

        if (transform.position.x < target.x)
        {
            transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            transform.localScale = new Vector2(-scale, scale);
        }

        float timeElapsed = 0;

        animator.SetBool("walking", true);
        while (timeElapsed < initialLerpDuration) {
            float t = timeElapsed / initialLerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(pos, target, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("walking", false);


        transform.position = target;

        animator.SetBool("running", true);

        for (int i = 0; i < Random.Range(1,3); i++) {
            pos = transform.position;
            timeElapsed = 0;
            target = new Vector2(5.75f, Random.Range(8, 13));

            if (transform.position.x < target.x)
            {
                transform.localScale = new Vector2(scale, scale);
            }
            else
            {
                transform.localScale = new Vector2(-scale, scale);
            }

            while (timeElapsed < 1.5f) {
                float t = timeElapsed / 1.5f;
                t = t * t * (3f - 2f * t);
                transform.position = Vector2.Lerp(pos, target, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = target;

            pos = transform.position;
            timeElapsed = 0;
            target = new Vector2(-6, Random.Range(8, 13));

            if (transform.position.x < target.x)
            {
                transform.localScale = new Vector2(scale, scale);
            }
            else
            {
                transform.localScale = new Vector2(-scale, scale);
            }

            while (timeElapsed < 1.5f) {
                float t = timeElapsed / 1.5f;
                t = t * t * (3f - 2f * t);
                transform.position = Vector2.Lerp(pos, target, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = target;

        }

        animator.SetBool("running", false);


        StartCoroutine("idleWalk");
        yield return new WaitForSeconds(1f);
        canAct = true;
    }

    IEnumerator runAroundRand() {
        Debug.Log("runAroundRand");

        canAct = false;
        for (int i = 0; i < Random.Range(4, 8); i++) {
            Vector2 target = new Vector2(Random.Range(-7f, 8.5f), Random.Range(8f, 13f));

            float lerpDuration = Vector2.Distance(transform.position, target) / 8;
            Vector2 pos = transform.position;

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
        }
        

        /*
        for (int i = 0; i < Random.Range(4, 8); i++)
        {
            Vector2 target = new Vector2(Random.Range(-7f, 8.5f), Random.Range(8f, 13f));

            StartCoroutine(moveDog(target));
        }
        */

        StartCoroutine("idleWalk");
        yield return new WaitForSeconds(1f);
        canAct = true;
    }

    // Lie down

    public IEnumerator speak(string text, float duration = 1f) {
        Debug.Log("speak");

        canAct = false;
        textInBubble.GetComponent<TMP_Text>().text = text;
        speechBubble.SetActive(true);
        animator.SetBool("barking", true);
        // PLay bark sound
        yield return new WaitForSeconds(duration);
        animator.SetBool("barking", false);
        speechBubble.SetActive(false);
        canAct = true;
    }


    IEnumerator interact() {
        Debug.Log("interact");

        canAct = false;
        GameObject otherDog = gameController.dogsInDaYard[Random.Range(0, gameController.dogsInDaYard.Count)];

        while (otherDog == this.gameObject) {
            otherDog = gameController.dogsInDaYard[Random.Range(0, gameController.dogsInDaYard.Count)];
        }

        otherDog.GetComponent<dogScr>().canAct = false;

        /*
        if (otherDog.transform.localScale.x > 0)
        {

            //StartCoroutine(moveDogSub(new Vector2(otherDog.transform.position.x + 3, otherDog.transform.position.y)));

            //////////// Move Dog Sub Beginning
            animator.SetBool("walking", true);

            Vector2 targ = new Vector2(otherDog.transform.position.x + 3, otherDog.transform.position.y);
            float lerpDuration = Random.Range(1f - energy / 5, 2f - energy / 5);

            Vector2 pos = transform.position;

            transform.localScale = transform.position.x < targ.x ? new Vector2(scale, scale) : new Vector2(-scale, scale);

            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                float t = timeElapsed / lerpDuration;
                t = t * t * (3f - 2f * t);
                transform.position = Vector2.Lerp(pos, targ, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = targ;

            animator.SetBool("walking", false);
            //////////// Move Dog Sub End
            
            transform.localScale = new Vector2(-scale, scale);
        } else {
            //StartCoroutine(moveDogSub(new Vector2(otherDog.transform.position.x - 3, otherDog.transform.position.y)));


            transform.localScale = new Vector2(scale, scale);
        }
        */

        int disFromDog = otherDog.transform.localScale.x > 0 ? 3 : -3;

        //////////// Move Dog Sub Beginning
        animator.SetBool("walking", true);

        Vector2 targ = new Vector2(otherDog.transform.position.x + disFromDog, otherDog.transform.position.y);
        float lerpDuration = Random.Range(1f - energy / 5, 2f - energy / 5);

        Vector2 pos = transform.position;

        transform.localScale = transform.position.x < targ.x ? new Vector2(scale, scale) : new Vector2(-scale, scale);

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(pos, targ, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targ;

        animator.SetBool("walking", false);
        //////////// Move Dog Sub End
        ///
        transform.localScale = otherDog.transform.localScale.x > 0 ? new Vector2(-scale, scale) : new Vector2(scale, scale);


        // Wag  Or  Lie Down  Or  Bark

        yield return new WaitForSeconds(2);

        int ranNum = Random.Range(0, 2);

        if (Random.Range(0, 10 * vocality) > 15) {
            StartCoroutine(speak("Bark"));
        } else {
            switch (ranNum) {
                case 0:
                    StartCoroutine("wag");
                    break;
                case 1:
                    StartCoroutine("sitWag");
                    break;
                default:
                    StartCoroutine("wag");
                    break;
            }
        }


        if (otherDog.GetComponent<dogScr>().dogInstance.dogSociability > 2) {
            otherDog.GetComponent<dogScr>().StartCoroutine(speak("Bark"));
            otherDog.GetComponent<dogScr>().loveIncrease(1);
            loveIncrease(1);
        } else {
            otherDog.GetComponent<dogScr>().StartCoroutine(speak("Grrr"));
        }

        yield return new WaitForSeconds(2);

        otherDog.GetComponent<dogScr>().canAct = true;
        canAct = true;
    }

    IEnumerator sleep() {
        Debug.Log("sleep");

        canAct = false;
        GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteLibrary>().spriteLibraryAsset.GetSprite("Lying", "headDown");
        yield return new WaitForSeconds(20f - energy * 3);
        GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteLibrary>().spriteLibraryAsset.GetSprite("MainSprite", "Main");
        canAct = true;
    }


}
