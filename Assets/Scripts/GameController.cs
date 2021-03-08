using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController activeInstance;

    public Light sun;
    public LightManager lightManager;
    public GameObject chunkPrefab;
    public GameObject playerPrefab;
    public GameObject playerSpawnEffectPrefab;
    private const float SPAWN_TIME = 1f;
    private const float SPAWN_EFFECT_ADDITIONAL_TIME = 2f;
    private const float SPAWN_AREA_SIZE = 10f;

    public GameObject playerSpawnPosition;

    private const float RECOMENDED_SPEED = 170f;

    [Space(10)]

    public GameObject startCamera;
    public Player activePlayer;

    [HideInInspector]
    public bool gameIsRunning = false;
    [SerializeField]
    private float currentScore = 0f;
    public float hightScore;

    public bool musicIsMuted = false;

    public const float TIME_UNTILL_SCENE_RELOAD = 3f;

    public float sensetivity = 1f;
    public float sunSpeed = 0.01f;
    public bool nightTime = false;

    private const float NIGHT_TIME_ROTATION = 180f;

    [Space(10)]

    //public GameObject[] Chunks;
    public Chunk lastChunk;
    public Chunk currentChunk;
    public Chunk nextChunk;

    private void Awake() {
        if (activeInstance == null) {
            activeInstance = this;

            Load();
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        nextChunk = SpawnNewChunk();
        gameIsRunning = false;
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
            AddScore(0.0001f * Mathf.Pow(activePlayer.playerController.GetVelocity(), 2));

            InGameUI.activeInstance.SetScoreText(currentScore);
            InGameUI.activeInstance.SetSpeedText(activePlayer.playerController.GetVelocity(), (activePlayer.playerController.GetVelocity() > RECOMENDED_SPEED));
        }

        if (gameIsRunning == true) {
            sun.transform.Rotate(Vector3.right, sunSpeed);

            if (sun.transform.localRotation.eulerAngles.x > NIGHT_TIME_ROTATION && nightTime == false) { //turn on the lights
                activePlayer.spotLight.gameObject.SetActive(true);

                lightManager.FadeInLights();

                nightTime = true;
            }
            else if (sun.transform.localRotation.eulerAngles.x < NIGHT_TIME_ROTATION && nightTime == true) { //turn off the lights
                activePlayer.spotLight.gameObject.SetActive(false);

                lightManager.FadeOutLights();

                nightTime = false;
            }
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
            DeleteBuildingsInSpawningArea();

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

    private void DeleteBuildingsInSpawningArea() {
        RaycastHit[] hits = Physics.BoxCastAll(playerSpawnPosition.transform.position, new Vector3(SPAWN_AREA_SIZE / 2, SPAWN_AREA_SIZE / 2, SPAWN_AREA_SIZE / 2), Vector3.up);
        foreach (RaycastHit rh in hits) {
            Building possibleBuilding = rh.transform.GetComponentInParent<Building>();
            if (possibleBuilding != null) {
                Destroy(possibleBuilding.gameObject);
            }
        }
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

    public void Save() {
        PlayerPrefs.SetFloat("hightscore", hightScore);
        PlayerPrefs.SetFloat("sensetivity", sensetivity);
        PlayerPrefs.SetString("MusicIsMuted", musicIsMuted.ToString());
        PlayerPrefs.Save();
    }

    public void Load() {
        hightScore = PlayerPrefs.GetFloat("hightscore");
        sensetivity = PlayerPrefs.GetFloat("sensetivity");
        musicIsMuted = (PlayerPrefs.GetString("MusicIsMuted").ToLower() == "True".ToLower());
    }
}