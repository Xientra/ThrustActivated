using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour {

    public GameObject[] Buildings;

    public float WorldSizeX = 100f;
    public float WorldSizeZ = 100f;

    public float tileSize = 8f;

    private float TilesX;
    private float TilesZ;



    private float[] rotations = { 0, 90, 180, 270 };

    void Start() {

        TilesX = WorldSizeX / tileSize;
        TilesZ = WorldSizeZ / tileSize;

        int[,] Tilemap = new int[(int)TilesX, (int)TilesZ];

        for (int i = 0; i < TilesX; i++) {
            for (int ii = 0; ii < TilesZ; ii++) {
                MaybeCreateBuilding((i * tileSize) + (tileSize / 2) - (WorldSizeX / 2), (ii * tileSize) + (tileSize / 2) - (WorldSizeZ / 2));
            }
        }
    }


    void Update() {

    }

    void MaybeCreateBuilding(float posX, float posZ) {
        if (Random.Range(0, 10) == 0) {
            GameObject gO = Instantiate(Buildings[Random.Range(0, Buildings.Length)], new Vector3(posX, 0, posZ), Quaternion.Euler(0, rotations[Random.Range(0, rotations.Length)], 0), this.transform);
            gO.transform.localScale *= Random.Range(0.8f, 1.4f); 
        }
    }
}