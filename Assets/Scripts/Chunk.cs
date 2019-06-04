using UnityEngine;
//using System;

public class Chunk : MonoBehaviour {

    public GameObject Ground;
    public GameObject Collider;
    public GameObject BarrierTop;
    private Vector3 BarrierTopOriginalScale;

    public GameObject[] Buildings;

    public bool SpawnBuildingsOnStart = true;


    public const float tileSize = 8f;
    private float[] rotations = { 0, 90, 180, 270 };


    void Start() {
        BarrierTopOriginalScale = BarrierTop.transform.localScale;

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
        CheckForBarrierTop();
    }

    public void GenerateBuildings(float _newGroundSizeX, float _newGroundSizeZ, float _newGroundOffsetX, float _newGroundOffsetZ) {

        GameObject _buildings = new GameObject("Buildings");
        _buildings.transform.parent = this.transform;

        float _tilesX = _newGroundSizeX / tileSize;
        float _tilesZ = _newGroundSizeZ / tileSize;

        int[,] Tilemap = new int[(int)_tilesX, (int)_tilesZ];

        for (int i = 0; i < _tilesX; i++) {
            for (int ii = 0; ii < _tilesZ; ii++) {
                MaybeCreateBuilding((i * tileSize) + (tileSize / 2) - (_newGroundSizeX / 2) + _newGroundOffsetX, (ii * tileSize) + (tileSize / 2) - (_newGroundSizeZ / 2) + _newGroundOffsetZ, _buildings.transform);
            }
        }
    }
    public void GenerateBuildings(Transform _newGroundTransform) {
        GenerateBuildings(_newGroundTransform.lossyScale.x, _newGroundTransform.lossyScale.z, _newGroundTransform.position.x, _newGroundTransform.position.z);
    }
    /// <summary>
    /// Generates the Buildings based on the 'Ground' GameObject assinged to it.
    /// </summary>
    public void GenerateBuildings() {
        GenerateBuildings(Ground.transform.lossyScale.x, Ground.transform.lossyScale.z, Ground.transform.position.x, Ground.transform.position.z);
    }

    void MaybeCreateBuilding(float posX, float posZ, Transform _parent) {
        if (Random.Range(0, 10) == 0) {
            GameObject gO = Instantiate(Buildings[Random.Range(0, Buildings.Length)], new Vector3(posX, 0, posZ), Quaternion.Euler(0, rotations[Random.Range(0, rotations.Length)], 0), _parent);
            gO.transform.localScale *= Random.Range(0.8f, 1.4f); 
        }
    }


    public bool PlayerIsPresent() {
        Transform playerT = GameController.activeInstance.activePlayer.transform;

        if (playerT.position.z - transform.position.z < Collider.transform.lossyScale.z) {
            return true;
        }

        return false;
    }

    public bool IsPlayerInSecondHalf() {
        Transform playerT = GameController.activeInstance.activePlayer.transform;

        if (playerT.position.z - transform.position.z > Collider.transform.lossyScale.z / 2) {
            return true;
        }

        return false;
    }

    public void CheckForBarrierTop() {
        Transform playerT = GameController.activeInstance.activePlayer.transform;

        if (playerT.position.y > Collider.transform.localScale.y / 2 && PlayerIsPresent()) { //player is in upper half and in this chunk

            BarrierTop.SetActive(true);
            BarrierTop.transform.position = new Vector3(playerT.position.x, BarrierTop.transform.position.y, playerT.position.z);
            BarrierTop.transform.localScale = BarrierTopOriginalScale * (1 / (Collider.transform.localScale.y / 2) * (playerT.position.y - Collider.transform.localScale.y / 2));

            Material _mat = BarrierTop.GetComponent<MeshRenderer>().material;
            _mat.mainTextureScale = 0.1f * BarrierTopOriginalScale * (1 / (Collider.transform.localScale.y / 2) * (playerT.position.y - Collider.transform.localScale.y / 2));
            _mat.mainTextureOffset = new Vector2((int)playerT.position.z / 10 - playerT.position.z / 10, (int)playerT.position.x / 10 - playerT.position.x / 10);

            //if (playerT.position.y > WorldParameter.localScale.y) { //player is above the DangerZone
            //    BarrierTop.transform.rotation = Quaternion.Euler(-90, 90, 0);
            //}
            //else {
            //    BarrierTop.transform.rotation = Quaternion.Euler(90, -90, 0);
            //}

        }
        else {
            BarrierTop.SetActive(false);
        }
    }
}