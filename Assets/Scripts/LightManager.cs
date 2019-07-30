using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    public float startIntensity = -2f;
    public float endIntensity = 2f;
    public float fadeStep = 0.1f;
    public float fadeTimeMin = 5f;
    public float fadeTimeMax = 10f;

    public BuildingLight[] buildingLights;

    void Start() {

        DeactivateLights();
        //FadeInLights();
    }

    void Update() {
        //if (mat != null) {
        //    float emission = Time.time * 0.5f;//Mathf.PingPong(Time.time, 1.0f);
        //    Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'

        //    Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        //    mat.SetColor("_EmissionColor", finalColor);
        //}
    }

    public void FadeInLights() {
        foreach (BuildingLight bl in buildingLights) {
            StartCoroutine(FadeInMaterialIntensity(bl.material, bl.activeColor, UnityEngine.Random.Range(fadeTimeMin, fadeTimeMax)));
        }
    }
    private IEnumerator FadeInMaterialIntensity(Material _mat, Color _activeColor, float _fadeTime) {

        float intensity = startIntensity;

        while (intensity < endIntensity) {
            Color finalColor = _activeColor * Mathf.LinearToGammaSpace(intensity);

            _mat.SetColor("_EmissionColor", finalColor);

            intensity += fadeStep * (endIntensity - startIntensity);

            yield return new WaitForSeconds(fadeStep * _fadeTime);
        }
    }



    public void FadeOutLights() {
        foreach (BuildingLight bl in buildingLights) {
            StartCoroutine(FadeOutMaterialIntensity(bl.material, bl.activeColor, UnityEngine.Random.Range(fadeTimeMin, fadeTimeMax)));
        }
    }
    private IEnumerator FadeOutMaterialIntensity(Material _mat, Color _activeColor, float _fadeTime) {

        float intensity = endIntensity;

        while (intensity > startIntensity) {
            Color finalColor = _activeColor * Mathf.LinearToGammaSpace(intensity);

            _mat.SetColor("_EmissionColor", finalColor);

            intensity -= fadeStep * (endIntensity - startIntensity);

            yield return new WaitForSeconds(fadeStep * _fadeTime);
        }
    }

    public void DeactivateLights() {
        foreach (BuildingLight bl in buildingLights) {
            Color finalColor = bl.activeColor * Mathf.LinearToGammaSpace(startIntensity);

            bl.material.SetColor("_EmissionColor", finalColor);
        }
    }
}

[Serializable]
public struct BuildingLight {
    public Material material;
    public Color activeColor;
}