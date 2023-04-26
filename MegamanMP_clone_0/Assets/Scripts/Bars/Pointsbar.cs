using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pointsbar : MonoBehaviour
{
    Transform _target;
    [SerializeField] float _yOffset;
    [SerializeField] Image _myFillable;

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
        StopAllCoroutines();
        StartCoroutine(LerpAmount(amount));
        //_myFillable.fillAmount = amount;
    }

    IEnumerator LerpAmount(float amount)
    {
        float ticks = 0;
        float startAmount = _myFillable.fillAmount;
        while (ticks <= 0.5)
        {
            _myFillable.fillAmount = Mathf.Lerp(startAmount, amount, ticks);
            ticks += Time.deltaTime;
            yield return null;
        }
    }

}
