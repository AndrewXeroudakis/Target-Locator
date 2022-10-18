using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUIController : MonoBehaviour
{
    #region Variables
    [Header("Main UI Elements")]
    [SerializeField]
    private GameObject mainScreen;
    [SerializeField]
    private GameObject mainContainer;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private TMP_Text directionsText;
    [SerializeField]
    private TMP_Text costText;
    [SerializeField]
    private GameObject bottomSideUI;
    #endregion

    #region Unity Functions
    void Awake()
    {
        SubscribeButtons();
        mainScreen.SetActive(false);
    }
    #endregion

    #region Methods
    void SubscribeButtons()
    {
        // Setup
        restartButton.onClick.AddListener(RestartButtonPressed);
    }

    public void Display()
    {
        // Enable main container
        mainContainer.SetActive(true);

        // Enable main screen
        mainScreen.SetActive(true);

        // Disable restart button
        restartButton.gameObject.SetActive(false);
    }

    void RestartButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Disable setup screen
        mainScreen.SetActive(false);

        // Restart
        AppManager.Instance.Restart();
    }

    public void EnableRestart()
    {
        // Enable restart button
        restartButton.gameObject.SetActive(true);
    }

    public void EnableCredits()
    {
        // Enable credits
        bottomSideUI.SetActive(true);
    }

    public void AddDirection(Vector2Int _currentGridPosition, string _direction)
    {
        directionsText.text += string.Format("\n- {0}, {1}", _currentGridPosition, _direction);
    }

    public void SetCost(int _cost)
    {
        costText.text = "Cost: " + _cost;
    }
    #endregion
}
