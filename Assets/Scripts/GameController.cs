using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController activeInstance;

    public Chunk StartChunk;
    public Chunk chunkPrefab;

    public Player activePlayer;

    private void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        if (activePlayer == null) {
            activePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }


        StartChunk.GenerateBuildings(StartChunk.WorldParameter.lossyScale.x, StartChunk.WorldParameter.lossyScale.z, StartChunk.WorldParameter.position.x, StartChunk.WorldParameter.position.z);
    }

    void Update() {

    }
}