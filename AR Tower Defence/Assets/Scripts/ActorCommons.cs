using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorCommons : MonoBehaviour {
    public float sp;
    public float hp;
    [SerializeField] Transform healthBar;
    public AudioClip[] destroySound;

    private float startSp, startHp;
    private bool sounds;
    private Slider hpBar, spBar;

    private ParticleSystem smoke;
    private ParticleSystem fire;
    private ParticleSystem explosion;
    private AudioSource source;

    void Awake() {
        startSp = sp;
        startHp = hp;
        spBar = healthBar.GetChild(0).transform.GetChild(0).GetComponent<Slider>();
        hpBar = healthBar.GetChild(0).transform.GetChild(1).GetComponent<Slider>();
        spBar.value = sp / startSp;
        hpBar.value = hp / startHp;
        if ( startSp == 0 ) { ToggleShieldBar(false); }
        smoke = transform.root.Find("Smoke").GetComponent<ParticleSystem>();
        fire = transform.root.Find("Fire").GetComponent<ParticleSystem>();
        explosion = transform.root.Find("Explosion").GetComponent<ParticleSystem>();
        source = transform.root.GetComponentInChildren<AudioSource>();
        sounds = PlayerPrefs.GetInt("sounds", 1) == 1;
    }

    void Start() {
        if ( GameObject.Find("BarMutator3") != null ) {
            explosion.transform.localScale *= 3.0f;
        }
    }

    public void ApplyDamage(float dmg) {
        sp -= dmg;
        if ( sp < 0.0f ) {
            hp += sp;
            sp = 0.0f;
        }
        if ( hp <= 0.0f ) {
            hp = 0.0f;
            PlaySoundDestroyed();
            healthBar.gameObject.SetActive(false);
            explosion.Play();
            fire.Play();
        } else if ( hp <= startHp/2 ) {
            smoke.Play();
        } else {
            smoke.Stop();
            fire.Stop();
        }
        spBar.value = sp / startSp;
        hpBar.value = hp / startHp;
    }

    public float GetHealthPercentage() {
        return hp / startHp;
    }

    public float GetShieldPercentage() {
        return sp / startSp;
    }

    public float GetMaxHealth() {
        return startHp;
    }

    public void SetMaxHealth(float newHealth) {
        hp = newHealth;
        startHp = newHealth;
        ApplyDamage(0.0f);
    }

    public void SetMaxHealth2(float newHealth) {
        hp = newHealth * hp / startHp;
        startHp = newHealth;
        ApplyDamage(0.0f);
    }

    public void GiveHealth(float health) {
        hp += health;
        if ( hp > startHp ) { hp = startHp; }
    }

    public void GiveShield(float shield) {
        sp += shield;
        ToggleShieldBar(true);
        if ( startSp == 0 ) { startSp = sp; }
        if ( sp > startSp ) { sp = startSp; }
    }

    public void RemoveShield() {
        sp = 0;
        startSp = 0;
        ToggleShieldBar(false);
    }

    public void ToggleShieldBar(bool flag) {
        healthBar.GetChild(0).transform.GetChild(0).gameObject.SetActive(flag);
    }

    public bool HasShield() {
        return sp > 0.0f;
    }

    public bool IsAlive() {
        return hp > 0.0f;
    }

    public bool HasMaxHealth() {
        return hp == startHp;
    }

    public float HealthMissing() {
        return startHp - hp;
    }

    public void PlaySoundDestroyed() {
        if ( sounds && (destroySound.Length > 0) ) {
            source.PlayOneShot(destroySound[Random.Range(0, destroySound.Length)], 1.0f);
        }
    }
}
