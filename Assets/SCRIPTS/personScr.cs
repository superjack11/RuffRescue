using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class personScr : MonoBehaviour
{

    public GameObject cam;

    public bool canBePressed;

    private Animator animator;

    private GameObject exclaim;

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        exclaim = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        animator.SetBool("walking", false);
        cam = GameObject.Find("Main Camera");
        canvas = transform.GetComponentInChildren<Canvas>();
        canvas.worldCamera = cam.GetComponent<Camera>();
        canBePressed = false;
        exclaim.SetActive(false);

        StartCoroutine(move(new Vector2(0,-.5f)));

    }

    public void tapped() {
        if (canBePressed) {
            cam.GetComponent<dialogueController>().pressPerson();
            exclaim.SetActive(false);
            canBePressed = false;
        }
    }

    public void killSelf() {
        Destroy(this.gameObject);
    }

    IEnumerator move(Vector2 destination, bool die = false)
    {
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

        if (die)
        {
            Destroy(this.gameObject);
        }
        animator.SetBool("walking", false);
        exclaim.SetActive(true);
        canBePressed = true;

    }

    public void leave () {
        StartCoroutine(move(new Vector2(14,0), true));
    }

}
