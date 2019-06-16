using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShoot : MonoBehaviour {


    public GameObject[] LaserOrigins;
    public GameObject LaserHitEffectPrefab;

    private void Start() {
        //LaserLineRenderer.enabled = false;
        foreach (GameObject los in LaserOrigins) {
            los.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    private void Shoot() {


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity)) {

            foreach (GameObject los in LaserOrigins) {
                los.GetComponent<LineRenderer>().SetPosition(0, los.transform.position);
                los.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                los.SetActive(true);
                StartCoroutine(DeactivateLaser(los));
            }

            GameObject _explosion = Instantiate(LaserHitEffectPrefab, hit.point, Quaternion.identity);
            Destroy(_explosion, 3f);
        }
        else {
            foreach (GameObject los in LaserOrigins) {
                los.GetComponent<LineRenderer>().SetPosition(0, los.transform.position);
                los.GetComponent<LineRenderer>().SetPosition(1, transform.position + transform.forward * 100);
                los.SetActive(true);
                StartCoroutine(DeactivateLaser(los));
            }
        }
    }

    IEnumerator DeactivateLaser(GameObject go) {
        yield return 0;
        go.SetActive(false);
    }
}
