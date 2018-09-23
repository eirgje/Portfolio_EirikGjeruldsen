using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour {

    private Vector3 startSpawn = Vector3.zero;
    private Vector3 slideSpawn = Vector3.zero;
    private Transform playerTransform = null;

    private bool finishedSlide = false;
    public void SlideFinished() { finishedSlide = true; }

    private void Awake()
    {
        startSpawn = transform.GetChild(0).transform.position;
        slideSpawn = transform.GetChild(1).transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Respawn()
    {
        if (!finishedSlide)
        {
            playerTransform.position = startSpawn;
        }
        else if (finishedSlide)
        {
            playerTransform.position = slideSpawn;
        }
    }
}
