using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShaker : MonoBehaviour {
    private Vector3 inclStart;

    void Start() {
        inclStart = new Vector3(Input.acceleration.x, Input.acceleration.y, Input.acceleration.y);
        inclStart.Normalize();
    }

    void Update() {
        Vector3 dir = Vector3.zero;
        dir.x = inclStart.x - Input.acceleration.x;
        dir.y = inclStart.y - Input.acceleration.y;
        dir.z = inclStart.z - Input.acceleration.z;

        transform.position = dir;
    }
}
