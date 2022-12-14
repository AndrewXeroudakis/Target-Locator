using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : Singleton<Character>
{
    #region Variables
    private static readonly float Y_POSITION = 0.5f;

    [SerializeField]
    private GameObject fireworkVFX;

    [SerializeField]
    private GameObject nextPositionModel;

    // Grid Searching
    public Vector2Int currentGridPosition;

    // States
    public enum State
    {
        Idle,
        Searching,
        ReachedTarget
    }
    private Coroutine currentStateCoroutine;
    #endregion

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion

    #region Methods
    public void Spawn(Vector2Int _gridPosition)
    {
        currentGridPosition = _gridPosition;

        // set world position
        Vector3 nodeWorldPosition = AppManager.Instance.gridController.GetWorldPosition(_gridPosition);
        transform.position = new Vector3(nodeWorldPosition.x, Y_POSITION, nodeWorldPosition.z);

        // enable gameObject
        gameObject.SetActive(true);

        // set state
        ChangeState(State.Idle);
    }

    #region States
    public void ChangeState(State _state)
    {
        // stop current state
        if (currentStateCoroutine != null)
        {
            StopCoroutine(currentStateCoroutine);
            currentStateCoroutine = null;
        }

        // start new state
        switch (_state)
        {
            case State.Idle:
                currentStateCoroutine = StartCoroutine(IdleState());
                break;
            case State.Searching:
                currentStateCoroutine = StartCoroutine(SearchingState());
                break;
            case State.ReachedTarget:
                currentStateCoroutine = StartCoroutine(ReachedTargetState());
                break;
        }
    }

    private IEnumerator IdleState()
    {
        yield return new WaitForSeconds(3f);

        // Change State
        ChangeState(State.Searching);
    }

    private IEnumerator SearchingState()
    {
        yield return new WaitForSeconds(1f);

        nextPositionModel.gameObject.SetActive(true);
        nextPositionModel.transform.position = transform.position;
        TargetLocator.Direction currentDirection;
        Vector2Int gridSize = AppManager.Instance.gridController.GridSize;
        Vector2Int min = Vector2Int.zero;
        Vector2Int max = new Vector2Int(gridSize.x - 1, gridSize.y - 1);
        while (min.x <= max.x && min.y <= max.y)
        {
            // Ask for direction
            currentDirection = TargetLocator.GetDirectionToTarget(currentGridPosition);
            UIManager.Instance.mainUIController.AddDirection(currentGridPosition, currentDirection.ToString());
            UIManager.Instance.mainUIController.SetCost(TargetLocator.Cost);

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

                // Drop Nodes
                AppManager.Instance.gridController.DropExcludedNodes(min, max, AppManager.Instance.gridController.GetWorldPosition(currentGridPosition));
                AudioManager.Instance.PlaySound("DropNodes");

                // Move to the next grid position using DOTween
                currentGridPosition = new Vector2Int(midX, midY);
                Vector3 nodeWorldPosition = AppManager.Instance.gridController.GetWorldPosition(currentGridPosition);
                Vector3 characterWorldPosition = new Vector3(nodeWorldPosition.x, Y_POSITION, nodeWorldPosition.z);
                float distance = Vector3.Distance(transform.position, characterWorldPosition);
                float duration = distance * 0.25f;
                transform.DOMove(characterWorldPosition, duration);

                // set next position model position
                nextPositionModel.transform.position = characterWorldPosition;

                yield return new WaitForSeconds(duration + 1.5f);
            }
        }

        // Drop Nodes
        AppManager.Instance.gridController.DropExcludedNodes(currentGridPosition, currentGridPosition, AppManager.Instance.gridController.GetWorldPosition(currentGridPosition));

        // Change State
        ChangeState(State.ReachedTarget);
    }

    private IEnumerator ReachedTargetState()
    {
        PlayVFX();
        AudioManager.Instance.PlaySound("DropNodes");
        AudioManager.Instance.PlaySound("Fireworks");
        AudioManager.Instance.PlaySound("Cheers");
        UIManager.Instance.mainUIController.EnableCredits();

        yield return new WaitForSeconds(3f);

        UIManager.Instance.mainUIController.EnableRestart();
    }
    #endregion

    #region Grid Searching 

    private Vector2Int GetNextDiagonalGridPosition(Vector2Int _start, int _limitX, int _limitY, Vector2Int _direction)
    {
        Vector2Int nextDiagonalGridPosition = _start;
        List<Vector2Int> diagonalGridPositions = new List<Vector2Int>();
        diagonalGridPositions.Add(nextDiagonalGridPosition);
        while (nextDiagonalGridPosition.x != _limitX && nextDiagonalGridPosition.y != _limitY)
        {
            nextDiagonalGridPosition += _direction;
            diagonalGridPositions.Add(nextDiagonalGridPosition);
        }
        int index = Mathf.RoundToInt(diagonalGridPositions.Count / 2);
        return diagonalGridPositions[index];
    }

    #endregion

    #region VFX
    private void PlayVFX()
    {
        fireworkVFX.gameObject.SetActive(true);
    }
    #endregion

    #endregion
}
