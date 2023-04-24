using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class CanvasLifebar : MonoBehaviour
{
    [SerializeField] Lifebar _lifebarPrefab;

    public event Action OnUpdateBar = delegate { };

    public void SpawnBar(PlayerModel target)
    {
        Lifebar lifebar = Instantiate(_lifebarPrefab, target.transform.position, Quaternion.identity, transform).SetTarget(target);
        OnUpdateBar += lifebar.UpdatePosition; //me suscribo
        target.OnPlayerDestroyed += () => OnUpdateBar -= lifebar.UpdatePosition; //me dessuscribo
    }

    void LateUpdate()
    {
        OnUpdateBar(); //triggereo en cad frame
    }
}
