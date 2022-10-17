using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    #region Variables
    [SerializeField]
    private Character character;
    [SerializeField]
    private Target target;
    [SerializeField]
    private GameObject nodePrefab;
    public GameObject NodePrefab { get { return nodePrefab; } private set { nodePrefab = value; } }

    public GridController gridController;
    #endregion

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // --- Test --- //
        // create grid
        gridController = new GridController();
        gridController.CreateGrid(new Vector2Int(10, 10));

        // set target
        target.Spawn(TargetLocator.GenerateTargetGridPosition(gridController.GridSize));

        // set character
        character.Spawn(gridController.GetCharacterStartingGridPosition());
    }
    #endregion

    #region Methods
    #endregion
}
