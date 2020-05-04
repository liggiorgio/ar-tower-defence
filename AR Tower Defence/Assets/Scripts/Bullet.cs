using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public enum TargetLayer { Player, Enemies };
    public TargetLayer targetMask;
    public GameObject sparks;

    [SerializeField] private float life;
    [SerializeField] private float speed;
    [SerializeField] private float force;
    private float damage;
    private int shooter;
    private Turret turrFrom;
    private bool hasHit = false;
    private string mask;
    private bool isFireworks;

    void Awake() {
        isFireworks = GameObject.Find("BarMutator3") != null;
    }

    void Start() {
        switch ( targetMask ) {
            case TargetLayer.Player: mask = "Player"; break;
            case TargetLayer.Enemies: mask = "Enemies"; break;
        }
        Destroy(this.gameObject, life);
    }

    void FixedUpdate() {
        int layerMask = LayerMask.GetMask(mask, "Ground");
        if ( !hasHit ) {
            RaycastHit hit;
            if ( Physics.Raycast(transform.position, transform.forward, out hit, speed, layerMask) ) {
                hasHit = true;
                transform.Translate(transform.forward * hit.distance, Space.World);
                Rigidbody rb = hit.transform.root.GetComponent<Rigidbody>();
                if ( rb != null ) {
                    rb.AddForceAtPosition(transform.forward * force, hit.point, ForceMode.Impulse);
                }
                Enemy enemy = hit.transform.root.GetComponentInChildren<Enemy>();
                if ( enemy != null ) {
                    enemy.DeliverDamage(damage, shooter);
                    if ( enemy.HasShield() && enemy.GetComponent<ActorCommons>().HasShield() ) {
                        hasHit = false;
                        Vector3 reflected = Vector3.Reflect(transform.forward, hit.normal);
                        transform.rotation = Quaternion.LookRotation(reflected, Vector3.up);
                        transform.Translate(transform.forward * (speed - hit.distance), Space.World);
                    } else {
                        if ( turrFrom != null ) {
                            // Vampirism works only on health, not shields
                            turrFrom.ConfirmHit();
                        }
                    }
                } else if ( hit.transform.tag == "Player" ) {
                    CommandPost cp = hit.transform.root.GetComponent<CommandPost>();
                    if ( cp != null ) {
                        cp.DeliverDamage(damage);
                    } else {
                        hit.transform.root.GetComponent<Turret>().DeliverDamage(damage);
                    }
                }
                GameObject spks = Instantiate(sparks, hit.point, Quaternion.identity);
                if ( shooter == 1 ) {
                    if ( isFireworks ) {
                        spks.transform.localScale *= 3.0f;
                    }
                }
            } else {
                transform.Translate(transform.forward * speed, Space.World);
            }
            if ( hasHit ) {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void SetProperties(float dmg, int shotBy) {
        damage = dmg;
        shooter = shotBy;
        turrFrom = null;
    }

    public void SetProperties(float dmg, int shotBy, Turret turr) {
        damage = dmg;
        shooter = shotBy;
        turrFrom = turr;
    }
}
