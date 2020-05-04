using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityMovementAI;

public class GameManager : MonoBehaviour {
    public GameObject CommandPost;
    public GameObject GatlingTurret;
    public GameObject GaussTurret;
    public GameObject LaserTurret;
    public GameObject[] EnemyMinions;
    public GameObject[] EnemyBosses;
    public GameObject Ground;
    public GameObject scoreText;
    public enum Difficulty { Normal, Hard, Delta };

    private GameObject placementIndicatorPost;
    private GameObject placementIndicatorTurret;

    private enum State { Begin, BaseDeploy, WaveHalf, WaveBegin, Battle, Intermission };
    private GameUIManager gui;
    [HideInInspector] public Prefs prefs;
    private State gameState;
    private Difficulty difficulty;
    
    private Text textActionLabel;
    private Text textActionDescr;
    private Text textWave;
    private Text textEnemies;
    private Text textScore;
    private Text textCredits;
    private Text textCountdown;
    private Text textBonusProgress;
    private Text textBonusComplete;
    private Text textBonusPartial;


    private Text textGatlings;
    private Text textGatlingsRed;
    private Text textGausses;
    private Text textGaussesRed;
    private Text textLasers;
    private Text textLasersRed;
    private CanvasGroup cgGatlings;
    private CanvasGroup cgGausses;
    private CanvasGroup cgLasers;
    private Slider slGatlings;
    private Slider slGausses;
    private Slider slLasers;
    private Image imgOrbitalAABB;
    private Text textOrbitalLabel;
    private Slider slOrbital;
    private GameObject[] orbitalTarget = new GameObject[2];
    private Transform centerTransform;

    private GameObject oCommandPost = null;
    private ArrayList arrGatlings = new ArrayList();
    private ArrayList arrGausses = new ArrayList();
    private ArrayList arrLasers = new ArrayList();
    private GameObject oBoss;
    public ArrayList arrEnemies = new ArrayList();
    private GameObject oGround;

    private IEnumerator scoreCoroutine;
    private IEnumerator bonusCoroutine;
    private IEnumerator bonus2Coroutine;
    private IEnumerator multikillCoroutine;
    private bool isPlaying;
    private float difficultyFactor;
    private int[] highScores = new int[25];
    private bool[] waveUnlocked = new bool[25];
    private bool[] mutators = new bool[5];
    private float mutatorsFactor;
    private const int CreditsStart = 500;

    private int waveStart;
    private int waveCurrent;
    private int enemiesLeft;
    private int enemiesTotal;
    private int scoreCurrent;
    private int creditsCurrent;
    private int creditsEarned;
    private int sessionCreditsEarned;
    private int creditsSpent;
    private int creditsPrev;
    private int currTurretType;
    private int currGametime;
    private int sessionGametime;
    private int currEngineer;
    private bool isScrooge;
    private bool freshStart;
    [HideInInspector] public int prevPlayerExp;
    [HideInInspector] public int prevPostExp;
    [HideInInspector] public int prevGatlingExp;
    [HideInInspector] public int prevGaussExp;
    [HideInInspector] public int prevLaserExp;
    private int startPlayerExp;
    private int xpPerf, xpKills, xpMedals, xpComms, xpBonus;
    private int lvlPost, lvlPostPrev = -1;
    private int lvlGatling, lvlGatlingPrev = -1;
    private int lvlGauss, lvlGaussPrev = -1;
    private int lvlLaser, lvlLaserPrev = -1;
    private int builtTurrets;
    private int repairedTurrets;
    private int upgradedTurrets;
    private int currTurrets;
    private int maxGatlings;
    private int maxGausses;
    private int maxLasers;
    private int currKills;
    private int currBossKills;
    private int gatlingKills;
    private int gaussKills;
    private int laserKills;
    private int orbitalKills;
    private int sessionKills;
    private int multikillCounter;
    private int currSpree;
    private int bestSpree;
    private Transform engageTarget;
    private float engageCharge;
    private int engageSound;
    private int orbitalCharges;
    private GameObject buttonEmp;
    private int bonusType = -1;
    private string bonusDescr;
    private int bonusScore = -1;
    private int bonusKills = 0;
    private int bonusKills2 = 0;
    private GameObject boxPartial;
    private int bonusPrize;
    private float bonusKillsFirstTime;
    private ArrayList bonusKillsTime = new ArrayList();
    private string postgameStrings;
    private string postgameValues;
    private int[] medals = new int[9];
    private int lvlComm0, lvlComm1, lvlComm2, lvlComm3, lvlComm4, lvlComm5;
    private int lvlComm0Prev, lvlComm1Prev, lvlComm2Prev, lvlComm3Prev, lvlComm4Prev, lvlComm5Prev;
    private bool[] comm0Prog, comm1Prog, comm2Prog, comm3Prog, comm4Prog, comm5Prog;
    
    private Transform infoUnit;
    private Image infoImage;
    private CanvasGroup cgUnit;
    private Text textUnitName;
    private Text textUnitLevel;
    private Text textUnitFullValues;
    private Text textUnitBaseValues;
    private Text textUnitLabelBottom;

    private readonly int[] TURR_PRICE = { 150, 250, 400 };

    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool isPlacementValid = false;
    private bool userClicked = false;
    private bool isSuperReload;

    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if ( objs.Length > 1 ) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start() {
        isPlaying = false;
        gameState = State.Begin;
        ResetAll();
    }

    void Update() {
        if ( isPlaying ) {
            switch ( gameState ) {
                case State.BaseDeploy: {
                    UpdatePlacementPose();
                    UpdatePlacementIndicatorPost();
                    if ( isPlacementValid ) {
                        ShowActionInfo(0, 0);
                        if ( userClicked ) {
                            DeployBase(placementPose.position, placementPose.rotation);
                        }
                    } else {
                        ShowActionInfo(-4, 0);
                    }
                } break;
                case State.WaveHalf: {
                    int layerInteraction = LayerMask.GetMask("Interaction");
                    int layerDeploy = LayerMask.GetMask("Deploy");
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                    RaycastHit hit;
                    if ( Physics.Raycast(ray, out hit, 200.0f, layerInteraction, QueryTriggerInteraction.Collide) ) {
                        // Trigger hit, not deploying
                        placementIndicatorTurret.SetActive(false);
                        cgUnit.alpha = 1.0f;
                        infoImage.rectTransform.position = Camera.main.WorldToScreenPoint(hit.transform.root.position + 5.0f * Camera.main.transform.right + 2.0f * Vector3.up);
                        DeployCommons target = hit.transform.root.GetComponent<DeployCommons>();
                        Turret t = hit.transform.root.GetComponent<Turret>();
                        if ( t != null ) {
                            // Pointing any turret
                            if ( infoUnit != hit.transform.root ) {
                                infoUnit = hit.transform.root;
                                UpdateInfoPanel(t);
                            }
                            int _action = t.QueryStatus(creditsCurrent);
                            switch ( _action ) {
                                case -2: { ShowActionInfo(-4, 0); } break;
                                case -1: { ShowActionInfo(-4, 0); target.ToggleRadius(0, true); } break;
                                case 0: {
                                    ShowActionInfo(3, t.GetUpgradeCost());
                                    target.ToggleRadius(3, true);
                                    if ( userClicked ) {
                                        int _cost = t.GetUpgradeCost();
                                        t.Upgrade();
                                        UpdateInfoPanel(t);
                                        upgradedTurrets++;
                                        CheckAchievement(7);
                                        isScrooge = false;
                                        AddEngineerAction();
                                        AddPlayerXp(10, 3);
                                        int _pxp = (int) (((float) _cost) / 3.0f);
                                        int _txp = (int) (((float) _cost) * 2.0f / 3.0f);
                                        prefs.AddPostXp(_pxp);
                                        switch ( t.GetType() ) {
                                            case 0: prefs.AddGatlingXp(_txp); break;
                                            case 1: prefs.AddGaussXp(_txp); break;
                                            case 2: prefs.AddLaserXp(_txp); break;
                                        }
                                        SpendMoney(_cost);
                                        UpdateUIDeploy();
                                        UpdateTurretContour();
                                    }
                                } break;
                                default: {
                                    target.ToggleRadius(2, true);
                                    if ( _action <= creditsCurrent ) {
                                        ShowActionInfo(1, _action);
                                    } else {
                                        ShowActionInfo(2, _action);
                                    }
                                    if ( userClicked ) {
                                        t.Repair(_action);
                                        repairedTurrets++;
                                        CheckAchievement(6);
                                        UpdateTurretsHealth();
                                        AddEngineerAction();
                                        AddPlayerXp(5, 3);
                                        int _pxp = (int) (((float) _action) / 1.5f);
                                        int _txp = (int) (((float) _action) / 0.75f);
                                        prefs.AddPostXp(_pxp);
                                        switch ( t.GetType() ) {
                                            case 0: prefs.AddGatlingXp(_txp); break;
                                            case 1: prefs.AddGaussXp(_txp); break;
                                            case 2: prefs.AddLaserXp(_txp); break;
                                        }
                                        SpendMoney(2 * _action);
                                        UpdateUIDeploy();
                                        UpdateTurretContour();
                                    }
                                } break;
                            }
                        } else {
                            // Pointing Command Post
                            if ( infoUnit != hit.transform.root ) {
                                infoUnit = hit.transform.root;
                                UpdateInfoPanel();
                            }
                            target.ToggleRadius(0, true);
                            ShowActionInfo(-4, 0);
                        }
                    } else {
                        cgUnit.alpha = 0.0f;
                        UpdatePlacementPose();
                        UpdatePlacementIndicatorTurret();
                        Collider[] colliders = Physics.OverlapSphere(placementPose.position, 2.5f, layerDeploy, QueryTriggerInteraction.Collide);
                        if ( colliders.Length == 0 ) {
                            // Deployable zone, check money
                            if ( creditsCurrent >= TURR_PRICE[currTurretType] ) {
                                switch ( currTurretType ) {
                                    case 0: { if (arrGatlings.Count < maxGatlings) { SetPlacementIndicatorValidity(true); ShowActionInfo(0, TURR_PRICE[currTurretType]); } else { SetPlacementIndicatorValidity(false); ShowActionInfo(-2, 0); } } break;
                                    case 1: { if (arrGausses.Count < maxGausses) { SetPlacementIndicatorValidity(true); ShowActionInfo(0, TURR_PRICE[currTurretType]); } else { SetPlacementIndicatorValidity(false); ShowActionInfo(-2, 0); } } break;
                                    case 2: { if (arrLasers.Count < maxLasers) { SetPlacementIndicatorValidity(true); ShowActionInfo(0, TURR_PRICE[currTurretType]); } else { SetPlacementIndicatorValidity(false); ShowActionInfo(-2, 0); } } break;
                                }
                            } else {
                                SetPlacementIndicatorValidity(false);
                                ShowActionInfo(-3, TURR_PRICE[currTurretType]);
                            }
                            if ( isPlacementValid && userClicked ) {
                                DeployTurret(placementPose.position, placementPose.rotation);
                            }
                            if ( !isPlacementValid ) {
                                ShowActionInfo(-4, 0);
                            }
                        } else {
                            // Too close to other units
                            SetPlacementIndicatorValidity(false);
                            ShowActionInfo(-1, 0);
                            foreach ( Collider c in colliders ) {
                                c.transform.root.GetComponent<DeployCommons>().ToggleRadius(1, true);
                            }
                        }
                    }
                } break;
                case State.Battle: {
                    if ( orbitalCharges > 0 ) {
                        int layerEngaging = LayerMask.GetMask("Engaging");
                        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                        RaycastHit hit;
                        if ( Input.touchCount == 0 ) {
                            engageCharge = 0.0f;
                            engageSound = 0;
                            if ( Physics.Raycast(ray, out hit, 200.0f, layerEngaging, QueryTriggerInteraction.Collide) ) {
                                ActorCommons en = hit.transform.root.GetComponentInChildren<ActorCommons>();
                                if ( en.IsAlive() ) {
                                    engageTarget = hit.transform.root;
                                    imgOrbitalAABB.color = new Color(1.0f, 0.7764706f, 0.0f, 1.0f);
                                    imgOrbitalAABB.rectTransform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
                                    imgOrbitalAABB.rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
                                    imgOrbitalAABB.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
                                    textOrbitalLabel.text = "DETECTING";
                                    textOrbitalLabel.color = new Color(1.0f, 0.7764706f, 0.0f, 1.0f);                            
                                } else {
                                    engageTarget = null;
                                    imgOrbitalAABB.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1.0f);
                                    imgOrbitalAABB.rectTransform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
                                    imgOrbitalAABB.rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
                                    imgOrbitalAABB.rectTransform.rotation = Quaternion.identity;
                                    textOrbitalLabel.text = "READY (" + orbitalCharges + "/2)";
                                    textOrbitalLabel.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1.0f);
                                }
                            } else {
                                engageTarget = null;
                                imgOrbitalAABB.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1.0f);
                                imgOrbitalAABB.rectTransform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
                                imgOrbitalAABB.rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
                                imgOrbitalAABB.rectTransform.rotation = Quaternion.identity;
                                textOrbitalLabel.text = "READY (" + orbitalCharges + "/2)";
                                textOrbitalLabel.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1.0f);
                            }
                        } else {
                            if ( engageTarget == null ) {
                                engageCharge = 0.0f;
                                engageSound = 0;
                                imgOrbitalAABB.color = new Color(0.487006f, 0.5377358f, 0.5349175f, 1.0f);
                                imgOrbitalAABB.rectTransform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
                                imgOrbitalAABB.rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
                                imgOrbitalAABB.rectTransform.rotation = Quaternion.identity;
                                if ( textOrbitalLabel.text != "TARGET LOST" && textOrbitalLabel.text != "ENGAGED!" ) {
                                    if ( textOrbitalLabel.text != "NO TARGET" ) { gui.PlaySound(gui.orbitalNone); }
                                    textOrbitalLabel.text = "NO TARGET";
                                }
                                textOrbitalLabel.color = new Color(0.7860448f, 0.8679245f, 0.8633757f, 1.0f);
                            } else {
                                ActorCommons en = engageTarget.GetComponentInChildren<ActorCommons>();
                                if ( en.IsAlive() ) {
                                    Renderer rend = engageTarget.GetChild(0).GetChild(0).GetComponent<Renderer>();
                                    Vector3 scrPos = Camera.main.WorldToScreenPoint(rend.bounds.center);
                                    imgOrbitalAABB.color = new Color(0.9019608f, 0.0f, 0.0f, 1.0f);
                                    imgOrbitalAABB.rectTransform.position = scrPos;
                                    imgOrbitalAABB.rectTransform.sizeDelta = Compute2DAABB(rend.bounds);
                                    imgOrbitalAABB.rectTransform.rotation = Quaternion.identity;
                                    engageCharge += Time.deltaTime;
                                    if ( engageSound == 3 && engageCharge > 0.6f && engageCharge < 0.8f ) { gui.PlaySound(gui.orbitalEngaging); engageSound = 0; }
                                    if ( engageSound == 2 && engageCharge > 0.4f && engageCharge < 0.6f ) { gui.PlaySound(gui.orbitalEngaging); engageSound++; }
                                    if ( engageSound == 1 && engageCharge > 0.2f && engageCharge < 0.4f ) { gui.PlaySound(gui.orbitalEngaging); engageSound++; }
                                    if ( engageSound == 0 && engageCharge < 0.2f ) { gui.PlaySound(gui.orbitalEngaging); engageSound++; }
                                    textOrbitalLabel.text = "ENGAGING";
                                    textOrbitalLabel.color = new Color(0.9019608f, 0.0f, 0.0f, 1.0f);
                                    
                                    Vector3 vForward = Camera.main.transform.forward;
                                    Vector3 vTarget = Vector3.Normalize(engageTarget.position - Camera.main.transform.position);
                                    if ( Vector3.Dot(vForward, vTarget) < Mathf.Cos(Mathf.PI/12) ) {
                                        engageTarget = null;
                                        engageCharge = 0.0f;
                                        engageSound = 0;
                                        imgOrbitalAABB.color = new Color(0.487006f, 0.5377358f, 0.5349175f, 1.0f);
                                        imgOrbitalAABB.rectTransform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
                                        imgOrbitalAABB.rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
                                        imgOrbitalAABB.rectTransform.rotation = Quaternion.identity;
                                        if ( textOrbitalLabel.text != "TARGET LOST" ) { gui.PlaySound(gui.orbitalLost); }
                                        textOrbitalLabel.text = "TARGET LOST";
                                        textOrbitalLabel.color = new Color(0.7860448f, 0.8679245f, 0.8633757f, 1.0f);
                                    }
                                } else {
                                    engageTarget = null;
                                    engageCharge = 0.0f;
                                    engageSound = 0;
                                    imgOrbitalAABB.color = new Color(0.487006f, 0.5377358f, 0.5349175f, 1.0f);
                                    imgOrbitalAABB.rectTransform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
                                    imgOrbitalAABB.rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
                                    imgOrbitalAABB.rectTransform.rotation = Quaternion.identity;
                                    if ( textOrbitalLabel.text != "TARGET LOST" ) { gui.PlaySound(gui.orbitalLost); }
                                    textOrbitalLabel.text = "TARGET LOST";
                                    textOrbitalLabel.color = new Color(0.7860448f, 0.8679245f, 0.8633757f, 1.0f);
                                }
                            }
                        }
                        slOrbital.value = engageCharge;
                    } else {
                        engageTarget = null;
                        engageSound = 0;
                        imgOrbitalAABB.color = new Color(0.487006f, 0.5377358f, 0.5349175f, 0.5f);
                        imgOrbitalAABB.rectTransform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
                        imgOrbitalAABB.rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
                        imgOrbitalAABB.rectTransform.rotation = Quaternion.identity;
                        textOrbitalLabel.text = "COOLING DOWN...";
                        textOrbitalLabel.color = new Color(0.7860448f, 0.8679245f, 0.8633757f, 1.0f);
                    }
                    if ( engageCharge >= 1.0f && orbitalCharges > 0 ) {
                        orbitalTarget[2-orbitalCharges].SetActive(true);
                        oCommandPost.GetComponent<CommandPost>().OrbitalAttack(engageTarget, orbitalTarget[2-orbitalCharges]);
                        engageTarget = null;
                        engageSound = 0;
                        orbitalCharges--;
                        if ( textOrbitalLabel.text != "ENGAGED!" ) {
                            PlayerPrefs.SetInt("plEngaged", PlayerPrefs.GetInt("plEngaged") + 1);
                            CheckAchievement(8);
                            gui.PlaySound(gui.orbitalEngaged);
                        }
                        textOrbitalLabel.text = "ENGAGED!";
                        if ( orbitalCharges == 0 ) {
                            StartCoroutine(ReloadOrbitalStrike());
                        }
                    }
                } break;
                default: { ShowActionInfo(-4, 0); } break;
            }
            userClicked = false;
        }
    }

    private void UpdateInfoPanel() {
        // Command Post info
        CommandPost cp = oCommandPost.GetComponent<CommandPost>();
        textUnitName.text = "COMMAND POST";
        textUnitLevel.text = "LEVEL " + prefs.Int2Roman(lvlPost);
        textUnitFullValues.text = cp.GetFullValues();
        textUnitBaseValues.text = cp.GetBaseValues();
        textUnitLabelBottom.text = "\n\nREPAIR:";
    }

    private void UpdateInfoPanel(Turret t) {
        // Turret info
        switch ( (int) t.GetType() ) {
            case 0: textUnitName.text = "GATLING SENTRY"; break;
            case 1: textUnitName.text = "GAUSS CANNON"; break;
            case 2: textUnitName.text = "LASER TURRET"; break;
        }
        textUnitLevel.text = "LEVEL " + prefs.Int2Roman(t.GetLevel());
        textUnitFullValues.text = t.GetFullValues();
        textUnitBaseValues.text = t.GetBaseValues();
        textUnitLabelBottom.text = "\n\nATK RATE:";
    }

    private Vector2 Compute2DAABB(Bounds b) {
        if ( b == null ) { return Vector2.zero; }
        Vector3[] verts = new Vector3[8];
        verts[0] = new Vector3(b.min.x, b.min.y, b.min.z);
        verts[1] = new Vector3(b.min.x, b.min.y, b.max.z);
        verts[2] = new Vector3(b.min.x, b.max.y, b.min.z);
        verts[3] = new Vector3(b.min.x, b.max.y, b.max.z);
        verts[4] = new Vector3(b.max.x, b.min.y, b.min.z);
        verts[5] = new Vector3(b.max.x, b.min.y, b.max.z);
        verts[6] = new Vector3(b.max.x, b.max.y, b.min.z);
        verts[7] = new Vector3(b.max.x, b.max.y, b.max.z);

        Vector2 min = Camera.main.WorldToScreenPoint(verts[0]);
        Vector2 max = min;
        for ( int i = 1; i < 8; ++i ) {
            Vector2 scrPos = Camera.main.WorldToScreenPoint(verts[i]);
            min.x = Mathf.Min(min.x, scrPos.x);
            min.y = Mathf.Min(min.y, scrPos.y);
            max.x = Mathf.Max(max.x, scrPos.x);
            max.y = Mathf.Max(max.y, scrPos.y);
        }

        min.y = (Screen.height - min.y);
        max.y = (Screen.height - max.y);
        Vector2 res = 1.5f * (new Vector2(max.x - min.x, min.y - max.y));
        return new Vector2(Mathf.Max(100.0f, res.x), Mathf.Max(100.0f, res.y));
    }

    private void ShowActionInfo(int action, int cost) {
        // -4: empty; -3: not enough money; -2: turret limit; -1: can't deploy here; 0: deploy; 1: repair; 2: partial repair; 3: upgrade
        Color white = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1.0f);
        Color green = new Color(0.1087576f, 0.8867924f, 0.1787133f, 1.0f);
        Color yellow = new Color(1.0f, 0.7764706f, 0.0f, 1.0f);
        Color blue = new Color(0.2509804f, 0.5184898f, 1.0f, 1.0f);
        Color red = new Color(0.9019608f, 0.0f, 0.0f, 1.0f);
        switch ( action ) {
            case -3: {
                textActionLabel.text = "Deploy:";
                textActionLabel.color = green;
                textActionDescr.text = cost + " cR";
                textActionDescr.color = red;
            } break;
            case -2: {
                textActionLabel.text = "Deploy limit reached";
                textActionLabel.color = red;
                textActionDescr.text = "";
            } break;
            case -1: {
                textActionLabel.text = "Cannot deploy here";
                textActionLabel.color = red;
                textActionDescr.text = "";
            } break;
            case 0: {
                textActionLabel.text = "Deploy:";
                textActionLabel.color = green;
                textActionDescr.text = cost + " cR";
                textActionDescr.color = white;
            } break;
            case 1: {
                textActionLabel.text = "Repair:";
                textActionLabel.color = yellow;
                textActionDescr.text = cost + " cR";
                textActionDescr.color = white;
            } break;
            case 2: {
                textActionLabel.text = "Repair:";
                textActionLabel.color = yellow;
                textActionDescr.text = cost + " cR";
                textActionDescr.color = red;
            } break;
            case 3: {
                textActionLabel.text = "Upgrade:";
                textActionLabel.color = blue;
                textActionDescr.text = cost + " cR";
                textActionDescr.color = white;
            } break;
            default: { textActionLabel.text = ""; textActionDescr.text = ""; } break;
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if ( Application.loadedLevelName == "Game" ) {
            gui = FindObjectOfType<GameUIManager>();
            prefs = FindObjectOfType<Prefs>();
            arOrigin = FindObjectOfType<ARSessionOrigin>();
            placementIndicatorPost = GameObject.Find("CommandPostShape");
            placementIndicatorTurret = GameObject.Find("TurretShape");
            GameObject.Find("ButtonInteraction").GetComponent<Button>().onClick.AddListener(Interaction);
            GameObject.Find("CanvasPostgame").GetComponent<Canvas>().enabled = false;
            AttachGUIElements();
            GameObject.Find("ButtonRetry").GetComponent<Button>().onClick.AddListener(Retry);
            GameObject.Find("ButtonRestart").GetComponent<Button>().onClick.AddListener(Restart);
            buttonEmp = GameObject.Find("ButtonEmp");
            buttonEmp.GetComponent<Button>().onClick.AddListener(UseEMP);
            buttonEmp.SetActive(false);
            for ( int i = 0; i < 5; ++i ) {
                if ( !mutators[i] ) { GameObject.Find("BarMutator" + i).SetActive(false); }
            }
            isSuperReload = GameObject.Find("BarMutator4") != null;
            StartGame();
            //GameObject.Find("ButtonNewBase").GetComponent<Button>().onClick.AddListener(delegate{DeployBase(Vector3.zero, Quaternion.identity);});
            //GameObject.Find("ButtonNewTurret").GetComponent<Button>().onClick.AddListener(DeployTurret);
            ShowActionInfo(-4, 0);
        } else {
            isPlaying = false;
            gameState = State.BaseDeploy;
            creditsPrev = CreditsStart;
        }
    }

    public void Interaction() {
        userClicked = true;
    }

    private void UpdatePlacementPose() {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        isPlacementValid = hits.Count > 0;
        if ( isPlacementValid ) {
            placementPose = hits[0].pose;
        }
    }

    private void UpdatePlacementIndicatorPost() {
        if ( isPlacementValid ) {
            placementIndicatorPost.SetActive(true);
            placementIndicatorPost.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else {
            placementIndicatorPost.SetActive(false);
        }
    }

    private void UpdatePlacementIndicatorTurret() {
        if ( isPlacementValid ) {
            placementIndicatorTurret.SetActive(true);
            placementIndicatorTurret.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else {
            placementIndicatorTurret.SetActive(false);
        }
    }

    private void SetPlacementIndicatorValidity(bool valid) {
        if ( valid ) {
            placementIndicatorTurret.transform.GetChild(0).gameObject.SetActive(true);
            placementIndicatorTurret.transform.GetChild(1).gameObject.SetActive(false);
        } else {
            placementIndicatorTurret.transform.GetChild(0).gameObject.SetActive(false);
            placementIndicatorTurret.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void AddPlayerXp(int exp, int reason) {
        switch ( reason ) {
            case 0: { xpKills += exp; } break;
            case 1: { xpBonus += exp; } break;
            case 2: { xpMedals += exp; } break;
            case 3: { xpPerf += exp; } break;
            case 4: { xpComms += exp; } break;
            default: break;
        }
        int rnkPrev = prefs.ComputePlayerLevel(PlayerPrefs.GetInt("plExp"));
        prefs.AddPlayerXp(exp);
        int rnkNow = prefs.ComputePlayerLevel(PlayerPrefs.GetInt("plExp"));
        //Debug.Log(rnkPrev + " vs. " + rnkNow + ": " + (rnkNow > rnkPrev));
        if ( rnkNow > rnkPrev ) { CheckAchievement(3); }
    }

    public void AddScore(int score) {
        scoreCurrent += (int) Mathf.Round(score * (difficultyFactor + mutatorsFactor));
        ShowScoreOverlay(score);
    }

    private void ShowScoreOverlay(int score) {
        Transform obj = Instantiate(scoreText, Vector3.zero, Quaternion.identity).transform;
        obj.SetParent(centerTransform, false);
        obj.GetChild(0).GetComponent<Text>().text = "+" + score;
    }

    public void PlayerKills(GameObject enemy, bool isBoss, int score, int shooter) {
        if ( arrEnemies.Contains(enemy) ) {
            arrEnemies.Remove(enemy);
            currKills += 1;
            if ( isBoss ) { currBossKills += 1; oBoss = null; CheckAchievement(9); }
            if ( multikillCoroutine == null ) {
                multikillCoroutine = MultikillCoroutine();
                StartCoroutine(multikillCoroutine);
            } else {
                StopCoroutine(multikillCoroutine);
                multikillCoroutine = MultikillCoroutine();
                StartCoroutine(multikillCoroutine);
            }
            multikillCounter++;
            enemiesLeft -= 1;
            currSpree++;
            EarnMoney( (int) (((float) score ) * (difficultyFactor + mutatorsFactor) / 2.5f) );
            switch ( shooter ) {
                case 0: gatlingKills++; break;
                case 1: gaussKills++; break;
                case 2: laserKills++; break;
                case 3: orbitalKills++; break;
                default: break;
            }
            UpdateCommendations();
            if ( bonusType == 1 && bonusKills < 10 ) {
                bonusKills++;
                if ( bonusKillsFirstTime == 0 ) { bonusKillsFirstTime = Time.time; }
                bonusKillsTime.Add(Time.time);
                bonusDescr = "Not completed (" + bonusKills + "/10)";
                bonusScore += 100;
                string newStr = bonusKills.ToString() + textBonusProgress.text.Substring(textBonusProgress.text.IndexOf('/'));
                textBonusProgress.text = newStr;
                if ( bonus2Coroutine == null ) {
                    bonus2Coroutine = Bonus2Coroutine();
                    StartCoroutine(bonus2Coroutine);
                    textBonusProgress.text = bonusKills.ToString() + "/10 (0:15)";
                }
                if ( bonusKills >= 10 ) {
                    StopCoroutine(bonus2Coroutine);
                    bonus2Coroutine = null;
                    WinBonusWave();
                }
            }
            if ( bonusType == 3 && bonusKills2 < 3 ) {
                if ( shooter == 3 ) {
                    bonusKills2++;
                    bonusDescr = "Not completed (" + bonusKills2 + "/3)";
                    bonusScore += 500;
                    textBonusProgress.text = bonusKills2.ToString() + " / 3";
                    if ( bonusKills2 >= 3 ) {
                        WinBonusWave();
                    }
                }
            }
            if ( currSpree == 10 ) { AddMedal(3); CheckAchievement(1); }
            if ( currSpree == 20 ) { AddMedal(4); CheckAchievement(1); }
            if ( currSpree > bestSpree ) { bestSpree = currSpree; }
            if ( isBoss ) { AddPlayerXp(35, 0); } else { AddPlayerXp(15, 0); }
            if ( isBoss && shooter == 3 ) { AddMedal(6); }
            AddScore(score);
            UpdateUIProgress();
            UpdateUIScore();
            UpdateUICredits();
            if ( enemiesLeft == 0 ) { WinWave(); }
        }
    }

    public void EnemyKills(GameObject turret, int type) {
        switch ( type ) {
            case 0: {
                if ( arrGatlings.Contains(turret) ) {
                    arrGatlings.Remove(turret);
                }
            } break;
            case 1: {
                if ( arrGausses.Contains(turret) ) {
                    arrGausses.Remove(turret);
                }
            } break;
            case 2: {
                if ( arrLasers.Contains(turret) ) {
                    arrLasers.Remove(turret);
                }
            } break;
        }
        currSpree = 0;
        if ( ( bonusType == 2 ) && ( bonusScore > 0 ) ) { bonusScore = 0; LoseBonusWave(); }
        if ( arrGatlings.Count + arrGausses.Count + arrLasers.Count == 0 ) { LoseWave(); }
        UpdateUIResources();
    }

    public void SetDifficulty(int diff) {
        switch ( diff ) {
            case 0: { difficulty = Difficulty.Normal; difficultyFactor = 1.0f; } break;
            case 1: { difficulty = Difficulty.Hard; difficultyFactor = 1.5f; } break;
            case 2: { difficulty = Difficulty.Delta; difficultyFactor = 2.0f; } break;
        }
    }

    public string GetDifficultyName() {
        switch ( difficulty ) {
            case Difficulty.Hard: { return "Hard"; } break;
            case Difficulty.Delta: { return "Delta"; } break;
            default: { return "Normal"; } break;
        }
    }

    public float GetDifficultyFactor() {
        return difficultyFactor;
    }

    private void LoadHighScores() {
        // High scores
        highScores[0] = PlayerPrefs.GetInt("plBest0", 0);
        waveUnlocked[0] = true;
        for (int i = 1; i < 25; ++i) {
            highScores[i] = PlayerPrefs.GetInt("plBest" + i, 0);
            waveUnlocked[i] = (highScores[i] >= 0) && (highScores[i-1] > 0) ? true : false;
        }
    }

    public int GetHighScore(int wave) {
        wave = Mathf.Clamp(wave, 0, 24);
        return highScores[wave];
    }

    public void SetWaveStart(int wave, int dir) {
        wave = (wave + 25) % 25;
        while ( !waveUnlocked[wave] ) {
            wave = (wave + dir + 25) % 25;
        }
        waveStart = wave;
    }

    public void ResetAll() {
        SetDifficulty(0);
        LoadHighScores();
        SetWaveStart(0, 0);
        waveCurrent = waveStart;
        scoreCurrent = 0;
        creditsPrev = CreditsStart;
        creditsEarned = 0;
        ClearMutators();
    }

    public void Retry() {
        oCommandPost = null;
        arrGatlings.Clear();
        arrGausses.Clear();
        arrLasers.Clear();
        oBoss = null;
        arrEnemies.Clear();
        LoadHighScores();
        waveStart = waveCurrent;
        StopAllCoroutines();
        SceneManager.LoadScene("Game");
    }

    public void Restart() {
        oCommandPost = null;
        arrGatlings.Clear();
        arrGausses.Clear();
        arrLasers.Clear();
        oBoss = null;
        arrEnemies.Clear();
        creditsPrev = CreditsStart;
        LoadHighScores();
        SetWaveStart(0, 0);
        StopAllCoroutines();
        SceneManager.LoadScene("Game");
    }

    public void SetWaveStartIncrease() {
        SetWaveStart(waveStart + 1, 1);
    }

    public void SetWaveStartDecrease() {
        SetWaveStart(waveStart - 1, -1);
    }

    public int GetWaveStart() {
        return waveStart;
    }

    public int GetWaveCurrent() {
        return waveCurrent;
    }

    public void ToggleMutator(int n) {
        mutators[n] = !mutators[n];
        ComputeMutatorsFactor();
    }

    public bool IsMutatorActive(int n) {
        return mutators[n];
    }

    public string GetMutatorsName() {
        int _n = 0;
        for (int i = 0; i < 5; ++i) { _n += mutators[i] ? 1 : 0; }
        if ( _n == 0 ) {
            return "None";
        } else {
            if ( _n == 5 ) { return "All active"; } else { return _n + " active"; }
        }
    }

    private void ClearMutators() {
        for (int i = 0; i < 5; ++i) { mutators[i] = false; }
        ComputeMutatorsFactor();
    }

    private void ComputeMutatorsFactor() {
        if ( mutators[0] ) { mutatorsFactor = 25.0f; } else { mutatorsFactor = 0.0f; }
        if ( mutators[1] ) { mutatorsFactor += 10.0f; }
        if ( mutators[2] ) { mutatorsFactor += 5.0f; }
        if ( mutators[4] ) { mutatorsFactor -= 25.0f; }
        mutatorsFactor /= 100;
    }

    public float GetMutatorsFactor() {
        return mutatorsFactor;
    }

    public void StartGame() {
        ResumeGame();
        waveCurrent = waveStart;
        scoreCurrent = 0;
        creditsCurrent = creditsPrev;
        creditsEarned = 0;
        sessionCreditsEarned = 0;
        creditsSpent = 0;
        currTurretType = -1;
        currGametime = 0;
        sessionGametime = 0;
        freshStart = waveStart == 0;
        builtTurrets = 0;
        repairedTurrets = 0;
        upgradedTurrets = 0;
        currTurrets = 0;
        currKills = 0;
        currBossKills = 0;
        gatlingKills = 0;
        gaussKills = 0;
        laserKills = 0;
        orbitalKills = 0;
        sessionKills = 0;
        multikillCounter = 0;
        currSpree = 0;
        bestSpree = 0;
        medals = new int[9];
        lvlComm0Prev = prefs.ComputeComm0Level(PlayerPrefs.GetInt("plComm0"));
        lvlComm1Prev = prefs.ComputeComm1Level(PlayerPrefs.GetInt("plComm1"));
        lvlComm2Prev = prefs.ComputeComm2Level(PlayerPrefs.GetInt("plComm2"));
        lvlComm3Prev = prefs.ComputeComm3Level(PlayerPrefs.GetInt("plComm3"));
        lvlComm4Prev = prefs.ComputeComm4Level(PlayerPrefs.GetInt("plComm4"));
        lvlComm5Prev = prefs.ComputeComm5Level(PlayerPrefs.GetInt("plComm5"));
        lvlComm0 = -1;
        lvlComm1 = -1;
        lvlComm2 = -1;
        lvlComm3 = -1;
        lvlComm4 = -1;
        lvlComm5 = -1;
        comm0Prog = new bool[4]; comm1Prog = new bool[4]; comm2Prog = new bool[4];
        comm3Prog = new bool[4]; comm4Prog = new bool[4]; comm5Prog = new bool[4];
        for ( int i = 0; i < 4; ++i ) {
            comm0Prog[i] = PlayerPrefs.GetInt("plComm0") >= ((float) prefs.ComputeComm0MaxXP(lvlComm0Prev)) * (i+1) / 4.0f;
            comm1Prog[i] = PlayerPrefs.GetInt("plComm1") >= ((float) prefs.ComputeComm1MaxXP(lvlComm1Prev)) * (i+1) / 4.0f;
            comm2Prog[i] = PlayerPrefs.GetInt("plComm2") >= ((float) prefs.ComputeComm2MaxXP(lvlComm2Prev)) * (i+1) / 4.0f;
            comm3Prog[i] = PlayerPrefs.GetInt("plComm3") >= ((float) prefs.ComputeComm3MaxXP(lvlComm3Prev)) * (i+1) / 4.0f;
            comm4Prog[i] = PlayerPrefs.GetInt("plComm4") >= ((float) prefs.ComputeComm4MaxXP(lvlComm4Prev)) * (i+1) / 4.0f;
            comm5Prog[i] = PlayerPrefs.GetInt("plComm5") >= ((float) prefs.ComputeComm5MaxXP(lvlComm5Prev)) * (i+1) / 4.0f;
        }
        postgameStrings = GetDifficultyName() + "\n" + GetMutatorsName();
        postgameValues = GetDifficultyFactor().ToString("0.0") + "\n" + GetMutatorsFactor().ToString("0.00");
        prevPlayerExp = PlayerPrefs.GetInt("plExp");
        prevPostExp = PlayerPrefs.GetInt("plPostExp");
        prevGatlingExp = PlayerPrefs.GetInt("plGatlingExp");
        prevGaussExp = PlayerPrefs.GetInt("plGaussExp");
        prevLaserExp = PlayerPrefs.GetInt("plLaserExp");
        startPlayerExp = prevPlayerExp;
        lvlPostPrev = -1;
        lvlGatlingPrev = -1;
        lvlGaussPrev = -1;
        lvlLaserPrev = -1;
        xpPerf = 0; xpKills = 0; xpMedals = 0; xpComms = 0; xpBonus = 0;
        UpdateUIDeploy();
        gui.ClearDeploy();
        gui.HideDeployButtons();
        gui.ShowHUD();
        placementIndicatorPost.SetActive(false);
        placementIndicatorTurret.SetActive(false);
        imgOrbitalAABB.enabled = false;
        orbitalCharges = 2;
        textOrbitalLabel.text = "";
        slOrbital.transform.GetChild(0).gameObject.SetActive(false);
        /*text.text = "Starting Wave: " + (GetWaveCurrent() + 1) +
            "\nDifficulty: " + GetDifficultyName() + " (" + GetDifficultyFactor().ToString("0.0#") + "x)" +
            "\nMutators: " + GetMutatorsName() + " (" + GetMutatorsFactor().ToString("0.0#") + "x)";*/
        DoBaseDeploy();
    }

    public void StopGame() {
        isPlaying = false;
        gameState = State.Begin;
        Time.timeScale = 1.0f;
        DetachGUIElements();
        SceneManager.LoadScene("LoadingMenu");
        GameObject.Destroy(this.gameObject);
    }

    public void PauseGame() {
        isPlaying = false;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame() {
        isPlaying = true;
        Time.timeScale = 1.0f;
    }

    public bool IsBonusWave() {
        return (waveCurrent - waveStart + 1) % 3 == 0;
    }

    public bool IsBossWave() {
        return (waveCurrent + 1) % 5 == 0;
    }

    private void AttachGUIElements() {
        // Get objects for GUI
        textActionLabel = GameObject.Find("TextActionLabel").GetComponent<Text>();
        textActionDescr = GameObject.Find("TextActionDescription").GetComponent<Text>();
        textWave = GameObject.Find("TextWaveNumber").GetComponent<Text>();
        textEnemies = GameObject.Find("TextEnemiesValue").GetComponent<Text>();
        textScore = GameObject.Find("TextScoreValue").GetComponent<Text>();
        textCredits = GameObject.Find("TextCreditsValue").GetComponent<Text>();
        textCountdown = GameObject.Find("TextCountdown").GetComponent<Text>();
        textBonusProgress = GameObject.Find("TextBonusProgress").GetComponent<Text>();
        textBonusComplete = GameObject.Find("TextBonusCompleteValue").GetComponent<Text>();
        textBonusPartial = GameObject.Find("TextBonusPartialValue").GetComponent<Text>();
        boxPartial = GameObject.Find("BoxPartial");
        GameObject.Find("PanelMessageBonusWave").SetActive(false);
        cgGatlings = GameObject.Find("PanelUnits").transform.GetChild(0).GetComponent<CanvasGroup>();
        cgGausses = GameObject.Find("PanelUnits").transform.GetChild(1).GetComponent<CanvasGroup>();
        cgLasers = GameObject.Find("PanelUnits").transform.GetChild(2).GetComponent<CanvasGroup>();
        textGatlingsRed = GameObject.Find("PanelUnits").transform.GetChild(0).GetChild(2).GetComponent<Text>();
        textGatlings = GameObject.Find("PanelUnits").transform.GetChild(0).GetChild(3).GetComponent<Text>();
        textGaussesRed = GameObject.Find("PanelUnits").transform.GetChild(1).GetChild(2).GetComponent<Text>();
        textGausses = GameObject.Find("PanelUnits").transform.GetChild(1).GetChild(3).GetComponent<Text>();
        textLasersRed = GameObject.Find("PanelUnits").transform.GetChild(2).GetChild(2).GetComponent<Text>();
        textLasers = GameObject.Find("PanelUnits").transform.GetChild(2).GetChild(3).GetComponent<Text>();
        slGatlings = GameObject.Find("PanelUnits").transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Slider>();
        slGausses = GameObject.Find("PanelUnits").transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<Slider>();
        slLasers = GameObject.Find("PanelUnits").transform.GetChild(2).GetChild(4).GetChild(0).GetComponent<Slider>();
        imgOrbitalAABB = GameObject.Find("ImageOrbitalAABB").GetComponent<Image>();
        slOrbital = GameObject.Find("SliderOrbital").GetComponent<Slider>();
        textOrbitalLabel = GameObject.Find("TextOrbitalActionLabel").GetComponent<Text>();
        orbitalTarget[0] = GameObject.Find("ImageOrbitalTarget0");
        orbitalTarget[1] = GameObject.Find("ImageOrbitalTarget1");
        orbitalTarget[0].SetActive(false);
        orbitalTarget[1].SetActive(false);
        GameObject.Find("PanelUnits").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("PanelUnits").transform.GetChild(1).gameObject.SetActive(false);
        GameObject.Find("PanelUnits").transform.GetChild(2).gameObject.SetActive(false);
        GameObject.Find("PanelBonusData").SetActive(false);
        Transform panelUnitInfo = GameObject.Find("PanelUnitInfo").transform;
        infoImage = panelUnitInfo.GetComponent<Image>();
        cgUnit = panelUnitInfo.GetComponent<CanvasGroup>();
        textUnitName = panelUnitInfo.GetChild(0).GetComponent<Text>();
        textUnitLevel = panelUnitInfo.GetChild(1).GetComponent<Text>();
        textUnitFullValues = panelUnitInfo.GetChild(2).GetComponent<Text>();
        textUnitBaseValues = panelUnitInfo.GetChild(3).GetComponent<Text>();
        textUnitLabelBottom = panelUnitInfo.GetChild(6).GetComponent<Text>();
        centerTransform = GameObject.Find("ObjScreenCenter").transform;
        cgUnit.alpha = 0.0f;
    }

    private void DetachGUIElements() {
        // Free objects for GUI
        textWave = null;
        textEnemies = null;
        textScore = null;
        textCredits = null;
        textCountdown = null;
        textBonusProgress = null;
    }

    public int GetState() {
        switch ( gameState ) {
            case State.Begin: return -1; break;
            case State.BaseDeploy: return 0; break;
            default: return -4; break;
        }
    }

    public void DeployBase(Vector3 pos, Quaternion rot) {
        GameObject.Find("CanvasPostgame").GetComponent<Canvas>().enabled = true;
        if ( oCommandPost == null ) {
            oCommandPost = Instantiate(CommandPost, pos, rot);
            UpgradeCommandPost(lvlPost);
            oGround = Instantiate(Ground, pos, rot);
            placementIndicatorPost.SetActive(false);
            gui.ClickDeployType(0);
            StartCoroutine(GameTimer());
            DoWaveHalf();
        }
    }

    public void SetTurretType(int type) {
        currTurretType = type;
    }

    public void DeployTurret(Vector3 pos, Quaternion rot) {
        if ( (gameState != State.WaveHalf) || (creditsCurrent < TURR_PRICE[currTurretType]) ) return;
        switch ( currTurretType ) {
            case 0: {
                // Gatling
                if ( arrGatlings.Count < maxGatlings ) {
                    GameObject.Find("PanelUnits").transform.GetChild(0).gameObject.SetActive(true);
                    arrGatlings.Add(Instantiate(GatlingTurret, pos, rot));
                    builtTurrets++;
                    UpdateCommendations();
                    AddEngineerAction();
                    AddPlayerXp(10, 3);
                    isScrooge = false;
                    prefs.AddPostXp(25);
                    prefs.AddGatlingXp(50);
                    SpendMoney(150);
                    UpdateUIDeploy();
                    UpdateTurretsHealth();
                    if ( pos.y < oGround.transform.position.y ) {
                        oGround.transform.position = new Vector3(oGround.transform.position.x, pos.y, oGround.transform.position.z);
                    }
                }
            } break;
            case 1: {
                // Gauss
                if ( arrGausses.Count < maxGausses ) {
                    GameObject.Find("PanelUnits").transform.GetChild(1).gameObject.SetActive(true);
                    arrGausses.Add(Instantiate(GaussTurret, pos, rot));
                    builtTurrets++;
                    UpdateCommendations();
                    AddEngineerAction();
                    AddPlayerXp(10, 3);
                    isScrooge = false;
                    prefs.AddPostXp(50);
                    prefs.AddGaussXp(75);
                    SpendMoney(250);
                    UpdateUIDeploy();
                    UpdateTurretsHealth();
                    if ( pos.y < oGround.transform.position.y ) {
                        oGround.transform.position = new Vector3(oGround.transform.position.x, pos.y, oGround.transform.position.z);
                    }
                }
            } break;
            case 2: {
                // Laser
                if ( arrLasers.Count < maxLasers ) {
                    GameObject.Find("PanelUnits").transform.GetChild(2).gameObject.SetActive(true);
                    arrLasers.Add(Instantiate(LaserTurret, pos, rot));
                    builtTurrets++;
                    UpdateCommendations();
                    AddEngineerAction();
                    AddPlayerXp(10, 3);
                    isScrooge = false;
                    prefs.AddPostXp(75);
                    prefs.AddLaserXp(100);
                    SpendMoney(400);
                    UpdateUIDeploy();
                    UpdateTurretsHealth();
                    if ( pos.y < oGround.transform.position.y ) {
                        oGround.transform.position = new Vector3(oGround.transform.position.x, pos.y, oGround.transform.position.z);
                    }
                }
            } break;
            default: break;
        }
    }

    public void DeployTurret() {
        Vector3 pos = new Vector3(Random.Range(-20.0f, 20.0f), 0.0f, Random.Range(-20.0f, 20.0f));
        Quaternion rot = Quaternion.Euler(0.0f, Random.value * 360.0f, 0.0f);
        DeployTurret(pos, rot);
    }

    public void UseEMP() {
        oCommandPost.GetComponent<CommandPost>().EmpBlast();
        buttonEmp.SetActive(false);
    }

    public void UpdateTurretsHealth() {
        float m; int n;
        // Gatlings
        m = 0.0f; n = 0;
        foreach ( GameObject t in arrGatlings ) {
            m += t.GetComponent<ActorCommons>().GetHealthPercentage();
            n++;
        }
        if ( n > 0 ) { slGatlings.value = m / n; } else { slGatlings.value = 0.0f; }
        // Gausses
        m = 0.0f; n = 0;
        foreach ( GameObject t in arrGausses ) {
            m += t.GetComponent<ActorCommons>().GetHealthPercentage();
            n++;
        }
        if ( n > 0 ) { slGausses.value = m / n; } else { slGausses.value = 0.0f; }
        // Lasers
        m = 0.0f; n = 0;
        foreach ( GameObject t in arrLasers ) {
            m += t.GetComponent<ActorCommons>().GetHealthPercentage();
            n++;
        }
        if ( n > 0 ) { slLasers.value = m / n; } else { slLasers.value = 0.0f; }
    }

    private void SpendMoney(int amount) {
        creditsCurrent -= amount;
        creditsSpent += amount;
        UpdateUICredits();
    }

    public void EarnMoney(int amount) {
        creditsCurrent += amount;
        creditsEarned += amount;
        sessionCreditsEarned += amount;
        UpdateUICredits();
        UpdateCommendations();
    }

    private void WinWave() {
        if ( multikillCoroutine != null ) {
            if ( multikillCounter >= 3 ) {
                AddMedal(5);
            }
            multikillCounter = 0;
            multikillCoroutine = null;
        }
        if ( bonusType == 0 && bonusCoroutine != null ) {
            StopCoroutine(bonusCoroutine);
            bonusCoroutine = null;
            WinBonusWave();
        }
        if ( bonusType == 1 ) {
            if ( bonus2Coroutine != null ) {
                if ( bonusKills >= 10 ) {
                    WinBonusWave();
                } else {
                    LoseBonusWave();
                }
                StopCoroutine(bonus2Coroutine);
                bonus2Coroutine = null;
            }
        }
        if ( bonusType == 2 && ( arrGatlings.Count + arrGausses.Count + arrLasers.Count == currTurrets ) ) {
            WinBonusWave();
        }
        if ( IsBossWave() ) { AddMedal(2); }
        if ( isScrooge && ( arrGatlings.Count + arrGausses.Count + arrLasers.Count == currTurrets ) && ( arrGatlings.Count + arrGausses.Count + arrLasers.Count < maxGatlings + maxGausses + maxLasers ) ) { AddMedal(7); }
        //if ( waveStart == 0 && waveCurrent == 24 ) { ACHIEVEMENT_UNLOCKED }
        if ( freshStart && ( waveCurrent == 24 ) ) { AddMedal(8); }
        DoIntermission(true);
    }

    public void LoseWave() {
        if ( bonusCoroutine != null ) {
            StopCoroutine(bonusCoroutine);
            bonusCoroutine = null;
            LoseBonusWave();
        }
        if ( bonus2Coroutine != null ) {
            StopCoroutine(bonus2Coroutine);
            bonus2Coroutine = null;
            LoseBonusWave();
        }
        if ( multikillCoroutine != null ) {
            StopCoroutine(multikillCoroutine);
            multikillCoroutine = null;
        }
        foreach ( GameObject go in arrGatlings ) { go.GetComponent<Turret>().StopAllCoroutines(); }
        foreach ( GameObject go in arrGausses ) { go.GetComponent<Turret>().StopAllCoroutines(); }
        foreach ( GameObject go in arrLasers ) { go.GetComponent<Turret>().StopAllCoroutines(); }
        foreach ( GameObject go in arrEnemies ) { go.transform.GetChild(0).GetComponent<Enemy>().StopAllCoroutines(); }
        if ( oBoss != null ) { oBoss.transform.GetChild(0).GetComponent<Enemy>().StopAllCoroutines(); }
        DoIntermission(false);
    }

    private void NextWave() {
        if ( oCommandPost != null ) {
            if ( waveCurrent == 24 ) {
                DoGameOver();
                return;
            } else {
                if ( IsBossWave() ) {
                    // Double repair after a boss wave
                    oCommandPost.GetComponent<CommandPost>().Repair();
                }
                waveCurrent++; DoWaveHalf();
                oCommandPost.GetComponent<CommandPost>().Repair();
            }
        }
        creditsPrev = creditsCurrent;
        enemiesLeft = PromptWave(waveCurrent);
        enemiesTotal = enemiesLeft;
        bonusScore = -1;
        bonusType = -1;
        bonusKills = 0;
        bonusKills2 = 0;
        bonusPrize = -1;
        bonusKillsFirstTime = 0;
        bonusKillsTime = new ArrayList();
        currEngineer = 0;
        isScrooge = true;
        slOrbital.transform.GetChild(0).gameObject.SetActive(false);
        slOrbital.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0.9019608f, 0.0f, 0.0f, 1.0f);
        buttonEmp.GetComponent<Button>().interactable = false;
        buttonEmp.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0.5f;
        slOrbital.value = 0.0f;
        orbitalCharges = 2;
        UpdateUIWave(waveCurrent, false);
        UpdateUIProgress();
        UpdateUIScore();
        UpdateUICredits();
        UpdateTurretContour();
    }

    private void DoBaseDeploy() {
        gameState = State.BaseDeploy;
        NextWave();
        StartCoroutine(OnGameBegin());
    }

    private void DoWaveHalf() {
        gameState = State.WaveHalf;
        StartCoroutine(OnWaveCountdownBegin());
        StartCoroutine(OnWaveCountdown());
    }

    private void DoWaveBegin() {
        gameState = State.WaveBegin;
        currTurrets = arrGatlings.Count + arrGausses.Count + arrLasers.Count;
        placementIndicatorTurret.SetActive(false);
        cgUnit.alpha = 0.0f;
        UpdateUIWave(waveCurrent, true);
        StartCoroutine(OnWaveCountdownEnd());
        if ( bonusType == 0 ) {
            bonusCoroutine = BonusCoroutine();
            StartCoroutine(bonusCoroutine);
        }
        ClearTurretContour();
    }

    private void DoBattle() {
        gameState = State.Battle;
        imgOrbitalAABB.enabled = true;
        slOrbital.transform.GetChild(0).gameObject.SetActive(true);
        buttonEmp.GetComponent<Button>().interactable = true;
        buttonEmp.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1.0f;
        StartCoroutine(EnemySpawnerCoroutine());
    }

    private void DoIntermission(bool won) {
        imgOrbitalAABB.enabled = false;
        slOrbital.transform.GetChild(0).gameObject.SetActive(false);
        textOrbitalLabel.text = "";
        gameState = State.Intermission;
        gui.ToggleWin(won);
        int finalTurrets = arrGatlings.Count + arrGausses.Count + arrLasers.Count;
        gui.SetPostgame(waveCurrent, currGametime, currKills, creditsEarned, sessionCreditsEarned, finalTurrets, postgameStrings, postgameValues, bonusDescr, bonusScore, scoreCurrent);
        scoreCurrent += ((int) (finalTurrets * 100 * (difficultyFactor + mutatorsFactor)));
        if ( bonusScore >= 0 ) { scoreCurrent += bonusScore; }
        prevPlayerExp = PlayerPrefs.GetInt("plExp");
        prevPostExp = PlayerPrefs.GetInt("plPostExp");
        prevGatlingExp = PlayerPrefs.GetInt("plGatlingExp");
        prevGaussExp = PlayerPrefs.GetInt("plGaussExp");
        prevLaserExp = PlayerPrefs.GetInt("plLaserExp");
        if ( waveCurrent == waveStart ) { prefs.AddGame(); }
        prefs.AddGameTime(currGametime); currGametime = 0;
        prefs.AddKills(currKills, currBossKills, gatlingKills, gaussKills, laserKills, orbitalKills); sessionKills += currKills;
        currKills = 0; currBossKills = 0; gatlingKills = 0; gaussKills = 0; laserKills = 0; orbitalKills = 0;
        prefs.AddMedals(medals[0], medals[1], medals[2], medals[3], medals[4], medals[5], medals[6], medals[7], medals[8]);
        medals = new int[9];
        prefs.CheckMostKills(sessionKills);
        prefs.CheckBestSpree(bestSpree);
        prefs.CheckLongestWaves(waveCurrent - waveStart + 1);
        prefs.CheckLongestGametime(sessionGametime);
        int _diff = currTurrets - finalTurrets;
        if ( won ) {
            StartCoroutine(OnWaveWin());
            prefs.AddWaveWon();
            if ( _diff == 0 ) { prefs.AddWavePerfect(); } else { prefs.AddTurretsLost(_diff); }
            CheckAchievement(4);
            CheckAchievement(5);
        } else {
            StartCoroutine(OnWaveLose());
            prefs.AddWaveLost();
            prefs.AddTurretsLost(_diff);
        }
        prefs.AddTurretsBuild(builtTurrets); builtTurrets = 0;
        prefs.AddTurretsRepaired(repairedTurrets); repairedTurrets = 0;
        prefs.AddTurretsUpgraded(upgradedTurrets); upgradedTurrets = 0;
        prefs.AddMoneyEarned(creditsEarned); creditsEarned = 0;
        prefs.AddMoneySpent(creditsSpent); creditsSpent = 0;
        prefs.CheckNewHighscore(waveCurrent, scoreCurrent);
        UpdateCommendations();
        prefs.Save();
    }

    private void DoGameOver() {
        gui.ShowGameOver();
    }

    public int GetTurretLevel(int type) {
        switch ( type ) {
            case 0: return GetGatlingLevel(); break;
            case 1: return GetGaussLevel(); break;
            case 2: return GetLaserLevel(); break;
            default: return -1;
        }
    }

    public int GetGatlingLevel() {
        return lvlGatling;
    }

    public int GetGaussLevel() {
        return lvlGauss;
    }

    public int GetLaserLevel() {
        return lvlLaser;
    }

    private void UpdateUIWave(int wave, bool start) {
        textWave.text = "WAVE " + (wave + 1).ToString();
        gui.ShowPhase(wave, start);
    }

    private void UpdateUIProgress() {
        float currProgress = ((float) enemiesLeft) / enemiesTotal;
        if ( enemiesLeft < 5 ) {
            // Switch to number
            gui.HideWaveSlider();
            textEnemies.text = enemiesLeft.ToString();
        } else {
            // Switch to gauge
            gui.ShowWaveSlider();
            gui.SetWaveSlider(currProgress);
        }
    }

    private void UpdateUIScore() {
        if ( scoreCoroutine != null ) {
            StopCoroutine(scoreCoroutine);
        }
        scoreCoroutine = OnScoreUpdate(scoreCurrent);
        StartCoroutine(scoreCoroutine);
    }

    private void UpdateUICredits() {
        textCredits.text = "cR " + creditsCurrent.ToString();
    }

    private void UpdateUIDeploy() {
        int _exp; float _min, _max, _prog;
        // Command Post
        _exp = PlayerPrefs.GetInt("plPostExp");
        lvlPost = prefs.ComputePostLevel(_exp);
        if ( lvlPostPrev == -1 ) {
            lvlPostPrev = lvlPost;
        } else {
            if ( lvlPost > lvlPostPrev ) {
                AdvanceUnitLevel(0, lvlPost);
                lvlPostPrev = lvlPost;
            }
        }
        _min = prefs.ComputePostMinXP(lvlPost);
        _max = prefs.ComputePostMaxXP(lvlPost);
        _prog = ((float)(_exp - _min)) / (_max - _min);
        if ( _min == _max ) { _prog = 1; };
        gui.SetDeployButton(0, prefs.Int2Roman(lvlPost), _prog);
        // Gatling
        _exp = PlayerPrefs.GetInt("plGatlingExp");
        lvlGatling = prefs.ComputeGatlingLevel(_exp);
        if ( lvlGatlingPrev == -1 ) {
            lvlGatlingPrev = lvlGatling;
        } else {
            if ( lvlGatling > lvlGatlingPrev ) {
                AdvanceUnitLevel(1, lvlGatling);
                lvlGatlingPrev = lvlGatling;
            }
        }
        _min = prefs.ComputeGatlingMinXP(lvlGatling);
        _max = prefs.ComputeGatlingMaxXP(lvlGatling);
        _prog = ((float)(_exp - _min)) / (_max - _min);
        if ( _min == _max ) { _prog = 1; };
        gui.SetDeployButton(1, prefs.Int2Roman(lvlGatling), _prog);
        // Gauss
        _exp = PlayerPrefs.GetInt("plGaussExp");
        lvlGauss = prefs.ComputeGaussLevel(_exp);
        if ( lvlGaussPrev == -1 ) {
            lvlGaussPrev = lvlGauss;
        } else {
            if ( lvlGauss > lvlGaussPrev ) {
                AdvanceUnitLevel(2, lvlGauss);
                lvlGaussPrev = lvlGauss;
            }
        }
        _min = prefs.ComputeGaussMinXP(lvlGauss);
        _max = prefs.ComputeGaussMaxXP(lvlGauss);
        _prog = ((float)(_exp - _min)) / (_max - _min);
        if ( _min == _max ) { _prog = 1; };
        gui.SetDeployButton(2, prefs.Int2Roman(lvlGauss), _prog);
        // Laser
        _exp = PlayerPrefs.GetInt("plLaserExp");
        lvlLaser = prefs.ComputeLaserLevel(_exp);
        if ( lvlLaserPrev == -1 ) {
            lvlLaserPrev = lvlLaser;
        } else {
            if ( lvlLaser > lvlLaserPrev ) {
                AdvanceUnitLevel(3, lvlLaser);
                lvlLaserPrev = lvlLaser;
            }
        }
        _min = prefs.ComputeLaserMinXP(lvlLaser);
        _max = prefs.ComputeLaserMaxXP(lvlLaser);
        _prog = ((float)(_exp - _min)) / (_max - _min);
        if ( _min == _max ) { _prog = 1; };
        gui.SetDeployButton(3, prefs.Int2Roman(lvlLaser), _prog);
        // Resources
        maxGatlings = prefs.ComputeGatlingAmount(lvlGatling);
        maxGausses = prefs.ComputeGaussAmount(lvlGauss);
        maxLasers = prefs.ComputeLaserAmount(lvlLaser);
        UpdateUIResources();
    }

    private void UpdateUIResources() {
        // Res - gatlings
        if ( arrGatlings.Count == maxGatlings ) {
            textGatlingsRed.text = arrGatlings.Count + "/" + maxGatlings;
            textGatlings.text = "/" + maxGatlings;
        } else {
            textGatlingsRed.text = "";
            textGatlings.text = arrGatlings.Count + "/" + maxGatlings;
        }
        // Res - gausses
        if ( arrGausses.Count == maxGausses ) {
            textGaussesRed.text = arrGausses.Count + "/" + maxGausses;
            textGausses.text = "/" + maxGausses;
        } else {
            textGaussesRed.text = "";
            textGausses.text = arrGausses.Count + "/" + maxGausses;
        }
        // Res - lasers
        if ( arrLasers.Count == maxLasers ) {
            textLasersRed.text = arrLasers.Count + "/" + maxLasers;
            textLasers.text = "/" + maxLasers;
        } else {
            textLasersRed.text = "";
            textLasers.text = arrLasers.Count + "/" + maxLasers;
        }
        if ( arrGatlings.Count == 0 ) { cgGatlings.alpha = 0.5f; } else { cgGatlings.alpha = 1.0f; }
        if ( arrGausses.Count == 0 ) { cgGausses.alpha = 0.5f; } else { cgGausses.alpha = 1.0f; }
        if ( arrLasers.Count == 0 ) { cgLasers.alpha = 0.5f; } else { cgLasers.alpha = 1.0f; }
    }

    private void UpdateCommendations() {
        int _exp, _min, _max;
        // Commendation 0: enemy kills
        _exp = PlayerPrefs.GetInt("plComm0") + currKills;
        lvlComm0 = prefs.ComputeComm0Level(_exp);
        _min = prefs.ComputeComm0MinXP(lvlComm0);
        _max = prefs.ComputeComm0MaxXP(lvlComm0);
        if ( lvlComm0 > lvlComm0Prev ) {
            AdvanceCommendation(0, lvlComm0);
            lvlComm0Prev = lvlComm0;
            for ( int i = 0; i < 4; ++i ) { comm0Prog[i] = lvlComm0 < 6; }
        } else {
            for ( int i = 0; i < 4; ++i ) {
                if ( (_exp - _min) >= ((float) _max - _min) * (i+1) / 4.0f ) {
                    if ( !comm0Prog[i] ) { gui.PushCommendationProgress(0, lvlComm0 + 1, _exp, _max); }
                    comm0Prog[i] = true;
                }
            }
        }
        // Commendation 1: turrets deploy
        _exp = PlayerPrefs.GetInt("plComm1") + builtTurrets;
        lvlComm1 = prefs.ComputeComm1Level(_exp);
        _min = prefs.ComputeComm1MinXP(lvlComm1);
        _max = prefs.ComputeComm1MaxXP(lvlComm1);
        if ( lvlComm1 > lvlComm1Prev ) {
            AdvanceCommendation(1, lvlComm1);
            lvlComm1Prev = lvlComm1;
            for ( int i = 0; i < 4; ++i ) { comm1Prog[i] = lvlComm1 < 6; }
        } else {
            for ( int i = 0; i < 4; ++i ) {
                if ( (_exp - _min) >= ((float) _max - _min) * (i+1) / 4.0f ) {
                    if ( !comm1Prog[i] ) { gui.PushCommendationProgress(1, lvlComm1 + 1, _exp, _max); }
                    comm1Prog[i] = true;
                }
            }
        }
        // Commendation 2: completed waves
        _exp = PlayerPrefs.GetInt("plComm2");
        lvlComm2 = prefs.ComputeComm2Level(_exp);
        _min = prefs.ComputeComm2MinXP(lvlComm2);
        _max = prefs.ComputeComm2MaxXP(lvlComm2);
        if ( lvlComm2 > lvlComm2Prev ) {
            AdvanceCommendation(2, lvlComm2);
            lvlComm2Prev = lvlComm2;
            for ( int i = 0; i < 4; ++i ) { comm2Prog[i] = lvlComm2 < 6; }
        } else {
            for ( int i = 0; i < 4; ++i ) {
                if ( (_exp - _min) >= ((float) _max - _min) * (i+1) / 4.0f ) {
                    if ( !comm2Prog[i] ) { gui.PushCommendationProgress(2, lvlComm2 + 1, _exp, _max); }
                    comm2Prog[i] = true;
                }
            }
        }
        // Commendation 3: perfect waves
        _exp = PlayerPrefs.GetInt("plComm3");
        lvlComm3 = prefs.ComputeComm3Level(_exp);
        _min = prefs.ComputeComm3MinXP(lvlComm3);
        _max = prefs.ComputeComm3MaxXP(lvlComm3);
        if ( lvlComm3 > lvlComm3Prev ) {
            AdvanceCommendation(3, lvlComm3);
            lvlComm3Prev = lvlComm3;
            for ( int i = 0; i < 4; ++i ) { comm3Prog[i] = lvlComm3 < 6; }
        } else {
            for ( int i = 0; i < 4; ++i ) {
                if ( (_exp - _min) >= ((float) _max - _min) * (i+1) / 4.0f ) {
                    if ( !comm3Prog[i] ) { gui.PushCommendationProgress(3, lvlComm3 + 1, _exp, _max); }
                    comm3Prog[i] = true;
                }
            }
        }
        // Commendation 4: Orbital Strike kills
        _exp = PlayerPrefs.GetInt("plComm4") + orbitalKills;
        lvlComm4 = prefs.ComputeComm4Level(_exp);
        _min = prefs.ComputeComm4MinXP(lvlComm4);
        _max = prefs.ComputeComm4MaxXP(lvlComm4);
        if ( lvlComm4 > lvlComm4Prev ) {
            AdvanceCommendation(4, lvlComm4);
            lvlComm4Prev = lvlComm4;
            for ( int i = 0; i < 4; ++i ) { comm4Prog[i] = lvlComm4 < 6; }
        } else {
            for ( int i = 0; i < 4; ++i ) {
                if ( (_exp - _min) >= ((float) _max - _min) * (i+1) / 4.0f ) {
                    if ( !comm4Prog[i] ) { gui.PushCommendationProgress(4, lvlComm4 + 1, _exp, _max); }
                    comm4Prog[i] = true;
                }
            }
        }
        // Commendation 5: money earned
        _exp = PlayerPrefs.GetInt("plComm5") + creditsEarned;
        lvlComm5 = prefs.ComputeComm5Level(_exp);
        _min = prefs.ComputeComm5MinXP(lvlComm5);
        _max = prefs.ComputeComm5MaxXP(lvlComm5);
        if ( lvlComm5 > lvlComm5Prev ) {
            AdvanceCommendation(5, lvlComm5);
            lvlComm5Prev = lvlComm5;
            for ( int i = 0; i < 4; ++i ) { comm5Prog[i] = lvlComm5 < 6; }
        } else {
            for ( int i = 0; i < 4; ++i ) {
                if ( (_exp - _min) >= ((float) _max - _min) * (i+1) / 4.0f ) {
                    if ( !comm5Prog[i] ) { gui.PushCommendationProgress(5, lvlComm5 + 1, _exp, _max); }
                    comm5Prog[i] = true;
                }
            }
        }
    }

    public void UpdateTurretContour() {
        Turret[] turrs = FindObjectsOfType<Turret>();
        foreach ( Turret t in turrs ) {
            t.UpdateContour(creditsCurrent);
        }
    }

    private void ClearTurretContour() {
        Turret[] turrs = FindObjectsOfType<Turret>();
        foreach ( Turret t in turrs ) {
            t.HideContour();
        }
    }

    public int GetCredits() {
        return creditsCurrent;
    }

    private int PromptWave(int wave) {
        int cycle = wave / 5;
        switch ( wave % 5 ) {
            case 0: { return 5 + 5 * cycle; } break;   // waves 1, 6, 11, 16, 21
            case 1: { return 10 + 5 * cycle; } break;   // waves 2, 7, 12, 17, 22
            case 2: { return 15 + 5 * cycle; } break;   // waves 3, 8, 13, 18, 23
            case 3: { return 20 + 5 * cycle; } break;   // waves 4, 9, 14, 19, 24
            case 4: { return 25 + 5 * cycle; } break;   // waves 5, 10, 15, 20, 25
            default: { return 0; } break;   // ooops
        }
    }

    IEnumerator EnemySpawnerCoroutine() {
        int remaining = enemiesTotal;
        bool finished = false;
        Vector3 offset = new Vector3(Random.Range(10, 20), Random.Range(10, 20), Random.Range(10, 20));
        GameObject unit;
        Transform seekTarget = GameObject.Find("Target").transform;
        Transform fleeTarget = GameObject.Find("ReactorCore").transform;
        if ( IsBossWave() ) {
            oBoss = Instantiate(EnemyBosses[Random.Range(0, EnemyBosses.Length)], oCommandPost.transform.position + offset, Quaternion.identity);
            oBoss.GetComponent<SeekUnit>().target = seekTarget;
            oBoss.GetComponent<FleeUnit>().target = fleeTarget;
            arrEnemies.Add(oBoss);
            remaining--;
        }
        for ( int i = 0; i < 4; ++i ) {
            unit = Instantiate(EnemyMinions[Random.Range(0, EnemyMinions.Length)], oCommandPost.transform.position + offset, Quaternion.identity);
            unit.GetComponent<SeekUnit>().target = seekTarget;
            unit.GetComponent<FleeUnit>().target = fleeTarget;
            arrEnemies.Add(unit);
            remaining--;
        }
        while ( !finished ) {
            yield return new WaitForSeconds(1.0f);
            if ( arrEnemies.Count < 5 ) {
                if ( remaining > 0 ) {
                    unit = Instantiate(EnemyMinions[Random.Range(0, EnemyMinions.Length)], oCommandPost.transform.position + offset, Quaternion.identity);
                    unit.GetComponent<SeekUnit>().target = seekTarget;
                    unit.GetComponent<FleeUnit>().target = fleeTarget;
                    arrEnemies.Add(unit);
                    remaining--;
                } else {
                    finished = true;
                }
            }
        }
        yield return null;
    }

    private int PromptBonusWave() {
        bonusScore = 0;
        bonusPrize = GetBonusPrize();
        string msgPrize = "";
        switch ( bonusPrize ) {
            case 0: case 1: {
                msgPrize = "Full Turrets repairs";
                textBonusComplete.color = new Color(1.0f, 0.7764706f, 0.0f, 1.0f);
            } break;
            case 2: {
                msgPrize = "Command Post Shield";
                textBonusComplete.color = new Color(0.2509804f, 0.5184898f, 1.0f, 1.0f);
            } break;
            case 3: {
                msgPrize = "EMP Device";
                textBonusComplete.color = new Color(0.1087576f, 0.8867924f, 0.1787133f, 1.0f);
            } break;
        }
        bonusType = Random.Range(0, 4);
        switch ( bonusType ) {
            case 0: {
                // Complete the wave in 1:30
                bonusDescr = "Not completed";
                textBonusProgress.text = "1:30";
                textBonusComplete.text = msgPrize;
                boxPartial.SetActive(false);
                return 0;
            } break;
            case 1: {
                // Kill 10 enemies in any 15 seconds window
                bonusDescr = "Not completed (0/10)";
                textBonusProgress.text = "0/10 (0:20)";
                textBonusPartial.text = "Up to 1000 cR";
                textBonusComplete.text = msgPrize;
                boxPartial.SetActive(true);
                return 1;
            } break;
            case 2: {
                // Complete the wave without losing any turret
                bonusDescr = "Not completed";
                textBonusProgress.text = "Don't lose\nany turret";
                textBonusComplete.text = msgPrize;
                boxPartial.SetActive(false);
                bonusScore = 1250;
                return 2;
            } break;
            case 3: {
                // Kill 3 enemies with the Orbital Strike
                bonusDescr = "Not completed (0/3)";
                textBonusProgress.text = "0/3";
                textBonusPartial.text = "Up to 1500 cR";
                textBonusComplete.text = msgPrize;
                boxPartial.SetActive(true);
                return 3;
            } break;
            default: { bonusDescr = ""; bonusScore = -1; textBonusProgress.text = "???"; return -1; }
        }
    }

    private int GetBonusPrize() {
        // 0-1: instant repair; 2: shield; 3: EMP
        return Random.Range(0, 4);
    }

    private void WinBonusWave() {
        switch ( bonusType ) {
            case 0: {
                bonusDescr = "Completed!";
                bonusScore = 1250;
                EarnMoney(625);
                gui.ShowLoot(625);
            } break;
            case 1: {
                bonusDescr = "Completed! (10/10)";
                bonusScore = 1500;
                EarnMoney(750);
                gui.ShowLoot(750);
            } break;
            case 2: {
                bonusDescr = "Completed!";
                bonusScore = 1250;
                EarnMoney(625);
                gui.ShowLoot(625);
            } break;
            case 3: {
                bonusDescr = "Completed!";
                bonusScore = 1500;
                EarnMoney(750);
                gui.ShowLoot(750);
            } break;
            default: return;
        }
        switch ( bonusPrize ) {
            case 0: case 1: {
                // Repair all turrets
                Turret[] turrets = FindObjectsOfType<Turret>();
                foreach ( Turret t in turrets ) {
                    t.Repair(500);
                    UpdateUIResources();
                    UpdateTurretsHealth();
                }
            } break;
            case 2: {
                // Give Command Post a shield
                oCommandPost.GetComponent<CommandPost>().GainShield();
            } break;
            case 3: {
                // Give player EMP
                buttonEmp.SetActive(true);
            } break;
            default: break;
        }
        AddMedal(1);
        AddPlayerXp(125, 1);
        gui.HideBonusData();
        CheckAchievement(2);
    }

    private void LoseBonusWave() {
        EarnMoney(bonusScore);
        gui.HideBonusData();
        gui.ShowLoot(0);
    }

    IEnumerator OnGameBegin() {
        yield return new WaitForSeconds(3.0f);
        if ( oCommandPost == null ) {
            gui.ShowWelcomeMessage();
        }
    }

    IEnumerator OnWaveCountdownBegin() {
        gui.ShowDeployButtons();
        yield return new WaitForSeconds(0.5f);
        if ( (waveCurrent % 5 == 0) && (waveCurrent > 0) ) {
            gui.ShowUpgradeEnemies(waveCurrent / 5);
        }
        if ( IsBossWave() ) {
            yield return new WaitForSeconds(5.0f);
            gui.ShowBossMessage();
        } else {
            if ( IsBonusWave() ) {
                yield return new WaitForSeconds(5.0f);
                int quest = PromptBonusWave();
                gui.UpdateBonusDescription(quest);
                gui.ShowBonusMessage();
            }
        }
    }

    IEnumerator OnWaveCountdown() {
        int timer = 30;
        while ( timer > 0 ) {
            if ( timer <= 5 ) {
                gui.PlaySound(gui.acCountdown);
            }
            yield return new WaitForSeconds(0.25f);
            textCountdown.text = "0:" + timer.ToString("00");
            yield return new WaitForSeconds(0.75f);
            timer--;
        }
        yield return new WaitForSeconds(0.25f);
        gui.PlaySound(gui.acCountdownEnd);
        textCountdown.text = "0:" + timer.ToString("00");
        gameState = State.WaveBegin;
        DoWaveBegin();
    }

    IEnumerator OnWaveCountdownEnd() {
        yield return new WaitForSeconds(0.5f);
        gui.HideDeploy();
        yield return new WaitForSeconds(0.5f);
        DoBattle();
    }

    IEnumerator OnWaveWin() {
        gui.PlaySound(gui.acCountdownEnd);
        yield return new WaitForSeconds(0.5f);
        gui.HideHUD();
        yield return new WaitForSeconds(0.5f);
        if ( IsBossWave() ) {
            gui.PlaySound(gui.acJingleWin[1]);
        } else {
            gui.PlaySound(gui.acJingleWin[Random.Range(0, gui.acJingleWin.Length)]);
        }
        yield return new WaitForSeconds(8.0f);
        if ( IsBossWave() ) {
            // show career progress
            float intermission = 7.25f;
            if ( xpKills > 0 ) { intermission += (xpKills / 5.0f) * Time.deltaTime; }
            if ( xpPerf > 0 ) { intermission += (xpPerf / 5.0f) * Time.deltaTime; }
            if ( xpMedals > 0 ) { intermission += (xpMedals / 5.0f) * Time.deltaTime; }
            if ( xpComms > 0 ) { intermission += (xpComms / 5.0f) * Time.deltaTime; }
            if ( xpBonus > 0 ) { intermission += (xpBonus / 5.0f) * Time.deltaTime; }
            gui.ShowPlayerProgress(startPlayerExp, prevPlayerExp, xpPerf, xpKills, xpMedals, xpComms, xpBonus);
            startPlayerExp = prevPlayerExp;
            xpPerf = 0; xpKills = 0; xpMedals = 0; xpComms = 0; xpBonus = 0;
            yield return new WaitForSeconds(intermission);
        }
        gui.ShowHUD();
        yield return new WaitForSeconds(0.5f);
        NextWave();
    }

    IEnumerator OnWaveLose() {
        gui.PlaySound(gui.acCountdownEnd);
        yield return new WaitForSeconds(0.5f);
        gui.HideHUD();
        yield return new WaitForSeconds(0.5f);
        gui.PlaySound(gui.acJingleLose[Random.Range(0, gui.acJingleLose.Length)]);
        yield return new WaitForSeconds(3.0f);
        // show career progress
        gui.ShowPlayerProgress(startPlayerExp, prevPlayerExp, xpPerf, xpKills, xpMedals, xpComms, xpBonus);
        startPlayerExp = prevPlayerExp;
        xpPerf = 0; xpKills = 0; xpMedals = 0; xpComms = 0; xpBonus = 0;
    }

    IEnumerator OnScoreUpdate(int finalVal) {
        int startVal;
        int.TryParse(textScore.text, out startVal);
        float val = startVal;
        while ( finalVal - val > 1.0f ) {
            val = Mathf.Lerp(val, finalVal, 0.25f);
            textScore.text = ((int) val).ToString();
            yield return new WaitForEndOfFrame();
        }
        textScore.text = scoreCurrent.ToString();
    }

    IEnumerator GameTimer() {
        while ( true ) {
            yield return new WaitForSeconds(1.0f);
            currGametime += 1;
            sessionGametime += 1;
        }
    }

    IEnumerator BonusCoroutine() {
        int _timer = 90, _time;
        int _s, _m;
        while ( _timer >= 0 ) {
            yield return new WaitForSeconds(1.0f);
            _timer--;
            _time = _timer;
            _s = _time % 60;
            _time /= 60;
            _m = _time % 60;
            textBonusProgress.text = _m.ToString("0") + ":" + _s.ToString("00");
        }
        bonusCoroutine = null;
        LoseBonusWave();
    }

    IEnumerator Bonus2Coroutine() {
        int _timer = 20, _time = 0;
        int _s, _m;
        while ( true ) {
            while ( _timer > 0 ) {
                yield return new WaitForSeconds(0.75f);
                _timer--;
                _time = _timer;
                _s = _time % 60;
                _time /= 60;
                _m = _time % 60;
                textBonusProgress.text = bonusKills + "/10 (" + _m.ToString("0") + ":" + _s.ToString("00") + ")";
                yield return new WaitForSeconds(0.25f);
            }
            if ( bonusKillsTime.Count > 1 ) {
                // Next sub-timer based on kills
                _timer = (int) ( ((float) bonusKillsTime[1]) - ((float) bonusKillsTime[0]) );
                bonusKillsTime.RemoveAt(0);
                bonusKillsFirstTime = (float) bonusKillsTime[0];
                bonusKills--;
                bonusScore -= 100;
            } else {
                // Not enough kills, restart
                bonusKills = 0;
                bonusScore = 0;
                bonusKillsTime.Clear();
                bonusKillsFirstTime = 0;
                textBonusProgress.text = "0/10 (0:00)";
                bonusDescr = "Not completed (0/10)";
                StopCoroutine(bonus2Coroutine);
                bonus2Coroutine = null;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator MultikillCoroutine() {
        yield return new WaitForSeconds(1.5f);
        if ( multikillCounter >= 3 ) {
            AddMedal(5);
        }
        multikillCounter = 0;
        multikillCoroutine = null;
    }

    IEnumerator ReloadOrbitalStrike() {
        float time = 10.0f;
        if ( isSuperReload ) {
            time = 5.0f;
        }
        float timeBase = time;
        slOrbital.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.white;
        while ( time > 0 ) {
            time -= Time.deltaTime;
            imgOrbitalAABB.rectTransform.localScale = Vector3.one + Vector3.one * (0.125f * Mathf.Sin(4*Time.time));
            if ( gameState != State.Battle ) {
                imgOrbitalAABB.rectTransform.localScale = Vector3.one;
                orbitalCharges = 2;
                yield return null;
            }
            slOrbital.value = time / timeBase;
            yield return new WaitForEndOfFrame();
        }
        slOrbital.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0.9019608f, 0.0f, 0.0f, 1.0f);
        slOrbital.value = 0.0f;
        imgOrbitalAABB.rectTransform.localScale = Vector3.one;
        orbitalCharges = 2;
        if ( gameState == State.Battle ) {
            gui.PlaySound(gui.orbitalReady);
        }
    }

    private void AddMedal(int index) {
        AddScore(75);
        EarnMoney(50);
        AddPlayerXp(15, 2);
        medals[index]++;
        gui.PushMedal(index);
        CheckAchievement(11);
    }

    private void AddEngineerAction() {
        currEngineer++;
        if ( currEngineer == 5 ) {
            AddMedal(0);
            currEngineer = 0;
        }
    }

    private void AdvanceUnitLevel(int type, int level) {
        switch ( type ) {
            case 0: {
                AddPlayerXp(150 * level, 3);
                gui.ShowAdvanceLevel(0, level);
                UpgradeCommandPost(level);
            } break;
            default: { AddPlayerXp(50 * level, 3); gui.ShowAdvanceLevel(type, level); } break;
        }
    }

    private void AdvanceCommendation(int comm, int lvl) {
        AddPlayerXp(250 * lvl, 4);
        gui.PushCommendation(comm, lvl);
        if ( comm == 0 && lvl >= 2) { CheckAchievement(0); }
        if ( lvl >= 4 ) { CheckAchievement(10); }
    }

    private void UpgradeCommandPost(int level) {
        oCommandPost.GetComponent<CommandPost>().SetLevel(level);
        if ( level > 1 ) {
            CheckAchievement(12);
        }
    }

    public void CheckAchievement(int num) {
        switch ( num ) {
            case 0: {
                // Unlock "Vampire" - Obtain the 'Target Practice' bronze tier
                if ( PlayerPrefs.GetInt("plChievo0") == 0 ) {
                    PlayerPrefs.SetInt("plChievo0", 1);
                    gui.PushAchievement(0);
                }
            } break;
            case 1: {
                // Unlock "Enemy Regeneration" - Earn 25 spree medals
                if ( PlayerPrefs.GetInt("plChievo1") == 0 ) {
                    int mdl = PlayerPrefs.GetInt("plMedal3") + PlayerPrefs.GetInt("plMedal4") + medals[3] + medals[4];
                    if ( mdl >= 25 ) {
                        PlayerPrefs.SetInt("plChievo1", 1);
                        gui.PushAchievement(1);
                    } else {
                        if ( mdl % 5 == 0 || mdl >= 23 ) {
                            gui.PushAchievementProgress(1, mdl, 25);
                        }
                    }
                }
            } break;
            case 2: {
                // Unlock "Super Enemy Radar" - Complete 5 bonus objectives
                if ( PlayerPrefs.GetInt("plChievo2") == 0 ) {
                    int bns = PlayerPrefs.GetInt("plMedal1") + 1;
                    if ( bns >= 5 ) {
                        PlayerPrefs.SetInt("plChievo2", 1);
                        gui.PushAchievement(2);
                    } else {
                        gui.PushAchievementProgress(2, bns, 5);
                    }
                }
            } break;
            case 3: {
                // Unlock "Super Reload" - Reach the 'Commander' rank
                if ( PlayerPrefs.GetInt("plChievo3") == 0 ) {
                    int rnk = prefs.ComputePlayerLevel(PlayerPrefs.GetInt("plExp"));
                    if ( rnk >= 25 ) {
                        PlayerPrefs.SetInt("plChievo3", 1);
                        gui.PushAchievement(3);
                    } else {
                        rnk++;
                        if ( rnk == 2 || rnk == 5 || rnk == 10 || rnk == 15 || rnk == 20 || rnk >= 24 ) {
                            gui.PushAchievementProgress(3, rnk, 26);
                        }
                    }
                }
            } break;
            case 4: {
                // Unlock "Delta" difficulty - Complete waves 1-5 on Hard on a single run
                if ( PlayerPrefs.GetInt("plChievo4") == 0 && difficulty == Difficulty.Hard && waveStart == 0 ) {
                    if ( waveCurrent >= 4 ) {
                        PlayerPrefs.SetInt("plChievo4", 1);
                        gui.PushAchievement(4);
                    } else {
                        gui.PushAchievementProgress(4, waveCurrent + 1, 5);
                    }
                }
            } break;
            case 5: {
                // Welcome to Tower Defence - Play and complete 10 waves (any difficulty)
                if ( PlayerPrefs.GetInt("plChievo5") == 0 ) {
                    int gms = PlayerPrefs.GetInt("plComm2");
                    if ( gms >= 10 ) {
                        PlayerPrefs.SetInt("plChievo5", 1);
                        gui.PushAchievement(5);
                    } else {
                        gui.PushAchievementProgress(5, gms, 10);
                    } 
                }
            } break;
            case 6: {
                // Extraordinary Maintenance - Repair turrets 50 times
                if ( PlayerPrefs.GetInt("plChievo6") == 0 ) {
                    int rep = PlayerPrefs.GetInt("plTurrRepaired") + repairedTurrets;
                    if ( rep >= 50 ) {
                        PlayerPrefs.SetInt("plChievo6", 1);
                        gui.PushAchievement(6);
                    } else {
                        if ( rep == 5 || rep == 10 || rep == 25 || rep == 45 || rep >= 48 ) {
                            gui.PushAchievementProgress(6, rep, 50);
                        }
                    }
                }
            } break;
            case 7: {
                // Technology boost - Upgrade turrets 50 times
                if ( PlayerPrefs.GetInt("plChievo7") == 0 ) {
                    int upg = PlayerPrefs.GetInt("plTurrUpgraded") + upgradedTurrets;
                    if ( upg >= 50 ) {
                        PlayerPrefs.SetInt("plChievo7", 1);
                        gui.PushAchievement(7);
                    } else {
                        if ( upg == 5 || upg == 10 || upg == 25 || upg == 45 || upg >= 48 ) {
                            gui.PushAchievementProgress(7, upg, 50);
                        }
                    }
                }
            } break;
            case 8: {
                // Space Patrol - Engage enemies 100 times with the Orbital Strike
                if ( PlayerPrefs.GetInt("plChievo8") == 0 ) {
                    int eng = PlayerPrefs.GetInt("plEngaged");
                    if ( eng >= 100 ) {
                        PlayerPrefs.SetInt("plChievo8", 1);
                        gui.PushAchievement(8);
                    } else {
                        if ( eng == 5 || eng == 15 || eng == 35 || eng == 50 || eng == 75 || eng == 90 || eng >= 98 ) {
                            gui.PushAchievementProgress(8, eng, 100);
                        }
                    }
                }
            } break;
            case 9: {
                // Bossing Around - Kill 20 bosses (any difficulty)
                if ( PlayerPrefs.GetInt("plChievo9") == 0 ) {
                    int bss = PlayerPrefs.GetInt("plBossKilled") + currBossKills;
                    if ( bss >= 20 ) {
                        PlayerPrefs.SetInt("plChievo9", 1);
                        gui.PushAchievement(9);
                    } else {
                        gui.PushAchievementProgress(9, bss, 20);
                    }
                }
            } break;
            case 10: {
                // High Ranks - Obtain a gold tier commendation
                if ( PlayerPrefs.GetInt("plChievo10") == 0 ) {
                    PlayerPrefs.SetInt("plChievo10", 1);
                    gui.PushAchievement(10);
                }
            } break;
            case 11: {
                // Honourable Service - Obtain every original medal in the game
                if ( PlayerPrefs.GetInt("plChievo11") == 0 ) {
                    int tot = 0;
                    for ( int i = 0; i < 9; ++i ) {
                        tot += PlayerPrefs.GetInt("plMedal" + i) + medals[i] > 0 ? 1 : 0;
                    }
                    if ( tot == 9 ) {
                        PlayerPrefs.SetInt("plChievo11", 1);
                        gui.PushAchievement(11);
                    }
                }
            } break;
            case 12: {
                // Remarkable Technician - Upgrade the Command Post to its maximum level
                if ( PlayerPrefs.GetInt("plChievo12") == 0 ) {
                    if ( lvlPost == 8 ) {
                        PlayerPrefs.SetInt("plChievo12", 1);
                        gui.PushAchievement(12);
                    } else {
                        gui.PushAchievementProgress(12, lvlPost, 8);
                    }
                }
            } break;
        }
    }
}
