using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mopBucketScr : MonoBehaviour
{

    [SerializeField] private Sprite emptyBucket;
    [SerializeField] private Sprite bucketWithMop;
    [SerializeField] private GameObject mopPref;

    [HideInInspector] public GameObject mopObj;

    public bool mopping;

    // Start is called before the first frame update
    void Start()
    {
        mopping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {  // Touch Input
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && Vector2.Distance(Camera.main.ScreenToWorldPoint(touch.position), transform.position) < .5f) {
                GetComponent<SpriteRenderer>().sprite = emptyBucket;
                mopObj = Instantiate(mopPref, transform.position, Quaternion.identity);
                mopping = true;
            }

            if (touch.phase == TouchPhase.Moved) {
                mopObj.transform.position = Camera.main.ScreenToWorldPoint(touch.position);
            }

            if (touch.phase == TouchPhase.Ended) {
                GetComponent<SpriteRenderer>().sprite = bucketWithMop;
                Destroy(mopObj);
                mopping = false;
            }

        }

        if (Input.GetMouseButtonDown(0))
        { // Mouse Input
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(clickPos, transform.position) < .5f)
            {
                GetComponent<SpriteRenderer>().sprite = emptyBucket;
                mopObj = Instantiate(mopPref, transform.position, Quaternion.identity);
                mopping = true;
            }
        }

        if (Input.GetMouseButton(0) && mopObj != null) {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mopObj.transform.position = clickPos;
        }

        if (Input.GetMouseButtonUp(0)) {
            GetComponent<SpriteRenderer>().sprite = bucketWithMop;
            Destroy(mopObj);
            mopping = false;
        }
    }
}
