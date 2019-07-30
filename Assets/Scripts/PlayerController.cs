using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    public PlayerShoot playerShootScript;

    public bool removeControll = false;
    private bool releaseRotation = false;

    [Header("Movement: ")]

    public float forwardThrustPower = 5f;
    public float sidewayThrustpower = 1f;
    public float backwardThrustPower = 1f;
    [Space(1)]
    public Vector3 localStopPower = new Vector3(0.99f, 0.9f, 1f);

    [Space(10)]

    public float rotationSpeed = 1f;
    public float velocityRotationSpeed = 0.1f;
    private float maxSpeed;

    [Space(5)]

    private Vector3 thrust = new Vector3(0, 0, 0);
    private Quaternion addingRotation;

    [Header("Abillities: ")]

    public float maxBoostSpeed = 75;
    [Range(0f, 1f)]
    public float boostAcceleration = 0.1f;
    private Vector3 toAchieveVelocity;
    private bool boosting = false;
    [Space(2)]
    public float stopTime = 1f;
    public float velocityDecrease = 0.2f;
    private bool stoppingASecond = false;

    [Header("Camera: ")]

    public GameObject CameraAnchor;
    public Camera playerCamera;
    private float playerCameraDistanceMultiplier = 1f;
    private Vector3 playerCameraOriginalPosition;

    [Range(0f, 1f)]
    public float positionChangeSpeed = 1f;
    [Range(0f, 1f)]
    public float rotationChangeSpeed = 0.6f;
    [Range(0f, 1f)]
    public float fovIncreaseSpeed = 0.5f;
    [Range(0f, 1f)]
    public float fovChangeSpeed = 0.5f;

    private bool rotateCamera = false;
    private Quaternion additionalCameraRotation = Quaternion.identity;
    private float mouseWheelInput;
    public float scrollSpeed = 100f;
    public Vector2 scrollBounds = new Vector2(1, 10f);

    void Start() {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraAnchor.transform.parent = null;
        playerCamera = CameraAnchor.GetComponentInChildren<Camera>();
        playerCameraOriginalPosition = playerCamera.transform.localPosition;
    }

    void Update() {
        float zRot = 0;
        addingRotation = Quaternion.identity;


        if (removeControll == false) {
            if (releaseRotation == false) {
                /*=====Movement Input=====*/
                if (Input.GetKey(KeyCode.W)) {
                    thrust = new Vector3(thrust.x, thrust.y, forwardThrustPower);
                }
                //else if (Input.GetKey(KeyCode.S)) {
                //    thrust = new Vector3(thrust.x, thrust.y, backwardThrustPower);
                //}
                else {
                    thrust = new Vector3(thrust.x, thrust.y, 0);
                }
                //if (Input.GetKey(KeyCode.A)) {
                //    thrust = new Vector3(-sidewayThrustpower, thrust.y, thrust.z);
                //}
                //else if (Input.GetKey(KeyCode.D)) {
                //    thrust = new Vector3(sidewayThrustpower, thrust.y, thrust.z);
                //}
                //else {
                //    thrust = new Vector3(0, thrust.y, thrust.z);
                //}


                /*=====Rotation Input=====*/
                if (Input.GetKey(KeyCode.Q)) {

                    zRot = 1;

                    addingRotation = Quaternion.Euler(addingRotation.x, addingRotation.y, -rotationSpeed);
                }
                if (Input.GetKey(KeyCode.E)) {

                    zRot = -1;

                    addingRotation = Quaternion.Euler(addingRotation.x, addingRotation.y, rotationSpeed);
                }
                //Mouse Input
                addingRotation = Quaternion.Euler(-(Input.GetAxisRaw("Mouse Y")) * rotationSpeed * GameController.activeInstance.sensetivity, (Input.GetAxisRaw("Mouse X")) * rotationSpeed * GameController.activeInstance.sensetivity, zRot * rotationSpeed);
                mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");


                /*=====Abillity and Shoot Input=====*/
                if (stoppingASecond == false) {
                    if (Input.GetKeyDown(KeyCode.LeftShift)) {
                        if (rb.velocity.magnitude < maxBoostSpeed && boosting == false) {
                            toAchieveVelocity = rb.velocity.normalized * (rb.velocity.magnitude + 20);
                            boosting = true;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Space)) {
                        StopASecond();
                    }
                }
                if (Input.GetMouseButtonDown(0)) {
                    if (playerShootScript != null) {
                        playerShootScript.Shoot();
                    }
                }
            }

            /*=====Free Camera Input=====*/
            if (Input.GetKey(KeyCode.LeftControl)) {
                rotateCamera = true;
            }
            else {
                rotateCamera = false;
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                StartCoroutine(ResetCamera());

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                releaseRotation = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                releaseRotation = true;
            }
        }
    }

    void FixedUpdate() {
        if (stoppingASecond == false) {
            MovePlayer();
        }
        if (rotateCamera == true) {
            additionalCameraRotation *= addingRotation;
            ScrollCamera();
        }
        else {
            RotatePlayer();
        }
        if (boosting == true) {
            Boost();
        }

        MoveCamera(); //not done in Update()
        RotateCamera();
        ChangeCameraFOV();

        if (removeControll == false) RotationModification_RotateVelocity();
    }

    private void MovePlayer() {
        if (thrust != Vector3.zero) {
            rb.AddForce((transform.right * thrust.x) + (transform.forward * thrust.z), ForceMode.Force);
        }
    }
    private void RotatePlayer() {
        transform.localRotation *= addingRotation;
    }

    private void ChangeCameraFOV() {
        //playerCamera.fieldOfView = 60 + (rb.velocity.magnitude * fovIncreaseSpeed);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 60 + (rb.velocity.magnitude * fovIncreaseSpeed), fovChangeSpeed);

        //playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, -0.025f * playerCamera.fieldOfView + 3.5f, playerCamera.transform.localPosition.z);
    }
    private void MoveCamera() {
        if (positionChangeSpeed == 1) {
            CameraAnchor.transform.position = transform.position;
        }
        else {
            //CameraAnchor.transform.position = Vector3.Slerp(CameraAnchor.transform.position, transform.position, positionChangeSpeed);
        }
    }

    private void RotateCamera() {
        CameraAnchor.transform.rotation = Quaternion.Slerp(CameraAnchor.transform.rotation, transform.rotation * additionalCameraRotation, rotationChangeSpeed);
    }

    const float CAMERA_RESET_STEP = 0.02f;
    private const float CAMERA_RESET_SPEED = 0.05f;

    private IEnumerator ResetCamera() {
        float progress = 0f;
        while (progress < 1) {
            progress += CAMERA_RESET_STEP;
            additionalCameraRotation = Quaternion.Slerp(additionalCameraRotation, Quaternion.identity, progress);
            playerCameraDistanceMultiplier = 1f;
            playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, playerCameraOriginalPosition, progress);
            yield return new WaitForSeconds(CAMERA_RESET_STEP);
        }
    }
    private void ScrollCamera() {
        playerCameraDistanceMultiplier += mouseWheelInput * scrollSpeed;
        playerCameraDistanceMultiplier = Mathf.Clamp(playerCameraDistanceMultiplier, scrollBounds.x, scrollBounds.y);
        playerCamera.transform.localPosition = playerCamera.transform.localPosition.normalized * playerCameraOriginalPosition.magnitude * playerCameraDistanceMultiplier;

        //playerCamera.transform.localPosition += playerCamera.transform.localPosition.normalized * mouseWheelInput * scrollSpeed;
    }


    #region Rotation Modifications

    private void RotationModification_RotateVelocity() {
        //float velMag = rb.velocity.magnitude;
        rb.velocity = Vector3.Slerp(rb.velocity, transform.forward * rb.velocity.magnitude, velocityRotationSpeed);

        //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * velocityRotationSpeed, rb.velocity.z);

        //rb.MoveRotation(transform.localRotation * addingRotation);
        //rb.rotation = transform.localRotation * addingRotation;
        //transform.Rotate(addingRotation.eulerAngles);

        //rb.rotation = transform.localRotation * addingRotation;

        //rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, stopAngularVelocitySpeed);
    }

    private void RotationModification_DecreaseLocalVelocity() {

        Vector3 LocalVelocity = transform.InverseTransformDirection(rb.velocity);
        LocalVelocity = new Vector3(LocalVelocity.x * localStopPower.x, LocalVelocity.y * localStopPower.y, LocalVelocity.z * localStopPower.z);
        rb.velocity = transform.TransformDirection(LocalVelocity);

        Debug.DrawLine(transform.position, transform.position + rb.velocity);
        Debug.DrawLine(transform.position, transform.position + transform.right * LocalVelocity.x, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.up * LocalVelocity.y, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.forward * LocalVelocity.z, Color.blue);

        //rb.MoveRotation(transform.localRotation * addingRotation);
        //rb.rotation = transform.localRotation * addingRotation;
        //transform.Rotate(addingRotation.eulerAngles);

        //rb.rotation = transform.localRotation * addingRotation;
    }

    #endregion

    private void Boost() {
        if (toAchieveVelocity.magnitude - (0.1f * toAchieveVelocity.magnitude) > rb.velocity.magnitude) {
            rb.velocity = Vector3.Lerp(rb.velocity, toAchieveVelocity, boostAcceleration);
        }
        else {
            boosting = false;
        }
    }

    private void StopASecond() {
        rb.velocity = rb.velocity * velocityDecrease;
        maxSpeed = rb.velocity.magnitude;
        stoppingASecond = true;
        StartCoroutine(StartAfterASecond());
    }

    private IEnumerator StartAfterASecond() {
        yield return new WaitForSecondsRealtime(1f);
        rb.velocity = rb.velocity * (1 / velocityDecrease);
        stoppingASecond = false;
    }

    public float GetVelocity() {
        return rb.velocity.magnitude;
    }

    public float GetMaxSpeed() {
        if (rb.velocity.magnitude > maxSpeed) {
            maxSpeed = rb.velocity.magnitude;
        }
        return maxSpeed;
    }
}