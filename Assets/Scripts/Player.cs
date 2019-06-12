using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public GameObject SparksPrefab;
    private GameObject sparksObject;

    Rigidbody rb;

    bool inSaveZone = true;

    public float timeBeforeDeath = 3f;
    float deathTimeStamp;

    float sparksCooldown = 0.1f;
    float sparkTimeSpamp;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        
    }

    private void FixedUpdate() {
        if (inSaveZone == false) {
            InGameUI.activeInstance.DangerZonePanel.SetActive(true);



            if (Time.time > deathTimeStamp) {
                Destroy(this.gameObject);
            }
        }
        else {
            InGameUI.activeInstance.DangerZonePanel.SetActive(false);
        }

        if (transform.rotation.x > 90 || transform.rotation.x < -90 || transform.rotation.y > 90 || transform.rotation.y < -90) {
            InGameUI.activeInstance.ShowTurnBackText(0.05f);
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

    private void OnGUI() {
        if (inSaveZone == false) {
            GUI.contentColor = Color.red;
            GUI.Label(new Rect((Screen.width / 2) - 25, (Screen.height / 2), 1000, 1000), "Turn Back!" + "\n" + (deathTimeStamp - Time.time).ToString());
        }
    }
}