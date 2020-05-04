using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
    public GameObject menuCanvas;
    public GameObject loadingCanvas;
    public GameObject loadingBar;
    public Text bottomText;
    private Slider slider;

    private string[] msg = {
        "Gauss Turrets are more powerful but rotate slowly, find the right spot to deploy them.",
        "enemy Tanks can't attack directly, but they heal allies as long as their shield is on.",
        "enemy Fighters move at different speeds and deliver different damage.",
        "enemy Bosses are stronger and more aggressive than regular enemies.",
        "when an enemy shield goes down, it will fully recharge after five seconds.",
        "Laser Turrets are as effective as vulnerable, don't deploy them in an exposed area.",
        "the Command Post upgrades automatically while playing, Turrets must be upgraded manually.",
        "Turret Upgrades unlock new capabilities, but their cost increases progressively.",
        "Gatlings are your primer, but they can be reliable in the long run.",
        "destroy enemy Tanks first, so they can't heal other enemies.",
        "the Orbital Strike can be used twice in a row, then it needs time to fully reload.",
        "completing Bonus Waves grants you special items to use during the current game.",
        "the Command Post partially repairs automatically at the end of each wave.",
        "Turrets can be upgraded only if they have full health.",
        "enemy shields can deflect bullets, but leave a unit vulnerable when they go down.",
        "repair your Turrets whenever you can, or they will get destroyed later.",
        "deployment, repairs, and upgrades contribute to both a Turret's and the Command Post's level progression.",
        "enemies become stronger after each Boss Wave.",
        "the higher a Turret's level, the more Turrets of such type can be deployed.",
        "difficulty increases both enemies' health and damage.",
        "difficulty and mutators can increase both score and credits earned.",
        "in space, no one can hear you explode.",
        "Bonus Waves are triggered each three waves, unless it is a Boss Wave.",
        "deploy first, upgrade after you earn enough credits."
    };

    void Start() {
        menuCanvas.SetActive(false);
        loadingCanvas.SetActive(true);
        slider = loadingBar.GetComponent<Slider>();
        bottomText.text = "HINT: " + msg[Random.Range(0, msg.Length)];
        LoadGame();
    }

    public void LoadGame() {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously() {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        while ( !operation.isDone ) {
            slider.value = operation.progress / 0.9f;
            yield return null;
        }
    }
}
