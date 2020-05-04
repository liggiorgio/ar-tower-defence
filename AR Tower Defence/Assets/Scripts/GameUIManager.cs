using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {
    public GameObject buttonPause;
    public GameObject buttonResume;
    public GameObject windowPause;
    public GameObject panelMessageWelcome;
    public GameObject panelMessageBonus;
    public GameObject panelMessageBoss;
    public GameObject panelGameData;
    public GameObject panelBonusData;
    public GameObject panelDeploy;
    public GameObject buttonDeployGatling;
    public GameObject buttonDeployGauss;
    public GameObject buttonDeployLaser;
    public GameObject panelPhase;
    public GameObject panelLoot;
    public GameObject panelAdvanceLevel;
    public GameObject panelUpgradeEnemies;
    public GameObject panelCPShield;
    public GameObject goPostgame;
    public GameObject goFailed;
    public GameObject CompletedText;
    public GameObject FailedText;
    public GameObject TimeText;
    public GameObject PerformanceText;
    public GameObject LabelsText;
    public GameObject StringsText;
    public GameObject ValuesText;
    public GameObject ValueText;
    public GameObject BonusMessageImage;
    public GameObject BonusMessageText;
    public GameObject BonusPanelImage;
    public GameObject[] MedalPanels;
    public GameObject[] CommPanels;
    public GameObject[] CommProgPanels;
    public GameObject[] ChievoPanels;
    public GameObject[] ChievoProgPanels;

    public AudioClip acButtonClick;
    public AudioClip acGameBegin;
    public AudioClip acNotification;
    public AudioClip acNotificationBonus;
    public AudioClip acNotificationBoss;
    public AudioClip acCountdown;
    public AudioClip acCountdownEnd;
    public AudioClip acWaveBegin;
    public AudioClip acWaveBossBegin;
    public AudioClip[] acJingleWin;
    public AudioClip[] acJingleLose;
    public AudioClip acJingleGameOver;
    public AudioClip orbitalReady;
    public AudioClip orbitalNone;
    public AudioClip orbitalLost;
    public AudioClip orbitalEngaging;
    public AudioClip orbitalEngaged;
    public Sprite spButtonNormal;
    public Sprite spButtonHighlight;
    public Sprite[] bonusImages;
    public Sprite[] medalsImages;
    public Sprite[] commsImages;

    [SerializeField] private RectTransform progContainer;
    [SerializeField] private Text progTextPlayerName;
    [SerializeField] private Text progTextPlayerRank;
    [SerializeField] private Image progImagePlayerRank;
    [SerializeField] private Text progTextXpValue;
    [SerializeField] private Slider progSliderProgress;
    [SerializeField] private Text progTextXp1Label;
    [SerializeField] private Text progTextXp2Label;
    [SerializeField] private Text progTextKillsLabel;
    [SerializeField] private Text progTextKillsValue;
    [SerializeField] private Text progTextBonusLabel;
    [SerializeField] private Text progTextBonusValue;
    [SerializeField] private Text progTextMedalsLabel;
    [SerializeField] private Text progTextMedalsValue;
    [SerializeField] private Text progTextPerfLabel;
    [SerializeField] private Text progTextPerfValue;
    [SerializeField] private Text progTextCommsLabel;
    [SerializeField] private Text progTextCommsValue;
    [SerializeField] private Text progTextTotalLabel;
    [SerializeField] private Text progTextTotalValue;
    [SerializeField] private CanvasGroup canvasLevelUp;
    [SerializeField] private Text progTextNewRank;
    [SerializeField] private Image progImageNewRank;

    private GameManager gm;
    private AudioSource source;
    private AudioSource src2;
    private CanvasGroup canvasGame;
    private CanvasGroup canvasPostgame;
    private CanvasGroup canvasProgress;
    private CanvasGroup canvasGameOver;
    private IEnumerator progressCoroutine = null;
    private bool sounds;
    private bool msgMutex = false;
    private int nextPanel = 0;
    private bool[] mdlMutex = new bool[3];
    private string[] bonusTextDescriptions = { "Complete the wave in 1:30.", "Kill 10 enemies in any 20 seconds window.",
        "Complete the wave without losing any turret.", "Kill 3 enemies with the Orbital Strike.", "Get 5 unique medals during the wave." };
    private string[] medalTextName = { "Engineer", "Phat Loot", "Bossy",
        "Spree", "Frenzy", "Multikill", "Big Catch", "Legit Scrooge", "Hammer Away" };
    private string[] medalTextDescr = { "Worked on 5 turrets during deployment.", "Completed a bonus objective.", "Completed a boss wave.",
        "Killed 10 enemies without losses.", "Killed 20 enemies without losses.", "Killed 3 or more enemies in the blink of an eye.",
        "Killed a boss with the Orbital Strike.", "Won the wave without deploying or upgrading any turret.", "Completed all 25 waves in one sitting." };
    private string[] commTextName = { "Target Practice", "War Architect", "Veteran", "Untouchable", "Specialist", "Big Money" };
    private string[] commTextDescr = { "Iron", "Bronze", "Silver", "Gold", "Onyx", "Onyx (MAX)" };
    [SerializeField] private Sprite[] chievoSprites;
    private string[] chievoTextName = {
        "UNLOCK 'VAMPIRE'", "UNLOCK 'ENEMY REGENERATION'", "UNLOCK 'SUPER ENEMY RADAR'",
        "UNLOCK 'SUPER RELOAD'", "UNLOCK DELTA DIFFICULTY",
        "WELCOME TO TOWER DEFENCE", "EXTRAORDINARY MAINTENANCE", "TECHNOLOGY BOOST",
        "SPACE PATROL", "BOSSING AROUND",
        "TOUR OF DUTY", "HONOURABLE SERVICE", "REMARKABLE TECHNICIAN"
    };
    private string[] chievoTextDescr = {
        "Obtained the 'Target Practice' commendation bronze tier.", "Earned 25 spree medals.", "Completed 5 bonus objectives.",
        "Reached the Commander rank.", "Completed waves 1-5 on Hard in a single run.",
        "Played and completed 10 waves (any difficulty).", "Repaired turrets 50 times.", "Upgraded turrets 50 times.",
        "Engaged enemies 100 times with the Orbital Strike.", "Killed 20 enemy bosses (any difficulty).",
        "Obtained a gold tier commendation.", "Obtained every original medal in the game.", "Upgraded the Command Post to its maximum level."
    };

    void Awake() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvasGame = GameObject.Find("CanvasGame").GetComponent<CanvasGroup>();
        canvasPostgame = GameObject.Find("CanvasPostgame").GetComponent<CanvasGroup>();
        canvasProgress = GameObject.Find("CanvasProgress").GetComponent<CanvasGroup>();
        canvasGameOver = GameObject.Find("CanvasGameOver").GetComponent<CanvasGroup>();
        canvasGameOver.alpha = 0.0f;
        canvasGameOver.blocksRaycasts = false;
        canvasProgress.alpha = 0.0f;
        canvasProgress.blocksRaycasts = false;
        source = GetComponent<AudioSource>();
        src2 = transform.GetChild(0).GetComponent<AudioSource>();
        sounds = PlayerPrefs.GetInt("sounds", 1) == 1;
        progTextPlayerName.text = PlayerPrefs.GetString("plName");
    }

    void Start() {
        buttonPause.GetComponent<Button>().onClick.AddListener(gm.PauseGame);
        buttonResume.GetComponent<Button>().onClick.AddListener(gm.ResumeGame);
        goPostgame.SetActive(false);
        goFailed.SetActive(false);
    }

    public void ClickGamePause() {
        PlaySound(acButtonClick);
        ToggleWindowPause(true);
    }

    public void ClickGameResume() {
        PlaySound(acButtonClick);
        ToggleWindowPause(false);
    }

    public void ClickGameEnd() {
        PlayerPrefs.SetInt("plExp", gm.prevPlayerExp);
        PlayerPrefs.SetInt("plPostExp", gm.prevPostExp);
        PlayerPrefs.SetInt("plGatlingExp", gm.prevGatlingExp);
        PlayerPrefs.SetInt("plGaussExp", gm.prevGaussExp);
        PlayerPrefs.SetInt("plLaserExp", gm.prevLaserExp);
        PlayerPrefs.Save();
        gm.StopGame();
    }

    public void ClickDeployType(int btn) {
        PlaySound(acButtonClick);
        ClearDeploy();
        panelDeploy.transform.GetChild(btn + 2).GetComponent<Image>().sprite = spButtonHighlight;
        panelDeploy.transform.GetChild(btn + 2).GetChild(3).GetComponent<Image>().sprite = spButtonHighlight;
        gm.SetTurretType(btn);
    }

    public void ShowHUD() {
        ToggleButtonPause(true);
        TogglePanelGameData(true);
        TogglePanelBonusData(false);
        TogglePanelDeploy(true);
        StartCoroutine(AnimateFadeHUD(true));
    }

    public void HideHUD() {
        StartCoroutine(AnimateFadeHUD(false));
    }

    public void ShowGameOver() {
        StartCoroutine(AnimateGameOver());
    }

    public void ShowDeployButtons() {
        buttonDeployGatling.SetActive(true);
        buttonDeployGauss.SetActive(true);
        buttonDeployLaser.SetActive(true);
    }

    public void HideDeployButtons() {
        buttonDeployGatling.SetActive(false);
        buttonDeployGauss.SetActive(false);
        buttonDeployLaser.SetActive(false);
    }

    public void ShowPhase(int waveNo, bool start) {
        string msg;
        if ( start ) { msg = "PREPARE FOR "; } else { msg = "DEPLOY FOR "; }
        if ( gm.IsBossWave() ) { msg += "BOSS WAVE: "; } else { msg += "WAVE: "; }
        msg += (waveNo + 1).ToString();
        panelPhase.transform.GetChild(0).GetComponent<Text>().text = msg;
        StartCoroutine(AnimatePhasePanel(5.0f, start));
    }

    public void ShowLoot(int reward) {
        string msg = "CASH REWARD: ";
        msg += reward.ToString() + " cR";
        if ( reward == 0 ) {
            panelLoot.transform.GetChild(0).GetComponent<Text>().text = "BONUS OBJECTIVE FAILED";
            panelLoot.transform.GetChild(1).GetComponent<Text>().text = "REWARD LOST";
            StartCoroutine(AnimateLootPanel(5.0f, false));
        } else {
            panelLoot.transform.GetChild(0).GetComponent<Text>().text = "BONUS OBJECTIVE COMPLETED";
            panelLoot.transform.GetChild(1).GetComponent<Text>().text = msg;
            StartCoroutine(AnimateLootPanel(5.0f, true));
        }
    }

    public void ShowAdvanceLevel(int type, int newLevel) {
        string top = "", bottom = "";
        if ( ( type == 2 || type == 3) && ( newLevel == 1 ) ) { return; }
        switch ( type ) {
            case 0: top = "COMMAND POST UPGRADED"; break;
            case 1: top = "GATLINGS "; break;
            case 2: top = "GAUSSES "; break;
            case 3: top = "LASERS "; break;
            default: break;
        }
        if ( type != 0 ) {
            bottom = "LEVEL " + gm.prefs.Int2Roman(newLevel) + " REACHED";
            switch ( newLevel ) {
                case 2: {
                    top += "SPECIALIST";
                    switch ( type ) {
                        case 1: bottom += " / GAUSSES UNLOCKED"; break;
                        case 2: bottom += " / LASERS UNLOCKED"; break;
                        default: break;
                    }
                } break;
                case 3: top += "EXPERT"; break;
                case 4: top += "ARTISAN"; break;
                case 5: top += "MASTER"; break;
                default: break;
            }
        } else {
            switch ( newLevel ) {
                case 2: case 6: bottom = "INCREASED MAXIMUM HEALTH"; break;
                case 4: bottom = "INCREASED ORBITAL STRIKE ATTACK"; break;
                case 8: bottom = "INCREASED REPAIR EFFECTIVENESS"; break;
                default: bottom = "NEW EXPERIENCE POINTS EARNED"; break;
            }
        }
        panelAdvanceLevel.transform.GetChild(0).GetComponent<Text>().text = top;
        panelAdvanceLevel.transform.GetChild(1).GetComponent<Text>().text = bottom;
        StartCoroutine(AnimateAdvanceLevelPanel(5.0f));
    }

    public void ShowUpgradeEnemies(int msg) {
        string str = "";
        switch ( msg ) {
            case 1: {
                // Health x 1.5
                str = "HEALTH x 1.5";
            } break;
            case 2: {
                // Health x 1.5, damage x 1.5
                str = "HEALTH x 1.5   /   DAMAGE x 1.5";
            } break;
            case 3: {
                // Health x 2.0, damage x 1.5
                str = "HEALTH x 2.0   /   DAMAGE x 1.5";
            } break;
            case 4: {
                // Health x 2.5, damage x 2.0
                str = "HEALTH x 2.5   /   DAMAGE x 2.0";
            } break;
            default: return;
        }
        panelUpgradeEnemies.transform.GetChild(1).GetComponent<Text>().text = str;
        StartCoroutine(AnimateUpgradeEnemiesPanel(5.0f));
    }

    public void ShowWelcomeMessage() {
        StartCoroutine(AnimateWelcomeMessage(450.0f, 0.5f));
    }

    public void ShowBonusMessage() {
        StartCoroutine(AnimateBonusMessage(450.0f, 0.5f));
    }

    public void ShowBossMessage() {
        StartCoroutine(AnimateBossMessage(450.0f, 0.5f));
    }

    public void SetWaveSlider(float val) {
        if ( progressCoroutine != null ) {
            StopCoroutine(progressCoroutine);
        }
        progressCoroutine = AnimateWaveSlider(val);
        StartCoroutine(progressCoroutine);
    }

    public void ShowWaveSlider() {
        panelGameData.transform.GetChild(2).gameObject.SetActive(true);
        panelGameData.transform.GetChild(3).gameObject.SetActive(false);
    }

    public void HideWaveSlider() {
        panelGameData.transform.GetChild(2).gameObject.SetActive(false);
        panelGameData.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void ShowBonusData() {
        TogglePanelBonusData(true);
    }

    public void HideBonusData() {
        TogglePanelBonusData(false);
    }

    public void ToggleWin(bool won) {
        goPostgame.SetActive(won);
        goFailed.SetActive(!won);
    }

    public void SetDeployButton(int btn, string tier, float progr) {
        switch ( btn ) {
            case 0: {
                // Command Post
                GameObject.Find("TextPostTier").GetComponent<Text>().text = "LVL " + tier;
                GameObject.Find("SliderPostProgress").GetComponent<Slider>().value = progr;
            } break;
            case 1: {
                // Gatling
                GameObject.Find("TextGatlingTier").GetComponent<Text>().text = "LVL " + tier;
                GameObject.Find("SliderGatlingProgress").GetComponent<Slider>().value = progr;
            } break;
            case 2: {
                // Gauss
                if ( tier != "-" ) {
                    SetLockDeploy(false, false);
                    GameObject.Find("TextGaussTier").GetComponent<Text>().text = "LVL " + tier;
                } else {
                    SetLockDeploy(false, true);
                }
                GameObject.Find("SliderGaussProgress").GetComponent<Slider>().value = progr;
            } break;
            case 3: {
                // Laser
                if ( tier != "-" ) {
                    SetLockDeploy(true, false);
                    GameObject.Find("TextLaserTier").GetComponent<Text>().text = "LVL " + tier;
                } else {
                    SetLockDeploy(true, true);
                }
                GameObject.Find("SliderLaserProgress").GetComponent<Slider>().value = progr;
            } break;

        }
    }

    private void SetLockDeploy(bool btn, bool locked) {
        // 0: gauss, 1: laser
        GameObject button;
        if ( !btn ) { button = buttonDeployGauss; } else { button = buttonDeployLaser; }
        if ( locked ) {
            button.GetComponent<Button>().interactable = false;
            button.transform.GetChild(0).GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5f);
            button.transform.GetChild(1).gameObject.SetActive(false);
            button.transform.GetChild(2).gameObject.SetActive(false);
            button.transform.GetChild(3).GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5f);
            button.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5f);
            button.transform.GetChild(5).gameObject.SetActive(true);
            if ( button.transform.childCount > 6 ) {
                button.transform.GetChild(6).gameObject.SetActive(false);
            }
        } else {
            button.GetComponent<Button>().interactable = true;
            button.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            button.transform.GetChild(1).gameObject.SetActive(true);
            button.transform.GetChild(2).gameObject.SetActive(true);
            button.transform.GetChild(3).GetComponent<Image>().color = Color.white;
            button.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Image>().color = Color.white;
            button.transform.GetChild(5).gameObject.SetActive(false);
            if ( button.transform.childCount > 6 ) {
                button.transform.GetChild(6).gameObject.SetActive(true);
            }
        }
    }

    public void ClearDeploy() {
        for (int i = 2; i < 5; ++i) {
            panelDeploy.transform.GetChild(i).GetComponent<Image>().sprite = spButtonNormal;
            panelDeploy.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = spButtonNormal;
        }
    }
    
    public void ShowDeploy() {
        TogglePanelDeploy(true);
    }

    public void HideDeploy() {
        TogglePanelDeploy(false);
    }

    public void ShowPlayerProgress(int start, int end, int performance, int kills, int medals, int comms, int bonus) {
        StartCoroutine(AnimatePlayerProgress(start, end, performance, kills, medals, comms, bonus));
    }

    private void SetSounds(bool flag) {
        sounds = flag;
    }

    public void PlaySound(AudioClip clip) {
        if ( sounds ) {
            source.PlayOneShot(clip, 1.0f);
        }
    }

    public void SetPostgame(int wave, int time, int kills, int earned, int allEarned, int turrs, string strings, string multi, string bonusDescr, int bonus, int score) {
        CompletedText.GetComponent<Text>().text = "WAVE " + (wave + 1) + " COMPLETED!";
        FailedText.GetComponent<Text>().text = "WAVE " + (wave + 1) + " FAILED!";
        time -= 38; if ( wave == 0 ) { time += 8; }
        int _s = time % 60;
        time /= 60;
        int _m = time % 60;
        TimeText.GetComponent<Text>().text = _m.ToString("##0") + ":" + _s.ToString("00");
        PerformanceText.GetComponent<Text>().text = "KILLS: " + kills + "     INCOME: " + earned + "     TOTAL: " + (allEarned);
        ValueText.GetComponent<Text>().text = score.ToString();
        GameObject.Find("TextBestValue").GetComponent<Text>().text = gm.GetHighScore(wave).ToString();
        StartCoroutine(AnimatePostgame(turrs, strings, multi, bonusDescr, bonus, score));
    }

    public void UpdateBonusDescription(int type) {
        BonusMessageImage.GetComponent<Image>().sprite = bonusImages[type];
        BonusMessageText.GetComponent<Text>().text = bonusTextDescriptions[type];
        BonusPanelImage.GetComponent<Image>().sprite = bonusImages[type];
    }

    public void PushMedal(int medal) {
        int panel;
        if ( !mdlMutex[0] ) { nextPanel = 0; } else
        if ( !mdlMutex[1] ) { nextPanel = 1; } else
        if ( !mdlMutex[2] ) { nextPanel = 2; }
        panel = nextPanel;
        nextPanel = (nextPanel + 1) % 3;
        StartCoroutine(AnimateMedalMessage(panel, medal, -300.0f, 0.5f));
    }

    public void PushCommendation(int comm, int lvl) {
        int panel;
        if ( !mdlMutex[0] ) { nextPanel = 0; } else
        if ( !mdlMutex[1] ) { nextPanel = 1; } else
        if ( !mdlMutex[2] ) { nextPanel = 2; }
        panel = nextPanel;
        nextPanel = (nextPanel + 1) % 3;
        StartCoroutine(AnimateCommendationMessage(panel, comm, lvl, -350.0f, 0.5f));
    }

    public void PushCommendationProgress(int comm, int lvl, int exp, int maxExp) {
        int panel;
        if ( !mdlMutex[0] ) { nextPanel = 0; } else
        if ( !mdlMutex[1] ) { nextPanel = 1; } else
        if ( !mdlMutex[2] ) { nextPanel = 2; }
        panel = nextPanel;
        nextPanel = (nextPanel + 1) % 3;
        StartCoroutine(AnimateCommendationProgressMessage(panel, comm, lvl, exp, maxExp, -350.0f, 0.5f));
    }

    public void PushAchievement(int ach) {
        int panel;
        if ( !mdlMutex[0] ) { nextPanel = 0; } else
        if ( !mdlMutex[1] ) { nextPanel = 1; } else
        if ( !mdlMutex[2] ) { nextPanel = 2; }
        panel = nextPanel;
        nextPanel = (nextPanel + 1) % 3;
        StartCoroutine(AnimateAchievementMessage(panel, ach, -350.0f, 0.5f));
    }

    public void PushAchievementProgress(int ach, int prog, int maxProg) {
        int panel;
        if ( !mdlMutex[0] ) { nextPanel = 0; } else
        if ( !mdlMutex[1] ) { nextPanel = 1; } else
        if ( !mdlMutex[2] ) { nextPanel = 2; }
        panel = nextPanel;
        nextPanel = (nextPanel + 1) % 3;
        StartCoroutine(AnimateAchievementProgressMessage(panel, ach, prog, maxProg, -350.0f, 0.5f));
    }

    private void ToggleButtonPause(bool flag) {
        buttonPause.SetActive(flag);
    }

    private void ToggleWindowPause(bool flag) {
        windowPause.SetActive(flag);
    }

    private void TogglePanelPhase(bool flag) {
        panelPhase.SetActive(flag);
    }

    private void TogglePanelLoot(bool flag) {
        panelLoot.SetActive(flag);
    }

    private void TogglePanelAdvanceLevel(bool flag) {
        panelAdvanceLevel.SetActive(flag);
    }

    private void TogglePanelUpgradeEnemies(bool flag) {
        panelUpgradeEnemies.SetActive(flag);
    }

    private void TogglePanelMessageWelcome(bool flag) {
        panelMessageWelcome.SetActive(flag);
    }

    private void TogglePanelMessageBonus(bool flag) {
        panelMessageBonus.SetActive(flag);
    }

    private void TogglePanelDeploy(bool flag) {
        panelDeploy.SetActive(flag);
    }

    private void TogglePanelGameData(bool flag) {
        panelGameData.SetActive(flag);
    }

    private void TogglePanelBonusData(bool flag) {
        panelBonusData.SetActive(flag);
    }

    private void TogglePanelMessageBoss(bool flag) {
        panelMessageBoss.SetActive(flag);
    }

    IEnumerator AnimateWaveSlider(float finalVal) {
        Slider sl = panelGameData.transform.GetChild(2).GetComponent<Slider>();
        float val;
        while ( sl.value - finalVal > 0.01f ) {
            val = Mathf.Lerp(sl.value, finalVal, 0.25f);
            sl.value = val;
            yield return new WaitForEndOfFrame();
        }
        sl.value = finalVal;
    }

    IEnumerator AnimatePhasePanel(float time, bool start) {
        if ( !start ) {
            PlaySound(acGameBegin);
        }
        float alpha = 0.0f;
        Image img = panelPhase.GetComponent<Image>();
        Text txt = panelPhase.transform.GetChild(0).GetComponent<Text>();
        TogglePanelPhase(true);
        while ( alpha < 1.0f ) {
            alpha += Time.deltaTime;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        alpha = 1.0f;
        img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1.0f);
        if ( start ) {
            if (gm.IsBossWave() ) {
                PlaySound(acWaveBossBegin);
            } else {
                PlaySound(acWaveBegin);
            }
        }
        yield return new WaitForSeconds(time/2);
        while ( alpha > 0.0f ) {
            alpha -= Time.deltaTime;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        img.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.0f);
        TogglePanelPhase(false);
    }

    IEnumerator AnimateLootPanel(float time, bool won) {
        if ( won ) {
            PlaySound(acNotificationBonus);
        } else {
            PlaySound(acCountdownEnd);
        }
        float alpha = 0.0f;
        Image img = panelLoot.GetComponent<Image>();
        Text txt = panelLoot.transform.GetChild(0).GetComponent<Text>();
        Text txt2 = panelLoot.transform.GetChild(1).GetComponent<Text>();
        TogglePanelLoot(true);
        while ( alpha < 1.0f ) {
            alpha += Time.deltaTime;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        alpha = 1.0f;
        img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1.0f);
        txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
        yield return new WaitForSeconds(time/2);
        while ( alpha > 0.0f ) {
            alpha -= Time.deltaTime;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        img.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.0f);
        txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
        TogglePanelLoot(false);
    }

    IEnumerator AnimateAdvanceLevelPanel(float time) {
        PlaySound(acNotificationBoss);
        yield return new WaitForSeconds(0.25f);
        float alpha = 0.0f;
        Image img = panelAdvanceLevel.GetComponent<Image>();
        Text txt = panelAdvanceLevel.transform.GetChild(0).GetComponent<Text>();
        Text txt2 = panelAdvanceLevel.transform.GetChild(1).GetComponent<Text>();
        TogglePanelAdvanceLevel(true);
        while ( alpha < 1.0f ) {
            alpha += Time.deltaTime * 4.0f;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.5f, 1.0f, 0.9722222f, alpha);
            txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        alpha = 1.0f;
        img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        txt.color = new Color(0.5f, 1.0f, 0.9722222f, alpha);
        txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
        yield return new WaitForSeconds(time);
        while ( alpha > 0.0f ) {
            alpha -= Time.deltaTime * 4.0f;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.5f, 1.0f, 0.9722222f, alpha);
            txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        img.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        txt.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.0f);
        txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
        TogglePanelAdvanceLevel(false);
    }

    IEnumerator AnimateUpgradeEnemiesPanel(float time) {
        yield return new WaitForSeconds(0.25f);
        float alpha = 0.0f;
        Image img = panelUpgradeEnemies.GetComponent<Image>();
        Text txt = panelUpgradeEnemies.transform.GetChild(0).GetComponent<Text>();
        Text txt2 = panelUpgradeEnemies.transform.GetChild(1).GetComponent<Text>();
        TogglePanelUpgradeEnemies(true);
        while ( alpha < 1.0f ) {
            alpha += Time.deltaTime * 4.0f;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.9019608f, 0.0f, 0.0f, alpha);
            txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        alpha = 1.0f;
        img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        txt.color = new Color(0.9019608f, 0.0f, 0.0f, alpha);
        txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
        yield return new WaitForSeconds(time);
        while ( alpha > 0.0f ) {
            alpha -= Time.deltaTime * 4.0f;
            img.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            txt.color = new Color(0.9019608f, 0.0f, 0.0f, alpha);
            txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, alpha);
            yield return new WaitForEndOfFrame();
        }
        img.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        txt.color = new Color(0.9019608f, 0.0f, 0.0f, 0.0f);
        txt2.color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.0f);
        TogglePanelUpgradeEnemies(false);
    }

    IEnumerator AnimateWelcomeMessage(float goalX, float factor) {
        while ( msgMutex ) {
            yield return new WaitForEndOfFrame();
        }
        msgMutex = true;
        PlaySound(acNotification);
        yield return new WaitForSeconds(0.25f);
        TogglePanelMessageWelcome(true);
        Vector3 startPos = panelMessageWelcome.transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(panelMessageWelcome.transform.localPosition, goalPos) > 1.0f ) {
            panelMessageWelcome.transform.localPosition = Vector3.Lerp(panelMessageWelcome.transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        panelMessageWelcome.transform.localPosition = goalPos;
        yield return new WaitForSeconds(5.0f);
        while ( Vector3.Distance(panelMessageWelcome.transform.localPosition, startPos) > 1.0f ) {
            panelMessageWelcome.transform.localPosition = Vector3.Lerp(panelMessageWelcome.transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        panelMessageWelcome.transform.localPosition = startPos;
        TogglePanelMessageWelcome(false);
        msgMutex = false;
    }

    IEnumerator AnimateBonusMessage(float goalX, float factor) {
        while ( msgMutex ) {
            yield return new WaitForEndOfFrame();
        }
        msgMutex = true;
        PlaySound(acNotification);
        PlaySound(acNotificationBonus);
        yield return new WaitForSeconds(0.25f);
        TogglePanelMessageBonus(true);
        Vector3 startPos = panelMessageBonus.transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(panelMessageBonus.transform.localPosition, goalPos) > 1.0f ) {
            panelMessageBonus.transform.localPosition = Vector3.Lerp(panelMessageBonus.transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        panelMessageBonus.transform.localPosition = goalPos;
        yield return new WaitForSeconds(5.0f);
        ShowBonusData();
        while ( Vector3.Distance(panelMessageBonus.transform.localPosition, startPos) > 1.0f ) {
            panelMessageBonus.transform.localPosition = Vector3.Lerp(panelMessageBonus.transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        panelMessageBonus.transform.localPosition = startPos;
        TogglePanelMessageBonus(false);
        msgMutex = false;
    }

    IEnumerator AnimateBossMessage(float goalX, float factor) {
        while ( msgMutex ) {
            yield return new WaitForEndOfFrame();
        }
        msgMutex = true;
        PlaySound(acNotification);
        PlaySound(acNotificationBoss);
        yield return new WaitForSeconds(0.25f);
        TogglePanelMessageBoss(true);
        Vector3 startPos = panelMessageBoss.transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(panelMessageBoss.transform.localPosition, goalPos) > 1.0f ) {
            panelMessageBoss.transform.localPosition = Vector3.Lerp(panelMessageBoss.transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        panelMessageBoss.transform.localPosition = goalPos;
        yield return new WaitForSeconds(5.0f);
        while ( Vector3.Distance(panelMessageBoss.transform.localPosition, startPos) > 1.0f ) {
            panelMessageBoss.transform.localPosition = Vector3.Lerp(panelMessageBoss.transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        panelMessageBoss.transform.localPosition = startPos;
        TogglePanelMessageBoss(false);
        msgMutex = false;
    }

    IEnumerator AnimateMedalMessage(int panelNo, int medalNo, float goalX, float factor) {
        while ( mdlMutex[panelNo] ) {
            yield return new WaitForEndOfFrame();
        }
        mdlMutex[panelNo] = true;
        MedalPanels[panelNo].transform.GetChild(0).GetComponent<Image>().sprite = medalsImages[medalNo];
        MedalPanels[panelNo].transform.GetChild(1).GetComponent<Text>().text = medalTextName[medalNo];
        MedalPanels[panelNo].transform.GetChild(2).GetComponent<Text>().text = medalTextDescr[medalNo];
        PlaySound(acNotification);
        yield return new WaitForSeconds(0.25f);
        Vector3 startPos = MedalPanels[panelNo].transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(MedalPanels[panelNo].transform.localPosition, goalPos) > 1.0f ) {
            MedalPanels[panelNo].transform.localPosition = Vector3.Lerp(MedalPanels[panelNo].transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        MedalPanels[panelNo].transform.localPosition = goalPos;
        yield return new WaitForSeconds(4.0f);
        while ( Vector3.Distance(MedalPanels[panelNo].transform.localPosition, startPos) > 1.0f ) {
            MedalPanels[panelNo].transform.localPosition = Vector3.Lerp(MedalPanels[panelNo].transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        MedalPanels[panelNo].transform.localPosition = startPos;
        mdlMutex[panelNo] = false;
    }

    IEnumerator AnimateCommendationMessage(int panelNo, int comm, int lvl, float goalX, float factor) {
        while ( mdlMutex[panelNo] ) {
            yield return new WaitForEndOfFrame();
        }
        mdlMutex[panelNo] = true;
        CommPanels[panelNo].transform.GetChild(0).GetComponent<Image>().sprite = gm.prefs.commendationPlateSprites[lvl];
        CommPanels[panelNo].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = commsImages[comm];
        CommPanels[panelNo].transform.GetChild(1).GetComponent<Text>().text = commTextName[comm];
        CommPanels[panelNo].transform.GetChild(2).GetComponent<Text>().text = commTextDescr[lvl - 1] + " tier reached.";
        PlaySound(acNotification);
        PlaySound(acNotificationBoss);
        yield return new WaitForSeconds(0.25f);
        Vector3 startPos = CommPanels[panelNo].transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(CommPanels[panelNo].transform.localPosition, goalPos) > 1.0f ) {
            CommPanels[panelNo].transform.localPosition = Vector3.Lerp(CommPanels[panelNo].transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        CommPanels[panelNo].transform.localPosition = goalPos;
        yield return new WaitForSeconds(5.0f);
        while ( Vector3.Distance(CommPanels[panelNo].transform.localPosition, startPos) > 1.0f ) {
            CommPanels[panelNo].transform.localPosition = Vector3.Lerp(CommPanels[panelNo].transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        CommPanels[panelNo].transform.localPosition = startPos;
        mdlMutex[panelNo] = false;
    }

    IEnumerator AnimateCommendationProgressMessage(int panelNo, int comm, int lvl, int exp, int max, float goalX, float factor) {
        while ( mdlMutex[panelNo] ) {
            yield return new WaitForEndOfFrame();
        }
        mdlMutex[panelNo] = true;
        CommProgPanels[panelNo].transform.GetChild(0).GetComponent<Image>().sprite = gm.prefs.commendationPlateSprites[lvl];
        CommProgPanels[panelNo].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = commsImages[comm];
        CommProgPanels[panelNo].transform.GetChild(1).GetComponent<Text>().text = commTextName[comm];
        CommProgPanels[panelNo].transform.GetChild(3).GetComponent<Text>().text = exp + " / " + max;
        CommProgPanels[panelNo].transform.GetChild(4).GetComponent<Slider>().value = ((float) exp) / ((float) max);
        PlaySound(acNotification);
        yield return new WaitForSeconds(0.25f);
        Vector3 startPos = CommProgPanels[panelNo].transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(CommProgPanels[panelNo].transform.localPosition, goalPos) > 1.0f ) {
            CommProgPanels[panelNo].transform.localPosition = Vector3.Lerp(CommProgPanels[panelNo].transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        CommProgPanels[panelNo].transform.localPosition = goalPos;
        yield return new WaitForSeconds(5.0f);
        while ( Vector3.Distance(CommProgPanels[panelNo].transform.localPosition, startPos) > 1.0f ) {
            CommProgPanels[panelNo].transform.localPosition = Vector3.Lerp(CommProgPanels[panelNo].transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        CommProgPanels[panelNo].transform.localPosition = startPos;
        mdlMutex[panelNo] = false;
    }

    IEnumerator AnimateAchievementMessage(int panelNo, int achNo, float goalX, float factor) {
        while ( mdlMutex[panelNo] ) {
            yield return new WaitForEndOfFrame();
        }
        mdlMutex[panelNo] = true;
        ChievoPanels[panelNo].transform.GetChild(0).GetComponent<Image>().sprite = chievoSprites[Mathf.Min(achNo, chievoSprites.Length)];
        ChievoPanels[panelNo].transform.GetChild(1).GetComponent<Text>().text = chievoTextName[achNo];
        ChievoPanels[panelNo].transform.GetChild(2).GetComponent<Text>().text = chievoTextDescr[achNo];
        PlaySound(acNotification);
        PlaySound(acNotificationBoss);
        yield return new WaitForSeconds(0.25f);
        Vector3 startPos = ChievoPanels[panelNo].transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(ChievoPanels[panelNo].transform.localPosition, goalPos) > 1.0f ) {
            ChievoPanels[panelNo].transform.localPosition = Vector3.Lerp(ChievoPanels[panelNo].transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        ChievoPanels[panelNo].transform.localPosition = goalPos;
        yield return new WaitForSeconds(5.0f);
        while ( Vector3.Distance(ChievoPanels[panelNo].transform.localPosition, startPos) > 1.0f ) {
            ChievoPanels[panelNo].transform.localPosition = Vector3.Lerp(ChievoPanels[panelNo].transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        ChievoPanels[panelNo].transform.localPosition = startPos;
        mdlMutex[panelNo] = false;
    }

    IEnumerator AnimateAchievementProgressMessage(int panelNo, int achNo, int prg, int max, float goalX, float factor) {
        while ( mdlMutex[panelNo] ) {
            yield return new WaitForEndOfFrame();
        }
        mdlMutex[panelNo] = true;
        ChievoProgPanels[panelNo].transform.GetChild(0).GetComponent<Image>().sprite = chievoSprites[achNo];
        ChievoProgPanels[panelNo].transform.GetChild(1).GetComponent<Text>().text = chievoTextName[achNo];
        ChievoProgPanels[panelNo].transform.GetChild(3).GetComponent<Text>().text = prg + " / " + max;
        ChievoProgPanels[panelNo].transform.GetChild(4).GetComponent<Slider>().value = ((float) prg) / ((float) max);
        PlaySound(acNotification);
        yield return new WaitForSeconds(0.25f);
        Vector3 startPos = ChievoProgPanels[panelNo].transform.localPosition;
        Vector3 goalPos = startPos + new Vector3(Mathf.Sign(goalX)*500.0f + goalX, 0.0f, 0.0f);
        while ( Vector3.Distance(ChievoProgPanels[panelNo].transform.localPosition, goalPos) > 1.0f ) {
            ChievoProgPanels[panelNo].transform.localPosition = Vector3.Lerp(ChievoProgPanels[panelNo].transform.localPosition, goalPos, factor);
            yield return new WaitForEndOfFrame();
        }
        ChievoProgPanels[panelNo].transform.localPosition = goalPos;
        yield return new WaitForSeconds(5.0f);
        while ( Vector3.Distance(ChievoProgPanels[panelNo].transform.localPosition, startPos) > 1.0f ) {
            ChievoProgPanels[panelNo].transform.localPosition = Vector3.Lerp(ChievoProgPanels[panelNo].transform.localPosition, startPos, factor);
            yield return new WaitForEndOfFrame();
        }
        ChievoProgPanels[panelNo].transform.localPosition = startPos;
        mdlMutex[panelNo] = false;
    }

    IEnumerator AnimateFadeHUD(bool show) {
        float alpha;
        canvasGame.blocksRaycasts = show;
        canvasPostgame.blocksRaycasts = !show;
        if ( show ) {
            alpha = 0.0f;
            while ( alpha < 1.0f ) {
                alpha += Time.deltaTime * 4.0f;
                canvasGame.alpha = alpha;
                canvasPostgame.alpha = 1.0f - alpha;
                yield return new WaitForEndOfFrame();
            }
            alpha = 1.0f;
        } else {
            alpha = 1.0f;
            while ( alpha > 0.0f ) {
                alpha -= Time.deltaTime * 4.0f;
                canvasGame.alpha = alpha;
                canvasPostgame.alpha = 1.0f - alpha;
                yield return new WaitForEndOfFrame();
            }
            alpha = 0.0f;
        }
    }

    IEnumerator AnimatePostgame(int turrs, string strings, string multi, string bonusDescr, int bonus, int score) {
        LabelsText.SetActive(false);
        StringsText.SetActive(false);
        ValuesText.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        LabelsText.SetActive(true);
        StringsText.SetActive(true);
        ValuesText.SetActive(true);
        LabelsText.GetComponent<Text>().text = "SURVIVAL:\n";
        StringsText.GetComponent<Text>().text = turrs + " turrets survived\n";
        ValuesText.GetComponent<Text>().text = (turrs * 100) + "\n";
        yield return new WaitForSeconds(1.0f);
        LabelsText.GetComponent<Text>().text += "DIFFICULTY:\n";
        StringsText.GetComponent<Text>().text += gm.GetDifficultyName() + "\n";
        ValuesText.GetComponent<Text>().text += "x " + gm.GetDifficultyFactor().ToString("0.0") + "\n";
        yield return new WaitForSeconds(1.0f);
        LabelsText.GetComponent<Text>().text += "MUTATORS:\n";
        StringsText.GetComponent<Text>().text += gm.GetMutatorsName() + "\n";
        ValuesText.GetComponent<Text>().text += "x " + gm.GetMutatorsFactor().ToString("0.00") + "\n";
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(AnimateLerpInt(score + (int) (turrs * 100 * (gm.GetDifficultyFactor() + gm.GetMutatorsFactor()))));
        yield return new WaitForSeconds(1.0f);
        if ( bonus >= 0 ) {
            LabelsText.GetComponent<Text>().text += "BONUS WAVE:";
            StringsText.GetComponent<Text>().text += bonusDescr;
            ValuesText.GetComponent<Text>().text += bonus.ToString();
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(AnimateLerpInt(score + bonus + (int) (turrs * 100 * (gm.GetDifficultyFactor() + gm.GetMutatorsFactor()))));
        }
    }

    IEnumerator AnimateGameOver() {
        PlaySound(acJingleGameOver);
        float alpha;
        canvasGame.blocksRaycasts = false;
        canvasPostgame.blocksRaycasts = false;
        canvasGameOver.blocksRaycasts = true;

        alpha = 0.0f;
        while ( alpha < 1.0f ) {
            alpha += Time.deltaTime * 4.0f;
            canvasGame.alpha = 1.0f - alpha;
            canvasGameOver.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
        alpha = 1.0f;
    }

    IEnumerator AnimateLerpInt(int finalVal) {
        Text score = ValueText.GetComponent<Text>();
        int startVal;
        int.TryParse(score.text, out startVal);
        float val = startVal;
        while ( finalVal - val > 1.0f ) {
            val = Mathf.Lerp(val, finalVal, 0.25f);
            score.text = ((int) val).ToString();
            yield return new WaitForEndOfFrame();
        }
        score.text = finalVal.ToString();
    }

    IEnumerator AnimatePlayerProgress(int start, int end, int performance, int kills, int medals, int comms, int bonus) {
        canvasLevelUp.alpha = 0.0f;
        // Set up starting values
        int _exp = start;
        int _level = gm.prefs.ComputePlayerLevel(_exp);
        int _prevLevel = _level;
        int _xpMin = gm.prefs.ComputePlayerMinXP(_level);
        int _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
        progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
        progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
        progTextXpValue.text = _exp + " / " + _xpMax;
        progSliderProgress.value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        if ( _xpMin == _xpMax ) { progSliderProgress.value = 1; }
        progContainer.sizeDelta = new Vector2(1600.0f, 360.0f);
        progTextXp1Label.text = "";
        progTextXp2Label.text = "";
        progTextKillsLabel.text = "";
        progTextKillsValue.text = "";
        progTextBonusLabel.text = "";
        progTextBonusValue.text = "";
        progTextMedalsLabel.text = "";
        progTextMedalsValue.text = "";
        progTextPerfLabel.text = "";
        progTextPerfValue.text = "";
        progTextCommsLabel.text = "";
        progTextCommsValue.text = "";
        progTextTotalLabel.text = "";
        progTextTotalValue.text = "";
            
        // Animate fade-in
        float alpha;
        canvasProgress.blocksRaycasts = true;
        alpha = 0.0f;
        while ( alpha < 1.0f ) {
            alpha += Time.deltaTime * 4.0f;
            canvasProgress.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.25f);

        // Resize up
        float height = 360.0f;
        while ( 900.0f - height > 1.0f ) {
            height = Mathf.Lerp(height, 900.0f, 0.25f);
            progContainer.sizeDelta = new Vector2(1600.0f, height);
            yield return new WaitForEndOfFrame();
        }
        progContainer.sizeDelta = new Vector2(1600.0f, 900.0f);

        // Show starting labels
        progTextXp1Label.text = "EXPERIENCE";
        progTextXp2Label.text = "EXP POINTS";
        progTextTotalLabel.text = "TOTAL";
        progTextTotalValue.text = "0";
        yield return new WaitForSeconds(0.25f);

        // Show kills points
        progTextKillsLabel.text = "KILLS";
        progTextKillsValue.text = "0";
        if ( kills > 0 ) {
            yield return new WaitForSeconds(0.25f);
            alpha = 0.0f;
            _exp += kills;
            src2.Play();
            while ( alpha < 1.0f ) {
                alpha += 5.0f / kills;
                progTextKillsValue.text = ((int) Mathf.Lerp(0.0f, (float) kills, alpha)).ToString();
                progTextTotalValue.text = (int.Parse(progTextTotalValue.text) + (int) Mathf.Lerp(0.0f, (float) kills, 5.0f / kills)).ToString();
                start += (int) Mathf.Lerp(0.0f, (float) kills, 5.0f / kills);
                _level = gm.prefs.ComputePlayerLevel(start);
                if ( _level > _prevLevel ) {
                    _prevLevel = _level;
                    StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
                }
                _xpMin = gm.prefs.ComputePlayerMinXP(_level);
                _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
                progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
                progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
                progTextXpValue.text = start + " / " + _xpMax;
                progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
                yield return new WaitForEndOfFrame();
            }
            src2.Stop();
            progTextKillsValue.text = kills.ToString();
            progTextTotalValue.text = kills.ToString();
            start = _exp;
            _level = gm.prefs.ComputePlayerLevel(start);
            if ( _level > _prevLevel ) {
                _prevLevel = _level;
                StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
            }
            _xpMin = gm.prefs.ComputePlayerMinXP(_level);
            _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
            progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
            progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
            progTextXpValue.text = start + " / " + _xpMax;
            progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
            yield return new WaitForSeconds(0.25f);
        } else {
            yield return new WaitForSeconds(0.5f);
        }

        // Show bonus points
        progTextBonusLabel.text = "BONUS";
        progTextBonusValue.text = "0";
        if ( bonus > 0 ) {
            yield return new WaitForSeconds(0.25f);
            alpha = 0.0f;
            _exp += bonus;
            src2.Play();
            while ( alpha < 1.0f ) {
                alpha += 5.0f / bonus;
                progTextBonusValue.text = ((int) Mathf.Lerp(0.0f, (float) bonus, alpha)).ToString();
                progTextTotalValue.text = (int.Parse(progTextTotalValue.text) + (int) Mathf.Lerp(0.0f, (float) bonus, 5.0f / bonus)).ToString();
                start += (int) Mathf.Lerp(0.0f, (float) bonus, 5.0f / bonus);
                _level = gm.prefs.ComputePlayerLevel(start);
                if ( _level > _prevLevel ) {
                    _prevLevel = _level;
                    StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
                }
                _xpMin = gm.prefs.ComputePlayerMinXP(_level);
                _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
                progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
                progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
                progTextXpValue.text = start + " / " + _xpMax;
                progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
                yield return new WaitForEndOfFrame();
            }
            progTextBonusValue.text = bonus.ToString();
            progTextTotalValue.text = (kills + bonus).ToString();
            start = _exp;
            _level = gm.prefs.ComputePlayerLevel(start);
            if ( _level > _prevLevel ) {
                _prevLevel = _level;
                StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
            }
            src2.Stop();
            _xpMin = gm.prefs.ComputePlayerMinXP(_level);
            _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
            progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
            progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
            progTextXpValue.text = start + " / " + _xpMax;
            progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
            yield return new WaitForSeconds(0.25f);
        } else {
            yield return new WaitForSeconds(0.5f);
        }

        // Show medals points
        progTextMedalsLabel.text = "MEDALS";
        progTextMedalsValue.text = "0";
        if ( medals > 0 ) {
            yield return new WaitForSeconds(0.25f);
            alpha = 0.0f;
            _exp += medals;
            src2.Play();
            while ( alpha < 1.0f ) {
                alpha += 5.0f / medals;
                progTextMedalsValue.text = ((int) Mathf.Lerp(0.0f, (float) medals, alpha)).ToString();
                progTextTotalValue.text = (int.Parse(progTextTotalValue.text) + (int) Mathf.Lerp(0.0f, (float) medals, 5.0f / medals)).ToString();
                start += (int) Mathf.Lerp(0.0f, (float) medals, 5.0f / medals);
                _level = gm.prefs.ComputePlayerLevel(start);
                if ( _level > _prevLevel ) {
                    _prevLevel = _level;
                    StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
                }
                _xpMin = gm.prefs.ComputePlayerMinXP(_level);
                _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
                progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
                progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
                progTextXpValue.text = start + " / " + _xpMax;
                progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
                yield return new WaitForEndOfFrame();
            }
            src2.Stop();
            progTextMedalsValue.text = medals.ToString();
            progTextTotalValue.text = (kills + bonus + medals).ToString();
            start = _exp;
            _level = gm.prefs.ComputePlayerLevel(start);
            if ( _level > _prevLevel ) {
                _prevLevel = _level;
                StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
            }
            _xpMin = gm.prefs.ComputePlayerMinXP(_level);
            _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
            progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
            progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
            progTextXpValue.text = start + " / " + _xpMax;
            progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
            yield return new WaitForSeconds(0.25f);
        } else {
            yield return new WaitForSeconds(0.5f);
        }

        // Show performance points
        progTextPerfLabel.text = "PERFORMANCE";
        progTextPerfValue.text = "0";
        if ( performance > 0 ) {
            yield return new WaitForSeconds(0.25f);
            alpha = 0.0f;
            _exp += performance;
            src2.Play();
            while ( alpha < 1.0f ) {
                alpha += 5.0f / performance;
                progTextPerfValue.text = ((int) Mathf.Lerp(0.0f, (float) performance, alpha)).ToString();
                progTextTotalValue.text = (int.Parse(progTextTotalValue.text) + (int) Mathf.Lerp(0.0f, (float) performance, 5.0f / performance)).ToString();
                start += (int) Mathf.Lerp(0.0f, (float) performance, 5.0f / performance);
                _level = gm.prefs.ComputePlayerLevel(start);
                if ( _level > _prevLevel ) {
                    _prevLevel = _level;
                    StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
                }
                _xpMin = gm.prefs.ComputePlayerMinXP(_level);
                _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
                progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
                progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
                progTextXpValue.text = start + " / " + _xpMax;
                progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
                yield return new WaitForEndOfFrame();
            }
            src2.Stop();
            progTextPerfValue.text = performance.ToString();
            progTextTotalValue.text = (kills + bonus + medals + performance).ToString();
            start = _exp;
            _level = gm.prefs.ComputePlayerLevel(start);
            if ( _level > _prevLevel ) {
                _prevLevel = _level;
                StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
            }
            _xpMin = gm.prefs.ComputePlayerMinXP(_level);
            _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
            progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
            progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
            progTextXpValue.text = start + " / " + _xpMax;
            progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
            yield return new WaitForSeconds(0.25f);
        } else {
            yield return new WaitForSeconds(0.5f);
        }

        // Show commendations points
        progTextCommsLabel.text = "COMMENDATIONS";
        progTextCommsValue.text = "0";
        if ( comms > 0 ) {
            yield return new WaitForSeconds(0.25f);
            alpha = 0.0f;
            _exp += comms;
            src2.Play();
            while ( alpha < 1.0f ) {
                alpha += 5.0f / comms;
                progTextCommsValue.text = ((int) Mathf.Lerp(0.0f, (float) comms, alpha)).ToString();
                progTextTotalValue.text = (int.Parse(progTextTotalValue.text) + (int) Mathf.Lerp(0.0f, (float) comms, 5.0f / comms)).ToString();
                start += (int) Mathf.Lerp(0.0f, (float) comms, 5.0f / comms);
                _level = gm.prefs.ComputePlayerLevel(start);
                if ( _level > _prevLevel ) {
                    _prevLevel = _level;
                    StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
                }
                _xpMin = gm.prefs.ComputePlayerMinXP(_level);
                _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
                progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
                progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
                progTextXpValue.text = start + " / " + _xpMax;
                progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
                yield return new WaitForEndOfFrame();
            }
            src2.Stop();
            progTextCommsValue.text = comms.ToString();
            progTextTotalValue.text = (kills + bonus + medals + performance + comms).ToString();
            start = _exp;
            _level = gm.prefs.ComputePlayerLevel(start);
            if ( _level > _prevLevel ) {
                _prevLevel = _level;
                StartCoroutine(LevelUp(gm.prefs.ComputeRankName(_level), gm.prefs.ComputeRankImage(_level)));
            }
            _xpMin = gm.prefs.ComputePlayerMinXP(_level);
            _xpMax = gm.prefs.ComputePlayerMaxXP(_level);
            progTextPlayerRank.text = gm.prefs.ComputeRankName2(_level);
            progImagePlayerRank.sprite = gm.prefs.ComputeRankImage(_level);
            progTextXpValue.text = start + " / " + _xpMax;
            progSliderProgress.value = ((float)(start - _xpMin)) / (_xpMax - _xpMin);
            yield return new WaitForSeconds(0.25f);
        } else {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(3.0f);
        
        // Animate fade-out
        alpha = 1.0f;
        while ( alpha > 0.0f ) {
            alpha -= Time.deltaTime * 4.0f;
            canvasProgress.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
        canvasProgress.blocksRaycasts = false;
    }

    IEnumerator LevelUp(string newRankName, Sprite newRankImage) {
        progTextNewRank.text = newRankName;
        progImageNewRank.sprite = newRankImage;
        source.PlayOneShot(acNotificationBoss, 1.0f);
        float alpha = 0.0f;
        while ( alpha < 1.0f ) {
            alpha += Time.deltaTime * 4.0f;
            canvasLevelUp.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
        canvasLevelUp.alpha = 1.0f;
        yield return new WaitForSeconds(1.5f);
        alpha = 1.0f;
        while ( alpha > 0.0f ) {
            alpha -= Time.deltaTime * 4.0f;
            canvasLevelUp.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
        canvasLevelUp.alpha = 0.0f;
    }
}
