using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pointsbar : MonoBehaviour
{
    Transform _target;
    [SerializeField] float _yOffset;
    [SerializeField] TMPro.TextMeshPro _myText;

    public Pointsbar SetTarget(PlayerModel player)
    {
        _target = player.transform;
        player.OnPlayerDestroyed += () => Destroy(gameObject);
        player.OnUpdatePointsbar += UpdateBar;
        return this;
    }

    public void UpdatePosition()
    {
        transform.position = _target.position + Vector3.up * _yOffset;
    }

    public void UpdateBar(float amount)
    {
        _myText.text = amount.ToString();
    }

}
