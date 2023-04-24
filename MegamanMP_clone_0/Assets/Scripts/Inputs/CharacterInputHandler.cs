using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    float _moveInput;
    bool _isJumpPressed;
    bool _isFirePressed;

    PlayerModel _myModel;
    NetworkInputData _inputData;

    void Start()
    {
        _myModel = GetComponent<PlayerModel>();

        //if (!_myModel.HasInputAuthority)
        //{
        //    return;
        //}
        //else
        //{
        //    UpdateManager.Instance.RegisterUpdate();
        //}

        _inputData = new NetworkInputData();

    }

    //void FakeUpdate()
    //{
    //    //hay que hacerlo asi, disparando el update solo si tengo input auth. asi ejecuta mi update pero no el de los 15 enemigos
    //}

    void Update()
    {
        if (!_myModel.HasInputAuthority) //solo toma inputs de playermodels con input auth
        {
            return;
        }

        if (!GameManager.Instance.playerAgency) //solo toma inputs de playermodels con input auth
        {
            return;
        }


        _moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            _isJumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }
    }


    //esto lo ejecuta el spawner para enterarse de los inputs
    public NetworkInputData GetNetworkInput()
    {

        _inputData.movementInput = _moveInput;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        return _inputData;
    }

}
