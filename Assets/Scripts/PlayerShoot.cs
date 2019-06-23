using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController))]
public class PlayerShoot : MonoBehaviour {

    public GameObject[] LaserOriginPoints;
    public GameObject LaserHitEffectPrefab;

    private void Start() {
        foreach (GameObject los in LaserOriginPoints) {
            los.SetActive(false);
        }
    }

    public void Shoot() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity)) {

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
        yield return 0;
        go.SetActive(false);
    }
}
