using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupUIController : MonoBehaviour
{
    #region Variables
    [Header("Setup UI Elements")]
    [SerializeField]
    private GameObject setupScreen;
    [SerializeField]
    private GameObject setupContainer;
    [SerializeField]
    private TMP_InputField gridSizeXInputField;
    [SerializeField]
    private TMP_InputField gridSizeYInputField;
    [SerializeField]
    private Button createButton;

    private static readonly string MIN_INPUT_VALUE = "2";
    #endregion

    #region Unity Functions
    void Awake()
    {
        SubscribeButtons();
        setupScreen.SetActive(false);
        gridSizeXInputField.text = MIN_INPUT_VALUE;
        gridSizeYInputField.text = MIN_INPUT_VALUE;
    }
    #endregion

    #region Methods
    void SubscribeButtons()
    {
        // Setup
        createButton.onClick.AddListener(CreateButtonPressed);
        gridSizeXInputField.onValueChanged.AddListener(delegate { GridSizeXInputFieldValueChanged(); });
        gridSizeYInputField.onValueChanged.AddListener(delegate { GridSizeYInputFieldValueChanged(); });
    }

    public void Display()
    {
        // Enable setup container
        setupContainer.SetActive(true);

        // Enable setup screen
        setupScreen.SetActive(true);
    }

    void CreateButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Get grid size
        int x = int.Parse(gridSizeXInputField.text);
        int y = int.Parse(gridSizeYInputField.text);
        int min = int.Parse(MIN_INPUT_VALUE);
        if (x < min)
            x = min;
        if (y < min)
            y = min;

        // Initialize Grid Search
        AppManager.Instance.InitializeGridSearch(new Vector2Int(x, y));

        // Disable setup screen
        setupScreen.SetActive(false);
    }

    void GridSizeXInputFieldValueChanged()
    {
        string input = gridSizeXInputField.text;
        if (!Regex.IsMatch(input, @"^\d+$"))
            gridSizeXInputField.text = MIN_INPUT_VALUE;
    }

    void GridSizeYInputFieldValueChanged()
    {
        string input = gridSizeYInputField.text;
        if (!Regex.IsMatch(input, @"^\d+$"))
            gridSizeYInputField.text = MIN_INPUT_VALUE;
    }

    #endregion
}
