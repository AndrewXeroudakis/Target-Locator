using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : Singleton<Character>
{
    #region Variables
    private static readonly float Y_POSITION = 0.5f;
    // animation controller
    // current position on the grid

    // Grid Searching
    public Vector2Int previousGridPosition;
    public Vector2Int currentGridPosition;
    public TargetLocator.Direction currentDirection;

    // States
    public enum State
    {
        Idle,
        Move,
        AskForDirection,
        ReachedTarget
    }
    Coroutine currentStateCoroutine;
    #endregion

    #region Unity Functions
    #endregion

    #region Methods
    public void Spawn(Vector2Int _gridPosition)
    {
        previousGridPosition = _gridPosition;
        currentGridPosition = previousGridPosition;

        // set world position
        Vector3 nodeWorldPosition = AppManager.Instance.gridController.GetWorldPosition(_gridPosition);
        transform.position = new Vector3(nodeWorldPosition.x, Y_POSITION, nodeWorldPosition.z);

        // enable gameObject
        gameObject.SetActive(true);

        // set state
        ChangeState(State.Idle);

        // play spawn vfx
        // play spawn sfx
    }

    #region Grid Searching 


    private Vector2Int GetNextGridPositionToSearch()
    {
        // 1: Get to the middle for any previousGridPosition and direction
        Vector2Int directionVector = GridController.directionVectors[currentDirection];
        Vector2Int gridSize = AppManager.Instance.gridController.GridSize;

        int left = previousGridPosition.y;
        int right = currentGridPosition.y;
        int mid = Mathf.RoundToInt(left + ((right - left) / 2));
        Vector2Int nextGridPosition = new Vector2Int(previousGridPosition.x, mid);
        /*do
        {
            nextGridPosition += directionVector;
        } while (*//*nextGridPosition.x > 0 && nextGridPosition.x < edgeX &&*//* nextGridPosition.y > 0 && nextGridPosition.y < edgeY);*/
        return nextGridPosition;
        /*Vector2Int nextGridPosition = previousGridPosition;
        //int edgeX = Mathf.RoundToInt((gridSize.x - 1) / 2);
        int edgeY = Mathf.RoundToInt((gridSize.y - 1) / 2);
        do
        {
            nextGridPosition += directionVector;
        } while (*//*nextGridPosition.x > 0 && nextGridPosition.x < edgeX &&*//* nextGridPosition.y > 0 && nextGridPosition.y < edgeY);
        return nextGridPosition;*/

        /*Debug.Log(" --- GetNextGridPositionToSearch ---");
        Debug.Log(currentDirection);
        Vector2Int directionVector = GridController.directionVectors[currentDirection];
        Debug.Log(directionVector);
        int x = Mathf.RoundToInt(((AppManager.Instance.gridController.GridSize.x - 1) + (previousGridPosition.x * directionVector.x)) / 2);
        Debug.Log(x);
        int y = Mathf.RoundToInt(((AppManager.Instance.gridController.GridSize.y - 1) + (previousGridPosition.y * directionVector.y)) / 2);
        Debug.Log(y);
        return new Vector2Int(x, y);*/
    }
    #endregion

    #region States
    public void ChangeState(State _state)
    {
        // stop current state
        if (currentStateCoroutine != null)
            StopCoroutine(currentStateCoroutine);

        // start new state
        switch (_state)
        {
            case State.Idle:
                currentStateCoroutine = StartCoroutine(IdleState());
                break;
            case State.Move:
                currentStateCoroutine = StartCoroutine(MoveState());
                break;
            case State.AskForDirection:
                currentStateCoroutine = StartCoroutine(AskForDirectionState());
                break;
            case State.ReachedTarget:
                currentStateCoroutine = StartCoroutine(ReachedTargetState());
                break;
        }
    }

    private IEnumerator IdleState()
    {
        Debug.Log("Idle");
        yield return new WaitForSeconds(3f);

        int checks = 0;
        Vector2Int gridSize = AppManager.Instance.gridController.GridSize;
        Vector2Int min = Vector2Int.zero;
        Vector2Int max = new Vector2Int(gridSize.x - 1, gridSize.y - 1);
        while (min.x <= max.x && min.y <= max.y && checks < 100)
        {
            // Ask for direction
            currentDirection = TargetLocator.GetDirectionToTarget(currentGridPosition);
            Debug.Log(currentDirection);

            if (currentDirection == TargetLocator.Direction.OnTarget)
                break;
            else
            {
                Vector2Int directionVector = GridController.directionVectors[currentDirection];
                int midX = currentGridPosition.x;
                int midY = currentGridPosition.y;
                if (currentDirection == TargetLocator.Direction.Left)
                {
                    max = currentGridPosition + directionVector;
                    midY = min.y + ((max.y - min.y) / 2);
                }
                else if (currentDirection == TargetLocator.Direction.Right)
                {
                    min = currentGridPosition + directionVector;
                    midY = min.y + ((max.y - min.y) / 2);
                }
                else if (currentDirection == TargetLocator.Direction.Up)
                {
                    max = currentGridPosition + directionVector;
                    midX = min.x + ((max.x - min.x) / 2);
                }
                    
                else if (currentDirection == TargetLocator.Direction.Down)
                {
                    min = currentGridPosition + directionVector;
                    midX = min.x + ((max.x - min.x) / 2);
                }
                else if (currentDirection == TargetLocator.Direction.UpLeft)
                {
                    max = currentGridPosition + directionVector;
                    Vector2Int nextDiagonalGridPosition = GetNextDiagonalGridPosition(max, min.x, min.y, directionVector);
                    midX = nextDiagonalGridPosition.x;
                    midY = nextDiagonalGridPosition.y;
                }
                else if (currentDirection == TargetLocator.Direction.UpRight)
                {
                    min.y = currentGridPosition.y + directionVector.y;
                    max.x = currentGridPosition.x + directionVector.x;
                    Vector2Int nextDiagonalGridPosition = GetNextDiagonalGridPosition(currentGridPosition + directionVector, min.x, max.y, directionVector);
                    midX = nextDiagonalGridPosition.x;
                    midY = nextDiagonalGridPosition.y;
                }
                else if (currentDirection == TargetLocator.Direction.DownLeft)
                {
                    min.x = currentGridPosition.x + directionVector.x;
                    max.y = currentGridPosition.y + directionVector.y;
                    Vector2Int nextDiagonalGridPosition = GetNextDiagonalGridPosition(currentGridPosition + directionVector, max.x, min.y, directionVector);
                    midX = nextDiagonalGridPosition.x;
                    midY = nextDiagonalGridPosition.y;
                }
                else if (currentDirection == TargetLocator.Direction.DownRight)
                {
                    min = currentGridPosition + directionVector;
                    Vector2Int nextDiagonalGridPosition = GetNextDiagonalGridPosition(min, max.x, max.y, directionVector);
                    midX = nextDiagonalGridPosition.x;
                    midY = nextDiagonalGridPosition.y;
                }
                currentGridPosition = new Vector2Int(midX, midY);

                // move to that position using DOTween
                Vector3 nodeWorldPosition = AppManager.Instance.gridController.GetWorldPosition(currentGridPosition);
                Vector3 characterWorldPosition = new Vector3(nodeWorldPosition.x, Y_POSITION, nodeWorldPosition.z);
                float distance = Vector3.Distance(transform.position, characterWorldPosition);
                float duration = distance * 0.5f;
                transform.DOMove(characterWorldPosition, duration);

                yield return new WaitForSeconds(duration + 1f);

                checks++;
            }
        }

        Debug.Log("Target Reached!");
    }

    private Vector2Int GetNextDiagonalGridPosition(Vector2Int _start, int _limitX, int _limitY, Vector2Int _directionVector)
    {
        Vector2Int nextDiagonalGridPosition = _start;
        List<Vector2Int> diagonalGridPositions = new List<Vector2Int>();
        diagonalGridPositions.Add(nextDiagonalGridPosition);
        while (nextDiagonalGridPosition.x != _limitX && nextDiagonalGridPosition.y != _limitY)
        {
            nextDiagonalGridPosition += _directionVector;
            diagonalGridPositions.Add(nextDiagonalGridPosition);
        }
        int index = Mathf.RoundToInt(diagonalGridPositions.Count / 2);
        return diagonalGridPositions[index];
    }

    private IEnumerator MoveState()
    {
        Debug.Log("Move");
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator AskForDirectionState()
    {
        Debug.Log("AskForDirection");
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ReachedTargetState()
    {
        Debug.Log("ReachedTarget");
        yield return new WaitForSeconds(1f);
    }
    #endregion

    #endregion
}
