using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController
{
    #region Variables
    public Vector2Int GridSize { get; private set; }
    Node[,] grid;

    /// <summary>
    /// Vectors corresponding to the direction given by the TargetLocator
    /// </summary>
    public static Dictionary<TargetLocator.Direction, Vector2Int> directionVectors = new Dictionary<TargetLocator.Direction, Vector2Int>()
    {
        { TargetLocator.Direction.Up,           new Vector2Int(-1, 0)   },
        { TargetLocator.Direction.Down,         new Vector2Int(1, 0)    },
        { TargetLocator.Direction.Left,         new Vector2Int(0, -1)   },
        { TargetLocator.Direction.Right,        new Vector2Int(0, 1)    },
        { TargetLocator.Direction.UpLeft,       new Vector2Int(-1, -1)  },
        { TargetLocator.Direction.UpRight,      new Vector2Int(-1, 1)   },
        { TargetLocator.Direction.DownLeft,     new Vector2Int(1, -1)   },
        { TargetLocator.Direction.DownRight,    new Vector2Int(1, 1)    },
        { TargetLocator.Direction.OnTarget,     new Vector2Int(0, 0)    },
    };
    #endregion

    #region Unity Functions

    #endregion

    #region Methods
    /// <summary>
    /// Creates the grid from a given size
    /// </summary>
    /// <param name="_gridSize">The size of the grid</param>
    /// <returns></returns>
    public void CreateGrid(Vector2Int _gridSize)
    {
        GridSize = _gridSize;
        grid = new Node[_gridSize.x, _gridSize.y]; // x = x, y = z

        //AppManager.Instance.StartCoroutine(CreateGridEnumerator());

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                Vector3 nodeWorldPosition = GetWorldPosition(new Vector2Int(x, y));
                GameObject newNodeGameObject = MonoBehaviour.Instantiate(AppManager.Instance.NodePrefab, new Vector3(nodeWorldPosition.x, nodeWorldPosition.y/*10f*/, nodeWorldPosition.z), Quaternion.identity);
                Node newNode = newNodeGameObject.GetComponent<Node>();
                newNode.worldPosition = nodeWorldPosition;
                grid[x, y] = newNode;
            }
        }
    }

    private IEnumerator CreateGridEnumerator()
    {
        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                Vector3 nodeWorldPosition = GetWorldPosition(new Vector2Int(x, y));
                GameObject newNodeGameObject = MonoBehaviour.Instantiate(AppManager.Instance.NodePrefab, new Vector3(nodeWorldPosition.x, nodeWorldPosition.y/*10f*/, nodeWorldPosition.z), Quaternion.identity);
                Node newNode = newNodeGameObject.GetComponent<Node>();
                newNode.worldPosition = nodeWorldPosition;
                grid[x, y] = newNode;

                yield return new WaitForSeconds(0f); //0.005f
            }
        }
        
        // Invoke event when finished creating
    }

    public Vector3 GetWorldPosition(Vector2Int _gridPosition) => new Vector3(_gridPosition.x + Node.RADIUS, Node.Y_POSITION, _gridPosition.y + Node.RADIUS);

    public Vector2Int GetCharacterStartingGridPosition()
    {
        Vector2Int[] characterPositions = { Vector2Int.zero, new Vector2Int(GridSize.x - 1, 0), new Vector2Int(0, GridSize.y - 1), new Vector2Int(GridSize.x - 1, GridSize.y - 1) };
        return characterPositions[Random.Range(0, characterPositions.Length)];
    }

    #region Grid Visualization
    /// <summary>
    /// Plays a drop animation for every node that has been excluded from the search
    /// </summary>
    /// <param name="_min">The minimum grid position to be searched</param>
    /// /// <param name="_max">The maximum grid position to be searched</param>
    /// /// <param name="_current">The character's current world position</param>
    /// <returns></returns>
    public void DropExcludedNodes(Vector2Int _min, Vector2Int _max, Vector3 _current)
    {
        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                if (y < _min.y || x < _min.x || y > _max.y || x > _max.x)
                    if (!grid[x, y].hasDropped)
                        grid[x, y].Drop(_current);
            }
        }
    }

    private void HighlightNode()
    {

    }
    #endregion

    #endregion
}

