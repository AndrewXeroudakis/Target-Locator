using UnityEngine;

public class Target : Singleton<Target>
{
    #region Variables
    private static readonly float Y_POSITION = 0.5f;
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
        // Set world position
        Vector3 nodeWorldPosition = AppManager.Instance.gridController.GetWorldPosition(_gridPosition);
        transform.position = new Vector3(nodeWorldPosition.x, Y_POSITION, nodeWorldPosition.z);

        // Enable gameObject
        gameObject.SetActive(true);
    }

    #endregion
}
