using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchBehaviour : MonoBehaviour
{
    public GameObject eggPrefab;
    public float spawnTime = 5.0f;

    public Animation m_spawnAnimation;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) StartEggSpawn();
    }

    public void StartEggSpawn()
    {
        m_spawnAnimation.Play();
    }
    public void SpawnEgg()
    {
        var prefab = Instantiate(eggPrefab);
        prefab.transform.position = transform.position + Vector3.up * 0.25f;
    }
}
