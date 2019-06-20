using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject[] UI_Elements;

    //public void Update() {
    //    if (Input.GetKeyDown(KeyCode.Escape)) {
    //        EnableUI(true);
    //    }
    //}

    public void Btn_Start() {
        GameController.activeInstance.SpawnPlayer();
        EnableUI(false);
        InGameUI.activeInstance.scoreText.gameObject.SetActive(true);
        InGameUI.activeInstance.hightscoreText.gameObject.SetActive(false);
    }

    public void Btn_Exit() {
        Application.Quit();
    }

    private void EnableUI(bool _value) {
        foreach (GameObject go in UI_Elements) {
            go.SetActive(_value);
        }
    }
}