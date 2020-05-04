using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityMovementAI;

public class Enemy : MonoBehaviour {
    [SerializeField] private int barrels;
    public GameObject bullet;
    [SerializeField] private float rangeDist;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    public AudioClip fireSound;
    public AudioClip shieldGainSound;
    public AudioClip shieldLoseSound;
    [SerializeField] private bool hasShield;
    [SerializeField] private int scoreWorth;
    [SerializeField] private bool isBoss;
    private bool isEmped;
    private bool isFireworks;
    private bool isRegeneration;
    
    private ActorCommons actor;
    private EvadeUnit evade;
    private SeekUnit seek;
    private ArrayList guns = new ArrayList();
    private Renderer shield;
    private ParticleSystem flareGain;
    private ParticleSystem flareLose;
    [SerializeField] private ParticleSystem empEffect;
    private Transform target;
    private AudioSource source;
    private bool sounds;
    private int lastHitBy;
    private CanvasGroup cgBoss;
    private Slider slBoss;

    void Awake() {
        actor = GetComponent<ActorCommons>();
        isFireworks = GameObject.Find("BarMutator3") != null;
        isRegeneration = GameObject.Find("BarMutator1") != null;
        if ( GameObject.Find("BarMutator2") != null ) { rangeDist *= 2; }
        if ( isBoss ) {
            cgBoss = GameObject.Find("PanelBossIndicator").GetComponent<CanvasGroup>();
            slBoss = GameObject.Find("SliderBossHealthbar").GetComponent<Slider>();
        }
    }

    void Start() {
        // Difficulty & mutators modifiers
        float _dF = GameObject.Find("GameManager").GetComponent<GameManager>().GetDifficultyFactor();
        actor.SetMaxHealth(actor.GetMaxHealth() * _dF);
        damage *= _dF;
        // Wave progress modifiers
        int _wave = GameObject.Find("GameManager").GetComponent<GameManager>().GetWaveCurrent();
        if ( _wave > 19 ) {
            actor.SetMaxHealth(actor.GetMaxHealth() * 2.5f);
            damage *= 2.0f;
        } else if ( _wave > 14 ) {
            actor.SetMaxHealth(actor.GetMaxHealth() * 2.0f);
            damage *= 1.5f;
        } else if ( _wave > 9 ) {
            actor.SetMaxHealth(actor.GetMaxHealth() * 1.5f);
            damage *= 1.5f;
        } else if ( _wave > 4 ) {
            actor.SetMaxHealth(actor.GetMaxHealth() * 1.5f);
        }
        evade = transform.parent.GetComponent<EvadeUnit>();
        seek = transform.parent.GetComponent<SeekUnit>();
        source = transform.root.GetComponentInChildren<AudioSource>();
        sounds = PlayerPrefs.GetInt("sounds", 1) == 1;
        for ( int i = 0; i < barrels; ++i ) {
            guns.Add(transform.GetChild(2 + i));
            transform.GetChild(2 + i).localScale *= 0.0f;
        }
        if ( hasShield ) {
            shield = transform.GetChild(2).GetComponent<Renderer>();
            flareGain = transform.root.Find("FlareGain").GetComponent<ParticleSystem>();
            flareLose = transform.root.Find("FlareLose").GetComponent<ParticleSystem>();
        }
        StartCoroutine(EvadeAllies());
        StartCoroutine(AlternateSeek());
        StartCoroutine(SeekEnemies());
        if ( barrels > 0 ) {
            StartCoroutine(AttackEnemies());
        }
        if ( hasShield ) {
            StartCoroutine(HealAllies());
        }
        if ( isBoss ) {
            cgBoss.alpha = 1.0f;
            slBoss.value = 1.0f;
        }
        if ( isRegeneration ) {
            StartCoroutine(SelfHealing());
        }
    }

    void FixedUpdate() {
        if ( shield != null ) {
            if ( HasShield() ) {
                float offset = Time.time * 0.25f;
                shield.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
                /*if ( actor.HasShield() ) {
                    actor.GiveShield(Time.deltaTime);
                    actor.ApplyDamage(0);
                }*/
            }
        }
        for ( int i = 0; i < barrels; ++i ) {
            ((Transform) guns[i]).localScale *= 0.5f;
        }
    }

    public void Heal(float health) {
        actor.GiveHealth(health);
        actor.ApplyDamage(0);
        if ( isBoss ) {
            slBoss.value = actor.GetHealthPercentage();
        }
    }

    public void DeliverDamage(float dmg, int hitBy) {
        if ( hitBy != -1 ) { lastHitBy = hitBy; }
        if ( actor.IsAlive() ) {
            actor.ApplyDamage(dmg);
            if ( !actor.IsAlive() ) {
                Kill();
                GameObject.Find("GameManager").GetComponent<GameManager>().PlayerKills(transform.root.gameObject, isBoss, scoreWorth, hitBy);
            } else {
                if ( hasShield && !actor.HasShield() ) {
                    if ( shield.enabled ) {
                        shield.enabled = false;
                        flareLose.Play();
                        source.PlayOneShot(shieldLoseSound, 1.0f);
                        StartCoroutine(ReloadShield());
                    }
                }
                if ( isBoss ) {
                    slBoss.value = actor.GetHealthPercentage();
                }
            }
        }
    }

    public void ApplyEmp() {
        isEmped = true;
        empEffect.Play();
        StartCoroutine(EmpCoroutine());
    }

    private void Kill() {
        if ( hasShield ) {
            shield.enabled = false;
        }
        if ( isBoss ) {
            cgBoss.alpha = 0.0f;
        }
        transform.root.gameObject.tag = "Untagged";
        if ( isFireworks ) {
            transform.root.GetComponentInChildren<Rigidbody>().AddExplosionForce(15.0f, transform.root.position - transform.root.forward, 9.0f, 0.25f, ForceMode.Impulse);
        } else {
            transform.root.GetComponentInChildren<Rigidbody>().AddExplosionForce(5.0f, transform.root.position - transform.root.forward, 3.0f, 0.25f, ForceMode.Impulse);
        }
        StopAllCoroutines();
        DeactivateSteerings();
        StartCoroutine(SelfDestruct());
    }

    private void DeactivateSteerings() {
        transform.parent.GetComponent<MovementAIRigidbody>().enabled = false;
        transform.parent.GetComponent<SeekUnit>().enabled = false;
        transform.parent.GetComponent<Wander2>().enabled = false;
        transform.parent.GetComponent<Wander2Unit>().enabled = false;
        transform.parent.GetComponent<FleeUnit>().enabled = false;
        transform.parent.GetComponent<EvadeUnit>().enabled = false;
    }

    public bool HasShield() {
        return hasShield;
    }

    private Transform GetClosestEnemy(GameObject[] enemies, float range) {
        GameObject bestTarget = null;
        float closestDistanceSqr = range * range;
        Vector3 currentPosition = transform.position;
        Vector3 forwardProj, targetProj;
        foreach ( GameObject potentialTarget in enemies ) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            forwardProj = Vector3.Normalize(new Vector3(transform.forward.x, 0.0f, transform.forward.z));
            targetProj = Vector3.Normalize(new Vector3(directionToTarget.x, 0.0f, directionToTarget.z));
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if ( dSqrToTarget < closestDistanceSqr && Vector3.Dot(forwardProj, targetProj) >= Mathf.Cos(Mathf.PI/4) ) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget == null ? null: bestTarget.transform;
    }

    IEnumerator EvadeAllies() {
        GameManager gm = FindObjectOfType<GameManager>();
        MovementAIRigidbody[] allies;
        float dist, minDist;
        MovementAIRigidbody closestAlly;
        while ( true ) {
            allies = FindObjectsOfType<MovementAIRigidbody>();
            minDist = float.PositiveInfinity;
            closestAlly = null;
            foreach ( MovementAIRigidbody ally in allies ) {
                dist = (ally.transform.position - this.transform.position).sqrMagnitude;
                if ( dist < minDist && dist > 1.0f && gm.arrEnemies.Contains(ally) ) {
                    minDist = dist;
                    closestAlly = ally;
                }
            }
            evade.target = closestAlly;
            if ( closestAlly == null ) { evade.enabled = false; } else { evade.enabled = true; }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator AlternateSeek() {
        while ( true ) {
            if ( seek.enabled ) {
                seek.enabled = false;
                yield return new WaitForSeconds(Random.value * 0.35f);
            } else {
                seek.enabled = true;
                yield return new WaitForSeconds(Random.value * 4.15f);
            }
        }
    }

    IEnumerator SeekEnemies() {
        while ( true ) {
            target = GetClosestEnemy(GameObject.FindGameObjectsWithTag("Player"), rangeDist);
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator AttackEnemies() {
        int burst = Random.Range(2, 5), currBarrel = 0;
        Vector3 targetDir, forwardProj, targetProj;
        Transform currGun;
        while ( true ) {

            if ( target != null && !isEmped ) {
                currGun = (Transform) guns[currBarrel];
                targetDir = target.position - currGun.position;
                forwardProj = Vector3.Normalize(new Vector3(currGun.forward.x, 0.0f, currGun.forward.z));
                targetProj = Vector3.Normalize(new Vector3(targetDir.x, 0.0f, targetDir.z));
                if ( Vector3.Dot(forwardProj, targetProj) >= Mathf.Cos(Mathf.PI/4) ) {
                    for ( int i = 0; i < burst; ++i ) {
                        Transform blt = Instantiate(bullet, currGun.position, Quaternion.LookRotation(targetDir, Vector3.up)).transform;
                        blt.GetComponent<Bullet>().SetProperties(damage, -1);
                        ((Transform) guns[currBarrel]).localScale = Vector3.one;
                        currBarrel = (currBarrel + 1) % barrels;
                        --burst;
                        if ( sounds ) { source.PlayOneShot(fireSound, 1.0f); }
                        yield return new WaitForSeconds(1.0f / fireRate);
                    }
                    burst = Random.Range(2, 5);
                } else {
                    yield return new WaitForSeconds(Random.value);
                }
            } else {
                yield return new WaitForEndOfFrame();
            }

        }
    }

    IEnumerator HealAllies() {
        while ( true ) {
            if ( actor.HasShield() ) {
                Enemy[] allies = FindObjectsOfType<Enemy>();
                foreach ( Enemy ally in allies ) {
                    if ( !ally.HasShield() ) {
                        ally.Heal(1.0f);
                    }
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator ReloadShield() {
        yield return new WaitForSeconds(5.0f);
        shield.enabled = true;
        flareGain.Play();
        source.PlayOneShot(shieldGainSound, 1.0f);
        actor.GiveShield(100);
        actor.ApplyDamage(0);
    }

    IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(10.0f);
        Destroy(transform.root.gameObject);
    }

    IEnumerator EmpCoroutine() {
        yield return new WaitForSeconds(10.0f);
        isEmped = false;
    }

    IEnumerator SelfHealing() {
        while ( true ) {
            Heal(1.0f);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
