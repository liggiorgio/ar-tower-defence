using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateScore : MonoBehaviour {
    public float life;
    public float speed;

    private RectTransform rt;
    private CanvasGroup cg;

    void Start() {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        Destroy(this.gameObject, life + 0.5f);
    }

    void Update() {
        rt.Translate(0.0f, speed, 0.0f);
        if ( life < 0.5f ) {
            cg.alpha = (life / 0.5f);
        }
        life -= Time.deltaTime;
    }
}
