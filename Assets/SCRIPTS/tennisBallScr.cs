using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class tennisBallScr : MonoBehaviour
{
    public UnityEvent onGroundHitEvent;

    public Transform transObject;
    public Transform transBody;
    public Transform transShadow;

    public Vector2 groundVelocity;
    public float verticalVelocity;
    private float lastInitialVerticalVelocity;

    public bool isGrounded;
    private bool followingFinger;

    public float gravity = -10f;

    private Vector3 lastPos;
    private Touch touch;

    public float distanceFromTouchToBall = 2f;

    public float debugMultiplier = 5f;
    public float debugOtherNumVertVel = 5f;

    private Vector2 smoothDampRef;

    private gameController gameController;

    private void Start()
    {
        gameController = GameObject.FindWithTag("MainCamera").GetComponent<gameController>();
        followingFinger = false;
        Initialize(Vector2.up, 5f);
    }

    void Update() {

        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && Vector2.Distance(Camera.main.ScreenToWorldPoint(touch.position), transObject.position) < distanceFromTouchToBall) {
                followingFinger = true;
            }

            if (touch.phase == TouchPhase.Ended && followingFinger || Input.touchCount > 1) {
                followingFinger = false;
                Initialize(((Vector2)lastPos - (Vector2)transObject.position) * debugMultiplier, debugOtherNumVertVel);

                foreach(GameObject i in gameController.dogsInDaYard) {
                    if (Vector2.Distance(i.transform.position, transform.position) < 2f) {
                        i.GetComponent<dogScr>().chaseBall(transform.position);
                    }
                }
            }
        }

        if (followingFinger) {
            //Initialize(((Vector2)Camera.main.ScreenToWorldPoint(touch.position) - (Vector2)lastPos) * 20f, 0f);
            //   Works    transform.position = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(touch.position), Time.deltaTime * 4);
            transform.position = Vector2.SmoothDamp(transform.position, Camera.main.ScreenToWorldPoint(touch.position), ref smoothDampRef, .05f, 20);
            //transObject.position = new Vector2(Camera.main.ScreenToWorldPoint(touch.position).x, Camera.main.ScreenToWorldPoint(touch.position).y);
            //transform.position = Vector2.Lerp();
            lastPos = Camera.main.ScreenToWorldPoint(touch.position);

            transShadow.position = new Vector2(transObject.position.x, transObject.position.y - .5f);
        } else {
            transShadow.position = new Vector2(transObject.position.x, transObject.position.y - .1f);
        }

        UpdatePosition();
        CheckGroundHit();
    }

    

    public void Initialize(Vector2 groundVelocity, float verticalVelocity) {
        isGrounded = false;
        this.groundVelocity = groundVelocity;
        this.verticalVelocity = verticalVelocity;
        lastInitialVerticalVelocity = verticalVelocity;
    }

    void UpdatePosition() {

        if (!isGrounded) {
            verticalVelocity += gravity * Time.deltaTime;
            transBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
        }

        if (transform.position.x < 6.7 && transform.position.x > -7.2 && transform.position.y < 14.2 && transform.position.y > 6.7) {
            transObject.position += (Vector3)groundVelocity * Time.deltaTime;
        }
    }

    void CheckGroundHit() {
        if(transBody.position.y < transObject.position.y && !isGrounded) {
            transBody.position = transObject.position;
            isGrounded = true;
            GroundHit();
        }

    }

    void GroundHit() {
        onGroundHitEvent.Invoke();
        foreach (GameObject i in gameController.dogsInDaYard)
        {
            if (Vector2.Distance(i.transform.position, transform.position) < 2f)
            {
                i.GetComponent<dogScr>().chaseBall(transform.position);
            }
        }
    }

    public void Stick() {
        groundVelocity = Vector2.zero;
    }

    public void Bounce(float divisionFactor) {
        Initialize(groundVelocity, lastInitialVerticalVelocity / divisionFactor);

    }

    public void SlowDownGroundVelocity(float divisionFactor) {
        groundVelocity = groundVelocity / divisionFactor;
    }
}
