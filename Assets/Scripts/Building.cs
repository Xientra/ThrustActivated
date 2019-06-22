﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public float specialManeuverScore;
    public Collider specialManeuverCollider;

    private bool used = false;

    private void OnTriggerEnter(Collider other) {
        if (used == false) {
            if (other.CompareTag("Player")) {
                GameController.activeInstance.AddScore(((1f / 100f) * GameController.activeInstance.activePlayer.playerController.GetVelocity()) * specialManeuverScore);
                used = true;
            }
        }
    }
} 