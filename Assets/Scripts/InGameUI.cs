using System;
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
    public TextMeshProUGUI currentlyPlayingSongText;

    public TextMeshProUGUI specialPointsTextExample;

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

    public void ShowInGameUI(bool value) {
        scoreText.gameObject.SetActive(value);
        speedText.gameObject.SetActive(value);
        hightscoreText.gameObject.SetActive(!value);
        currentlyPlayingSongText.gameObject.SetActive(value);
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

    public void AddSpecialManuverPoints(float points) {
        TextMeshProUGUI txt = Instantiate(specialPointsTextExample, this.transform);
        txt.text = "+" + points.ToString();
        txt.gameObject.SetActive(true);
        StartCoroutine(MovePointsText(txt));

    }

    private const float SPECIAL_POINTS_MOVE_DISTANCE = 50f;
    private const float SPECIAL_POINTS_MOVE_STEPS = 30f;
    private const float SPECIAL_POINTS_MOVE_TIME = 0.5f;
    private const float SPECIAL_POINTS_NOTMOVE_TIME = 0.5f;

    private IEnumerator MovePointsText(TextMeshProUGUI _text) {
        _text.transform.Translate(0, -SPECIAL_POINTS_MOVE_DISTANCE, 0);

        for (int i = 0; i < SPECIAL_POINTS_MOVE_DISTANCE; i++) {

            _text.transform.Translate(0, SPECIAL_POINTS_MOVE_DISTANCE * (1 / SPECIAL_POINTS_MOVE_STEPS), 0);

            yield return new WaitForSeconds(SPECIAL_POINTS_MOVE_TIME / SPECIAL_POINTS_MOVE_DISTANCE);
        }
        Destroy(_text.gameObject, SPECIAL_POINTS_NOTMOVE_TIME);
    }
}