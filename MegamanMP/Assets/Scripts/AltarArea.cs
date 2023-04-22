using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class AltarArea : NetworkBehaviour
{
    //cuando tocas el altar te da 1 punto
    public int pointsGiven;

    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) //si me toca alguien que no es stateauth retorna. las balas solo pueden dañarme a mi mismo xd
        {
            return;
        }

        if (other.TryGetComponent(out PlayerModel otherPlayer))
        {
            Debug.Log(otherPlayer.gameObject.name + " entro al trigger");
            otherPlayer.ReachAltar(pointsGiven);
        }

    }
}
