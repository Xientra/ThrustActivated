﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Rigidbody playerRigidbody;

    public bool removeControll = false;

    public float forwardThrustPower = 5f;
    public float sidewayThrustpower = 1f;
    public float backwardThrustPower = 1f;

    [Space(1)]

    public Vector3 localStopPower = new Vector3(0.99f, 0.9f, 1f);

    [Space(10)]

    public float rotationSpeed = 1f;
    public float velocityRotationSpeed = 0.1f;

    [Space(5)]

    private Vector3 thrust = new Vector3(0, 0, 0);
    private Quaternion addingRotation;

    [Header("Camera: ")]

    public GameObject CameraAnchor;
    private Camera playerCamera;

    [Range(0f, 1f)]
    public float positionChangeSpeed = 0.4f;
    [Range(0f, 1f)]
    public float rotationChangeSpeed = 0.6f;
    [Range(0f, 1f)]
    public float fovIncreaseSpeed = 0.5f;

    private bool rotateCamera = false;
    private Quaternion additionalCameraRotation = Quaternion.identity; 

    [Header("Debug: ")]

    public Vector3 SpeedDebug;
    public float totalSpeedDebug;

    private void Awake() { // put this in gameController later
        //Time.fixedDeltaTime = 0.01f;
    }

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraAnchor.transform.parent = null;
        playerCamera = CameraAnchor.GetComponentInChildren<Camera>();
    }

    void Update() {

        /*=====Movement Input=====*/
        if (Input.GetKey(KeyCode.W)) {

            thrust = new Vector3(thrust.x, thrust.y, forwardThrustPower);
        }
        else if (Input.GetKey(KeyCode.S)) {

            thrust = new Vector3(thrust.x, thrust.y, backwardThrustPower);
        }
        else {
            thrust = new Vector3(thrust.x, thrust.y, 0);
        }

        if (Input.GetKey(KeyCode.A)) {

            thrust = new Vector3(-sidewayThrustpower, thrust.y, thrust.z);
        }
        else if (Input.GetKey(KeyCode.D)) {

            thrust = new Vector3(sidewayThrustpower, thrust.y, thrust.z);
        }
        else {
            thrust = new Vector3(0, thrust.y, thrust.z);
        }


        /*=====Rotation Input=====*/
        float zRot = 0;
        if (Input.GetKey(KeyCode.Q)) {

            zRot = 1;

            addingRotation = Quaternion.Euler(addingRotation.x, addingRotation.y, -rotationSpeed);
        }
        if (Input.GetKey(KeyCode.E)) {

            zRot = -1;

            addingRotation = Quaternion.Euler(addingRotation.x, addingRotation.y, rotationSpeed);
        }
        addingRotation = Quaternion.Euler(-(Input.GetAxisRaw("Mouse Y")) * rotationSpeed, (Input.GetAxisRaw("Mouse X")) * rotationSpeed, zRot * rotationSpeed);

        /*=====Other Input=====*/
        if (Input.GetKey(KeyCode.LeftShift)) {
            rotateCamera = true;
        }
        else {
            rotateCamera = false;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            StopPlayer();
        }
    }

    void FixedUpdate() {
        if (removeControll == false) {
            MovePlayer();
            if (rotateCamera == true) {
                additionalCameraRotation *= addingRotation;
            }
            else {
                RotatePlayer();
            }
        }

        FOV_Camera();
        //MoveCamera();
        MoveCameraAbsolute();
        RotateCamera();

        if (removeControll == false) {
            RotationModification_RotateVelocity();
        }
        //playerRigidbody.angularVelocity = Vector3.Lerp(playerRigidbody.angularVelocity, Vector3.zero, stopAngularVelocitySpeed);

        //Debug Stuff
        SpeedDebug = playerRigidbody.velocity;
        totalSpeedDebug = playerRigidbody.velocity.magnitude;
    }

    private void MovePlayer() {
        if (thrust != Vector3.zero) {
            playerRigidbody.AddForce((transform.right * thrust.x) + (transform.forward * thrust.z), ForceMode.Force);
        }
    }
    private void RotatePlayer() {
        transform.localRotation *= addingRotation;
    }

    private void FOV_Camera() {
        playerCamera.fieldOfView = 60 + (playerRigidbody.velocity.magnitude * fovIncreaseSpeed);
    }
    private void MoveCamera() {
        CameraAnchor.transform.position = Vector3.Slerp(CameraAnchor.transform.position, transform.position, positionChangeSpeed);
    }
    private void MoveCameraAbsolute() {
        CameraAnchor.transform.position = transform.position;
    }
    private void RotateCamera() {
        CameraAnchor.transform.rotation = Quaternion.Slerp(CameraAnchor.transform.rotation, transform.rotation * additionalCameraRotation, rotationChangeSpeed);
    }

    public void StopPlayer() {
        playerRigidbody.velocity = Vector3.zero;
    }


    private void RotationModification_RotateVelocity() {
        //float velMag = playerRigidbody.velocity.magnitude;
        playerRigidbody.velocity = Vector3.Slerp(playerRigidbody.velocity, transform.forward * playerRigidbody.velocity.magnitude, velocityRotationSpeed);

        //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y * velocityRotationSpeed, playerRigidbody.velocity.z);


        //playerRigidbody.MoveRotation(transform.localRotation * addingRotation);
        //playerRigidbody.rotation = transform.localRotation * addingRotation;
        //transform.Rotate(addingRotation.eulerAngles);

        //playerRigidbody.rotation = transform.localRotation * addingRotation;
    }

    private void RotationModification_DecreaseLocalVelocity() {

        Vector3 LocalVelocity = transform.InverseTransformDirection(playerRigidbody.velocity);
        LocalVelocity = new Vector3(LocalVelocity.x * localStopPower.x, LocalVelocity.y * localStopPower.y, LocalVelocity.z * localStopPower.z);
        playerRigidbody.velocity = transform.TransformDirection(LocalVelocity);

        Debug.DrawLine(transform.position, transform.position + playerRigidbody.velocity);
        Debug.DrawLine(transform.position, transform.position + transform.right * LocalVelocity.x, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.up * LocalVelocity.y, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.forward * LocalVelocity.z, Color.blue);


        //playerRigidbody.MoveRotation(transform.localRotation * addingRotation);
        //playerRigidbody.rotation = transform.localRotation * addingRotation;
        //transform.Rotate(addingRotation.eulerAngles);

        //playerRigidbody.rotation = transform.localRotation * addingRotation;
    }


    public float GetVelocity() {
        return playerRigidbody.velocity.magnitude;
    }
}