using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirtScr : MonoBehaviour
{

    private GameObject mop;
    private mopBucketScr mopBucketScr;

    [SerializeField] private GameObject moneyParticleSystem;

    private void Start()
    {
        mopBucketScr = GameObject.FindWithTag("mopBucket").GetComponent<mopBucketScr>();
    }

    private void Update()
    {
        // Phone Way
        if (mopBucketScr.mopping && Input.touchCount > 0 && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), transform.position) < 1f) {
            mop = mopBucketScr.mopObj;
            getMopped();
        }

        //Computer Way
        if (mopBucketScr.mopping && Input.GetMouseButton(0) && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) < 1f)
        {
            mop = mopBucketScr.mopObj;
            getMopped();
        }
    }

    void getMopped() {
        //mop.GetComponent<ParticleSystem>().Play();
        GameObject mps = Instantiate(moneyParticleSystem, transform.position, Quaternion.identity);
        mps.GetComponent<ParticleSystem>().Play();

        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1);
        Destroy(this.gameObject);
    }
}
