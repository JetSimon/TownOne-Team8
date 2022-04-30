using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGenerating : MonoBehaviour { 
    public Vector2 WorldUnitsInCamera;
    public Vector2 WorldToPixelAmount;

    public GameObject eggPrefab;
    public Camera mainCamera;
    private const float GENERATE_COOL_DOWN = 3f; // amount of second it takes to generate a new egg
    private float timeLeft = GENERATE_COOL_DOWN;
    // Start is called before the first frame update
    void Start()
    {
        //Finding Pixel To World Unit Conversion Based On Orthographic Size Of Camera
        WorldUnitsInCamera.y = mainCamera.orthographicSize * 2;
        WorldUnitsInCamera.x = WorldUnitsInCamera.y * Screen.width / Screen.height;

        WorldToPixelAmount.x = Screen.width / WorldUnitsInCamera.x;
        WorldToPixelAmount.y = Screen.height / WorldUnitsInCamera.y;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            float cameraWidthHalf = mainCamera.scaledPixelWidth / 2 / WorldToPixelAmount.x;
            float cameraHeightHalf = mainCamera.scaledPixelHeight / 2 / WorldToPixelAmount.y;
            Vector3 position = new Vector3(Random.Range(-cameraWidthHalf, cameraWidthHalf), Random.Range(-cameraHeightHalf, cameraHeightHalf), -1);
            Instantiate(eggPrefab, position, Quaternion.Euler(0, 0, 0), transform);
            print("egg generated");
            timeLeft = GENERATE_COOL_DOWN;
        }
    }
}
