using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class InGameUI : MonoBehaviour {

    public static InGameUI activeInstance;

    public Animator DangerZonePanelAnimator;
    public TextMeshProUGUI turnBackText;
    public TextMeshProUGUI dangerText;
    public TextMeshProUGUI timeRemainingText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hightscoreText;

    private int turnBackStringIndex = 0;
    private bool turnBackDirection = true;
    private const string TURN_BACK = "TURN BACK";
    private const float TURN_BACK_SHOW_SPEED = 0.05f;

    //private const string DANGER = "DANGER";
    private const string SPEED_WARING = "Exceeded recomended Speed";

    private void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        SetHightscoreText(GameController.activeInstance.hightScore);
    }

    public void EnableDangerZoneUI(bool _value) {
        DangerZonePanelAnimator.SetBool("visible", _value);
        dangerText.gameObject.SetActive(_value);
        timeRemainingText.gameObject.SetActive(_value);

        if (_value == true) {
            HideTurnBackText();
        }
    }

    public void ShowTurnBackText() {
        if (dangerText.IsActive() != true) {
            turnBackText.text = "";
            turnBackText.alignment = TextAlignmentOptions.Left;
            turnBackStringIndex = 0;
            turnBackDirection = true;
            turnBackText.gameObject.SetActive(true);
            InvokeRepeating("AnimateTurnBackText", 0, TURN_BACK_SHOW_SPEED);
        }
    }

    public void HideTurnBackText() {
        turnBackText.text = "";
        turnBackStringIndex = 0;
        turnBackDirection = true;
        turnBackText.gameObject.SetActive(false);
        CancelInvoke("AnimateTurnBackText");
    }

    private void AnimateTurnBackText() {
        if (turnBackDirection == true) {
            if (turnBackStringIndex < TURN_BACK.Length) {
                while (TURN_BACK[turnBackStringIndex] == ' ') {
                    turnBackText.text = turnBackText.text + TURN_BACK[turnBackStringIndex];
                    turnBackStringIndex++;
                }

                turnBackText.text = turnBackText.text + TURN_BACK[turnBackStringIndex];
                turnBackStringIndex++;
            }
            else {
                turnBackDirection = false;
                turnBackStringIndex = 0;
                turnBackText.alignment = TextAlignmentOptions.Right;
            }
        }
        else {
            if (turnBackStringIndex < TURN_BACK.Length) {
                StringBuilder sb = new StringBuilder(turnBackText.text);
                sb.Remove(0, 1);
                turnBackText.text = sb.ToString();
                turnBackStringIndex++;
            }
            else {
                turnBackDirection = true;
                turnBackStringIndex = 0;
                turnBackText.text = "";
                turnBackText.alignment = TextAlignmentOptions.Left;
            }
        }
    }

    /*
    private string AnimateText(string _text) {
        string _newText = "";

        if (turnBackDirection == true) {
            if (turnBackStringIndex < _text.Length) {
                while (_text[turnBackStringIndex] == ' ') {
                    turnBackText.text = turnBackText.text + _text[turnBackStringIndex];
                    turnBackStringIndex++;
                }

                turnBackText.text = turnBackText.text + _text[turnBackStringIndex];
                turnBackStringIndex++;
            }
            else {
                turnBackDirection = false;
                turnBackStringIndex = 0;
                turnBackText.alignment = TextAlignmentOptions.Right;
            }
        }
        else {
            if (turnBackStringIndex < _text.Length) {
                StringBuilder sb = new StringBuilder(turnBackText.text);
                sb.Remove(0, 1);
                turnBackText.text = sb.ToString();
                turnBackStringIndex++;
            }
            else {
                turnBackDirection = true;
                turnBackStringIndex = 0;
                turnBackText.text = "";
                turnBackText.alignment = TextAlignmentOptions.Left;
            }
        }

        return _newText;
    }
    */

    public void SetSpeedText(float _speed, bool showDangerWarning) {
        if (showDangerWarning == true) {
            speedText.text = "Speed: " + Mathf.Round(_speed).ToString() + "\n" + SPEED_WARING;
        }
        else {
            speedText.text = "Speed: " + Mathf.Round(_speed).ToString();
        }
    }

    public void SetScoreText(float _score) {
        scoreText.text = "Score: " + Mathf.Round(_score).ToString();
    }

    public void SetHightscoreText(float _score) {
        hightscoreText.text = "Hightscore: \n" + Mathf.Round(_score).ToString();
    }
}