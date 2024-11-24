using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    #region Fields

    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private ParticleSystem _damagedEffect;

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
        _playerHealth.SubcribeToHealthChange(OnHealthChanged);
    }

    private void UnbindEvents()
    {
        _playerHealth.UnsubcribeToHealthChange(OnHealthChanged);
    }

    private void OnHealthChanged(float lastValue, float newValue)
    {
        //Received Damage.
        if(newValue < lastValue)
            _damagedEffect.Play();
    }

    #endregion
}
