using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] NetworkRigidbody _rgbd;
    [SerializeField] Animator _animator;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] ParticleSystem _shootParticle;

    [SerializeField] Transform _firePosition;

    //[Networked(OnChanged = nameof(LifeChangedCallback))]
    //[SerializeField] float _life { get; set; }
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] int _points;
    
    
    int _previousSign, _currentSign;
    float _lastFireTime;

    [Networked(OnChanged = nameof(ShootChangedCallback))]
    bool IsFiring { get; set; }

    //[Networked(OnChanged = nameof(PointsChangedCallback))]

    public event Action<float> OnUpdateLifebar = delegate { };
    public event Action OnPlayerDestroyed = delegate {};

    void Start()
    {
        transform.forward = Vector3.right;
    }

    public override void Spawned()
    {
        base.Spawned();
        CanvasLifebar lifebarManager = FindObjectOfType<CanvasLifebar>();
        lifebarManager?.SpawnBar(this);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            Movement(networkInputData.movementInput);

            if (networkInputData.isJumpPressed)
            {
                Jump();
            }

            if (networkInputData.isFirePressed)
            {
                Shoot();
            }
        }
    }
    void Movement(float xAxis)
    {
        if (xAxis != 0)
        {
            _rgbd.Rigidbody.MovePosition(transform.position + Vector3.right * xAxis * _speed * Time.fixedDeltaTime);

            _currentSign = (int)Mathf.Sign(xAxis);

            if (_previousSign != _currentSign)
            {
                _previousSign = _currentSign;

                transform.rotation = Quaternion.Euler(Vector3.up * 90 * _currentSign);
            }

            _animator.SetFloat("MovementValue", Mathf.Abs(xAxis));
        }
        else if (_currentSign != 0)
        {
            _currentSign = 0;

            _animator.SetFloat("MovementValue", 0);
        }
    }

    void Jump()
    {
        _rgbd.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }
    void Shoot()
    {
        //generamos un tiempo de disparo para q el autoshoot no dependa del ping

        if (Time.time - _lastFireTime < 0.15f)
        {
            return;
        }

        StartCoroutine(ShootCooldown());
        _lastFireTime = Time.time;

        Runner.Spawn(_bulletPrefab, _firePosition.position, transform.rotation);
    }
    IEnumerator ShootCooldown()
    {
        IsFiring = true;
        _shootParticle.Play();
        yield return null;
        IsFiring = false;
    }
    static void ShootChangedCallback(Changed<PlayerModel> changed)
    {
        bool updatedShoot = changed.Behaviour.IsFiring; //me guardo la variable que me traen (si se disparo en el ultimo frame)

        changed.LoadOld(); //cargo lo del frame anterior

        bool oldShoot = changed.Behaviour.IsFiring; //guardo lo viejo en otra var

        if (!oldShoot && updatedShoot) //si no estaba disparando y estoy disparando
        {
            changed.Behaviour._shootParticle.Play();
        }
    }
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //static void LifeChangedCallback(Changed<PlayerModel> changed)
    //{
    //    changed.Behaviour.OnUpdateLifebar(changed.Behaviour._life / 100f);
    //}
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //static void PointsChangedCallback(Changed<PlayerModel> changed)
    //{
    //    Debug.Log("[PlayerModel - PointsChangedCallback]");
    //    //changed.Behaviour.OnUpdateLifebar(changed.Behaviour._life / 100f);
    //}
    void RPC_GetHit(float dmg)
    {
        //_life -= dmg;
        //Debug.LogWarning("le sacaron vida a " + this);

        //if (_life <= 0)
        //{
        //    Dead();
        //}
    }
    public void TakeDamage(float dmg)
    {
        RPC_GetHit(dmg);
    }
    public void ReachAltar(int points)
    {
        Debug.Log("[PlayerModel] - reach altar. sume " + points + " points");
        _points += points;
    }

    void Dead()
    {
        Debug.LogWarning("Player " + this + " muerto");
        //GameManager.Instance.playerCount--;
        Runner.Shutdown(); //en realidad deberia mostrarse un YOU DIED o algo asi
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        
        OnPlayerDestroyed();
    }
}