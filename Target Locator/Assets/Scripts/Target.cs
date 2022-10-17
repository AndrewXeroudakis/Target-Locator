using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    #region Variables
    private static readonly float Y_POSITION = 0.5f;

    // States
    public enum State
    {
        Idle,
        GiveDirection
    }
    Coroutine currentStateCoroutine;
    #endregion

    #region Unity Functions
    #endregion

    #region Methods
    public void Spawn(Vector2Int _gridPosition)
    {
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

    #endregion

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
            case State.GiveDirection:
                currentStateCoroutine = StartCoroutine(GiveDirectionState());
                break;
        }
    }

    private IEnumerator IdleState()
    {
        Debug.Log("Target Idle");
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator GiveDirectionState()
    {
        Debug.Log("Target GiveDirection");
        yield return new WaitForSeconds(1f);
    }
    #endregion

    #endregion
}
