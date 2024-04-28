using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid_Shop
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;

    public Grid_Shop(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width,height];

        for(int x=0; x<gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                UtilsClass.CreateWorldText(gridArray[x,z].ToString(), null, GetWorldPosition(x, z), 6, Color.white, TextAnchor.MiddleCenter);
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, z) * cellSize;
    }
}
