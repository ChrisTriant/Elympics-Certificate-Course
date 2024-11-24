using Elympics;
using System;
using UnityEngine;

public class PlayerHandler : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    #region Events

    [SerializeField] private PlayerHandlerEventChannelSO _onPlayerDied;

    #endregion

    #region Fields

    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MovementHandler _movementHandler;
    [SerializeField] private ActionHandler _actionHandler;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private int _playerID;
    [SerializeField] private string _playerName;
    [SerializeField] private Color _playerColor;

    private ElympicsBool _isPlayerActive = new();

    #endregion

    #region Properties

    public PlayerHealth Health => _playerHealth;

    public int ID => _playerID;
    public string PlayerName => _playerName;
    public Color PlayerColor => _playerColor;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        _isPlayerActive.Value = true;
        BindEvents();
    }

    private void Update()
    {
        if (Elympics.Player == PredictableFor)
            _inputHandler.UpdateInput();
    }

    private void OnDestroy()
    {
        UnbindEvents();
    }

    #endregion

    #region Public Methods

    public void ElympicsUpdate()
    {
        if (!_isPlayerActive.Value) return;

        GatheredInput currentInput;
        currentInput.MovementInput = Vector2.zero;
        currentInput.MouseWorldPosition = _inputHandler.DefaultMouseWorldPosition;
        currentInput.Attack = false;
        currentInput.Block = false;
        currentInput.SpawnMine = false;

        if(ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
        {
            inputReader.Read(out currentInput.MovementInput.x);
            inputReader.Read(out currentInput.MovementInput.y);
            inputReader.Read(out currentInput.MouseWorldPosition.x);
            inputReader.Read(out currentInput.MouseWorldPosition.y);
            inputReader.Read(out currentInput.MouseWorldPosition.z);
            inputReader.Read(out currentInput.Attack);
            inputReader.Read(out currentInput.Block);
            inputReader.Read(out currentInput.SpawnMine);
        }

        _movementHandler.HandleMovement(currentInput.MovementInput, currentInput.MouseWorldPosition);
        _actionHandler.HandleActions(currentInput.Attack , currentInput.Block, currentInput.SpawnMine, Elympics.Tick);
    }

    public void OnInputForClient(IInputWriter inputSerializer)
    {
        var input = _inputHandler.GetInput();
        inputSerializer.Write(input.MovementInput.x);
        inputSerializer.Write(input.MovementInput.y);
        inputSerializer.Write(input.MouseWorldPosition.x);
        inputSerializer.Write(input.MouseWorldPosition.y);
        inputSerializer.Write(input.MouseWorldPosition.z);
        inputSerializer.Write(input.Attack);
        inputSerializer.Write(input.Block);
        inputSerializer.Write(input.SpawnMine);
    }

    public void EnablePlayer() => _isPlayerActive.Value = true;
    public void DisablePlayer() => _isPlayerActive.Value = false;


    public void OnInputForBot(IInputWriter inputSerializer)
    {

    }


    #endregion

    #region Private Methods

    private void BindEvents()
    {
        _playerHealth.SubcribeToHealthChange(OnHealthChanged);
    }

    private void UnbindEvents()
    {
        _playerHealth.UnsubcribeToHealthChange(OnHealthChanged);
    }

    private void OnHealthChanged(float lastValue, float newValue)
    {
        if (newValue == 0) _onPlayerDied.RaiseEvent(this);
    }

    #endregion
}
