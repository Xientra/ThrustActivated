using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourScript : MonoBehaviour {

    public GameObject playerObject;

    [Range(0f, 1f)]
    public float positionChangeSpeed = 0.5f;
    [Range(0f, 1f)]
    public float rotationChangeSpeed = 0.5f;

    void Start() {

    }


    void FixedUpdate() {
        transform.position = Vector3.Slerp(transform.position, playerObject.transform.position, positionChangeSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerObject.transform.rotation, rotationChangeSpeed);

    }
}