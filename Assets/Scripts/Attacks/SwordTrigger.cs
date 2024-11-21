using System;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    #region Events

    public Action<Collider> OnHitObject = delegate { };

    #endregion

    #region LifeCycle

    private void OnTriggerEnter(Collider other)
    {
        OnHitObject.Invoke(other);
    }

    #endregion
}
