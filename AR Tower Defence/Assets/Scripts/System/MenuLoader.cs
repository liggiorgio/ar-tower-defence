using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour {
    public GameObject loadingBar;
    private Slider slider;

    void Start() {
        slider = loadingBar.GetComponent<Slider>();
        LoadMenu();
    }

    public void LoadMenu() {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously() {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Menu");
        while ( !operation.isDone ) {
            slider.value = operation.progress / 0.9f;
            yield return null;
        }
    }
}
