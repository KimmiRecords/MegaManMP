using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    [SerializeField] NetworkRigidbody _rgbd;

    [SerializeField]
    float _dmg;
    [SerializeField]
    float _initialForce;



    void Start()
    {
        _rgbd.Rigidbody.AddForce(transform.forward * _initialForce, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) //si me toca alguien que no es stateauth retorna. las balas solo pueden da�arme a mi mismo xd
        {
            return;
        }

        if (other.TryGetComponent(out PlayerModel otherPlayer))
        {
            otherPlayer.TakeDamage(_dmg); //hace da�o al que toca
        }

        Runner.Despawn(Object); //elimina esta bala
    }
}
