using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : ElympicsMonoBehaviour, IUpdatable
{
    #region Events

    [SerializeField] private PlayerHandlerEventChannelSO _onPlayerDied;

    #endregion

    #region Fields

    [SerializeField] private List<PlayerHandler> _players;

    [SerializeField] private long _timeBeforeGameOver;

    [SerializeField] private GameOverScreenHandler _gameOverScreenHandler;

    private List<PlayerHandler> _deadPlayers = new();
    private bool _gameEndTriggered = false;
    private long _gameOverTick;

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

    #region Private Methods

    private void BindEvents()
    {
        _onPlayerDied.OnEventRaised += HandlePlayerDeath;
        _gameOverScreenHandler.OnBackToMenu += BackToMenu;
        _gameOverScreenHandler.OnQuitGame += QuitGame;
    }

    private void UnbindEvents()
    {
        _onPlayerDied.OnEventRaised -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath(PlayerHandler player)
    {
        player.DisablePlayer();

        _deadPlayers.Add(player);

        if (_players.Count - _deadPlayers.Count == 1)
        {
            _gameOverTick = Elympics.Tick + _timeBeforeGameOver;
            _gameEndTriggered = true;
        }
    }

    public void ElympicsUpdate()
    {
        if(_gameEndTriggered && Elympics.Tick >= _gameOverTick)
        {
            _gameEndTriggered = false;

            if(Elympics.IsServer)
                Elympics.EndGame();

            var winner = _players.Except(_deadPlayers).FirstOrDefault();
            winner.DisablePlayer();
            _gameOverScreenHandler.DisplayGameOver(winner);
        }
    }

    #endregion

    #region Private Methods

    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
