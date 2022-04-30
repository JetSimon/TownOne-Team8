using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGenerating : MonoBehaviour {

    public GameObject eggPrefab;
    public Camera mainCamera;

    //The time between spawns
    public float generationCooldown = 3.0f;

    private float m_elapsed = 0;

    // Update is called once per frame
    void Update()
    {
        m_elapsed += Time.deltaTime;
        if (m_elapsed >= generationCooldown)
        {
            //Find the corners of the camera in world space
            float depth = -mainCamera.transform.localPosition.z;
            Vector2 min = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, depth));
            Vector2 max = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));

            //Generate a random position interpolated between the corners
            Vector2 position = new Vector2(
                Mathf.Lerp(min.x, max.x, Random.Range(0.0f, 1.0f)),
                Mathf.Lerp(min.y, max.y, Random.Range(0.0f, 1.0f))
            );
            
            //Instantiate the egg, reset the timer
            Instantiate(eggPrefab, position, Quaternion.identity, null);
            m_elapsed = 0;
        }
    }
}
