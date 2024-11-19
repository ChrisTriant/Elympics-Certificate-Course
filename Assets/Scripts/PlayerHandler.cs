using Elympics;
using UnityEngine;

public class PlayerHandler : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    #region Fields

    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MovementHandler _movementHandler;
    [SerializeField] private ActionHandler _actionHandler;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private int _playerID;

    #endregion

    #region Properties

    public PlayerHealth Health => _playerHealth;

    public int ID => _playerID;

    #endregion

    #region LifeCycle

    private void Update()
    {
        if (Elympics.Player == PredictableFor)
            _inputHandler.UpdateInput();
    }

    #endregion

    #region Public Methods

    public void ElympicsUpdate()
    {
        GatheredInput currentInput;
        currentInput.MovementInput = Vector2.zero;
        currentInput.MouseWorldPosition = _inputHandler.DefaultMouseWorldPosition;
        currentInput.Attack = false;
        currentInput.Block = false;

        if(ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
        {
            inputReader.Read(out currentInput.MovementInput.x);
            inputReader.Read(out currentInput.MovementInput.y);
            inputReader.Read(out currentInput.MouseWorldPosition.x);
            inputReader.Read(out currentInput.MouseWorldPosition.y);
            inputReader.Read(out currentInput.MouseWorldPosition.z);
            inputReader.Read(out currentInput.Attack);
            inputReader.Read(out currentInput.Block);
        }

        _movementHandler.HandleMovement(currentInput.MovementInput, currentInput.MouseWorldPosition);
        _actionHandler.HandleActions(currentInput.Attack , currentInput.Block, Elympics.Tick);
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
    }


    public void OnInputForBot(IInputWriter inputSerializer)
    {

    }

    #endregion
}
