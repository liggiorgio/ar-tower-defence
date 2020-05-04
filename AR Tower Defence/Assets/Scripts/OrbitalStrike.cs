using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbitalStrike : MonoBehaviour {
    public float damage = 50.0f;
    public AudioClip soundCharge, soundHit;
    [SerializeField] private Transform beams;
    [SerializeField] private ParticleSystem explosion;
    private Transform target;
    private Transform ground;
    private AudioSource source;
    private GameObject tracker;
    private bool isFollowing = true;
    private bool sounds;
    private bool isFireworks;

    void Awake() {
        source = GetComponent<AudioSource>();
        ground = GameObject.FindWithTag("Ground").transform;
        sounds = PlayerPrefs.GetInt("sounds", 1) == 1;
        isFireworks = GameObject.Find("BarMutator3") != null;
        beams.localScale = Vector3.zero;
    }
    
    void Start() {
        if ( isFireworks ) {
            explosion.transform.localScale *= 3.0f;
        }
        StartCoroutine(Fire());
    }

    void FixedUpdate() {
        if ( target != null ) {
            if ( isFollowing ) {
                transform.position = target.position;
            }
        } else {
            transform.position = new Vector3(transform.position.x, ground.position.y, transform.position.z);
        }
        beams.localScale = new Vector3(beams.localScale.x * 0.8f, 1.0f, beams.localScale.z * 0.8f);
    }

    void Update() {
        if ( tracker != null ) {
            tracker.GetComponent<Image>().rectTransform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    public void Set(Transform tgt, float dmg, GameObject tkr) {
        target = tgt;
        damage = dmg;
        tracker = tkr;
    }

    IEnumerator Fire() {
        if ( sounds ) { source.PlayOneShot(soundCharge, 1.0f); }
        yield return new WaitForSeconds(2.5f);
        tracker.SetActive(false);
        isFollowing = false;
        if ( sounds ) { source.PlayOneShot(soundHit, 1.0f); }
        Enemy en = target.root.GetComponentInChildren<Enemy>();
        if ( en != null ) {
            en.DeliverDamage(damage, 3);
            Transform t = en.transform.root;
            if ( isFireworks ) {
            t.GetComponentInChildren<Rigidbody>().AddExplosionForce(15.0f, t.position - t.forward, 9.0f, -1.0f, ForceMode.Impulse);
        } else {
            t.GetComponentInChildren<Rigidbody>().AddExplosionForce(5.0f, t.position - t.forward, 3.0f, -1.0f, ForceMode.Impulse);
        }
        }
        explosion.Play();
        beams.localScale = Vector3.one;
        Destroy(this.gameObject, 3.0f);
    }
}
