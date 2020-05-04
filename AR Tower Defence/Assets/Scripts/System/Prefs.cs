using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefs : MonoBehaviour {
    public GameObject profileTitle;
    public GameObject profileRankName;
    public GameObject profileRankProgress;
    public GameObject profileRankImage;
    public Sprite[] rankSprites;
    public GameObject profileXPText;
    public GameObject profileXPProgress;
    public GameObject profilePostText;
    public GameObject profileGatlingText;
    public GameObject profileGaussText;
    public GameObject profileLaserText;
    public GameObject postTierText;
    public GameObject postTierProgress;
    public GameObject postXPText;
    public GameObject postXPProgress;
    public GameObject postUp1Text;
    public GameObject postUp1Progress;
    public GameObject postUp2Text;
    public GameObject postUp2Progress;
    public GameObject postUp3Text;
    public GameObject postUp3Progress;
    public GameObject postUp4Text;
    public GameObject postUp4Progress;
    public GameObject gatlingTierText;
    public GameObject gatlingTierProgress;
    public GameObject gatlingXPText;
    public GameObject gatlingXPProgress;
    public GameObject gatlingAmount;
    public GameObject gatlingUp1Text;
    public GameObject gatlingUp1Progress;
    public GameObject gatlingUp2Text;
    public GameObject gatlingUp2Progress;
    public GameObject gatlingUp3Text;
    public GameObject gatlingUp3Progress;
    public GameObject gatlingUp4Text;
    public GameObject gatlingUp4Progress;
    public GameObject gaussTierText;
    public GameObject gaussTierProgress;
    public GameObject gaussXPText;
    public GameObject gaussXPProgress;
    public GameObject gaussAmount;
    public GameObject gaussUp1Text;
    public GameObject gaussUp1Progress;
    public GameObject gaussUp2Text;
    public GameObject gaussUp2Progress;
    public GameObject gaussUp3Text;
    public GameObject gaussUp3Progress;
    public GameObject gaussUp4Text;
    public GameObject gaussUp4Progress;
    public GameObject laserTierText;
    public GameObject laserTierProgress;
    public GameObject laserXPText;
    public GameObject laserXPProgress;
    public GameObject laserAmount;
    public GameObject laserUp1Text;
    public GameObject laserUp1Progress;
    public GameObject laserUp2Text;
    public GameObject laserUp2Progress;
    public GameObject laserUp3Text;
    public GameObject laserUp3Progress;
    public GameObject laserUp4Text;
    public GameObject laserUp4Progress;
    public GameObject[] medalText;
    public Sprite[] commendationPlateSprites;
    public GameObject[] commendationPlateImage;
    public GameObject[] commendationTierText;
    public GameObject[] commendationProgressText;
    public GameObject[] commendationProgressBar;
    public GameObject[] statsTitles;
    public GameObject[] statsNames;
    public GameObject[] statsValues;
    public GameObject textNewGameInfo;
    public GameObject textWaveStart;
    //private string playerName;
    //private int firstTime;
    private int[] xpPlayerValues = new int[] {
        0, 300, 650, 1050, 1500, 2000, 2550, 3150, 3800, 4500, 5250,
        6050, 6900, 7800, 8750, 9750, 10800, 11900, 13050, 14250, 15500,
        16800, 18150, 19550, 21000, 22500, 24050, 25650, 27300, 29000, 30750,
        32550, 34400, 36300, 38250, 40250, 42300, 44400, 46550, 48750, 55000, 55000
    };
    private int[] xpPostValues = new int[] {
        0, 0, 1200, 2450, 4500, 7900, 10100, 18400, 32800, 32800
    };
    private int[] xpGatlingValues = new int[] {
        0, 0, 500, 2250, 5000, 12000, 12000
    };
    private int[] xpGaussValues = new int[] {
        0, 500, 1200, 3050, 7000, 16000, 16000
    };
    private int[] xpLaserValues = new int[] {
        0, 1200, 2150, 4500, 9250, 17000, 17000
    };
    private int[] statComm0Values = new int[] {
        0, 100, 250, 600, 1500, 4000, 10000, 10000
    };
    private int[] statComm1Values = new int[] {
        0, 30, 70, 160, 350, 800, 2000, 2000
    };
    private int[] statComm2Values = new int[] {
        0, 25, 60, 150, 400, 1000, 2500, 2500
    };
    private int[] statComm3Values = new int[] {
        0, 15, 40, 100, 250, 600, 1500, 1500
    };
    private int[] statComm4Values = new int[] {
        0, 20, 60, 150, 380, 800, 2000, 2000
    };
    private int[] statComm5Values = new int[] {
        0, 15000, 40000, 100000, 220000, 540000, 1200000, 1200000
    };
    private string[] statTitles = { "Career", "Service Record", "Performance" };
    private string[] statNames = {
        "Player since\nGames played\nWaves played\nCareer progress\nRank progress\nUpgrades progress\nTime played\nMoney earned\nMoney spent\nMedals awarded\nCommendations progress",
        "Waves won\nWaves lost\nW/L ratio\nPerfect waves\nTurrets deployed\nTurrets repaired\nTurrets upgraded\nTurrets lost\nEnemies killed (all)\nFighters killed\nBosses killed",
        "Gatling kills\nGauss kills\nLaser kills\nOrbital Strike kills\nBest kill streak (any game)\nMost kills (any game)\nMost earned (any wave)\nMost spent (any wave)\nLongest wave streak\nLongest game"
    };
    private string[] statValues = {
        "01/01/2000\n0\n0\n0%\n0%\n0%\n0s\n0 cR\n0 cR\n0\n0%",
        "0\n0\n0.0\n0\n0\n0\n0\n0\n0\n0\n0",
        "0\n0\n0\n0\n0\n0\n0 cR\n0 cR\n0\n0s"
    };

    void Start() {
        if ( Application.loadedLevelName == "Menu" ) {
            if ( PlayerExists() ) {
                LoadCareer();
            }
            UpdateNewGameInfo();
            UpdateWaveCounter();
        } else if ( Application.loadedLevelName == "Menu" ) {
            // do other stuff
        }
    }

    // Settings & preferences
    public bool GetSounds() {
        return PlayerPrefs.GetInt("sounds", 1) == 1;
    }

    public bool GetMusic() {
        return PlayerPrefs.GetInt("music", 1) == 1;
    }

    public void ToggleSounds() {
        int _snd = ( PlayerPrefs.GetInt("sounds", 1) == 1 ) ? 0 : 1;
        PlayerPrefs.SetInt("sounds", _snd);
    }

    public void ToggleMusic() {
        int _msc = ( PlayerPrefs.GetInt("music", 1) == 1 ) ? 0 : 1;
        PlayerPrefs.SetInt("music", _msc);
    }

    public void Save() {
        PlayerPrefs.Save();
    }

    public void Clear(GameObject textInput) {
        PlayerPrefs.DeleteAll();
        Save();
        textInput.GetComponent<InputField>().text = "";
        GameObject.Find("SystemManager").GetComponent<UIManager>().MusicPlay();
    }

    public void Clear2(GameObject textProfileName) {
        textProfileName.GetComponent<Text>().text = "";
    }

    // Game progress & career
    public void UpdateName(GameObject textInput) {
        string _name = textInput.GetComponent<Text>().text.Trim();
        if ( _name.Length > 0) {
            if ( !PlayerExists() ) {
                InitCareer();
            }
            PlayerPrefs.SetString("plName", _name);
            Save();
            LoadCareer();
        }
    }

    public bool PlayerExists() {
        return PlayerPrefs.GetString("plName") != "";
    }

    public void InitCareer() {
        // Player
        PlayerPrefs.SetInt("plExp", 0);
        PlayerPrefs.SetInt("plPostExp", 0);
        PlayerPrefs.SetInt("plGatlingExp", 0);
        PlayerPrefs.SetInt("plGaussExp", 0);
        PlayerPrefs.SetInt("plLaserExp", 0);
        // Medals
        PlayerPrefs.SetInt("plMedal0", 0);
        PlayerPrefs.SetInt("plMedal1", 0);
        PlayerPrefs.SetInt("plMedal2", 0);
        PlayerPrefs.SetInt("plMedal3", 0);
        PlayerPrefs.SetInt("plMedal4", 0);
        PlayerPrefs.SetInt("plMedal5", 0);
        PlayerPrefs.SetInt("plMedal6", 0);
        PlayerPrefs.SetInt("plMedal7", 0);
        PlayerPrefs.SetInt("plMedal8", 0);
        // Commendations
        PlayerPrefs.SetInt("plComm0", 0);
        PlayerPrefs.SetInt("plComm1", 0);
        PlayerPrefs.SetInt("plComm2", 0);
        PlayerPrefs.SetInt("plComm3", 0);
        PlayerPrefs.SetInt("plComm4", 0);
        PlayerPrefs.SetInt("plComm5", 0);
        // Stats
        PlayerPrefs.SetString("plSince", DateTime.Today.ToString("d"));
        PlayerPrefs.SetInt("plGames", 0);
        PlayerPrefs.SetInt("plGametime", 0);
        PlayerPrefs.SetInt("plMoneySpent", 0);
        PlayerPrefs.SetInt("plWavesFailed", 0);
        PlayerPrefs.SetInt("plBossKilled", 0);
        PlayerPrefs.SetInt("plTurrRepaired", 0);
        PlayerPrefs.SetInt("plTurrUpgraded", 0);
        PlayerPrefs.SetInt("plTurrLost", 0);
        PlayerPrefs.SetInt("plGatlingKills", 0);
        PlayerPrefs.SetInt("plGaussKills", 0);
        PlayerPrefs.SetInt("plLaserKills", 0);
        PlayerPrefs.SetInt("plBestSpree", 0);
        PlayerPrefs.SetInt("plMostKills", 0);
        PlayerPrefs.SetInt("plMostEarned", 0);
        PlayerPrefs.SetInt("plMostSpent", 0);
        PlayerPrefs.SetInt("plLongestWaves", 0);
        PlayerPrefs.SetInt("plLongestTime", 0);
        // High scores
        PlayerPrefs.SetInt("plBest0", 0);
        PlayerPrefs.SetInt("plBest1", 0);
        PlayerPrefs.SetInt("plBest2", 0);
        PlayerPrefs.SetInt("plBest3", 0);
        PlayerPrefs.SetInt("plBest4", 0);
        PlayerPrefs.SetInt("plBest5", 0);
        PlayerPrefs.SetInt("plBest6", 0);
        PlayerPrefs.SetInt("plBest7", 0);
        PlayerPrefs.SetInt("plBest8", 0);
        PlayerPrefs.SetInt("plBest9", 0);
        PlayerPrefs.SetInt("plBest10", 0);
        PlayerPrefs.SetInt("plBest11", 0);
        PlayerPrefs.SetInt("plBest12", 0);
        PlayerPrefs.SetInt("plBest13", 0);
        PlayerPrefs.SetInt("plBest14", 0);
        PlayerPrefs.SetInt("plBest15", 0);
        PlayerPrefs.SetInt("plBest16", 0);
        PlayerPrefs.SetInt("plBest17", 0);
        PlayerPrefs.SetInt("plBest18", 0);
        PlayerPrefs.SetInt("plBest19", 0);
        PlayerPrefs.SetInt("plBest20", 0);
        PlayerPrefs.SetInt("plBest21", 0);
        PlayerPrefs.SetInt("plBest22", 0);
        PlayerPrefs.SetInt("plBest23", 0);
        PlayerPrefs.SetInt("plBest24", 0);
        // Achievements
        PlayerPrefs.SetInt("plChievo0", 0); // Unlockable item, not achievement
        PlayerPrefs.SetInt("plChievo1", 0); // Unlockable item, not achievement
        PlayerPrefs.SetInt("plChievo2", 0); // Unlockable item, not achievement
        PlayerPrefs.SetInt("plChievo3", 0); // Unlockable item, not achievement
        PlayerPrefs.SetInt("plChievo4", 0); // Unlockable item, not achievement
        PlayerPrefs.SetInt("plChievo5", 0);
        PlayerPrefs.SetInt("plChievo6", 0);
        PlayerPrefs.SetInt("plChievo7", 0);
        PlayerPrefs.SetInt("plChievo8", 0);
        PlayerPrefs.SetInt("plChievo9", 0);
        PlayerPrefs.SetInt("plChievo10", 0);
        PlayerPrefs.SetInt("plChievo11", 0);
        PlayerPrefs.SetInt("plChievo12", 0);
        PlayerPrefs.SetInt("plEngaged", 0);
    }

    public void UpdateCareer() {
        // update career values first
        // save them after
        // load them in menu finally
    }

    private void LoadCareer() {
        // Player
        int _exp = PlayerPrefs.GetInt("plExp");
        int _level = ComputePlayerLevel(_exp);
        int _xpMin = ComputePlayerMinXP(_level);
        int _xpMax = ComputePlayerMaxXP(_level);
        profileTitle.GetComponent<Text>().text = PlayerPrefs.GetString("plName", "<missing>");
        profileRankName.GetComponent<Text>().text = ComputeRankName(_level);
        profileRankProgress.GetComponent<Slider>().value = _level + 1;
        profileRankImage.GetComponent<Image>().sprite = ComputeRankImage(_level);
        profileXPText.GetComponent<Text>().text = _exp + " / " + _xpMax;
        profileXPProgress.GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        if ( _xpMin == _xpMax ) { profileXPProgress.GetComponent<Slider>().value = 1; };
        string _strCareer = ((((float)Mathf.Min(_exp, ComputePlayerMaxXP(40))) / ComputePlayerMaxXP(40))*100).ToString("##0.##") + "%";
        string _strRank = (_level + 1) + "/40";
        // Command Post
        int _intUpgrades = 0;
        _exp = PlayerPrefs.GetInt("plPostExp");
        _level = ComputePostLevel(_exp);
        _xpMin = ComputePostMinXP(_level);
        _xpMax = ComputePostMaxXP(_level);
        _intUpgrades += Mathf.Min(_exp, ComputePostMinXP(8));
        profilePostText.GetComponent<Text>().text = Int2Roman(_level);
        postTierText.GetComponent<Text>().text = "Level " + Int2Roman(_level);
        postTierProgress.GetComponent<Slider>().value = _level;
        postXPText.GetComponent<Text>().text = _exp + " / " + _xpMax;
        postXPProgress.GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        if ( _xpMin == _xpMax ) { postXPProgress.GetComponent<Slider>().value = 1; };
        if ( _level >= 2 ) { postUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { postUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 4 ) { postUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { postUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 6 ) { postUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { postUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level == 8 ) { postUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { postUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        postUp1Progress.GetComponent<Slider>().value = ((float) _exp) / ComputePostMinXP(2);
        postUp2Progress.GetComponent<Slider>().value = ((float) _exp) / ComputePostMinXP(4);
        postUp3Progress.GetComponent<Slider>().value = ((float) _exp) / ComputePostMinXP(6);
        postUp4Progress.GetComponent<Slider>().value = ((float) _exp) / ComputePostMinXP(8);
        // Gatling
        _exp = PlayerPrefs.GetInt("plGatlingExp");
        _level = ComputeGatlingLevel(_exp);
        profileGatlingText.GetComponent<Text>().text = Int2Roman(_level);
        _xpMin = ComputeGatlingMinXP(_level);
        _xpMax = ComputeGatlingMaxXP(_level);
        if ( _level == 0 ) {
            gatlingTierText.GetComponent<Text>().text = "Locked";
        } else {
            gatlingTierText.GetComponent<Text>().text = "Level " + Int2Roman(_level);
        }
        gatlingTierProgress.GetComponent<Slider>().value = _level;
        gatlingXPText.GetComponent<Text>().text = _exp + " / " + _xpMax;
        gatlingXPProgress.GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        if ( _xpMin == _xpMax ) { gatlingXPProgress.GetComponent<Slider>().value = 1; };
        gatlingAmount.GetComponent<Text>().text = ComputeGatlingAmount(_level).ToString();
        if ( _level >= 2 ) { gatlingUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gatlingUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 3 ) { gatlingUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gatlingUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 4 ) { gatlingUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gatlingUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level == 5 ) { gatlingUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gatlingUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        gatlingUp1Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGatlingMinXP(2);
        gatlingUp2Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGatlingMinXP(3);
        gatlingUp3Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGatlingMinXP(4);
        gatlingUp4Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGatlingMinXP(5);
        _intUpgrades += Mathf.Min(_exp, ComputeGatlingMinXP(5));
        // Gauss
        _exp = PlayerPrefs.GetInt("plGaussExp");
        _level = ComputeGaussLevel(_exp);
        profileGaussText.GetComponent<Text>().text = Int2Roman(_level);
        _xpMin = ComputeGaussMinXP(_level);
        _xpMax = ComputeGaussMaxXP(_level);
        if ( _level == 0 ) {
            gaussTierText.GetComponent<Text>().text = "Locked";
        } else {
            gaussTierText.GetComponent<Text>().text = "Level " + Int2Roman(_level);
        }
        gaussTierProgress.GetComponent<Slider>().value = _level;
        gaussXPText.GetComponent<Text>().text = _exp + " / " + _xpMax;
        gaussXPProgress.GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        if ( _xpMin == _xpMax ) { gaussXPProgress.GetComponent<Slider>().value = 1; };
        gaussAmount.GetComponent<Text>().text = ComputeGaussAmount(_level).ToString();
        if ( _level >= 2 ) { gaussUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gaussUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 3 ) { gaussUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gaussUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 4 ) { gaussUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gaussUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level == 5 ) { gaussUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { gaussUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        gaussUp1Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGaussMinXP(2);
        gaussUp2Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGaussMinXP(3);
        gaussUp3Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGaussMinXP(4);
        gaussUp4Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeGaussMinXP(5);
        _intUpgrades += Mathf.Min(_exp, ComputeGaussMinXP(5));
        // Laser
        _exp = PlayerPrefs.GetInt("plLaserExp");
        _level = ComputeLaserLevel(_exp);
        profileLaserText.GetComponent<Text>().text = Int2Roman(_level);
        _xpMin = ComputeLaserMinXP(_level);
        _xpMax = ComputeLaserMaxXP(_level);
        if ( _level == 0 ) {
            laserTierText.GetComponent<Text>().text = "Locked";
        } else {
            laserTierText.GetComponent<Text>().text = "Level " + Int2Roman(_level);
        }
        laserTierProgress.GetComponent<Slider>().value = _level;
        laserXPText.GetComponent<Text>().text = _exp + " / " + _xpMax;
        laserXPProgress.GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        if ( _xpMin == _xpMax ) { laserXPProgress.GetComponent<Slider>().value = 1; };
        laserAmount.GetComponent<Text>().text = ComputeLaserAmount(_level).ToString();
        if ( _level >= 2 ) { laserUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { laserUp1Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 3 ) { laserUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { laserUp2Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level >= 4 ) { laserUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { laserUp3Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        if ( _level == 5 ) { laserUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 1f); } else { laserUp4Text.GetComponent<Text>().color = new Color(0.8941177f, 0.9647059f, 0.9607843f, 0.5f); }
        laserUp1Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeLaserMinXP(2);
        laserUp2Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeLaserMinXP(3);
        laserUp3Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeLaserMinXP(4);
        laserUp4Progress.GetComponent<Slider>().value = ((float) _exp) / ComputeLaserMinXP(5);
        _intUpgrades += Mathf.Min(_exp, ComputeLaserMinXP(5));
        string _strUpgrades = ((((float) _intUpgrades) / (ComputePostMaxXP(8) + ComputeGatlingMaxXP(5) + ComputeGaussMaxXP(5) + ComputeLaserMaxXP(5)))*100).ToString("##0.##") + "%";
        // Medals
        int _intMedals = 0;
        for (int i = 0; i < medalText.Length; ++i) {
            _intMedals += PlayerPrefs.GetInt("plMedal" + i);
            medalText[i].GetComponent<Text>().text = PlayerPrefs.GetInt("plMedal" + i).ToString();
        }
        // Commendations
        int _intComm = 0;
        _exp = PlayerPrefs.GetInt("plComm0");
        _intComm += Mathf.Min(_exp, ComputeComm0MaxXP(6));
        _level = ComputeComm0Level(_exp);
        commendationTierText[0].GetComponent<Text>().text = Int2Tier(_level);
        commendationPlateImage[0].GetComponent<Image>().sprite = Int2Image(_level);
        if ( _level == 0 ) {
            commendationPlateImage[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            commendationPlateImage[0].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        } else {
            commendationPlateImage[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            commendationPlateImage[0].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        _xpMin = ComputeComm0MinXP(_level);
        _xpMax = ComputeComm0MaxXP(_level);
        commendationProgressText[0].GetComponent<Text>().text = _exp + " / " + _xpMax;
        commendationProgressBar[0].GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        _exp = PlayerPrefs.GetInt("plComm1");
        _intComm += Mathf.Min(_exp, ComputeComm1MaxXP(6));
        _level = ComputeComm1Level(_exp);
        commendationTierText[1].GetComponent<Text>().text = Int2Tier(_level);
        commendationPlateImage[1].GetComponent<Image>().sprite = Int2Image(_level);
        if ( _level == 0 ) {
            commendationPlateImage[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            commendationPlateImage[1].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        } else {
            commendationPlateImage[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            commendationPlateImage[1].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        _xpMin = ComputeComm1MinXP(_level);
        _xpMax = ComputeComm1MaxXP(_level);
        commendationProgressText[1].GetComponent<Text>().text = _exp + " / " + _xpMax;
        commendationProgressBar[1].GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        _exp = PlayerPrefs.GetInt("plComm2");
        _intComm += Mathf.Min(_exp, ComputeComm2MaxXP(6));
        _level = ComputeComm2Level(_exp);
        commendationTierText[2].GetComponent<Text>().text = Int2Tier(_level);
        commendationPlateImage[2].GetComponent<Image>().sprite = Int2Image(_level);
        if ( _level == 0 ) {
            commendationPlateImage[2].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            commendationPlateImage[2].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        } else {
            commendationPlateImage[2].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            commendationPlateImage[2].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        _xpMin = ComputeComm2MinXP(_level);
        _xpMax = ComputeComm2MaxXP(_level);
        commendationProgressText[2].GetComponent<Text>().text = _exp + " / " + _xpMax;
        commendationProgressBar[2].GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        _exp = PlayerPrefs.GetInt("plComm3");
        _intComm += Mathf.Min(_exp, ComputeComm3MaxXP(6));
        _level = ComputeComm3Level(_exp);
        commendationTierText[3].GetComponent<Text>().text = Int2Tier(_level);
        commendationPlateImage[3].GetComponent<Image>().sprite = Int2Image(_level);
        if ( _level == 0 ) {
            commendationPlateImage[3].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            commendationPlateImage[3].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        } else {
            commendationPlateImage[3].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            commendationPlateImage[3].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        _xpMin = ComputeComm3MinXP(_level);
        _xpMax = ComputeComm3MaxXP(_level);
        commendationProgressText[3].GetComponent<Text>().text = _exp + " / " + _xpMax;
        commendationProgressBar[3].GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        _exp = PlayerPrefs.GetInt("plComm4");
        _intComm += Mathf.Min(_exp, ComputeComm4MaxXP(6));
        _level = ComputeComm4Level(_exp);
        commendationTierText[4].GetComponent<Text>().text = Int2Tier(_level);
        commendationPlateImage[4].GetComponent<Image>().sprite = Int2Image(_level);
        if ( _level == 0 ) {
            commendationPlateImage[4].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            commendationPlateImage[4].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        } else {
            commendationPlateImage[4].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            commendationPlateImage[4].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        _xpMin = ComputeComm4MinXP(_level);
        _xpMax = ComputeComm4MaxXP(_level);
        commendationProgressText[4].GetComponent<Text>().text = _exp + " / " + _xpMax;
        commendationProgressBar[4].GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        _exp = PlayerPrefs.GetInt("plComm5");
        _intComm += Mathf.Min(_exp, ComputeComm5MaxXP(6));
        _level = ComputeComm5Level(_exp);
        commendationTierText[5].GetComponent<Text>().text = Int2Tier(_level);
        commendationPlateImage[5].GetComponent<Image>().sprite = Int2Image(_level);
        if ( _level == 0 ) {
            commendationPlateImage[5].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            commendationPlateImage[5].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        } else {
            commendationPlateImage[5].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            commendationPlateImage[5].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        _xpMin = ComputeComm5MinXP(_level);
        _xpMax = ComputeComm5MaxXP(_level);
        commendationProgressText[5].GetComponent<Text>().text = _exp + " / " + _xpMax;
        commendationProgressBar[5].GetComponent<Slider>().value = ((float)(_exp - _xpMin)) / (_xpMax - _xpMin);
        string _strComm = ((((float) _intComm) / (ComputeComm0MaxXP(6) + ComputeComm1MaxXP(6) + ComputeComm2MaxXP(6) + ComputeComm3MaxXP(6) + ComputeComm4MaxXP(6) + ComputeComm5MaxXP(6)))*100).ToString("##0.##") + "%";
        // Other stats
        string newVal = PlayerPrefs.GetString("plSince") + "\n";
        newVal += PlayerPrefs.GetInt("plGames") + "\n" + (PlayerPrefs.GetInt("plComm2") + PlayerPrefs.GetInt("plWavesFailed")) + "\n" +
                _strCareer + "\n" + _strRank + "\n" + _strUpgrades + "\n";
        int _time = PlayerPrefs.GetInt("plGametime");
        int _s = _time % 60;
        _time /= 60;
        int _m = _time % 60;
        _time /= 60;
        int _h = _time % 24;
        _time /= 24;
        int _d = _time;
        string _strTime = "";
        if ( _d > 0 ) { _strTime += _d + "d "; }
        if ( _h > 0 ) { _strTime += _h + "h "; }
        if ( _m > 0 ) { _strTime += _m + "m "; }
        if ( (_s > 0) || (_strTime == "") ) { _strTime += _s + "s"; }
        _strTime = _strTime.Trim();
        newVal += _strTime + "\n" + PlayerPrefs.GetInt("plComm5") + " cR\n" + PlayerPrefs.GetInt("plMoneySpent") + " cR\n" + _intMedals + "\n" + _strComm;
        statValues[0] = newVal;
        newVal = PlayerPrefs.GetInt("plComm2") + "\n" + PlayerPrefs.GetInt("plWavesFailed") + "\n";
        float _ratio;
        if ( PlayerPrefs.GetInt("plWavesFailed") == 0 ) { _ratio = PlayerPrefs.GetInt("plComm2"); } else { _ratio = ((float) (PlayerPrefs.GetInt("plComm2"))) / PlayerPrefs.GetInt("plWavesFailed"); }
        newVal += _ratio.ToString("####0.###") + "\n" + PlayerPrefs.GetInt("plComm3") + "\n" + PlayerPrefs.GetInt("plComm1") + "\n" + PlayerPrefs.GetInt("plTurrRepaired") + "\n" +
                PlayerPrefs.GetInt("plTurrUpgraded") + "\n" + PlayerPrefs.GetInt("plTurrLost") + "\n" + PlayerPrefs.GetInt("plComm0") + "\n" + (PlayerPrefs.GetInt("plComm0")-PlayerPrefs.GetInt("plBossKilled")) + "\n" + PlayerPrefs.GetInt("plBossKilled");
        statValues[1] = newVal;
        newVal = PlayerPrefs.GetInt("plGatlingKills") + "\n" + PlayerPrefs.GetInt("plGaussKills") + "\n" + PlayerPrefs.GetInt("plLaserKills") + "\n" +
                PlayerPrefs.GetInt("plComm4") + "\n" + PlayerPrefs.GetInt("plBestSpree") + "\n" + PlayerPrefs.GetInt("plMostKills") + "\n" +
                PlayerPrefs.GetInt("plMostEarned") + " cR\n" + PlayerPrefs.GetInt("plMostSpent") + " cR\n" + PlayerPrefs.GetInt("plLongestWaves") + "\n";
        _time = PlayerPrefs.GetInt("plLongestTime");
        _s = _time % 60;
        _time /= 60;
        _m = _time % 60;
        _time /= 60;
        _h = _time % 24;
        _time /= 24;
        _d = _time;
        _strTime = "";
        if ( _d > 0 ) { _strTime += _d + "d "; }
        if ( _h > 0 ) { _strTime += _h + "h "; }
        if ( _m > 0 ) { _strTime += _m + "m "; }
        if ( (_s > 0) || (_strTime == "") ) { _strTime += _s + "s"; }
        _strTime = _strTime.Trim();
        newVal += _strTime;
        statValues[2] = newVal;
        for (int i = 0; i < 3; ++i) {
            statsTitles[i].GetComponent<Text>().text = statTitles[i];
            statsNames[i].GetComponent<Text>().text = statNames[i];
            statsValues[i].GetComponent<Text>().text = statValues[i];
        }
        // New game options
        UpdateWaveCounter();
        UpdateNewGameInfo();
    }

    public int ComputePlayerLevel(int xp) {
        int _level = 1;
        while ( xp >= xpPlayerValues[_level] ) { _level++; if (_level == xpPlayerValues.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputePlayerMinXP(int lvl) {
        return xpPlayerValues[lvl];
    }

    public int ComputePlayerMaxXP(int lvl) {
        return xpPlayerValues[lvl + 1];
    }

    public string ComputeRankName(int lvl) {
        switch ( lvl ) {
            case 0: return "Recruit"; break;
            case 1: return "Apprentice"; break;
            case 2: return "Apprentice\nGrade 1"; break;
            case 3: return "Private"; break;
            case 4: return "Corporal"; break;
            case 5: return "Corporal\nGrade 1"; break;
            case 6: return "Sergeant"; break;
            case 7: return "Sergeant\nGrade 1"; break;
            case 8: return "Sergeant\nGrade 2"; break;
            case 9: return "Gunnery Sergeant"; break;
            case 10: return "Gunnery Sergeant\nGrade 1"; break;
            case 11: return "Gunnery Sergeant\nGrade 2"; break;
            case 12: return "Gunnery Sergeant\nGrade 3"; break;
            case 13: return "Lieutenant"; break;
            case 14: return "Lieutenant\nGrade 1"; break;
            case 15: return "Lieutenant\nGrade 2"; break;
            case 16: return "Lieutenant\nGrade 3"; break;
            case 17: return "Captain"; break;
            case 18: return "Captain\nGrade 1"; break;
            case 19: return "Captain\nGrade 2"; break;
            case 20: return "Captain\nGrade 3"; break;
            case 21: return "Major"; break;
            case 22: return "Major\nGrade 1"; break;
            case 23: return "Major\nGrade 2"; break;
            case 24: return "Major\nGrade 3"; break;
            case 25: return "Commander"; break;
            case 26: return "Commander\nGrade 1"; break;
            case 27: return "Commander\nGrade 2"; break;
            case 28: return "Commander\nGrade 3"; break;
            case 29: return "Colonel"; break;
            case 30: return "Colonel\nGrade 1"; break;
            case 31: return "Colonel\nGrade 2"; break;
            case 32: return "Colonel\nGrade 3"; break;
            case 33: return "Brigadier"; break;
            case 34: return "Brigadier\nGrade 1"; break;
            case 35: return "Brigadier\nGrade 2"; break;
            case 36: return "Brigadier\nGrade 3"; break;
            case 37: return "General"; break;
            case 38: return "General\nGrade 1"; break;
            case 39: return "Fleet General"; break;
            default: return "<missing>"; break;
        }
    }

    public string ComputeRankName2(int lvl) {
        switch ( lvl ) {
            case 0: return "Recruit"; break;
            case 1: return "Apprentice"; break;
            case 2: return "Apprentice Grade 1"; break;
            case 3: return "Private"; break;
            case 4: return "Corporal"; break;
            case 5: return "Corporal Grade 1"; break;
            case 6: return "Sergeant"; break;
            case 7: return "Sergeant Grade 1"; break;
            case 8: return "Sergeant Grade 2"; break;
            case 9: return "Gunnery Sergeant"; break;
            case 10: return "Gunnery Sergeant Grade 1"; break;
            case 11: return "Gunnery Sergeant Grade 2"; break;
            case 12: return "Gunnery Sergeant Grade 3"; break;
            case 13: return "Lieutenant"; break;
            case 14: return "Lieutenant Grade 1"; break;
            case 15: return "Lieutenant Grade 2"; break;
            case 16: return "Lieutenant Grade 3"; break;
            case 17: return "Captain"; break;
            case 18: return "Captain Grade 1"; break;
            case 19: return "Captain Grade 2"; break;
            case 20: return "Captain Grade 3"; break;
            case 21: return "Major"; break;
            case 22: return "Major Grade 1"; break;
            case 23: return "Major Grade 2"; break;
            case 24: return "Major Grade 3"; break;
            case 25: return "Commander"; break;
            case 26: return "Commander Grade 1"; break;
            case 27: return "Commander Grade 2"; break;
            case 28: return "Commander Grade 3"; break;
            case 29: return "Colonel"; break;
            case 30: return "Colonel Grade 1"; break;
            case 31: return "Colonel Grade 2"; break;
            case 32: return "Colonel Grade 3"; break;
            case 33: return "Brigadier"; break;
            case 34: return "Brigadier Grade 1"; break;
            case 35: return "Brigadier Grade 2"; break;
            case 36: return "Brigadier Grade 3"; break;
            case 37: return "General"; break;
            case 38: return "General Grade 1"; break;
            case 39: return "Fleet General"; break;
            default: return "<missing>"; break;
        }
    }

    public Sprite ComputeRankImage(int lvl) {
        return rankSprites[lvl];
    }

    public string Int2Roman(int num) {
        switch ( num ) {
            case 1: return "I"; break;
            case 2: return "II"; break;
            case 3: return "III"; break;
            case 4: return "IV"; break;
            case 5: return "V"; break;
            case 6: return "VI"; break;
            case 7: return "VII"; break;
            case 8: return "VIII"; break;
            default: return "-";
        }
    }

    public int ComputePostLevel(int xp) {
        int _level = 0;
        while ( xp >= xpPostValues[_level] ) { _level++; if (_level == xpPostValues.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputePostMinXP(int lvl) {
        return xpPostValues[lvl];
    }

    public int ComputePostMaxXP(int lvl) {
        return xpPostValues[lvl + 1];
    }

    public int ComputeGatlingLevel(int xp) {
        int _level = 0;
        while ( xp >= xpGatlingValues[_level] ) { _level++; if (_level == xpGatlingValues.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeGatlingAmount(int lvl) {
        switch ( lvl ) {
            case 4: return 7; break;
            case 5: return 9; break;
            default: return 5;
        }
    }

    public int ComputeGatlingMinXP(int lvl) {
        return xpGatlingValues[lvl];
    }

    public int ComputeGatlingMaxXP(int lvl) {
        return xpGatlingValues[lvl + 1];
    }

    public int ComputeGaussLevel(int xp) {
        int _level = 0;
        while ( xp >= xpGaussValues[_level] ) { _level++; if (_level == xpGaussValues.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeGaussAmount(int lvl) {
        switch ( lvl ) {
            case 1: return 3; break;
            case 2: return 4; break;
            case 3: return 5; break;
            case 4: return 6; break;
            case 5: return 7; break;
            default: return 0;
        }
    }

    public int ComputeGaussMinXP(int lvl) {
        return xpGaussValues[lvl];
    }

    public int ComputeGaussMaxXP(int lvl) {
        return xpGaussValues[lvl + 1];
    }

    public int ComputeLaserLevel(int xp) {
        int _level = 0;
        while ( xp >= xpLaserValues[_level] ) { _level++; if (_level == xpLaserValues.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeLaserAmount(int lvl) {
        switch ( lvl ) {
            case 1: return 1; break;
            case 2: return 2; break;
            case 3: return 3; break;
            case 4: return 4; break;
            case 5: return 5; break;
            default: return 0;
        }
    }

    public int ComputeLaserMinXP(int lvl) {
        return xpLaserValues[lvl];
    }

    public int ComputeLaserMaxXP(int lvl) {
        return xpLaserValues[lvl + 1];
    }

    private string Int2Tier(int num) {
        switch ( num ) {
            case 0: return "Locked"; break;
            case 1: return "Iron"; break;
            case 2: return "Bronze"; break;
            case 3: return "Silver"; break;
            case 4: return "Gold"; break;
            case 5: return "Onyx"; break;
            default: return "Onyx (MAX)"; break;
        }
    }

    private Sprite Int2Image(int num) {
        num = Mathf.Clamp(num, 0, 6);
        return commendationPlateSprites[num];
    }

    public int ComputeComm0Level(int xp) {
        int _level = 0;
        while ( xp >= statComm0Values[_level] ) { _level++; if (_level == statComm0Values.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeComm0MinXP(int lvl) {
        return statComm0Values[lvl];
    }

    public int ComputeComm0MaxXP(int lvl) {
        return statComm0Values[lvl + 1];
    }

    public int ComputeComm1Level(int xp) {
        int _level = 0;
        while ( xp >= statComm1Values[_level] ) { _level++; if (_level == statComm1Values.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeComm1MinXP(int lvl) {
        return statComm1Values[lvl];
    }

    public int ComputeComm1MaxXP(int lvl) {
        return statComm1Values[lvl + 1];
    }

    public int ComputeComm2Level(int xp) {
        int _level = 0;
        while ( xp >= statComm2Values[_level] ) { _level++; if (_level == statComm2Values.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeComm2MinXP(int lvl) {
        return statComm2Values[lvl];
    }

    public int ComputeComm2MaxXP(int lvl) {
        return statComm2Values[lvl + 1];
    }

    public int ComputeComm3Level(int xp) {
        int _level = 0;
        while ( xp >= statComm3Values[_level] ) { _level++; if (_level == statComm3Values.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeComm3MinXP(int lvl) {
        return statComm3Values[lvl];
    }

    public int ComputeComm3MaxXP(int lvl) {
        return statComm3Values[lvl + 1];
    }

    public int ComputeComm4Level(int xp) {
        int _level = 0;
        while ( xp >= statComm4Values[_level] ) { _level++; if (_level == statComm4Values.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeComm4MinXP(int lvl) {
        return statComm4Values[lvl];
    }

    public int ComputeComm4MaxXP(int lvl) {
        return statComm4Values[lvl + 1];
    }

    public int ComputeComm5Level(int xp) {
        int _level = 0;
        while ( xp >= statComm5Values[_level] ) { _level++; if (_level == statComm5Values.Length - 1) { return _level - 1; } }
        return _level - 1;
    }

    public int ComputeComm5MinXP(int lvl) {
        return statComm5Values[lvl];
    }

    public int ComputeComm5MaxXP(int lvl) {
        return statComm5Values[lvl + 1];
    }

    public void UpdateNewGameInfo() {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        int _ws = gm.GetWaveStart();
        string _str = (_ws+1) + "\n" + gm.GetHighScore(_ws) + "\n" + gm.GetDifficultyName() + "\n" + gm.GetMutatorsName();
        textNewGameInfo.GetComponent<Text>().text = _str;
    }

    public void UpdateWaveCounter() {
        string _wave = (GameObject.Find("GameManager").GetComponent<GameManager>().GetWaveStart() + 1).ToString();
        textWaveStart.GetComponent<Text>().text = _wave;
    }

    // In-game progress functions
    public void AddPlayerXp(int amount) {
        PlayerPrefs.SetInt("plExp", PlayerPrefs.GetInt("plExp") + amount);
    }

    public void AddPostXp(int amount) {
        PlayerPrefs.SetInt("plPostExp", PlayerPrefs.GetInt("plPostExp") + amount);
    }

    public void AddGatlingXp(int amount) {
        PlayerPrefs.SetInt("plGatlingExp", PlayerPrefs.GetInt("plGatlingExp") + amount);
        int _exp = PlayerPrefs.GetInt("plGaussExp");
        int _lvl = ComputeGaussLevel(_exp);
        if ( _lvl == 0 ) {
            float gVal = Mathf.Min(xpGaussValues[1], ((float) PlayerPrefs.GetInt("plGatlingExp")) / xpGatlingValues[2] * xpGaussValues[1]);
            PlayerPrefs.SetInt("plGaussExp", (int) gVal);
        }
    }

    public void AddGaussXp(int amount) {
        PlayerPrefs.SetInt("plGaussExp", PlayerPrefs.GetInt("plGaussExp") + amount);
        int _exp = PlayerPrefs.GetInt("plGaussExp");
        int _lvl = ComputeGaussLevel(_exp);
        if ( _lvl > 0 ) {
            _exp = PlayerPrefs.GetInt("plLaserExp");
            _lvl = ComputeLaserLevel(_exp);
            if ( _lvl == 0 ) {
                float lVal = Mathf.Min(xpLaserValues[1], ((float) PlayerPrefs.GetInt("plGaussExp") - xpGaussValues[1]) / (xpGaussValues[2] - xpGaussValues[1]) * xpLaserValues[1]);
                PlayerPrefs.SetInt("plLaserExp", (int) lVal);
            }
        }
    }

    public void AddLaserXp(int amount) {
        PlayerPrefs.SetInt("plLaserExp", PlayerPrefs.GetInt("plLaserExp") + amount);
    }

    public void AddGame() {
        PlayerPrefs.SetInt("plGames", PlayerPrefs.GetInt("plGames") + 1);
    }

    public void AddWaveWon() {
        PlayerPrefs.SetInt("plComm2", PlayerPrefs.GetInt("plComm2") + 1);
    }

    public void AddWavePerfect() {
        PlayerPrefs.SetInt("plComm3", PlayerPrefs.GetInt("plComm3") + 1);
    }

    public void AddWaveLost() {
        PlayerPrefs.SetInt("plWavesFailed", PlayerPrefs.GetInt("plWavesFailed") + 1);
    }

    public void AddGameTime(int time) {
        PlayerPrefs.SetInt("plGametime", PlayerPrefs.GetInt("plGametime") + time);
    }

    public void AddTurretsBuild(int amount) {
        PlayerPrefs.SetInt("plComm1", PlayerPrefs.GetInt("plComm1") + amount);
    }

    public void AddTurretsRepaired(int amount) {
        PlayerPrefs.SetInt("plTurrRepaired", PlayerPrefs.GetInt("plTurrRepaired") + amount);
    }

    public void AddTurretsUpgraded(int amount) {
        PlayerPrefs.SetInt("plTurrUpgraded", PlayerPrefs.GetInt("plTurrUpgraded") + amount);
    }

    public void AddTurretsLost(int amount) {
        PlayerPrefs.SetInt("plTurrLost", PlayerPrefs.GetInt("plTurrLost") + amount);
    }

    public void AddKills(int kills, int bosses, int gatling, int gauss, int laser, int orbital) {
        PlayerPrefs.SetInt("plComm0", PlayerPrefs.GetInt("plComm0") + kills);
        PlayerPrefs.SetInt("plBossKilled", PlayerPrefs.GetInt("plBossKilled") + bosses);
        PlayerPrefs.SetInt("plGatlingKills", PlayerPrefs.GetInt("plGatlingKills") + gatling);
        PlayerPrefs.SetInt("plGaussKills", PlayerPrefs.GetInt("plGaussKills") + gauss);
        PlayerPrefs.SetInt("plLaserKills", PlayerPrefs.GetInt("plLaserKills") + laser);
        PlayerPrefs.SetInt("plComm4", PlayerPrefs.GetInt("plComm4") + orbital);
    }

    public void AddMoneyEarned(int amount) {
        PlayerPrefs.SetInt("plComm5", PlayerPrefs.GetInt("plComm5") + amount);
        // Check most earned
        if ( amount > PlayerPrefs.GetInt("plMostEarned") ) {
            PlayerPrefs.SetInt("plMostEarned", amount);
        }
    }

    public void AddMoneySpent(int amount) {
        PlayerPrefs.SetInt("plMoneySpent", PlayerPrefs.GetInt("plMoneySpent") + amount);
        // Check most spent
        if ( amount > PlayerPrefs.GetInt("plMostSpent") ) {
            PlayerPrefs.SetInt("plMostSpent", amount);
        }
    }

    public void AddMedals(int m0, int m1, int m2, int m3, int m4, int m5, int m6, int m7, int m8) {
        PlayerPrefs.SetInt("plMedal0", PlayerPrefs.GetInt("plMedal0") + m0);
        PlayerPrefs.SetInt("plMedal1", PlayerPrefs.GetInt("plMedal1") + m1);
        PlayerPrefs.SetInt("plMedal2", PlayerPrefs.GetInt("plMedal2") + m2);
        PlayerPrefs.SetInt("plMedal3", PlayerPrefs.GetInt("plMedal3") + m3);
        PlayerPrefs.SetInt("plMedal4", PlayerPrefs.GetInt("plMedal4") + m4);
        PlayerPrefs.SetInt("plMedal5", PlayerPrefs.GetInt("plMedal5") + m5);
        PlayerPrefs.SetInt("plMedal6", PlayerPrefs.GetInt("plMedal6") + m6);
        PlayerPrefs.SetInt("plMedal7", PlayerPrefs.GetInt("plMedal7") + m7);
        PlayerPrefs.SetInt("plMedal8", PlayerPrefs.GetInt("plMedal8") + m8);
    }

    public void CheckNewHighscore(int wave, int score) {
        if ( score > PlayerPrefs.GetInt("plBest" + wave) ) {
            PlayerPrefs.SetInt("plBest" + wave, score);
        }
    }

    public void CheckMostKills(int kills) {
        if ( kills > PlayerPrefs.GetInt("plMostKills") ) {
            PlayerPrefs.SetInt("plMostKills", kills);
        }
    }

    public void CheckBestSpree(int spree) {
        if ( spree > PlayerPrefs.GetInt("plBestSpree") ) {
            PlayerPrefs.SetInt("plBestSpree", spree);
        }
    }

    public void CheckLongestWaves(int streak) {
        if ( streak > PlayerPrefs.GetInt("plLongestWaves") ) {
            PlayerPrefs.SetInt("plLongestWaves", streak);
        }
    }

    public void CheckLongestGametime(int time) {
        if ( time > PlayerPrefs.GetInt("plLongestTime") ) {
            PlayerPrefs.SetInt("plLongestTime", time);
        }
    }
}
