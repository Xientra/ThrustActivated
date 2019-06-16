using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public GameObject SparksPrefab;
    private GameObject sparksObject;

    Rigidbody rb;
    private Vector3 lastPosition;

    private bool flyingBackwarts = false;

    bool inSaveZone = true;

    public float timeBeforeDeath = 3f;
    float deathTimeStamp;


    float sparksCooldown = 0.1f;
    float sparkTimeSpamp;

    public const float TIME_UNTIL_TURN_BACK_TEXT_SHOWS = 1f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    void Update() {
        
    }

    private void FixedUpdate() {
        if (inSaveZone == false) {
            InGameUI.activeInstance.EnableDangerZoneUI(true);
            string _time = (deathTimeStamp - Time.time).ToString();
            if ((deathTimeStamp - Time.time) > 0) {
                if (_time.Length >= 4) InGameUI.activeInstance.timeRemainingText.text = _time.Substring(0, 4);
            }
            else InGameUI.activeInstance.timeRemainingText.text = "0,00";

            if (Time.time > deathTimeStamp) {
                Destroy(this.gameObject);
            }
        }
        else {
            InGameUI.activeInstance.EnableDangerZoneUI(false);
        }

        if (lastPosition.z > transform.position.z && flyingBackwarts == false) {
            flyingBackwarts = true;
            StartCoroutine(StarShowingTurnBackText());
        }
        else if (lastPosition.z <= transform.position.z && flyingBackwarts == true) {
            flyingBackwarts = false;
            InGameUI.activeInstance.HideTurnBackText();
        }
        lastPosition = transform.position;
    }

    private IEnumerator StarShowingTurnBackText() {
        yield return new WaitForSeconds(TIME_UNTIL_TURN_BACK_TEXT_SHOWS);
        if (lastPosition.z > transform.position.z && flyingBackwarts == true) {
            InGameUI.activeInstance.ShowTurnBackText();
        }
    }

    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.gameObject.layer == 9 /*World*/) {
    //        ContactPoint cp = collision.GetContact(0);
    //        GameObject _sparks = Instantiate(SparksPrefab, cp.point + cp.normal * 0.1f, Quaternion.LookRotation(cp.normal));
    //        Destroy(_sparks, 1.1f);
    //    }
    //}
    
    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer == 9 /*World*/) {
            ContactPoint cp = collision.GetContact(0);

            if (Time.time > sparkTimeSpamp) {
                GameObject _sparks = Instantiate(SparksPrefab, cp.point + cp.normal * 0.1f, Quaternion.LookRotation(cp.normal));
                Destroy(_sparks, 1.1f);

                sparkTimeSpamp = Time.time + sparksCooldown;
            }
        }
    }
    /*
    private void OnCollisionExit(Collision collision) {
        Destroy(sparksObject);
    }
    */

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("SaveZone")) {
            inSaveZone = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("SaveZone")) {
            inSaveZone = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("SaveZone")) {
            inSaveZone = false;
            deathTimeStamp = Time.time + timeBeforeDeath;
        }
    }

    //private void OnGUI() {
    //    if (inSaveZone == false) {
    //        GUI.contentColor = Color.red;
    //        GUI.Label(new Rect((Screen.width / 2) - 25, (Screen.height / 2), 1000, 1000), "Turn Back!" + "\n" + (deathTimeStamp - Time.time).ToString());
    //    }
    //}
}