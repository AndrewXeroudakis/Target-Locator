using UnityEngine;
using DG.Tweening;

public class Node : MonoBehaviour
{
    #region Variables
    public Vector3 worldPosition;
    public bool hasDropped = false;
    Tween moveTween;
    Tween rotateTween;

    public static readonly float RADIUS = 0.5f;
    public static readonly float Y_POSITION = -0.125f;
    private static readonly float Y_POSITION_DROP_OFFSET = 50f;
    private static readonly float DROP_DELAY_MULTIPLIER = 0.075f;
    private static readonly float DROP_ANGLE_OFFSET = 20f;
    private static readonly float TWEEN_DURATION = 3f;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        hasDropped = false;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Plays a drop animation that disables the node on complete
    /// </summary>
    /// <param name="_currentWorldPosition">The current world position of the character</param>
    /// <returns></returns>
    public void Drop(Vector3 _currentWorldPosition)
    {
        hasDropped = true;
        float distance = Vector3.Distance(worldPosition, _currentWorldPosition);
        float angle = Vector3.Angle(worldPosition, _currentWorldPosition);
        moveTween = transform.DOMove(new Vector3(worldPosition.x, worldPosition.y - Y_POSITION_DROP_OFFSET, worldPosition.z), TWEEN_DURATION).SetDelay(distance * DROP_DELAY_MULTIPLIER).SetEase(Ease.InSine).OnComplete(() => gameObject.SetActive(false));
        rotateTween = transform.DORotate(new Vector3(Random.Range(angle - DROP_ANGLE_OFFSET, angle + DROP_ANGLE_OFFSET), 0f, 0f), TWEEN_DURATION);
    }
    #endregion
}
