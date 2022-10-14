using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    #region Variables
    private static readonly float Y_POSITION = 0.5f;
    // animation controller
    // current position on the grid

    // Grid Searching
    public Vector2Int previousGridPosition;

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
        Debug.Log("target previousGridPosition = " + previousGridPosition);
        // set world position
        Vector3 nodeWorldPosition = AppManager.Instance.gridController.GetWorldPosition(_gridPosition);
        transform.position = new Vector3(nodeWorldPosition.x, Y_POSITION, nodeWorldPosition.z);

        // enable gameObject
        gameObject.SetActive(true);

        // set state
        //ChangeState(State.Idle);

        // play spawn vfx
        // play spawn sfx
    }

    #region Grid Searching 

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
        yield return new WaitForSeconds(1f);
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
