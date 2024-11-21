using Elympics;
using System;
using UnityEngine;

public class MineHandler : ElympicsMonoBehaviour, IUpdatable
{
    #region Events

    public Action<Collider> OnTriggered = delegate { };

    #endregion

    #region Fields

    [SerializeField] private Collider _collider;
    [SerializeField] private GameObject _inactiveMine;
    [SerializeField] private GameObject _activeMine;

    [SerializeField] private float _damage;
    [SerializeField] private int _armingTime;

    private ElympicsBool _shouldBeDestroyed = new();

    private long _armedTick;
    private bool _isArmed;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHandler>(out var otherHandler))
        {
            otherHandler.Health.DealDamage(_damage);
            _shouldBeDestroyed.Value = true;
        }
    }

    #endregion

    #region Public Methods

    public void ElympicsUpdate()
    {
        if (_shouldBeDestroyed.Value) 
            ElympicsDestroy(gameObject);

        if (_isArmed) return;

        if (Elympics.Tick > _armedTick)
            ArmMine();
    }

    public void SetupMine(Vector3 position, long tick)
    {
        transform.position = new Vector3(position.x, 0, position.z);
        _armedTick = tick + _armingTime;   
    }

    #endregion

    #region Private Methods

    private void ArmMine()
    {
        _isArmed = true;
        _collider.enabled = true;
        _activeMine.SetActive(true);
        _inactiveMine.SetActive(false);
    }

    #endregion
}
