using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour {
    public GameObject bullet;
    public AudioClip fireSound;
    public AudioClip[] repairSound;
    public AudioClip upgradeSound;

    enum Type { Gatling = 0, Gauss = 1, Laser = 2 };
    [SerializeField] Type turretType = Type.Gatling;
    [SerializeField] Material MatStandard;
    [SerializeField] Material MatRepairTurret;
    [SerializeField] Material MatRepairBase;
    [SerializeField] Material MatUpgradeTurret;
    [SerializeField] Material MatUpgradeBase;
    private Transform target;
    private Renderer rendTurr;
    private Renderer rendNeck;
    private Renderer rendBase;

    private float rotSpeed;
    private float rangeDist;
    private float fireRate;
    private float damage;
    private int level;
    private int upgradeCost;

    private Transform turret;
    private Transform muzzle;
    private AudioSource source;
    private ActorCommons actor;
    private Renderer scorch;
    [SerializeField] private Text textLevel;
    private bool snd;

    private float pitch;
    private float yaw;
    private bool isAimInRange;

    private bool isFireworks;
    private bool isSuperReload;
    private bool isVampire;
    private float healValue;
    
    void Awake() {
        actor = GetComponent<ActorCommons>();
        isFireworks = GameObject.Find("BarMutator3") != null;
        isSuperReload = GameObject.Find("BarMutator4") != null;
        isVampire = GameObject.Find("BarMutator0") != null;
        healValue = GameObject.Find("GameManager").GetComponent<GameManager>().GetDifficultyFactor() - 0.5f;
    }

    void Start() {
        level++;
        turret = transform.GetChild(1).transform;
        muzzle = transform.GetChild(1).transform.GetChild(0).transform;
        muzzle.localScale = Vector3.zero;
        source = transform.GetChild(2).GetComponent<AudioSource>();
        rendBase = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        rendNeck = transform.GetChild(0).GetChild(1).GetComponent<Renderer>();
        rendTurr = transform.GetChild(1).GetComponent<Renderer>();
        scorch = transform.Find("Scorch").GetComponent<Renderer>();
        scorch.enabled = false;
        snd = PlayerPrefs.GetInt("sounds", 1) == 1;
        yaw = transform.eulerAngles.y;
        switch ( turretType ) {
            case Type.Gatling: {
                rotSpeed = 90.0f;
                rangeDist = 40.0f;
                fireRate = 3.0f;
                damage = 3.0f;
                upgradeCost = 100;
            } break;
            case Type.Gauss: {
                rotSpeed = 60.0f;
                rangeDist = 60.0f;
                fireRate = 0.3f;
                damage = 10.0f;
                upgradeCost = 200;
            } break;
            case Type.Laser: {
                rotSpeed = 120.0f;
                rangeDist = 70.0f;
                fireRate = 4.0f;
                damage = 5.0f;
                upgradeCost = 350;
            } break;
        }
        if ( isSuperReload ) { fireRate *= 1.5f; }
        StartCoroutine(SeekEnemy());
        StartCoroutine(FireEnemy());
        UpdateLevelText();
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateTurretContour();
    }

    void FixedUpdate() {
        if ( actor.IsAlive() ) {
            Aim();
        }
        muzzle.localScale *= 0.75f;
    }

    public int GetType() {
        return (int) turretType;
    }

    public int GetLevel() {
        return level;
    }

    public int GetUpgradeCost() {
        return upgradeCost;
    }

    public int QueryStatus(int budget) {
        // -2: dead; -1: nothing to do; 0: upgradeable; >= 0: repair
        if ( !actor.IsAlive() ) { return -2; }
        if ( actor.HasMaxHealth() || isVampire ) {
            if ( level < GameObject.Find("GameManager").GetComponent<GameManager>().GetTurretLevel((int) turretType) && ( budget >= upgradeCost) ) {
                return 0;
            } else {
                return -1;
            }
        } else {
            if ( actor.HealthMissing() > 0 && !isVampire ) {
                return (int) Mathf.Min(budget, Mathf.Ceil(actor.HealthMissing()));
            } else {
                return -1;
            }
        }
    }

    public void Repair(int health) {
        if ( actor.IsAlive() ) {
            actor.GiveHealth(health);
            actor.ApplyDamage(0);
            if ( repairSound.Length > 0 ) {
                source.PlayOneShot(repairSound[Random.Range(0, repairSound.Length)]);
            }
        }
    }

    public void Upgrade() {
        source.PlayOneShot(upgradeSound, 1.0f);
        level++;
        UpdateLevelText();
        switch ( turretType ) {
            case Type.Gatling: {
                switch ( level ) {
                    case 2: {
                        if ( isVampire ) {
                            actor.SetMaxHealth2(78);
                        } else {
                            actor.SetMaxHealth(78);
                        }
                        upgradeCost = 250;
                    } break;
                    case 3: {
                        fireRate = 3.6f;
                        upgradeCost = 400;
                    } break;
                    case 4: {
                        damage = 3.75f;
                        upgradeCost = 700;
                    } break;
                    case 5: {
                        if ( isVampire ) {
                            actor.SetMaxHealth2(97.5f);
                        } else {
                            actor.SetMaxHealth(97.5f);
                        }
                    } break;
                }
            } break;
            case Type.Gauss: {
                switch ( level ) {
                    case 2: {
                        if ( isVampire ) {
                            actor.SetMaxHealth2(96);
                        } else {
                            actor.SetMaxHealth(96);
                        }
                        upgradeCost = 350;
                    } break;
                    case 3: {
                        damage = 12.5f;
                        upgradeCost = 500;
                    } break;
                    case 4: {
                        damage = 15.0f;
                        upgradeCost = 800;
                    } break;
                    case 5: {
                        fireRate = 0.375f;
                    } break;
                }
            } break;
            case Type.Laser: {
                switch ( level ) {
                    case 2: {
                        if ( isVampire ) {
                            actor.SetMaxHealth2(62.5f);
                        } else {
                            actor.SetMaxHealth(62.5f);
                        }
                        upgradeCost = 450;
                    } break;
                    case 3: {
                        fireRate = 5.0f;
                        upgradeCost = 600;
                    } break;
                    case 4: {
                        damage = 6.25f;
                        upgradeCost = 900;
                    } break;
                    case 5: {
                        fireRate = 6.0f;
                    } break;
                }
            } break;
        }
        if ( isSuperReload ) { fireRate *= 1.5f; }
    }

    public void UpdateContour(int budget) {
        if ( !actor.IsAlive() ) { HideContour(); return; }
        if ( actor.HasMaxHealth() || isVampire ) {
            if ( level < GameObject.Find("GameManager").GetComponent<GameManager>().GetTurretLevel((int) turretType) && ( budget >= upgradeCost) ) {
                rendBase.material = MatUpgradeBase;
                rendNeck.material = MatUpgradeBase;
                rendTurr.material = MatUpgradeTurret;
            } else {
                HideContour();
            }
        } else {
            if ( actor.HealthMissing() > 0.0f && !isVampire ) {
                rendBase.material = MatRepairBase;
                rendNeck.material = MatRepairBase;
                rendTurr.material = MatRepairTurret;
            } else {
                HideContour();
            }
        }
    }

    public void HideContour() {
        rendBase.material = MatStandard;
        rendNeck.material = MatStandard;
        rendTurr.material = MatStandard;
    }

    public void DeliverDamage(float dmg) {
        if ( actor.IsAlive() ) {
            actor.ApplyDamage(dmg);
            if ( !actor.IsAlive() ) {
                Kill();
                GameObject.Find("GameManager").GetComponent<GameManager>().EnemyKills(transform.root.gameObject, (int) turretType);
            }
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateTurretsHealth();
        }
    }

    private void Kill() {
        scorch.enabled = true;
        transform.Find("TargetCollider").gameObject.SetActive(false);
        turret.GetComponent<Rigidbody>().isKinematic = false;
        Vector3 offset = new Vector3(Random.value, Random.value, Random.value);
        if ( isFireworks ) {
            turret.GetComponent<Rigidbody>().AddExplosionForce(15.0f, transform.root.position + offset, 9.0f, 1.0f, ForceMode.Impulse);
        } else {
            turret.GetComponent<Rigidbody>().AddExplosionForce(5.0f, transform.root.position + offset, 3.0f, 1.0f, ForceMode.Impulse);
        }
        StopAllCoroutines();
        StartCoroutine(SelfDestruct());
    }

    private void PlaySound(AudioClip clip) {
        if ( snd ) {
            source.PlayOneShot(clip, 1.0f);
        }
    }

    public string GetBaseValues() {
        return actor.GetMaxHealth().ToString("##0.#") + "\n" +
            damage.ToString("##0.#") + "\n" +
            fireRate.ToString("#0.0##");
    }

    public string GetFullValues() {
        string str0 = "", str1 = "", str2 = "";
        switch ( turretType ) {
            case Type.Gatling: {
                switch ( level ) {
                    case 1: {
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 2: {
                        str0 = " (+20%)";
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 3: {
                        str0 = " (+20%)";
                        if ( isSuperReload ) {
                            str2 = " (+70%)";
                        } else {
                            str2 = " (+20%)";
                        }
                    } break;
                    case 4: {
                        str0 = " (+20%)";
                        str1 = " (+25%)";
                        if ( isSuperReload ) {
                            str2 = " (+70%)";
                        } else {
                            str2 = " (+20%)";
                        }
                    } break;
                    case 5: {
                        str0 = " (+50%)";
                        str1 = " (+25%)";
                        if ( isSuperReload ) {
                            str2 = " (+70%)";
                        } else {
                            str2 = " (+20%)";
                        }
                    } break;
                }
            } break;
            case Type.Gauss: {
                switch ( level ) {
                    case 1: {
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 2: {
                        str0 = " (+20%)";
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 3: {
                        str0 = " (+20%)";
                        str1 = " (+25%)";
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 4: {
                        str0 = " (+20%)";
                        str1 = " (+50%)";
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 5: {
                        str0 = " (+20%)";
                        str1 = " (+50%)";
                        if ( isSuperReload ) {
                            str2 = " (+75%)";
                        } else {
                            str2 = " (+25%)";
                        }
                    } break;
                }
            } break;
            case Type.Laser: {
                switch ( level ) {
                    case 1: {
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 2: {
                        str0 = " (+25%)";
                        if ( isSuperReload ) {
                            str2 = " (+50%)";
                        }
                    } break;
                    case 3: {
                        str0 = " (+25%)";
                        if ( isSuperReload ) {
                            str2 = " (+75%)";
                        } else {
                            str2 = " (+25%)";
                        }
                    } break;
                    case 4: {
                        str0 = " (+25%)";
                        str1 = " (+25%)";
                        if ( isSuperReload ) {
                            str2 = " (+75%)";
                        } else {
                            str2 = " (+25%)";
                        }
                    } break;
                    case 5: {
                        str0 = " (+25%)";
                        str1 = " (+25%)";
                        if ( isSuperReload ) {
                            str2 = " (+100%)";
                        } else {
                            str2 = " (+50%)";
                        }
                    } break;
                }
            } break;
        }
        return actor.GetMaxHealth().ToString("##0.#") + str0 + "\n" +
            damage.ToString("##0.#") + str1 + "\n" +
            fireRate.ToString("#0.0##") + str2;
    }

    private void UpdateLevelText() {
        string roman;
        switch ( level ) {
            case 1: roman = "I"; break;
            case 2: roman = "II"; break;
            case 3: roman = "III"; break;
            case 4: roman = "IV"; break;
            case 5: roman = "V"; break;
            default: roman = "X"; break;
        }
        textLevel.text = roman;
    }

    private void Aim() {
        isAimInRange = false;
        if ( target == null ) { return; }
        float aimOffset;
        switch ( turretType ) {
            case Type.Gatling: aimOffset = 2.0f; break;
            case Type.Laser: aimOffset = 1.0f; break;
            default: aimOffset = 0.0f; break;
        }
        Vector3 targetDir = target.position + target.right * aimOffset - (turret.position + 0.212f * turret.up);
        Vector3 forwardProj = Vector3.Normalize(new Vector3(turret.forward.x, 0.0f, turret.forward.z));
        Vector3 targetProj = Vector3.Normalize(new Vector3(targetDir.x, 0.0f, targetDir.z));
        if ( Vector3.Dot(forwardProj, targetProj) >= Mathf.Cos(Mathf.PI/16) ) { isAimInRange = true; }
        float targetPitch = Vector3.SignedAngle(targetProj, targetDir, Vector3.Cross(Vector3.up, targetDir));

        float yawDiff = Vector3.SignedAngle(forwardProj, targetProj, Vector3.up);
        float pitchDiff = targetPitch - pitch;

        if ( Mathf.Abs(yawDiff) > rotSpeed * Time.deltaTime )   { yaw += Mathf.Sign(yawDiff) * rotSpeed * Time.deltaTime; }     else { yaw += yawDiff; }
        if ( Mathf.Abs(pitchDiff) > rotSpeed/2 * Time.deltaTime ) { pitch += Mathf.Sign(pitchDiff) * rotSpeed/2 * Time.deltaTime; } else { pitch += pitchDiff; }

        if ( pitch < -60 ) { pitch = -60; }
        if ( pitch > 30 )  { pitch = 30; }

        turret.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }

    private Transform GetClosestEnemy(GameObject[] enemies, float range) {
        GameObject bestTarget = null;
        float closestDistanceSqr = range * range;
        Vector3 currentPosition = transform.position;
        foreach ( GameObject potentialTarget in enemies ) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if ( dSqrToTarget < closestDistanceSqr ) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget == null ? null: bestTarget.transform;
    }

    IEnumerator SeekEnemy() {
        while ( true ) {
            target = GetClosestEnemy(GameObject.FindGameObjectsWithTag("Enemy"), rangeDist);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator FireEnemy() {
        Vector3 from, to;
        while ( true ) {
            from = turret.position + 0.212f * turret.up + turret.forward * 2.0f;
            to = turret.forward;
            if ( isAimInRange ) {
                switch ( turretType ) {
                    case Type.Gatling: FireGatling(from); break;
                    case Type.Gauss: FireGauss(from); break;
                    case Type.Laser: FireLaser(from); break;
                    default: break;
                }
                yield return new WaitForSeconds(1.0f / fireRate);
            } else {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void FireGatling(Vector3 from) {
        Transform blt = Instantiate(bullet, from, turret.rotation).transform;
        blt.GetComponent<Bullet>().SetProperties(damage, (int) turretType, this);
        PlaySound(fireSound);
        muzzle.localScale = Vector3.one * 1.5f;
    }

    private void FireGauss(Vector3 from) {
        Transform blt = Instantiate(bullet, from, turret.rotation).transform;
        blt.GetComponent<Bullet>().SetProperties(damage, (int) turretType, this);
        PlaySound(fireSound);
        muzzle.localScale = Vector3.one * 1.5f;
    }

    private void FireLaser(Vector3 from) {
        Transform blt = Instantiate(bullet, from, turret.rotation).transform;
        blt.GetComponent<Bullet>().SetProperties(damage, (int) turretType, this);
        PlaySound(fireSound);
        muzzle.localScale = Vector3.one * 1.5f;
    }

    public void ConfirmHit() {
        if ( actor.IsAlive() && isVampire ) {
            actor.GiveHealth(healValue);
            actor.ApplyDamage(0);
        }
    }

    IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(10.0f);
        Destroy(transform.root.gameObject);
    }
}
