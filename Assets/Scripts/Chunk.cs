using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

    public GameObject[] BuildingPrefabs;
    public List<Building> buildings;
    public List<Light> everyLight;

    public GameObject Ground;
    public GameObject Collider;

    public GameObject BarrierSides;
    public GameObject BarrierBack;
    public GameObject BarrierTop;
    private Vector3 BarrierTopOriginalScale;
    private Quaternion BarrierTopOriginalRotation;

    public bool InstaSpawnBuildings = false;
    public bool SpawnBuildingsOnStart = true;


    public const float tileSize = 8f;
    private float[] rotations = { 0, 90, 180, 270 };


    void Start() {
        BarrierTopOriginalScale = BarrierTop.transform.localScale;
        BarrierTopOriginalRotation = BarrierTop.transform.rotation;

        if (SpawnBuildingsOnStart == true) {
            if (Ground != null) {
                GenerateBuildings();
            }
            else {
                Debug.LogError("Ground needs to be assing to Spawn a World on Start.");
            }
        }
    }

    void FixedUpdate() {
        //CheckForBarrierTop();
    }

    /// <summary>
    /// Generates the Buildings based on the 'Ground' GameObject assinged to it.
    /// </summary>
    public void GenerateBuildings() {
        GenerateBuildings(Ground.transform.lossyScale.x, Ground.transform.lossyScale.z, Ground.transform.position.x, Ground.transform.position.z);
    }
    public void GenerateBuildings(Transform _newGroundTransform) {
        GenerateBuildings(_newGroundTransform.lossyScale.x, _newGroundTransform.lossyScale.z, _newGroundTransform.position.x, _newGroundTransform.position.z);
    }
    public void GenerateBuildings(float _newGroundSizeX, float _newGroundSizeZ, float _newGroundOffsetX, float _newGroundOffsetZ) {

        GameObject _buildings = new GameObject("Buildings");
        _buildings.transform.parent = this.transform;

        float _tilesX = _newGroundSizeX / tileSize;
        float _tilesZ = _newGroundSizeZ / tileSize;

        int[,] Tilemap = new int[(int)_tilesX, (int)_tilesZ];

        if (InstaSpawnBuildings == true) {
            for (int i = 0; i < _tilesX; i++) {
                for (int ii = 0; ii < _tilesZ; ii++) {
                    MaybeCreateBuilding((i * tileSize) + (tileSize / 2) - (_newGroundSizeX / 2) + _newGroundOffsetX, (ii * tileSize) + (tileSize / 2) - (_newGroundSizeZ / 2) + _newGroundOffsetZ, _buildings.transform);
                }
            }
        }
        else {
            StartCoroutine(GenerateBuildingsOverTime(_tilesX, _tilesZ, _newGroundSizeX, _newGroundSizeZ, _newGroundOffsetX, _newGroundOffsetZ, _buildings.transform));
        }
    }

    private IEnumerator GenerateBuildingsOverTime(float _tilesX, float _tilesZ, float _newGroundSizeX, float _newGroundSizeZ, float _newGroundOffsetX, float _newGroundOffsetZ, Transform _parent) {
        for (int ii = 0; ii < _tilesZ; ii++) {

            for (int i = 0; i < _tilesX; i++) {
                bool b = MaybeCreateBuilding((i * tileSize) + (tileSize / 2) - (_newGroundSizeX / 2) + _newGroundOffsetX, (ii * tileSize) + (tileSize / 2) - (_newGroundSizeZ / 2) + _newGroundOffsetZ, _parent);
                //if (b == true) yield return 0;
            }
            yield return null;
        }
    }

    bool MaybeCreateBuilding(float posX, float posZ, Transform _parent) {
        if (Random.Range(0, 10) == 0) {
            GameObject gO = Instantiate(BuildingPrefabs[Random.Range(0, BuildingPrefabs.Length)], new Vector3(posX, 0, posZ), Quaternion.Euler(0, rotations[Random.Range(0, rotations.Length)], 0), _parent);
            gO.transform.localScale *= Random.Range(0.8f, 1.4f);

            buildings.Add(gO.GetComponent<Building>());
            //everyLight.AddRange(gO.GetComponent<Building>().lights);
            return true;
        }
        return false;
    }


    public void SetToCurrentChunk(float _delay) {
        StartCoroutine(DelaySetToCurrentChunk(_delay));
    }
    private IEnumerator DelaySetToCurrentChunk(float _delay) {
        yield return new WaitForSeconds(_delay);
        BarrierBack.SetActive(true);
    }
    public void SetToLastChunk(float _delay) {
        StartCoroutine(DelaySetToLastChunk(_delay));
    }
    private IEnumerator DelaySetToLastChunk(float _delay) {
        yield return new WaitForSeconds(_delay);
        Collider.SetActive(false);
        BarrierBack.SetActive(false);
        BarrierSides.SetActive(false);
    }

    public bool PlayerIsPresent() {
        if (GameController.activeInstance.gameIsRunning == true) {
            Transform playerT = GameController.activeInstance.activePlayer.transform;

            if (playerT.position.z - transform.position.z < Collider.transform.lossyScale.z) {
                return true;
            }
        }
        return false;
    }

    public bool IsPlayerInSecondHalf() {
        if (GameController.activeInstance.gameIsRunning == true) {
            Transform playerT = GameController.activeInstance.activePlayer.transform;

            if (playerT.position.z - transform.position.z > Collider.transform.lossyScale.z / 2) {
                return true;
            }
        }
        return false;
    }

    public void CheckForBarrierTop() {
        if (GameController.activeInstance.gameIsRunning == true) {
            Transform playerT = GameController.activeInstance.activePlayer.transform;

            if (playerT.position.y > Collider.transform.localScale.y / 2 && PlayerIsPresent()) { //player is in upper half and in this chunk

                BarrierTop.SetActive(true);
                BarrierTop.transform.position = new Vector3(playerT.position.x, BarrierTop.transform.position.y, playerT.position.z);
                BarrierTop.transform.localScale = BarrierTopOriginalScale * (1 / (Collider.transform.localScale.y / 2) * (playerT.position.y - Collider.transform.localScale.y / 2));

                Material _mat = BarrierTop.GetComponent<MeshRenderer>().material;
                _mat.mainTextureScale = 0.1f * BarrierTop.transform.localScale;
                _mat.mainTextureOffset = new Vector2((int)playerT.position.z / 10 - playerT.position.z / 10, (int)playerT.position.x / 10 - playerT.position.x / 10);

                if (playerT.position.y > Collider.transform.localScale.y) { //player is above the DangerZone
                    BarrierTop.transform.rotation = Quaternion.Euler(-90, -90, 0);
                }
                else {
                    BarrierTop.transform.rotation = BarrierTopOriginalRotation;
                }

                //BarrierTop.SetActive(true);
                //BarrierTop.transform.position = new Vector3(playerT.position.x, BarrierTop.transform.position.y, playerT.position.z);
                //BarrierTop.transform.localScale = BarrierTopOriginalScale * (1 / (Collider.transform.localScale.y / 2) * (playerT.position.y - Collider.transform.localScale.y / 2));

                //Material _mat = BarrierTop.GetComponent<MeshRenderer>().material;
                //_mat.mainTextureScale = 1f * new Vector2(BarrierTop.transform.localScale.x, BarrierTop.transform.localScale.z);//BarrierTopOriginalScale * (1 / (Collider.transform.localScale.y / 2) * (playerT.position.y - Collider.transform.localScale.y / 2));
                //_mat.mainTextureOffset = new Vector2((int)playerT.position.z / 10 - playerT.position.z / 10, (int)playerT.position.x / 10 - playerT.position.x / 10);

                ////if (playerT.position.y > Collider.transform.localScale.y) { //player is above the DangerZone
                ////    BarrierTop.transform.rotation = Quaternion.Euler(0, -90, 0);
                ////}
                ////else {
                ////    BarrierTop.transform.rotation = BarrierTopOriginalRotation;
                ////}

            }
            else {
                BarrierTop.SetActive(false);
            }
        }
    }

    public void EnableLights(bool _value) {
        //foreach (Building b in _chunk.buildings) {
        //    b.SetLit(_value);
        //}

        foreach (Light l in everyLight) {
            if (l != null) {
                l.gameObject.SetActive(_value);
            }
        }
    }
}