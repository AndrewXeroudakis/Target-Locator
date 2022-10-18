using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private GameObject nodesParent;
    public GameObject NodesParent { get { return nodesParent; } private set { nodesParent = value; } }

    public GridController gridController;
    #endregion

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // Display setup screen
        UIManager.Instance.setupUIController.Display();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    #endregion

    #region Methods
    public void InitializeGridSearch(Vector2Int _gridSize)
    {
        // Create grid
        gridController = new GridController();
        gridController.CreateGrid(_gridSize);

        // Set target
        target.Spawn(TargetLocator.GenerateTargetGridPosition(gridController.GridSize));

        // Set character
        character.Spawn(gridController.GetCharacterStartingGridPosition());

        // Display main screen
        UIManager.Instance.mainUIController.Display();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
