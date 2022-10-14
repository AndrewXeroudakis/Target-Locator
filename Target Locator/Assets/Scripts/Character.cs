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
        // 1: Get to the edge for any previousGridPosition and direction
        Debug.Log("// --- GetNextGridPositionToSearch --- //");
        Debug.Log(currentDirection);
        Vector2Int directionVector = GridController.directionVectors[currentDirection];
        Debug.Log(directionVector);

        Vector2Int gridSize = AppManager.Instance.gridController.GridSize;
        Vector2Int nextGridPosition = previousGridPosition;
        do
        {
            nextGridPosition += directionVector;
        } while (nextGridPosition.x > 0 && nextGridPosition.x < (gridSize.x - 1)/2 && nextGridPosition.y > 0 && nextGridPosition.y < (gridSize.y - 1)/2 );
        return nextGridPosition;

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

        // Ask for direction
        Debug.Log("character previousGridPosition = " + previousGridPosition);
        currentDirection = TargetLocator.GetDirectionToTarget(previousGridPosition);
        Debug.Log(currentDirection);

        if (currentDirection != TargetLocator.Direction.OnTarget)
            Debug.Log("Begin Search");

        //while (currentDirection != TargetLocator.Direction.OnTarget || Input.GetKey(KeyCode.Backspace))
        {
            // get next grid position
            previousGridPosition = currentGridPosition;
            currentGridPosition = GetNextGridPositionToSearch();
            Debug.Log(currentGridPosition);

            Vector3 nodeWorldPosition = AppManager.Instance.gridController.GetWorldPosition(currentGridPosition);
            Vector3 characterWorldPosition = new Vector3(nodeWorldPosition.x, Y_POSITION, nodeWorldPosition.z);

            // move to that position using DOTween
            float distance = Vector3.Distance(transform.position, characterWorldPosition);
            transform.DOMove(characterWorldPosition, distance * 0.5f);


            // Move to direction
            //while (currentGridPosition != || Input.GetKey(KeyCode.Backspace))
            {
                // Move to direction
            }
        }

        //Debug.Log("Target Reached!");
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
