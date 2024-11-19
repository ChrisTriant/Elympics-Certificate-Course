using Elympics;
using UnityEngine;

public class ActionHandler : MonoBehaviour, IObservable
{
    #region Fields

    [SerializeField] private AbilityHandler _abilityHandler;

    private ElympicsInt _currentActionState = new();

    #endregion

    #region Properties

    public ActionState CurrentActionState => (ActionState)_currentActionState.Value;

    #endregion

    #region Public Methods

    public void HandleActions(bool attack, bool block, long tick)
    {
        ActionState lastState = (ActionState)_currentActionState.Value;
        switch (lastState)
        {
            case ActionState.Idle:
                if (block && _abilityHandler.CanBlock(tick))
                {
                    _abilityHandler.StartBlock(tick);
                    _currentActionState.Value = (int)ActionState.Blocking;
                }
                else if (attack)
                {
                    _abilityHandler.StartAttack(tick);
                    _currentActionState.Value = (int)ActionState.Attacking;
                }
                break;

            case ActionState.Attacking:
                if (block && _abilityHandler.CanBlock(tick))
                {
                    _abilityHandler.EndAttack(tick);
                    _abilityHandler.StartBlock(tick);
                    _currentActionState.Value = (int)ActionState.Blocking;
                }

                if (_abilityHandler.ProcessAttack(tick))
                    _currentActionState.Value = (int)ActionState.Idle;

                break;

            case ActionState.Blocking:
                _abilityHandler.ProcessBlock(tick);
                break;
        }
    }

    #endregion

    #region 

    public enum ActionState
    {
        Idle = 0,
        Attacking = 1,
        Blocking = 2
    }

    #endregion
}
