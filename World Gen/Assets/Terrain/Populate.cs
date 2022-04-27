using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Populate : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    [Range(0, 1)]
    public float basicTreeChance;
    [Range(0, 1)]
    public float rockTreeChance;
    [Range(0, 1)]
    public float lushTreeChance;
    [Range(0, 1)]
    public float grassTreeChance;
    [Range(0, 1)]
    public float iceTreeChance;

    [Range(0, 1)]
    public float basicStoneChance;
    [Range(0, 1)]
    public float rockStoneChance;
    [Range(0, 1)]
    public float lushStoneChance;
    [Range(0, 1)]
    public float grassStoneChance;
    [Range(0, 1)]
    public float iceStoneChance;

    public GameObject Treefab;
    public GameObject Orefab;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;

        TerrainManager.InitTerrainResourceData(terrainData.alphamapWidth, terrainData.alphamapHeight);

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for(int x = 0; x < terrainData.alphamapWidth; x++)
            {
                //int rand = 0;

                //Normalise X/Y Coordinates
                float yN = (float)y / (float)terrainData.alphamapHeight;
                float xN = (float)x / (float)terrainData.alphamapWidth;

                //sample the hight at this coordienates then normalise it
                float height = terrainData.GetHeight(Mathf.RoundToInt(yN * terrainData.heightmapResolution), Mathf.RoundToInt(xN * terrainData.heightmapResolution)); // terrainData.heightmapHight * heightmapWidth allways returning 1025
                //float heightN = (height - terrainData.bounds.min.y) / (terrainData.bounds.max.y - terrainData.bounds.min.y);

                int terrainType = TerrainManager.GetTerrainMaxW(x, y);

                TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.None);

                if (terrainType == (int)TerrainManager.terrainTypes.Basic)
                {
                    if (Random.Range(0.0f, 1.0f) <= basicTreeChance)
                    {
                        Instantiate(Treefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Tree);
                    }
                    else if (Random.Range(0.0f, 1.0f) <= basicStoneChance)
                    {
                        Instantiate(Orefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Rock);
                    }

                }
                else if (terrainType == (int)TerrainManager.terrainTypes.Lush)
                {
                    if (Random.Range(0.0f, 1.0f) <= lushTreeChance)
                    {
                        Instantiate(Treefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Tree);
                    }
                    else if (Random.Range(0.0f, 1.0f) <= lushStoneChance)
                    {
                        Instantiate(Orefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Rock);
                    }
                    
                }
                else if (terrainType == (int)TerrainManager.terrainTypes.Grass)
                {
                    if (Random.Range(0.0f, 1.0f) <= grassTreeChance)
                    {
                        Instantiate(Treefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Tree);
                    }
                    else if (Random.Range(0.0f, 1.0f) <= grassStoneChance)
                    {
                        Instantiate(Orefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Rock);
                    }
                }
                else if (terrainType == (int)TerrainManager.terrainTypes.Rock)
                {

                    if (Random.Range(0.0f, 1.0f) <= rockTreeChance)
                    {
                        Instantiate(Orefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Rock);
                    }
                    else if (Random.Range(0.0f, 1.0f) <= rockStoneChance)
                    {
                        Instantiate(Orefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Rock);
                    }
                }
                else if (terrainType == (int)TerrainManager.terrainTypes.Ice)
                {

                    if (Random.Range(0.0f, 1.0f) <= iceTreeChance)
                    {
                        Instantiate(Orefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Rock);
                    }
                    else if (Random.Range(0.0f, 1.0f) <= iceStoneChance)
                    {
                        Instantiate(Orefab, new Vector3(y, height, x), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        TerrainManager.SetTerrainResourceData(x, y, (int)TerrainManager.resources.Rock);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
