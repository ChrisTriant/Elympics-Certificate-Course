using Elympics;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IObservable, IInitializable
{
    #region Fields

    [SerializeField] private float _maxHealth;
    [SerializeField] private ActionHandler _actionHandler;
    [SerializeField] private ElympicsFloat _currentHealth = new();

    #endregion

    #region Properties

    public float MaxHealth => _maxHealth;

    public float Health => _currentHealth.Value;

    #endregion

    #region Public Methods

    //IInitializable
    public void Initialize()
    {
        _currentHealth.Value = _maxHealth;
    }

    public void DealDamage(float damage)
    {
        if (_actionHandler.CurrentActionState == ActionHandler.ActionState.Blocking) return;

        if (_currentHealth.Value > 0)
        {
            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value - damage, 0, _maxHealth);
        }
    }

    public void SubcribeToHealthChange(ElympicsVar<float>.ValueChangedCallback onHealthChangedCallback) => _currentHealth.ValueChanged += onHealthChangedCallback;
    public void UnsubcribeToHealthChange(ElympicsVar<float>.ValueChangedCallback onHealthChangedCallback) => _currentHealth.ValueChanged -= onHealthChangedCallback;

    #endregion
}
