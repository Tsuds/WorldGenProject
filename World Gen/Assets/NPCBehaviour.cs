using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    private int xTerrainPos;
    private int yTerrainPos;

    private bool safe = false;

    private int[] navPath;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        terrainData = terrain.terrainData;

        //allign location to terrain coords 
        xTerrainPos = (int)this.transform.position.x;
        yTerrainPos = (int)this.transform.position.z;

        transform.position = new Vector3(xTerrainPos, terrainData.GetHeight(xTerrainPos, yTerrainPos), yTerrainPos);

        while (safe == false)
        {
            if (TerrainManager.GetTerrainResourceData(xTerrainPos, yTerrainPos) != (int)TerrainManager.resources.None)
            {
                //PUNT, may have to alter as there is an secario where can loop endlessly
                if(xTerrainPos > 0)
                {
                    xTerrainPos--;
                }
                else
                {
                    xTerrainPos++;
                }
            }
            else
            {
                safe = true;
            }
        }

        transform.position = new Vector3(xTerrainPos, terrainData.GetHeight(xTerrainPos, yTerrainPos), yTerrainPos);

        FindGoal((int)TerrainManager.resources.Tree, new Vector2(this.transform.position.x, this.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Navigate(Vector2 goal, Vector2 start)
    {
        bool found = false;
        Vector2[] navQueue = new Vector2[terrainData.alphamapWidth * terrainData.alphamapHeight];
        int[,] pathing = new int[terrainData.alphamapWidth, terrainData.alphamapHeight];

        navQueue[0] = start;
        pathing[(int)start.x, (int)start.y] = (int)TerrainManager.directions.None;

        
        int curentInQueue = 0;
        int queueSize = 0;

        bool invalidNeigbour;

        while (found == false)
        {
            if (navQueue[curentInQueue] != goal)
            {
                Vector2 neigbour = new Vector2(0, 0);

                //up down left right
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0: //Up
                            neigbour = new Vector2(navQueue[curentInQueue].x, navQueue[curentInQueue].y + 1);
                            break;
                        case 1: //Down
                            neigbour = new Vector2(navQueue[curentInQueue].x, navQueue[curentInQueue].y - 1);
                            break;
                        case 2: //Left
                            neigbour = new Vector2(navQueue[curentInQueue].x - 1, navQueue[curentInQueue].y);
                            break;
                        case 3: //Right
                            neigbour = new Vector2(navQueue[curentInQueue].x + 1, navQueue[curentInQueue].y);
                            break;
                    }

                    invalidNeigbour = false;

                    if (neigbour.x < 0 || neigbour.y < 0 || neigbour.x > terrainData.alphamapWidth || neigbour.y > terrainData.alphamapHeight)
                    {
                        invalidNeigbour = false; //not realy repeated but outside the bounds of the map
                    }
                    else
                    {
                        for (int j = 0; j < navQueue.Length; j++)
                        {
                            if (navQueue[j].x == neigbour.x && navQueue[j].y == neigbour.y)
                            {
                                invalidNeigbour = false;
                            }
                        }
                    }


                    if (!invalidNeigbour)
                    {
                        queueSize++;
                        navQueue[queueSize].x = neigbour.x;
                        navQueue[queueSize].y = neigbour.y;

                        switch (i)
                        {
                            case 0: //Up
                                pathing[(int)neigbour.x, (int)neigbour.y] = (int)TerrainManager.directions.Down;
                                break;
                            case 1: //Down
                                pathing[(int)neigbour.x, (int)neigbour.y] = (int)TerrainManager.directions.Up;
                                break;
                            case 2: //Left
                                pathing[(int)neigbour.x, (int)neigbour.y] = (int)TerrainManager.directions.Right;
                                break;
                            case 3: //Right
                                pathing[(int)neigbour.x, (int)neigbour.y] = (int)TerrainManager.directions.Left;
                                break;
                        }                   
                    }
                }
                curentInQueue++;
            }
            else
            {
                found = true;
            }
        }
        //Path Back
    }

    Vector2 FindGoal(int goal, Vector2 start)
    {
        bool found = false;
        Vector2[] searchQueue = new Vector2[terrainData.alphamapWidth * terrainData.alphamapHeight];

        searchQueue[0] = start;
        
        int curentInQueue = 0;
        int queueSize = 0;

        bool invalidNeigbour;

        while(found == false)
        {
            Debug.Log(searchQueue[curentInQueue]);

            if (TerrainManager.GetTerrainResourceData((int)searchQueue[curentInQueue].y, (int)searchQueue[curentInQueue].x) != goal)
            {
                Vector2 neigbour = new Vector2(0, 0);
                
                //up down left right
                for(int i = 0; i < 4; i++)
                {
                    switch(i)
                    {
                        case 0: //Up
                            neigbour = new Vector2(searchQueue[curentInQueue].x, searchQueue[curentInQueue].y + 1);
                            break;
                        case 1: //Down
                            neigbour = new Vector2(searchQueue[curentInQueue].x, searchQueue[curentInQueue].y - 1);
                            break;
                        case 2: //Left
                            neigbour = new Vector2(searchQueue[curentInQueue].x - 1, searchQueue[curentInQueue].y);
                            break;
                        case 3: //Right
                            neigbour = new Vector2(searchQueue[curentInQueue].x + 1, searchQueue[curentInQueue].y);
                            break;
                    }

                    invalidNeigbour = false;

                    if (neigbour.x < 0 || neigbour.y < 0 || neigbour.x > terrainData.alphamapWidth || neigbour.y > terrainData.alphamapHeight)
                    {
                        invalidNeigbour = true; //not realy repeated but outside the bounds of the map
                    }
                    else
                    {
                        for (int j = 0; j < queueSize; j++)
                        {
                            if (searchQueue[j] == neigbour)
                            {
                                invalidNeigbour = true;
                            }
                        }
                    }

                    if (!invalidNeigbour)
                    {
                        queueSize++;
                        searchQueue[queueSize] = neigbour;
                    }
                }

                curentInQueue++;
            }
            else
            {
                found = true;
            }

            
        }

        
        return searchQueue[curentInQueue];
    }
}