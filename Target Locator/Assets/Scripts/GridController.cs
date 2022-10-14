using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController
{
    #region Variables
    public Vector2Int GridSize { get; private set; }
    Node[,] grid;

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
    public void CreateGrid(Vector2Int _gridSize)
    {
        GridSize = _gridSize;
        grid = new Node[_gridSize.x, _gridSize.y]; // x = x, y = z

        AppManager.Instance.StartCoroutine(CreateGridEnumerator());
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

                yield return new WaitForSeconds(0.005f);
            }
        }
        
        // Invoke event finished creating
    }

    public Vector3 GetWorldPosition(Vector2Int _gridPosition) => new Vector3(_gridPosition.x + Node.RADIUS, Node.Y_POSITION, _gridPosition.y + Node.RADIUS);

    public Vector2Int GetCharacterStartingGridPosition()
    {
        Vector2Int[] characterPositions = { Vector2Int.zero, new Vector2Int(GridSize.x - 1, 0), new Vector2Int(0, GridSize.y - 1), new Vector2Int(GridSize.x - 1, GridSize.y - 1) };
        return characterPositions[Random.Range(0, characterPositions.Length)];
    }

    private void HighlightCell()
    {

    }
    #endregion
}

