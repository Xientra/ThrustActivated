using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController activeInstance;

    public GameObject chunkPrefab;
    public GameObject playerPrefab;
    public GameObject playerSpawnEffectPrefab;
    private const float SPAWN_TIME = 2f;
    private const float SPAWN_EFFECT_ADDITIONAL_TIME = 2f;

    public GameObject playerSpawnPosition;

    [Space(10)]

    public GameObject startCamera;
    public Player activePlayer;

    [HideInInspector]
    public bool gameIsRunning = false;
    [SerializeField]
    private float currentScore = 0f;
    public float hightScore;

    private const float TIME_UNTILL_SCENE_RELOAD = 3f;

    [Space(10)]

    //public GameObject[] Chunks;
    public Chunk lastChunk;
    public Chunk currentChunk;
    public Chunk nextChunk;

    private void Awake() {
        if (activeInstance == null) {
            activeInstance = this;

            hightScore = PlayerPrefs.GetFloat("hightscore");
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        nextChunk = SpawnNewChunk();
    }

    void Update() {

    }

    private void FixedUpdate() {
        if (activePlayer == null) {
            gameIsRunning = false;
        }

        //spawning new Chunks
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


        if (gameIsRunning == true) {
            AddScore((50f - activePlayer.transform.position.y) * 0.0001f * Mathf.Pow(activePlayer.playerController.GetVelocity(), 2));
            InGameUI.activeInstance.scoreText.text = Mathf.Round(currentScore).ToString();
        }
    }

    private Chunk SpawnNewChunk() {
        GameObject chunkGo = Instantiate(chunkPrefab, currentChunk.transform.position + new Vector3(0, 0, currentChunk.Collider.transform.lossyScale.z), chunkPrefab.transform.rotation);
        Chunk chunk = chunkGo.GetComponent<Chunk>();
        //chunk.GenerateBuildings();

        return chunk;
    }

    public void SpawnPlayer() {
        if (activePlayer == false) {
            Player p = Instantiate(playerPrefab, playerSpawnPosition.transform.position, Quaternion.identity).GetComponent<Player>();
            PlayerController pc = p.GetComponent<PlayerController>();

            if (startCamera != null) {
                Destroy(pc.CameraAnchor);
                pc.CameraAnchor = startCamera;
            }

            StartCoroutine(SpawnPlayerEffect(p));
        }
        else {
            if (startCamera != null && activePlayer.playerController.CameraAnchor != null) {
                Destroy(startCamera);
            }
        }
    }
    private IEnumerator SpawnPlayerEffect(Player _player) {
        _player.GFX_Object.SetActive(false);
        _player.GetComponent<PlayerController>().removeControll = true;

        GameObject _effect = Instantiate(playerSpawnEffectPrefab, playerSpawnPosition.transform.position, playerSpawnEffectPrefab.transform.rotation);

        yield return new WaitForSeconds(SPAWN_TIME);

        _player.GFX_Object.SetActive(true);
        _player.GetComponent<PlayerController>().removeControll = false;

        activePlayer = _player;

        gameIsRunning = true;

        yield return new WaitForSeconds(SPAWN_EFFECT_ADDITIONAL_TIME);

        Destroy(_effect);
    }

    public void AddScore(float _amount) {
        currentScore += _amount;
    }

    public void GameOver() {
        if (currentScore > hightScore) {
            hightScore = currentScore;
            PlayerPrefs.SetFloat("hightscore", hightScore);
            PlayerPrefs.Save();
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene() {
        yield return new WaitForSeconds(TIME_UNTILL_SCENE_RELOAD);
        SceneManager.LoadScene("MainScene");
    }
}