using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour {

    public static MainMenu activeInstance;

    public GameObject Content;

    public Slider SensetivitySlider;
    public TextMeshProUGUI SensetivityText;
    public Toggle StartAtSunsetToggle;
    public Toggle MusicToggle;
    private const float HIGHTSCORE_FOR_SUNSET_START = 10000;
    private const float SUNSET_ROTATION = 110f;

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
        ShowMainMenu(true);
    }

    public void ShowMainMenu(bool _value) {
        Content.SetActive(_value);
        InGameUI.activeInstance.ShowInGameUI(!_value);

        if (GameController.activeInstance.hightScore > HIGHTSCORE_FOR_SUNSET_START) {
            StartAtSunsetToggle.gameObject.SetActive(true);
        }
        else {
            StartAtSunsetToggle.gameObject.SetActive(false);
        }

        MusicToggle.isOn = GameController.activeInstance.musicIsMuted;
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

    public void OnValueChange_SunsetStartToggle() {
        if (StartAtSunsetToggle.isOn == true) {
            GameController.activeInstance.sun.transform.Rotate(Vector3.right, SUNSET_ROTATION);
        }
        else {
            GameController.activeInstance.sun.transform.Rotate(Vector3.right, -SUNSET_ROTATION);
        }
    }

    public void OnValueChange_MusicToggle() {
        GameController.activeInstance.musicIsMuted = MusicToggle.isOn;
        GameController.activeInstance.Save();
    }
}