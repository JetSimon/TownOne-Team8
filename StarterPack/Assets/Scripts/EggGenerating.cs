using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EggGenerating : MonoBehaviour {
    public GameObject eggPrefab;
    public Tilemap tilemap;
    public Grid grid;
    private float gridX_Offset;
    private float gridY_Offset;

    private List<Vector3> availableTiles = new List<Vector3>();
    private string[] availableTileNames = {"level_tileset_0", "level_tileset_1", "level_tileset_2",
                                           "level_tileset_6", "level_tileset_7", "level_tileset_8",
                                           "level_tileset_12", "level_tileset_13", "level_tileset_14"};

    public List<Vector3> availablePlaces;

    private const float GENERATE_COOL_DOWN = 3f; // amount of second it takes to generate a new egg
    private float timeLeft = GENERATE_COOL_DOWN;
    // Start is called before the first frame update
    void Start()
    {
        gridX_Offset = grid.cellSize.x / 2;
        gridY_Offset = grid.cellSize.y / 2;

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        int xrange = tilemap.cellBounds.xMax - tilemap.cellBounds.xMin;
        int yrange = tilemap.cellBounds.yMax - tilemap.cellBounds.yMin;

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace);
                place.x += gridX_Offset;
                place.y += gridY_Offset;
                if (tilemap.HasTile(localPlace))
                {
                    int x = n - tilemap.cellBounds.xMin;
                    int y = p - tilemap.cellBounds.yMin;
                    
                    TileBase tile = allTiles[x + y * bounds.size.x];

                    foreach (string availableTilename in availableTileNames)
                    {
                        if (tile.name == availableTilename)
                        {
                            availableTiles.Add(place);
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            int length = availableTiles.Count;
            System.Random rnd = new System.Random();
            int tilesIndex = rnd.Next(length);
            Instantiate(eggPrefab, availableTiles[tilesIndex], Quaternion.Euler(0, 0, 0), transform);
            print("egg generated");
            timeLeft = GENERATE_COOL_DOWN;
        }
    }
}
