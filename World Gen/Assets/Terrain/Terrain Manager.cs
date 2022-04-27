using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainManager
{
    // Terrain Max Weight [X location, Y location] = Heavyest Splat Weihght
    private static int[,] terrainMaxW;

    // Rerrain Resource Data [x location, y location] = Resource available
    private static int[,] terrainResourceData;

    //Terrain Max Weight FUNCTIONS//

    public static void InitTerrainMaxW(int xSize, int ySize)
    {
        terrainMaxW = new int[xSize, ySize];
    }

    public static int GetTerrainMaxW(int x, int y)
    {
        return terrainMaxW[x, y];
    }

    public static void SetTerrainMaxW(int x, int y, int value)
    {
        terrainMaxW[x, y] = value;
    }

    // Terrain Resource Data Functions
    //init array
    public static void InitTerrainResourceData(int xSize, int ySize)
    {
        terrainResourceData = new int[xSize, ySize];
    }

    public static int GetTerrainResourceData(int x, int y)
    {
        return terrainResourceData[x, y];
    }

    public static void SetTerrainResourceData(int x, int y, int value)
    {
        terrainResourceData[x, y] = value;
    }
    

    //get

    //set

    //ENUMS//
    //Terrain types//
    public enum terrainTypes
    {
        Basic,
        Rock,
        Lush,
        Grass,
        Ice
    }

    public enum resources
    {
        None,
        Tree,
        Rock
    }

    public enum directions
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}
