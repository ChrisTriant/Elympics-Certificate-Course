using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region Constants

    private const float MAX_RAYCAST_RANGE = 1000;

    #endregion

    #region Fields

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private LayerMask _mouseRaycastMask;

    private GatheredInput _gatheredInput;

    #endregion

    #region Properties

    public Vector3 DefaultMouseWorldPosition => transform.position + transform.forward;

    #endregion

    #region Public Methods

    public void UpdateInput()
    {
        _gatheredInput.MouseWorldPosition = GetMouseWorldPosition();
        _gatheredInput.MovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Gather ability input and preserve the values across frames and until the GetInput method is called.
        _gatheredInput.Attack = Input.GetKeyDown(KeyCode.Mouse0) || _gatheredInput.Attack;  
        _gatheredInput.Block = Input.GetKeyDown(KeyCode.Mouse1) || _gatheredInput.Block;
        _gatheredInput.SpawnMine = Input.GetKeyDown(KeyCode.Space) || _gatheredInput.SpawnMine;
    }

    public GatheredInput GetInput()
    {
        GatheredInput returnedInput = _gatheredInput;
        _gatheredInput.MovementInput = Vector2.zero;
        _gatheredInput.MouseWorldPosition = DefaultMouseWorldPosition;
        _gatheredInput.Attack = false;
        _gatheredInput.Block = false;
        _gatheredInput.SpawnMine = false;
        return returnedInput;
    }

    #endregion

    #region Private Methods

    private Vector3 GetMouseWorldPosition()
    {
        var ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, MAX_RAYCAST_RANGE, _mouseRaycastMask))
            return hit.point;
        else
            return DefaultMouseWorldPosition;
    }


    #endregion
}

public struct GatheredInput
{
    public Vector2 MovementInput;
    public Vector3 MouseWorldPosition;
    public bool Attack;
    public bool Block;
    public bool SpawnMine;
}
