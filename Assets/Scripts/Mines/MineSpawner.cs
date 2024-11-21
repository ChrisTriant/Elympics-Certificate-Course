using UnityEngine;
using Elympics;

public class MineSpawner : ElympicsMonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject _minePrefab;
    [SerializeField] private long _mineCooldown;
    private long _mineAvailableTick;

    #endregion

    #region Public Methods

    public void TrySpawningMine(long tick)
    {
        if(Elympics.IsServer && tick >= _mineAvailableTick)
        {
            _mineAvailableTick = tick + _mineCooldown;
            MineHandler mineHandler = ElympicsInstantiate(_minePrefab.name, ElympicsPlayer.World).GetComponent<MineHandler>();
            mineHandler.SetupMine(transform.position, tick);
        }
    }

    #endregion
}
