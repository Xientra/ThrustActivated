using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class InGameUI : MonoBehaviour {

    public static InGameUI activeInstance;

    public TextMeshProUGUI turnBackText;
    public GameObject DangerZonePanel;

    private int turnBackStringIndex = 0;
    private bool turnBackDirection = true;
    private const string TURN_BACK = "TURN BACK";

    private void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        ShowTurnBackText(0.05f);
    }

    void Update() {

    }

    void FixedUpdate() {
    }

    public void ShowTurnBackText(float _showSpeed) {
        turnBackText.text = "";
        turnBackStringIndex = 0;
        turnBackDirection = true;
        InvokeRepeating("AnimateTurnBackText", 0, _showSpeed);
    }

    public void HideTurnBackText() {
        turnBackText.text = "";
        turnBackStringIndex = 0;
        turnBackDirection = true;
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
}