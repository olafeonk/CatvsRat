using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;
    
    [Serializable]
    public struct Grid
    {
        public int columns, rows;
        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;

    public GameObject gridTile;

    public List<Vector2> availablePoints = new List<Vector2>();

    private void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.columns = room.Width - 2;
        grid.rows = room.Height - 2;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        var localPosition = room.transform.localPosition;
        grid.verticalOffset += localPosition.y;
        grid.horizontalOffset += localPosition.x;

        for (var y = 0; y < grid.rows; y++)
        {
            for (var x = 0; x < grid.columns; x++)
            {
                var go = Instantiate(gridTile, transform);
                go.GetComponent<Transform>().position = new Vector2(x  - (grid.columns - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset));
                go.name = "X: " + x + ", Y: " + y;
                availablePoints.Add(go.transform.position);
                go.SetActive(false);
            }
        }
        GetComponentInParent<ObjectRoomSpawner>().InitialiseObjectSpawning();
    }
    
}