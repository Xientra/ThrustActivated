using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public GameObject SparksPrefab;
    public GameObject OnDeathEffectPrefab;
    public GameObject BrokenEffect;

    public GameObject GFX_Object;
    public Light spotLight;

    [HideInInspector]
    public PlayerController playerController;
    Rigidbody rb;
    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    private const float SURVIVALABLE_MAX_SPEED_PERCENT_DROP = 0.5f;

    private bool flyingBackwarts = false;

    bool inSaveZone = true;
    public bool isDead = false;

    public float timeBeforeDeath = 3f;
    float deathTimeStamp;


    float sparksCooldown = 0.1f;
    float sparkTimeSpamp;

    public const float TIME_UNTIL_TURN_BACK_TEXT_SHOWS = 1f;

    void Start() {
        playerController = GetComponent<PlayerController>();
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

            if (Time.time > deathTimeStamp && isDead == false) {
                DestroyPlayer();
            }
        }
        else {
            InGameUI.activeInstance.EnableDangerZoneUI(false);
        }

        if (isDead == false) {
            if (lastPosition.z > transform.position.z && flyingBackwarts == false) {
                flyingBackwarts = true;
                StartCoroutine(StarShowingTurnBackText());
            }
            else if (lastPosition.z <= transform.position.z && flyingBackwarts == true) {
                flyingBackwarts = false;
                InGameUI.activeInstance.HideTurnBackText();
            }

            if (rb.velocity.magnitude < SURVIVALABLE_MAX_SPEED_PERCENT_DROP * playerController.GetMaxSpeed()) {
                DestroyPlayer();
            }
        }

        lastVelocity = rb.velocity;
        lastPosition = transform.position;
    }

    private IEnumerator StarShowingTurnBackText() {
        yield return new WaitForSeconds(TIME_UNTIL_TURN_BACK_TEXT_SHOWS);
        if (lastPosition.z > transform.position.z && flyingBackwarts == true) {
            InGameUI.activeInstance.ShowTurnBackText();
        }
    }

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

    private void DestroyPlayer() {
        GameController.activeInstance.GameOver();
        isDead = true;

        Instantiate(BrokenEffect, this.transform);
        GameObject _effect = Instantiate(OnDeathEffectPrefab, transform.position, transform.rotation);
        Destroy(_effect, 5f);
        //Destroy(this.gameObject);

        playerController.removeControll = true;
        rb.useGravity = true;
    }
}