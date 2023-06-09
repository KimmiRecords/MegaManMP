using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    float _moveInput;
    bool _isJumpPressed;
    bool _isFirePressed;
    bool _isFire2Pressed;
    bool _isBootsPressed;

    PlayerModel _myModel;
    NetworkInputData _inputData;

    void Start()
    {
        _myModel = GetComponent<PlayerModel>();
        _inputData = new NetworkInputData();

        //if (!_myModel.HasInputAuthority)
        //{
        //    return;
        //}
        //else
        //{
        //    UpdateManager.Instance.RegisterUpdate();
        //}
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!GameManager.Instance.playerAgency) //solo toma inputs de playermodels con agency
        {
            return;
        }

        _moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _isFirePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _isFire2Pressed = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isBootsPressed = true;
        }
    }

    //esto lo ejecuta el playermodel para enterarse de los inputs
    public NetworkInputData GetNetworkInput()
    {
        _inputData.movementInput = _moveInput;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;
        _inputData.isFire2Pressed = _isFire2Pressed;
        _isFire2Pressed = false;

        _inputData.isBootsPressed = _isBootsPressed;
        _isBootsPressed = false;

        return _inputData;
    }

}
