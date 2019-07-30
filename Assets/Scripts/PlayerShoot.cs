using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController))]
public class PlayerShoot : MonoBehaviour {

    public PlayerController playerController;

    public GameObject[] LaserOriginPoints;
    public GameObject LaserHitEffectPrefab;

    private void Start() {
        playerController = GetComponent<PlayerController>();

        foreach (GameObject los in LaserOriginPoints) {
            los.SetActive(false);
        }
    }

    public void Shoot() {

        RaycastHit hit;
        if (Physics.Raycast(playerController.playerCamera.transform.position, transform.forward, out hit, Mathf.Infinity)) {

            foreach (GameObject los in LaserOriginPoints) {
                los.GetComponent<LineRenderer>().SetPosition(0, los.transform.position);
                los.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                los.SetActive(true);
                StartCoroutine(DeactivateLaser(los));
            }

            GameObject _explosion = Instantiate(LaserHitEffectPrefab, hit.point, Quaternion.identity);
            Destroy(_explosion, 3f);
        }
        else {
            foreach (GameObject los in LaserOriginPoints) {
                los.GetComponent<LineRenderer>().SetPosition(0, los.transform.position);
                los.GetComponent<LineRenderer>().SetPosition(1, transform.position + transform.forward * 100);
                los.SetActive(true);
                StartCoroutine(DeactivateLaser(los));
            }
        }
    }

    IEnumerator DeactivateLaser(GameObject go) {
        InvokeRepeating("UpdatePlayerLaser", 0, 0.02f);
        yield return new WaitForSeconds(0.05f);
        CancelInvoke("UpdatePlayerLaser");
        go.SetActive(false);
    }

    private void UpdatePlayerLaser() {
        foreach (GameObject los in LaserOriginPoints) {
            los.GetComponent<LineRenderer>().SetPosition(0, los.transform.position);
        }
    }
}