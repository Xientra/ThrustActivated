using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    public GameObject createOnDeath;

    private Rigidbody rb;

    public float speed = 10f;
    public float duration = 10f;

    void Start() {
        rb = GetComponent<Rigidbody>();

        Destroy(this.gameObject, duration);

        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") == false || other.gameObject.layer != 2 /*IgnoreRaycast*/) {
            GameObject _createOnDeath = Instantiate(createOnDeath, transform.position, transform.rotation);
            Destroy(_createOnDeath, 3f);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 3f);
        }
    }
}