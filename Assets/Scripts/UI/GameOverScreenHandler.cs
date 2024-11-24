using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenHandler : MonoBehaviour
{
    #region Events

    public event Action OnBackToMenu = delegate { };
    public event Action OnQuitGame = delegate { };

    #endregion

    #region Fields

    [SerializeField] private TMP_Text _winnerTitle;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Button _quitGameButton;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        BindEvents();
    }

    private void OnDestroy()
    {
        UnbindEvents();
    }

    #endregion

    #region Public Methods

    public void DisplayGameOver(PlayerHandler winner)
    {
        string winnerTitle = "Draw";
        Color winnerColor = Color.white;

        if(winner != null)
        {
            winnerTitle = winner.PlayerName;
            winnerColor = winner.PlayerColor;
        }

        gameObject.SetActive(true);

        _winnerTitle.text = winnerTitle;
        _winnerTitle.colorGradient = new VertexGradient(Color.white, Color.white, winnerColor, winnerColor);
    }

    public void RevertGameOverScreen()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Private Methods

    private void BindEvents()
    {
        _backToMenuButton.onClick.AddListener(BackToMenu);
        _quitGameButton.onClick.AddListener(QuitGame);
    }

    private void UnbindEvents()
    {
        _backToMenuButton.onClick.RemoveListener(BackToMenu);
        _quitGameButton.onClick.RemoveListener(QuitGame);
    }

    private void BackToMenu() => OnBackToMenu.Invoke();

    private void QuitGame() => OnQuitGame.Invoke();

    #endregion
}