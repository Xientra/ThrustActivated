using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour {

    public static MainMenu activeInstance;

    public GameObject Content;

    public Slider SensetivitySlider;
    public TextMeshProUGUI SensetivityText;

    void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        SetSensetivityElements();
    }

    public void ShowMainMenu(bool _value) {
        Content.SetActive(_value);
        InGameUI.activeInstance.scoreText.gameObject.SetActive(!_value);
        InGameUI.activeInstance.speedText.gameObject.SetActive(!_value);
        InGameUI.activeInstance.hightscoreText.gameObject.SetActive(_value);
    }

    public void Btn_Start() {
        GameController.activeInstance.SpawnPlayer();
        ShowMainMenu(false);
    }

    public void Btn_Exit() {
        Application.Quit();
    }

    public void SetSensetivityElements() {
        SensetivitySlider.value = GameController.activeInstance.sensetivity;

        string _sens = GameController.activeInstance.sensetivity.ToString();
        int _l = _sens.Length;
        if (_l > 4) _l = 4;

        SensetivityText.text = "Sensetivity: " + _sens.Substring(0, _l);
    }

    public void OnValueChange_SensetivitySlider() {
        GameController.activeInstance.sensetivity = SensetivitySlider.value;
        PlayerPrefs.SetFloat("sensetivity", GameController.activeInstance.sensetivity);
        PlayerPrefs.Save();

        SetSensetivityElements();
    }
}