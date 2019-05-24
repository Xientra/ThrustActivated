using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Rigidbody playerRigidbody;

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

    [Header("Debug: ")]

    public Vector3 SpeedDebug;
    public float totalSpeedDebug;

    private void Awake() { // put this in gameController later
        //Time.fixedDeltaTime = 0.01f;
    }

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }

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
    }

    void FixedUpdate() {
        Movement_RotateVelocity();

        //playerRigidbody.angularVelocity = Vector3.Lerp(playerRigidbody.angularVelocity, Vector3.zero, stopAngularVelocitySpeed);

        SpeedDebug = playerRigidbody.velocity;
        totalSpeedDebug = playerRigidbody.velocity.magnitude;
    }

    private void Movement_RotateVelocity() {
        if (thrust != Vector3.zero) {
            playerRigidbody.AddForce((transform.right * thrust.x) + (transform.forward * thrust.z), ForceMode.Force);
        }
        transform.localRotation *= addingRotation;

        //float velMag = playerRigidbody.velocity.magnitude;
        playerRigidbody.velocity = Vector3.Slerp(playerRigidbody.velocity, transform.forward * playerRigidbody.velocity.magnitude, velocityRotationSpeed);

        //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y * velocityRotationSpeed, playerRigidbody.velocity.z);


        //playerRigidbody.MoveRotation(transform.localRotation * addingRotation);
        //playerRigidbody.rotation = transform.localRotation * addingRotation;
        //transform.Rotate(addingRotation.eulerAngles);

        //playerRigidbody.rotation = transform.localRotation * addingRotation;
    }

    private void Movement_DecreaseLocalVelocity() {
        if (thrust != Vector3.zero) {
            playerRigidbody.AddForce((transform.right * thrust.x) + (transform.forward * thrust.z), ForceMode.Force);
        }
        transform.localRotation *= addingRotation;

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
}