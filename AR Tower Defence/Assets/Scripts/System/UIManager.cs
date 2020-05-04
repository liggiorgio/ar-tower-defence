using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject header;
    public GameObject start;
    public GameObject windowNameInput;
    public GameObject buttonPlay;
    public GameObject buttonAchievements;
    public GameObject buttonAbout;
    public GameObject buttonSettings;
    public GameObject buttonProfile;
    public GameObject buttonHelp;
    public GameObject windowAbout;
    public GameObject windowHelp;
    public GameObject[] itemHelp;
    public GameObject windowAchievements;
    public GameObject[] btnAchievement;
    public GameObject windowSettings;
    public GameObject windowConfirmClear;
    public GameObject windowProfile;
    public GameObject windowStatsCommand;
    public GameObject windowStatsGatling;
    public GameObject windowStatsGauss;
    public GameObject windowStatsLaser;
    public GameObject windowMedals;
    public GameObject[] btnMedals;
    public GameObject medalTextName;
    public GameObject medalTextInfo;
    public GameObject windowCommendations;
    public GameObject[] commendationPlates;
    public GameObject commendationNav;
    public GameObject commendationTextName;
    public GameObject commendationTextInfo;
    public GameObject windowStatsPlayer;
    public GameObject[] statSheets;
    public GameObject statNav;
    public GameObject windowNewGame;
    public GameObject windowWaves;
    public GameObject windowDifficulty;
    public GameObject windowDeltaLocked;
    public GameObject windowMutators;
    public GameObject windowMut0Locked;
    public GameObject windowMut1Locked;
    public GameObject windowMut2Locked;
    public GameObject windowMut4Locked;
    public GameObject[] btnDifficulties;
    public GameObject difficultyTextInfo;
    public GameObject[] btnMutators;
    public GameObject mutatorTextName;
    public GameObject mutatorTextInfo;
    public GameObject achievementTextName;
    public GameObject achievementTextInfo;
    public GameObject loader;
    public Sprite soundsOn;
    public Sprite soundsOff;
    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite tableInactive;
    public Sprite tableActive;
    public AudioClip menuClick;
    [SerializeField] private Text helpCounter;
    private Prefs prefs;
    private GameManager gm;
    private AudioSource source;
    private int currHelp;
    private string[] medalName = {  "Engineer", "Phat Loot", "Bossy",
                                    "Spree", "Frenzy", "Multikill",
                                    "Big Catch", "Legit Scrooge", "Hammer Away" };
    private string[] medalInfo = {  "Build/repair/upgrade 5 turrets during wave-half time",
                                    "Complete a bonus objective",
                                    "Complete a boss wave",
                                    "Kill 10 enemies without losing turrets",
                                    "Kill 20 enemies without losing turrets",
                                    "Kill 3 or more enemies within 1.5 seconds",
                                    "Kill a boss with the Orbital Strike",
                                    "Win a wave without deploying or upgrading any turret",
                                    "Complete all 25 waves in one sitting" };
    private string[] commendationName = { "Target Practice", "War Architect", "Veteran", "Untouchable", "Specialist", "Big Money" };
    private string[] commendationInfo = { "Kill any enemy",
                                        "Build any type of turrets",
                                        "Play and complete any wave",
                                        "Complete a wave without losing any turret",
                                        "Kill any enemy with an Orbital Strike attack",
                                        "Earn money during the game" };
    private string[] difficultyInfo = { "Balanced game settings, suitable for all players.",
                                        "Enemies are slightly stronger, but the pay is worth it.",
                                        "It's either them or you. Aim for good, and watch your six." };
    private string[] mutatorsName = { "Vampire",
                                      "Enemy Regeneration",
                                      "Super Enemy Radar",
                                      "Fireworks",
                                      "Super Reload" };
    private string[] mutatorsInfo = { "No repairs allowed, your turrets gain health when damaging enemies (not shields).\n+25%",
                                      "Enemy spacecrafts have auto-repair units installed aboard.\n+10%",
                                      "Enemies can detect your units from a further distance.\n+5%",
                                      "Physics and visual effects of explosions are increased threefold. Damage is not affected.\n+0%",
                                      "All your units' rate of fire increase by half, Orbital Strike cooldown time halves.\n-25%" };

    private string[] achievementsName = {
        "WELCOME TO TOWER DEFENCE", "EXTRAORDINARY MAINTENANCE",
        "TECHNOLOGY BOOST", "SPACE PATROL","BOSSING AROUND",
        "TOUR OF DUTY", "HONOURABLE SERVICE", "REMARKABLE TECHNICIAN" };
    private string[] achievementsInfo = {
        "Play and complete 10 waves (any difficulty).", "Repair turrets 50 times.",
        "Upgrade turrets 50 times.", "Engage enemies 100 times with the Orbital Strike.", "Kill 20 enemy bosses (any difficulty).",
        "Obtain a gold tier commendation.", "Obtain every original medal in the game.", "Upgrade the Command Post to its maximum level."
    };

    void Start() {
        prefs = gameObject.GetComponent<Prefs>();
        source = gameObject.GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        MusicPlay();
        buttonProfile.transform.GetChild(0).GetComponent<Text>().text = "";
        if ( PlayerPrefs.GetInt("plChievo0") == 1 ) {
            // Unlock Vampire
            GameObject.Find("ButtonMut0Locked").SetActive(false);
        }
        if ( PlayerPrefs.GetInt("plChievo1") == 1 ) {
            // Unlock Enemy Regeneration
            GameObject.Find("ButtonMut1Locked").SetActive(false);
        }
        if ( PlayerPrefs.GetInt("plChievo2") == 1 ) {
            // Unlock Super Enemy Radar
            GameObject.Find("ButtonMut2Locked").SetActive(false);
        }
        if ( PlayerPrefs.GetInt("plChievo3") == 1 ) {
            // Unlock Super Reload
            GameObject.Find("ButtonMut4Locked").SetActive(false);
        }
        if ( PlayerPrefs.GetInt("plChievo4") == 1 ) {
            // Unlock Delta difficulty
            GameObject.Find("ButtonDiff2Locked").SetActive(false);
        }
        // Locked/unlocked achievements
        for ( int i = 0; i < 8; ++i ) {
            if ( PlayerPrefs.GetInt("plChievo" + (i + 5).ToString()) == 1 ) {
                btnAchievement[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if ( prefs.PlayerExists() ) {
            GotoMain();
        } else {
            GotoStart();
        }
        ShowMedalInfo(-1);
        ShowCommendationInfo(-1);
        ShowStatsInfo(-1);
        ShowDifficultyInfo(0);
        ClearMutatorInfo();
        ClearAchievementInfo();
        ClearHelp();
    }

    public void GotoStart() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(true);
        ToggleStart(true);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoInputName() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(true);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoMain() {
        buttonProfile.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("plName");
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        if ( prefs.PlayerExists() ) {
            ToggleHeader(true);
            ToggleStart(false);
            ToggleInputName(false);
            ToggleMain(true);
            ToggleAbout(false);
            ToggleHelp(false);
            ToggleAchievements(false);
            ToggleSettings(false);
            ToggleClear(false);
            ToggleProfile(false);
            ToggleStatsCommand(false);
            ToggleStatsGatling(false);
            ToggleStatsGauss(false);
            ToggleStatsLaser(false);
            ToggleMedals(false);
            ToggleCommendations(false);
            ToggleStatsPlayer(false);
            ToggleNewGame(false);
            ToggleWaves(false);
            ToggleDifficulty(false);
            ToggleMutators(false);
        }
    }

    public void GotoAbout() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(true);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoHelp() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(true);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoAchievements() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(true);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoSettings() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(true);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
        if ( prefs.GetSounds() ) {
            GameObject.Find("ButtonSounds").GetComponent<Image>().sprite = soundsOn;
        } else {
            GameObject.Find("ButtonSounds").GetComponent<Image>().sprite = soundsOff;
        }
        if ( prefs.GetMusic() ) {
            GameObject.Find("ButtonMusic").GetComponent<Image>().sprite = musicOn;
        } else {
            GameObject.Find("ButtonMusic").GetComponent<Image>().sprite = musicOff;
        }
    }

    public void GotoClear() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(true);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoProfile() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(true);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoStatsCommand() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(true);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoStatsGatling() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(true);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoStatsGauss() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(true);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoStatsLaser() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(true);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoMedals() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(true);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoCommendations() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(true);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoStatsPlayer() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(true);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoNewGame() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(true);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoWaves() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(true);
        ToggleDifficulty(false);
        ToggleMutators(false);
    }

    public void GotoDifficulty() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(true);
        ToggleMutators(false);
    }

    public void GotoMutators() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ToggleHeader(false);
        ToggleStart(false);
        ToggleInputName(false);
        ToggleMain(false);
        ToggleAbout(false);
        ToggleHelp(false);
        ToggleAchievements(false);
        ToggleSettings(false);
        ToggleClear(false);
        ToggleProfile(false);
        ToggleStatsCommand(false);
        ToggleStatsGatling(false);
        ToggleStatsGauss(false);
        ToggleStatsLaser(false);
        ToggleMedals(false);
        ToggleCommendations(false);
        ToggleStatsPlayer(false);
        ToggleNewGame(false);
        ToggleWaves(false);
        ToggleDifficulty(false);
        ToggleMutators(true);
    }

    public void GotoStartGame() {
        loader.SetActive(true);
    }

    public void ClearHelp() {
        itemHelp[currHelp].SetActive(false);
        itemHelp[0].SetActive(true);
        currHelp = 0;
        helpCounter.text = (currHelp+1) + " / " + itemHelp.Length;
    }

    public void NextHelp() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        itemHelp[currHelp].SetActive(false);
        currHelp = (currHelp+1) % itemHelp.Length;
        itemHelp[currHelp].SetActive(true);
        helpCounter.text = (currHelp+1) + " / " + itemHelp.Length;
    }

    public void PrevHelp() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        itemHelp[currHelp].SetActive(false);
        currHelp = (itemHelp.Length+currHelp-1) % itemHelp.Length;
        itemHelp[currHelp].SetActive(true);
        helpCounter.text = (currHelp+1) + " / " + itemHelp.Length;
    }

    public void ClickGenericButton() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
    }

    public void ClickSounds() {
        prefs.ToggleSounds();
        if ( prefs.GetSounds() ) {
            GameObject.Find("ButtonSounds").GetComponent<Image>().sprite = soundsOn;
        } else {
            GameObject.Find("ButtonSounds").GetComponent<Image>().sprite = soundsOff;
        }
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
    }

    public void ClickMusic() {
        prefs.ToggleMusic();
        if ( prefs.GetMusic() ) {
            GameObject.Find("ButtonMusic").GetComponent<Image>().sprite = musicOn;
        } else {
            GameObject.Find("ButtonMusic").GetComponent<Image>().sprite = musicOff;
        }
        MusicPlay();
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
    }

    public void AcceptInput() {
        if ( buttonProfile.transform.GetChild(0).GetComponent<Text>().text != "" ) {
            GotoProfile();
        } else {
            GotoMain();
        }
        windowNameInput.transform.GetChild(2).GetComponent<InputField>().text = "";
    }

    public void CancelInput() {
        if ( prefs.PlayerExists() ) {
            GotoProfile();
        } else {
            GotoStart();
        }
        windowNameInput.transform.GetChild(2).GetComponent<InputField>().text = "";
    }

    public void ShowMedalInfo(int n) {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        if ( n == -1 ) {
            foreach (GameObject mdl in btnMedals ) {
                mdl.GetComponent<Image>().sprite = tableInactive;
            }
            medalTextName.GetComponent<Text>().text = "";
            medalTextInfo.GetComponent<Text>().text = "Click on a medal for details";
            return;
        }
        foreach (GameObject mdl in btnMedals ) {
            mdl.GetComponent<Image>().sprite = tableInactive;
        }
        btnMedals[n].GetComponent<Image>().sprite = tableActive;
        medalTextName.GetComponent<Text>().text = medalName[n];
        medalTextInfo.GetComponent<Text>().text = medalInfo[n];
    }

    public void ShowCommendationInfoNext() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        float xNav = commendationNav.transform.localPosition.x;
        if ( xNav == -188.0f ) {
            ShowCommendationInfo(1);
        } else if ( xNav == -112.0f ) {
            ShowCommendationInfo(2);
        } else if ( xNav == -38.0f ) {
            ShowCommendationInfo(3);
        } else if ( xNav == 38.0f ) {
            ShowCommendationInfo(4);
        } else if ( xNav == 112.0f ) {
            ShowCommendationInfo(5);
        } else if ( xNav == 188.0f ) {
            ShowCommendationInfo(0);
        }
    }

    public void ShowCommendationInfoPrev() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        float xNav = commendationNav.transform.localPosition.x;
        if ( xNav == -188.0f ) {
            ShowCommendationInfo(5);
        } else if ( xNav == -112.0f ) {
            ShowCommendationInfo(0);
        } else if ( xNav == -38.0f ) {
            ShowCommendationInfo(1);
        } else if ( xNav == 38.0f ) {
            ShowCommendationInfo(2);
        } else if ( xNav == 112.0f ) {
            ShowCommendationInfo(3);
        } else if ( xNav == 188.0f ) {
            ShowCommendationInfo(4);
        }
    }

    public void ShowCommendationInfo(int n) {
        if ( n == -1 ) { n = 0; }
        switch ( n ) {
            case 0: commendationNav.transform.localPosition = new Vector3(-188.0f, commendationNav.transform.localPosition.y, commendationNav.transform.localPosition.z); break;
            case 1: commendationNav.transform.localPosition = new Vector3(-112.0f, commendationNav.transform.localPosition.y, commendationNav.transform.localPosition.z); break;
            case 2: commendationNav.transform.localPosition = new Vector3(-38.0f, commendationNav.transform.localPosition.y, commendationNav.transform.localPosition.z); break;
            case 3: commendationNav.transform.localPosition = new Vector3(38.0f, commendationNav.transform.localPosition.y, commendationNav.transform.localPosition.z); break;
            case 4: commendationNav.transform.localPosition = new Vector3(112.0f, commendationNav.transform.localPosition.y, commendationNav.transform.localPosition.z); break;
            case 5: commendationNav.transform.localPosition = new Vector3(188.0f, commendationNav.transform.localPosition.y, commendationNav.transform.localPosition.z); break;
        }
        foreach (GameObject go in commendationPlates) {
            go.SetActive(false);
        }
        commendationPlates[n].SetActive(true);
        commendationTextName.GetComponent<Text>().text = commendationName[n];
        commendationTextInfo.GetComponent<Text>().text = commendationInfo[n];
    }

    public void ShowStatsInfoNext() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        float xNav = statNav.transform.localPosition.x;
        if ( xNav == -100.0f ) {
            ShowStatsInfo(1);
        } else if ( xNav == 0.0f ) {
            ShowStatsInfo(2);
        } else if ( xNav == 100.0f ) {
            ShowStatsInfo(0);
        }
    }

    public void ShowStatsInfoPrev() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        float xNav = statNav.transform.localPosition.x;
        if ( xNav == -100.0f ) {
            ShowStatsInfo(2);
        } else if ( xNav == 0.0f ) {
            ShowStatsInfo(0);
        } else if ( xNav == 100.0f ) {
            ShowStatsInfo(1);
        }
    }

    public void ShowStatsInfo(int n) {
        if ( n == -1 ) { n = 0; }
        switch ( n ) {
            case 0: statNav.transform.localPosition = new Vector3(-100.0f, statNav.transform.localPosition.y, statNav.transform.localPosition.z); break;
            case 1: statNav.transform.localPosition = new Vector3(0.0f, statNav.transform.localPosition.y, statNav.transform.localPosition.z); break;
            case 2: statNav.transform.localPosition = new Vector3(100.0f, statNav.transform.localPosition.y, statNav.transform.localPosition.z); break;
        }
        foreach (GameObject go in statSheets) {
            go.SetActive(false);
        }
        statSheets[n].SetActive(true);
    }

    public void ShowDifficultyInfo(int n) {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        foreach (GameObject bdf in btnDifficulties ) {
            bdf.GetComponent<Image>().sprite = tableInactive;
            bdf.transform.GetChild(1).GetComponent<Image>().sprite = tableInactive;
        }
        btnDifficulties[n].GetComponent<Image>().sprite = tableActive;
        btnDifficulties[n].transform.GetChild(1).GetComponent<Image>().sprite = tableActive;
        difficultyTextInfo.GetComponent<Text>().text = difficultyInfo[n];
    }

    public void ShowMutatorInfo(int n) {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        if ( GameObject.Find("GameManager").GetComponent<GameManager>().IsMutatorActive(n) ) {
            btnMutators[n].GetComponent<Image>().sprite = tableActive;
            btnMutators[n].transform.GetChild(1).GetComponent<Image>().sprite = tableActive;
        } else {
            btnMutators[n].GetComponent<Image>().sprite = tableInactive;
            btnMutators[n].transform.GetChild(1).GetComponent<Image>().sprite = tableInactive;
        }
        mutatorTextName.GetComponent<Text>().text = mutatorsName[n];
        mutatorTextInfo.GetComponent<Text>().text = mutatorsInfo[n];
    }

    public void ClearMutatorInfo() {
        for (int i = 4; i >= 0; --i) { ShowMutatorInfo(i); }
    }

    public void ShowMut0Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut0Locked.SetActive(true);
    }

    public void HideMut0Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut0Locked.SetActive(false);
    }

    public void ShowMut1Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut1Locked.SetActive(true);
    }

    public void HideMut1Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut1Locked.SetActive(false);
    }

    public void ShowMut2Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut2Locked.SetActive(true);
    }

    public void HideMut2Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut2Locked.SetActive(false);
    }

    public void ShowMut4Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut4Locked.SetActive(true);
    }

    public void HideMut4Locked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowMut4Locked.SetActive(false);
    }

    public void ShowDeltaLocked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowDeltaLocked.SetActive(true);
    }

    public void HideDeltaLocked() {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        windowDeltaLocked.SetActive(false);
    }

    public void ShowAchievementInfo(int n) {
        if ( prefs.GetSounds() ) { source.PlayOneShot(menuClick, 1.0f); }
        ClearAchievementInfo();
        btnAchievement[n].GetComponent<Image>().sprite = tableActive;
        achievementTextName.GetComponent<Text>().text = achievementsName[n];
        achievementTextInfo.GetComponent<Text>().text = achievementsInfo[n];
    }

    public void ClearAchievementInfo() {
        for (int i = 0; i < btnAchievement.Length; ++i) {
            btnAchievement[i].GetComponent<Image>().sprite = tableInactive;
        }
        achievementTextName.GetComponent<Text>().text = "";
        achievementTextInfo.GetComponent<Text>().text = "Click on an achievement for details";
    }

    public void MusicPlay() {
        if ( prefs.GetMusic() ) { source.Play(); } else { source.Stop(); }
    }

    private void ToggleHeader(bool flag) {
        header.SetActive(flag);
    }

    private void ToggleStart(bool flag) {
        start.SetActive(flag);
    }

    private void ToggleInputName(bool flag) {
        windowNameInput.SetActive(flag);
    }

    private void ToggleMain(bool flag) {
        buttonPlay.SetActive(flag);
        buttonAbout.SetActive(flag);
        buttonSettings.SetActive(flag);
        buttonProfile.SetActive(flag);
        buttonHelp.SetActive(flag);
        buttonAchievements.SetActive(flag);
    }

    private void ToggleAbout(bool flag) {
        windowAbout.SetActive(flag);
    }

    private void ToggleHelp(bool flag) {
        windowHelp.SetActive(flag);
    }

    private void ToggleAchievements(bool flag) {
        windowAchievements.SetActive(flag);
    }

    private void ToggleSettings(bool flag) {
        windowSettings.SetActive(flag);
    }

    private void ToggleClear(bool flag) {
        windowConfirmClear.SetActive(flag);
    }

    private void ToggleProfile(bool flag) {
        windowProfile.SetActive(flag);
    }

    private void ToggleStatsCommand(bool flag) {
        windowStatsCommand.SetActive(flag);
    }

    private void ToggleStatsGatling(bool flag) {
        windowStatsGatling.SetActive(flag);
    }

    private void ToggleStatsGauss(bool flag) {
        windowStatsGauss.SetActive(flag);
    }

    private void ToggleStatsLaser(bool flag) {
        windowStatsLaser.SetActive(flag);
    }

    private void ToggleMedals(bool flag) {
        windowMedals.SetActive(flag);
    }

    private void ToggleCommendations(bool flag) {
        windowCommendations.SetActive(flag);
    }

    private void ToggleStatsPlayer(bool flag) {
        windowStatsPlayer.SetActive(flag);
    }

    private void ToggleNewGame(bool flag) {
        windowNewGame.SetActive(flag);
    }

    private void ToggleWaves(bool flag) {
        windowWaves.SetActive(flag);
    }

    private void ToggleDifficulty(bool flag) {
        windowDifficulty.SetActive(flag);
    }

    private void ToggleMutators(bool flag) {
        windowMutators.SetActive(flag);
    }
}
