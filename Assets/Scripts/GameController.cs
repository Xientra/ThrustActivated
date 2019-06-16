using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController activeInstance;

    public GameObject chunkPrefab;
    public GameObject playerPrefab;

    public GameObject playerSpawnPosition;

    [Space(10)]

    public GameObject startCamera;
    public Player activePlayer;

    [HideInInspector]
    public bool gameIsRunning = true;

    [Space(10)]

    //public GameObject[] Chunks;
    public Chunk lastChunk;
    public Chunk currentChunk;
    public Chunk nextChunk;

    private void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        //if (activePlayer == null) {
        //    activePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //}

        nextChunk = SpawnNewChunk();
    }

    void Update() {

    }

    private void FixedUpdate() {
        if (activePlayer == null) {
            gameIsRunning = false;
        }

        if (currentChunk.PlayerIsPresent() == false && nextChunk.PlayerIsPresent() == true) {

            if (lastChunk != null) {
                Destroy(lastChunk.gameObject);
            }

            lastChunk = currentChunk;
            currentChunk = nextChunk;

            nextChunk = SpawnNewChunk();

            currentChunk.SetToCurrentChunk(3f);
            lastChunk.SetToLastChunk(3f);

            Debug.Log("Spawned new Chunk");
        }
    }

    public void SpawnPlayer() {
        PlayerController pc = Instantiate(playerPrefab, playerSpawnPosition.transform.position, Quaternion.identity).GetComponent<PlayerController>();

        if (startCamera != null) {
            Destroy(pc.CameraAnchor);
            pc.CameraAnchor = startCamera;
        }
    }

    private Chunk SpawnNewChunk() {
        GameObject chunkGo = Instantiate(chunkPrefab, currentChunk.transform.position + new Vector3(0, 0, currentChunk.Collider.transform.lossyScale.z), chunkPrefab.transform.rotation);
        Chunk chunk = chunkGo.GetComponent<Chunk>();
        //chunk.GenerateBuildings();

        return chunk;
    }
}