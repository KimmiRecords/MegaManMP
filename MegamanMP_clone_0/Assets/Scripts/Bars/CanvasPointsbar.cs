using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanvasPointsbar : MonoBehaviour
{
    [SerializeField] Pointsbar _pointsbarPrefab;

    public event Action OnUpdatePointsBar = delegate { };

    public void SpawnBar(PlayerModel target)
    {
        Pointsbar pointsbar = Instantiate(_pointsbarPrefab, target.transform.position, Quaternion.identity, transform).SetTarget(target);
        OnUpdatePointsBar += pointsbar.UpdatePosition; //me suscribo
        target.OnPlayerDestroyed += () => OnUpdatePointsBar -= pointsbar.UpdatePosition; //me dessuscribo
    }

    void LateUpdate()
    {
        OnUpdatePointsBar(); //triggereo en cad frame
    }
}