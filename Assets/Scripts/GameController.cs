using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController activeInstance;

    public GameObject chunkPrefab;

    public Player activePlayer;

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
        if (activePlayer == null) {
            activePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        nextChunk = SpawnNewChunk();
    }

    void Update() {

    }

    private void FixedUpdate() {
        if (currentChunk.PlayerIsPresent() == false && nextChunk.PlayerIsPresent() == true) {

            Debug.Log("hi?");
            if (lastChunk != null) {
                Destroy(lastChunk.gameObject);
            }

            lastChunk = currentChunk;
            currentChunk = nextChunk;

            nextChunk = SpawnNewChunk();

            currentChunk.SetToCurrentChunk(3f);
            lastChunk.SetToLastChunk(3f);
        }
    }

    private Chunk SpawnNewChunk() {
        GameObject chunkGo = Instantiate(chunkPrefab, currentChunk.transform.position + new Vector3(0, 0, currentChunk.Collider.transform.lossyScale.z), chunkPrefab.transform.rotation);
        Chunk chunk = chunkGo.GetComponent<Chunk>();
        //chunk.GenerateBuildings();

        return chunk;
    }
}