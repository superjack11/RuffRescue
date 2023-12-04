using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    float lerpDuration = 1f;

    public void moveCam(string targetPos) {
        Vector3 tg = new Vector3(0f, 10f, -10f);
        switch(targetPos) {
            case "outside":
                tg = new Vector3(0f, 10f, -10f);
                break;
            case "inside":
                tg = new Vector3(0f, 0f, -10f);
                break;
            default:
                tg = new Vector3(0f, 10f, -10f);
                break;
        }

        StartCoroutine(Lerp(tg));
    }

    public IEnumerator Lerp(Vector3 targetPos)
    {
        
        Vector3 pos = transform.position;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(pos, targetPos, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
    }
}
