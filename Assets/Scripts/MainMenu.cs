using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject Content;

    //public void Update() {
    //    if (Input.GetKeyDown(KeyCode.Escape)) {
    //        EnableUI(true);
    //    }
    //}

    public void Btn_Start() {
        GameController.activeInstance.SpawnPlayer();
        EnableUI(false);
        InGameUI.activeInstance.scoreText.gameObject.SetActive(true);
        InGameUI.activeInstance.speedText.gameObject.SetActive(true);
        InGameUI.activeInstance.hightscoreText.gameObject.SetActive(false);
    }

    public void Btn_Exit() {
        Application.Quit();
    }

    private void EnableUI(bool _value) {
        Content.SetActive(_value);
    }
}