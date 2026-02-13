using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private float _initialTime;
    [SerializeField] private UnityEvent _onEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_initialTime > 0)
                StartCoroutine(InvokeEvent());
            else _onEnter.Invoke();
        }
    }

    private IEnumerator InvokeEvent()
    {
        yield return new WaitForSeconds(_initialTime);
        _onEnter.Invoke();
    }
}
