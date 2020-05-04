using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPost : MonoBehaviour {
    public GameObject orbitalStrikeBullet;
    public GameObject empBlastWave;
    public AudioClip empSound;
    private float damage = 100.0f;
    private float repFactor = 0.1f;
    private Vector3 eulers;
    private Transform arms;
    private Transform ring;
    private Transform core;
    private AudioSource source;
    private bool sounds;
    private ActorCommons actor;
    private Renderer scorch;
    [SerializeField] private Text textLevel;
    private int level;

    private GameObject goShieldBar;
    private Slider sliderShield;
    private Slider sliderHealth;
    private Text textHealth;

    private bool isSuperReload;

    [SerializeField] private Renderer[] ghosts;
    [SerializeField] private ParticleSystem shieldGain;
    [SerializeField] private ParticleSystem shieldLose;
    [SerializeField] private ParticleSystem empCharge;

    void Awake() {
        actor = GetComponent<ActorCommons>();
        GameObject.Find("PanelCommandPostIndicator").GetComponent<CanvasGroup>().alpha = 1.0f;
        goShieldBar = GameObject.Find("SliderCPShield");
        sliderShield = GameObject.Find("SliderCPShield").GetComponent<Slider>();
        sliderHealth = GameObject.Find("SliderCPHealth").GetComponent<Slider>();
        textHealth = GameObject.Find("TextCPHealth").GetComponent<Text>();
        goShieldBar.SetActive(false);
        DeliverDamage(0);
        isSuperReload = GameObject.Find("BarMutator4") != null;
    }

    void Start() {
        eulers = new Vector3(0.0f, 90.0f, 0.0f);
        arms = transform.GetChild(1);
        ring = transform.GetChild(2);
        core = transform.GetChild(3);
        source = transform.GetChild(4).GetComponent<AudioSource>();
        scorch = transform.Find("Scorch").GetComponent<Renderer>();
        scorch.enabled = false;
        sounds = PlayerPrefs.GetInt("sounds", 1) == 1;
        UpdateLevelText();
    }

    void FixedUpdate() {
        // Animation
        arms.Rotate(eulers * Time.deltaTime, Space.World);
        ring.Rotate(-eulers * Time.deltaTime, Space.World);
        core.Rotate(eulers * Time.deltaTime, Space.World);
        core.Rotate(new Vector3(-180.0f, 45.0f, -90.0f) * Time.deltaTime, Space.Self);
        float scaleValue = 100.0f + 2.5f*Mathf.Sin(8.0f * Time.time);
        core.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }

    void Update() {
        if ( IsShieldOn() ) {
            float offset = Time.time * 0.25f;
            ghosts[0].material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
        }
    }

    public void Repair() {
        float amount = actor.GetMaxHealth();
        amount *= repFactor;
        actor.GiveHealth(amount);
        DeliverDamage(0);
    }

    public void OrbitalAttack(Transform target, GameObject tracker) {
        var blt = Instantiate(orbitalStrikeBullet, target.position, Quaternion.identity);
        blt.GetComponent<OrbitalStrike>().Set(target, damage, tracker);
    }

    public void EmpBlast() {
        StartCoroutine(DoEMP());
    }

    public void GainShield() {
        actor.GiveShield(100);
        DeliverDamage(0);
    }

    public void DeliverDamage(float dmg) {
        if ( actor.IsAlive() ) {
            actor.ApplyDamage(dmg);
            if ( !actor.IsAlive() ) {
                Kill();
                GameObject.Find("GameManager").GetComponent<GameManager>().LoseWave();
            }
            sliderHealth.value = actor.GetHealthPercentage();
            textHealth.text = (actor.GetHealthPercentage() * 100.0f).ToString("##0.#") + "%";
            if ( actor.HasShield() ) {
                goShieldBar.SetActive(true);
                if ( !IsShieldOn() ) {
                    shieldGain.Play();
                }
                ToggleShield(true);
                sliderShield.value = actor.GetShieldPercentage();
            } else {
                goShieldBar.SetActive(false);
                if ( IsShieldOn() ) {
                    shieldLose.Play();
                }
                ToggleShield(false);
                actor.RemoveShield();
            }
        }
    }

    private bool IsShieldOn() {
        return ghosts[0].enabled;
    }

    private void ToggleShield(bool flag) {
        foreach ( Renderer r in ghosts ) {
            r.enabled = flag;
        }
    }

    private void Kill() {
        scorch.enabled = true;
        ToggleShield(false);
        transform.Find("TargetCollider").gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public void PlaySound(AudioClip clip) {
        if ( sounds ) {
            source.PlayOneShot(clip, 1.0f);
        }
    }

    public string GetBaseValues() {
        return actor.GetMaxHealth() + "\n" +
            damage.ToString("##0") + "\n" +
            (actor.GetMaxHealth() * repFactor).ToString("#0");
    }

    public string GetFullValues() {
        string str0 = "", str1 = "", str2 = "";
        switch ( level ) {
            case 2: case 3: {
                str0 = " (+20%)";
                if ( isSuperReload ) {
                    str1 = "(C/D)";
                }
            } break;
            case 4: case 5: {
                str0 = " (+20%)";
                if ( isSuperReload ) {
                    str1 = " (+20%, SR)";
                } else {
                    str1 = " (+20%)";
                }
            } break;
            case 6: case 7: {
                str0 = " (+40%)";
                if ( isSuperReload ) {
                    str1 = " (+20%, SR)";
                } else {
                    str1 = " (+20%)";
                }
            } break;
            case 8: {
                str0 = " (+40%)";
                if ( isSuperReload ) {
                    str1 = " (+20%, SR)";
                } else {
                    str1 = " (+20%)";
                }
                str2 = " (+20%)";
            } break;
        }
        return actor.GetMaxHealth() + str0 + "\n" +
            damage.ToString("##0") + str1 + "\n" + 
            (actor.GetMaxHealth() * repFactor).ToString("#0.#") + str2;
    }

    public void SetLevel(int lvl) {
        level = lvl;
        UpdateLevelText();
        switch ( level ) {
            case 2: case 3: {
                actor.SetMaxHealth2(120.0f);
            } break;
            case 4: case 5: {
                actor.SetMaxHealth2(120.0f);
                damage = 120.0f;
            } break;
            case 6: case 7: {
                actor.SetMaxHealth2(140.0f);
                damage = 120.0f;
            } break;
            case 8: {
                actor.SetMaxHealth2(140.0f);
                damage = 120.0f;
                repFactor = 0.3f;
            } break;
        }
    }

    private void UpdateLevelText() {
        string roman;
        switch ( level ) {
            case 1: roman = "I"; break;
            case 2: roman = "II"; break;
            case 3: roman = "III"; break;
            case 4: roman = "IV"; break;
            case 5: roman = "V"; break;
            case 6: roman = "VI"; break;
            case 7: roman = "VII"; break;
            case 8: roman = "VIII"; break;
            default: roman = "X"; break;
        }
        textLevel.text = roman;
    }

    IEnumerator DoEMP() {
        PlaySound(empSound);
        empCharge.Play();
        yield return new WaitForSeconds(1.0f);
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach ( Enemy e in enemies ) { e.ApplyEmp(); }
        empCharge.Play();
        for ( int i = 0; i < 7; ++i ) {
            Instantiate(empBlastWave, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
