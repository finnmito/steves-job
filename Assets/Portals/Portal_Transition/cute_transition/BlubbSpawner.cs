using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlubbSpawner : MonoBehaviour {
    public const float MAX_COUNT = 643;
    public GameObject bubblePrefab;

    public void SpawnBubbles() {
        for (int i = 0; i < MAX_COUNT; ++i) {
            Instantiate(bubblePrefab, parent: this.gameObject.transform);
        }
    }

    public void KillAll() {
        foreach (Blubb b in GameObject.FindObjectsOfType<Blubb>()) {
            b.Kill();
        }
    }

}