using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    private Transform camera;

    void Start() {
        camera = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update() {
        transform.rotation = camera.transform.rotation;
    }
}
