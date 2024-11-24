using UnityEngine;
using UnityEngine.Events;

public class EventChannelBaseSO<T> : ScriptableObject
{
    public UnityAction<T> OnEventRaised;

    public void RaiseEvent(T obj) => OnEventRaised?.Invoke(obj);
}
