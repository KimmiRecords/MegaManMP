using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class AltarArea : NetworkBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) //si me toca alguien que no es stateauth retorna. el altar solo me suma puntos a mi
        {
            return;
        }

        if (other.TryGetComponent(out PlayerModel otherPlayer))
        {
            Debug.Log(otherPlayer.gameObject.name + " entro al trigger");
            otherPlayer.EnterAltar();
        }
    }

    
}
