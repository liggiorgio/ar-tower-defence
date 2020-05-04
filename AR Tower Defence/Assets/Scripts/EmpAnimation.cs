using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpAnimation : MonoBehaviour {
    public Material[] materials;
    private Renderer renderer;
    private int dir;

    void Start() {
        renderer = GetComponent<Renderer>();
        dir = 1 - 2 * Random.Range(0, 2);
        StartCoroutine(ChangeMaterial());
    }

    void FixedUpdate() {
        transform.localScale += 0.75f * Vector3.one;
        transform.Rotate(0.0f, 15.0f * dir, 0.0f, Space.World);
    }

    IEnumerator ChangeMaterial() {
        yield return new WaitForSeconds(0.05f);
        renderer.material = materials[0];
        yield return new WaitForSeconds(0.05f);
        renderer.material = materials[1];
        yield return new WaitForSeconds(0.05f);
        renderer.material = materials[2];
        yield return new WaitForSeconds(0.05f);
        renderer.material = materials[3];
        yield return new WaitForSeconds(0.05f);
        renderer.material = materials[4];
        yield return new WaitForSeconds(0.05f);
        Destroy(transform.parent.gameObject);
    }
}
