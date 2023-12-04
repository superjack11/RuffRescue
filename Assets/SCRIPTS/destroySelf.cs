using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySelf : MonoBehaviour
{
    public void DestroySelf() {
        Destroy(this.gameObject);
    }
}
