using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for map array

public class SplatMap : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;
    private float[,,] splatmapData;
    private float[] splatWeights;

    // Start is called before the first frame update
    void Start()
    {
        //Get Terrian componant
        terrain = GetComponent<Terrain>();

        //get a reference to terrain data
        terrainData = terrain.terrainData;


        //SPLAT MAP 25 - 116
        //custom Splatmap array, 3D array of floats
        splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        TerrainManager.InitTerrainMaxW(terrainData.alphamapWidth, terrainData.alphamapHeight);

        //Setup an Array to record the mix of texture weights at this point


        //TerrainManager.initSplatWeights(terrainData.alphamapLayers);

        //TerrainManager splatWeights = new float[terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                //Normalise x/y coordinates to a range of 0 - 1)
                float yN = (float)y / (float)terrainData.alphamapHeight;
                float xN = (float)x / (float)terrainData.alphamapWidth;

                //sample the hight at this coordienates then normalise it
                float height = terrainData.GetHeight(Mathf.RoundToInt(yN * terrainData.heightmapResolution), Mathf.RoundToInt(xN * terrainData.heightmapResolution)); // terrainData.heightmapHight * heightmapWidth allways returning 1025
                float heightN = (height - terrainData.bounds.min.y) / (terrainData.bounds.max.y - terrainData.bounds.min.y);


                //calculate the normal DOES NOT USE but keeping it incase
                //Vector3 normal = terrainData.GetInterpolatedNormal(yN, xN);

                //calculate steepness of terrain
                float steepness = terrainData.GetSteepness(yN, xN);

                splatWeights = new float[terrainData.alphamapLayers];

                ////THE FOLLOWING ARE THE RULES FOR THE SPLAT MAP
                /*
                    - basic terrain: Dirt, Texture[0]
                    - Steep ground: Rock, Texture[1]
                    - Low flat ground: Lush Grass, Texture[2]
                    - High flat ground: Lame Grass, Texture[3]
                    - very High Ground: Snow, Texture[4]
                */

                // Texture[0], Dirt, constant influence
                //TerrainManager.setSplatWeight(0, 0.2f);


                //TO HELP WITH TREE TEST IM MAKEIN 00 LUSH WILL NEED TO DELEAT WHEN DONE

                if (heightN < 0.85)
                {
                    splatWeights[(int)TerrainManager.terrainTypes.Basic] = 0.2f;

                    //Texture[1], Clif, Influence on high & steep terrain 
                    //TerrainManager.setSplatWeight(1, heightN * Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution)));

                    splatWeights[(int)TerrainManager.terrainTypes.Rock] = heightN * Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution));

                    //Texture[2], Lush gras, Influence on low and very shallow terrain
                    //TerrainManager.setSplatWeight(2, (1.0f - heightN) * (1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution / 5.0f))));

                    splatWeights[(int)TerrainManager.terrainTypes.Lush] = (1.0f - heightN) * (1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution / 5.0f)));

                    //Texture[3], Normal grass, influence on higher and shallow terrain
                    //splatWeights[3] = heightN * (1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution / 0.5f)));
                    //TerrainManager.setSplatWeight(3, heightN * (1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution / 0.5f))));

                    splatWeights[(int)TerrainManager.terrainTypes.Grass] = heightN * (1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution / 0.5f)));

                }

                //Texture[4], Snow, Very tippy top of the mountains
                else
                {
                    //TerrainManager.setSplatWeight(1, 1.0f - heightN);
                    splatWeights[(int)TerrainManager.terrainTypes.Rock] = 1.0f - heightN;

                    //TerrainManager.setSplatWeight(4, heightN);
                    splatWeights[(int)TerrainManager.terrainTypes.Ice] = 1.0f - heightN;

                }

                //Texture[1] higher altitudes
                //splatWeights[0] = 1.0f - heightN; //Mathf.Clamp01((terrainData.heightmapHeight - height) / terrainData.heightmapHeight); //ERROR HERE ALWAYS 1 without the magic number (change to hight value)

                //Texture[2] stronger on flatter terrain
                //splatWeights[1] = (1.0f - heightN) * (1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution / 2.0f))); //NOTE Steepness is unbounded so it is normalised
                // subtracted by 1.0 ^ for a grater weighting on flat surfaces

                //Texture[3] speep terrain
                //splatWeights[2] = heightN * Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution));

                //if (heightN < 0.55)
                //{
                // splatWeights[3] = (1.0f - heightN) * (1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapHeight / 0.1f)));
                //}

                //splatWeights[4] = heightN * Mathf.Clamp01(normal.z);

                TerrainManager.SetTerrainMaxW(x, y, CalMaxWeight());

                //Sum of all textures must equal 1 so narmalise the sum of the weights
                //float z = TerrainManager.sumSplatWeight();
                float SplatSum = splatWeights.Sum();

                //loop through each terraon texture
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    //Normalise
                    //TerrainManager.setSplatWeight(i, TerrainManager.getSplatWeight(i));

                    splatWeights[i] /= SplatSum;

                    //assign this point to the splatmap array
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }
        //Assign the new splat map to the terrain data:
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    int CalMaxWeight()
    {
        int maxT = 0;
        for (int i = 0; i < splatWeights.Length; i++)
        {
            maxT = splatWeights[i] > splatWeights[maxT] ? i : maxT;
        }

        return (maxT);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
