using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployCommons : MonoBehaviour {
    public AudioClip[] deploySound;
    public bool hasRadiusRepair;
    public bool hasRadiusUpgrade;

    private MeshRenderer radiusNormal;
    private MeshRenderer radiusInvalid;
    private MeshRenderer radiusRepair;
    private MeshRenderer radiusUpgrade;
    private AudioSource source;
    private bool sounds;

    void Awake() {
        source = transform.root.GetComponentInChildren<AudioSource>();
        sounds = PlayerPrefs.GetInt("sounds", 1) == 1;
        radiusNormal = transform.Find("RadiusWhite").GetComponent<MeshRenderer>();
        radiusInvalid = transform.Find("RadiusRed").GetComponent<MeshRenderer>();
        if ( hasRadiusRepair ) { radiusRepair = transform.Find("RadiusYellow").GetComponent<MeshRenderer>(); } else { radiusRepair = null; }
        if ( hasRadiusUpgrade ) { radiusUpgrade = transform.Find("RadiusBlue").GetComponent<MeshRenderer>(); } else { radiusUpgrade = null; }
        ToggleRadius(false);
        PlaySoundDeployed();
    }

    void FixedUpdate() {
        ToggleRadius(false);
    }

    public void ToggleRadius(bool flag) {
        ToggleRadius(flag, flag, flag, flag);
    }

    public void ToggleRadius(int type, bool flag) {
        switch ( type ) {
            case 0: { radiusNormal.enabled = flag; } break;
            case 1: { radiusInvalid.enabled = flag; } break;
            case 2: { if ( radiusRepair != null ) { radiusRepair.enabled = flag; } } break;
            case 3: { if ( radiusUpgrade != null ) { radiusUpgrade.enabled = flag; } } break;
            default: break;
        }
    }

    public void ToggleRadius(bool normal, bool invalid, bool repair, bool upgrade) {
        radiusNormal.enabled = normal;
        radiusInvalid.enabled = normal;
        if ( radiusRepair != null ) { radiusRepair.enabled = repair; }
        if ( radiusUpgrade != null ) { radiusUpgrade.enabled = upgrade; }
    }

    public void PlaySoundDeployed() {
        if ( sounds && (deploySound.Length > 0) ) {
            source.PlayOneShot(deploySound[Random.Range(0, deploySound.Length)], 1.0f);
        }
    }
}
