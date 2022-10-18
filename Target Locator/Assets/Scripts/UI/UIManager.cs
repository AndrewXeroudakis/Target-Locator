
public class UIManager : Singleton<UIManager>
{
    #region Variables
    public SetupUIController setupUIController;
    public MainUIController mainUIController;
    #endregion

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        AudioManager.Instance.PlaySound("Music");
    }
    #endregion

    #region Methods
    public void PlayOptionSelectedSFX()
    {
        AudioManager.Instance.PlaySound("Click");
    }
    #endregion
}
