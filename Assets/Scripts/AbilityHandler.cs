using UnityEngine;
using Elympics;

public class AbilityHandler : MonoBehaviour, IObservable
{
    #region Fields

    [SerializeField] private PlayerHandler _playerHandler;
    [SerializeField] private SwordTrigger _swordTrigger;
    [SerializeField] private Transform _swordPivot;
    [SerializeField] private GameObject _blockVisual;

    [SerializeField] private int _attackDuration;
    [SerializeField] private float _attackAngle;
    [SerializeField] private float _attackDamage;

    [SerializeField] private int _blockDuration;
    [SerializeField] private int _blockCooldown;

    private bool _alreadyHit;
    private long _attackEndTick;
    private ElympicsBool _swingRight = new();
    private bool _isAttacking = false;
    private long _blockAvailableTick;
    private long _blockEndTick;

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

    public void StartAttack(long tick)
    {
        _isAttacking = true;
        _swordTrigger.gameObject.SetActive(true);
        _alreadyHit = false;
        _attackEndTick = tick + _attackDuration;
    }

    public void EndAttack(long tick)
    {
        _isAttacking = false;
        _swordTrigger.gameObject.SetActive(false);
        _swingRight.Value = !_swingRight.Value;

        _swordPivot.localEulerAngles = (_swingRight.Value ? -1 : 1) * Vector3.up * _attackAngle;
    }

    public bool ProcessAttack(long tick)
    {
        if (tick >= _attackEndTick)
        {
            EndAttack(tick);
            return true;
        }
        else
        {
            float ratio = (float)(_attackEndTick - tick) / _attackDuration;
            float newRotation = Mathf.Lerp(-_attackAngle, _attackAngle, ratio) * (_swingRight.Value ? -1 : 1);
            _swordPivot.localEulerAngles = Vector3.up * newRotation;
            return false;
        }
    }

    public bool CanBlock(long tick) => tick > _blockAvailableTick;

    public void StartBlock(long tick)
    {
        _blockEndTick = tick + _blockDuration;
        _blockAvailableTick = _blockEndTick + _blockCooldown;
        _blockVisual.SetActive(true);
    }

    public void EndBlock(long tick)
    {
        _blockVisual.SetActive(false);
    }

    public void ProcessBlock(long tick)
    {
        if (tick > _blockEndTick)
            EndBlock(tick);
    }

    #endregion

    #region Private Methods

    private void BindEvents()
    {
        _swordTrigger.OnHitObject += OnHitObjectHandle;
    }

    private void UnbindEvents()
    {
        _swordTrigger.OnHitObject -= OnHitObjectHandle;
    }

    private void OnHitObjectHandle(Collider other)
    {
        if (!_isAttacking || _alreadyHit) return;

        var enemyAngle = Vector3.Angle(transform.forward, other.transform.position - transform.position);
        if (enemyAngle < _attackAngle)
        {
            if (other.TryGetComponent<PlayerHandler>(out var otherHandler))
            {
                if (otherHandler.ID != _playerHandler.ID)
                {
                    _alreadyHit = true;
                    otherHandler.Health.DealDamage(_attackDamage);
                }
            }
        }
    }

    #endregion
}
