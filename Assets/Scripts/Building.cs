using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public Collider specialManeuverCollider;
    public float specialManeuverScore;

    private bool used = false;

    /*
    [Header("Lights: ")]

    public Light[] lights;
    [SerializeField]
    private bool lit = false;
    public float lightingUpSpeedMin = 0.2f;
    public float lightingUpSpeedMax = 1.5f;
    */

    private void OnTriggerEnter(Collider other) {
        if (used == false) {
            if (other.CompareTag("Player")) {
                float _score = ((1f / 150f) * GameController.activeInstance.activePlayer.playerController.GetVelocity()) * specialManeuverScore;
                GameController.activeInstance.AddScore(_score);
                InGameUI.activeInstance.AddSpecialManuverPoints(_score);
                used = true;
            }
        }
    }

    //private void Start() {
    //    //System.Random rng = new System.Random();
    //    //ShuffleLights(rng);

    //    //SetLit(true);
    //}
    /*
    public void ShuffleLights(System.Random rng) {
        int n = lights.Length;
        while (n > 1) {
            int k = rng.Next(n--);
            Light temp = lights[n];
            lights[n] = lights[k];
            lights[k] = temp;
        }
    }

    public void SetLit(bool _value) {
        StartCoroutine(TurnOnTheLights(_value));
        lit = _value;
    }

    private IEnumerator TurnOnTheLights(bool _value) {
        //bool allLightsAreOn = false;

        for (int i = 0; i < lights.Length; i++) {
            yield return new WaitForSeconds(Random.Range(lightingUpSpeedMin, lightingUpSpeedMax));
            lights[i].gameObject.SetActive(_value);
        }
    }
    */
} 