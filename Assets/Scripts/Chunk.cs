using UnityEngine;
//using System;

public class Chunk : MonoBehaviour {

    public GameObject[] Buildings;
    //public GameObject[] NoColliderBuildings;

    public Transform WorldParameter;

    public GameObject Ground;
    public GameObject Collider;
    //public GameObject[] BarrierObjects;
    public GameObject BarrierTop;
    private Vector3 BarrierTopOriginalScale;


    public const float tileSize = 8f;

    private float[] rotations = { 0, 90, 180, 270 };

    [Space(5)]

    public bool SpawnWorldOnStart = false;


    void Start() {
        BarrierTopOriginalScale = BarrierTop.transform.localScale;


        if (SpawnWorldOnStart == true) {
            if (WorldParameter != null) {
                GenerateBuildings(WorldParameter);
            }
            else {
                Debug.LogError("WorldParameter is not assing to Spawn a World on Start.");
            }
        }
    }

    void Update() {
        CheckForBarrierTop();
    }

    public void GenerateBuildings(float _worldSizeX, float _worldSizeZ, float _worldOffsetX, float _worldOffsetZ) {

        GameObject _buildings = Instantiate(new GameObject("Buildings"), this.transform);

        float _tilesX = _worldSizeX / tileSize;
        float _tilesZ = _worldSizeZ / tileSize;

        int[,] Tilemap = new int[(int)_tilesX, (int)_tilesZ];

        for (int i = 0; i < _tilesX; i++) {
            for (int ii = 0; ii < _tilesZ; ii++) {
                MaybeCreateBuilding((i * tileSize) + (tileSize / 2) - (_worldSizeX / 2) + _worldOffsetX, (ii * tileSize) + (tileSize / 2) - (_worldSizeZ / 2) + _worldOffsetZ, _buildings.transform);
            }
        }
    }
    public void GenerateBuildings(Transform _worldParameter) {
        GenerateBuildings(_worldParameter.lossyScale.x, _worldParameter.lossyScale.z, _worldParameter.position.x, _worldParameter.position.z);
    }

    void MaybeCreateBuilding(float posX, float posZ, Transform _parent) {
        if (Random.Range(0, 10) == 0) {
            GameObject gO = Instantiate(Buildings[Random.Range(0, Buildings.Length)], new Vector3(posX, 0, posZ), Quaternion.Euler(0, rotations[Random.Range(0, rotations.Length)], 0), _parent);
            gO.transform.localScale *= Random.Range(0.8f, 1.4f); 
        }
    }

    public bool IsPlayerInSecondHalf() {
        Transform playerT = GameController.activeInstance.activePlayer.transform;

        if (playerT.position.z - transform.position.z > 500) {
            return true;
        }

        return false;
    }

    public void CheckForBarrierTop() {
        Transform playerT = GameController.activeInstance.activePlayer.transform;

        if (playerT.position.y > WorldParameter.localScale.y / 2) { //player is in upper half

            BarrierTop.SetActive(true);
            BarrierTop.transform.position = new Vector3(playerT.position.x, BarrierTop.transform.position.y, playerT.position.z);
            BarrierTop.transform.localScale = BarrierTopOriginalScale * (1 / (WorldParameter.localScale.y / 2) * (playerT.position.y - WorldParameter.localScale.y / 2));

            Material _mat = BarrierTop.GetComponent<MeshRenderer>().material;
            _mat.mainTextureScale = 0.1f * BarrierTopOriginalScale * (1 / (WorldParameter.localScale.y / 2) * (playerT.position.y - WorldParameter.localScale.y / 2));
            _mat.mainTextureOffset = new Vector2((int)playerT.position.z/10 - playerT.position.z/10, (int)playerT.position.x/10 - playerT.position.x/10);
        }
        else {
            BarrierTop.SetActive(false);
        }
    }
}