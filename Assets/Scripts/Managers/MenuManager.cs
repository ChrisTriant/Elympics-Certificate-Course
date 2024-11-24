using Elympics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private Button _playOnlineButton;

    private bool _searchInProgress;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        AwaitServerConnection();
    }

    #endregion

    #region Public Methods

    public void FindGame()
    {
        if (_searchInProgress) return;
        _searchInProgress = true;

        ElympicsLobbyClient.Instance.RoomsManager.StartQuickMatch("Default");
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    #region Private Methods

    private async void AwaitServerConnection()
    {
        _playOnlineButton.interactable = false;

        await ElympicsLobbyClient.Instance.ConnectToElympicsAsync(new ConnectionData()
        {
            AuthType = Elympics.Models.Authentication.AuthType.ClientSecret
        });

        _playOnlineButton.interactable = true;
    }

    #endregion
}
