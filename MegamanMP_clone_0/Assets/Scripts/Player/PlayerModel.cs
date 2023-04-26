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
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;

    [Networked(OnChanged = nameof(LifeChangedCallback))]
    [SerializeField] float _life { get; set; }

    [Networked(OnChanged = nameof(PointsChangedCallback))]
    [SerializeField] int _points { get; set; }

    [Networked(OnChanged = nameof(ShootChangedCallback))]
    bool IsFiring { get; set; }

    int _previousSign, _currentSign;
    float _lastFireTime;

    public event Action<float> OnUpdateLifebar = delegate { };
    public event Action<float> OnUpdatePointsbar = delegate { };

    public event Action OnPlayerDestroyed = delegate {};

    [Networked(OnChanged = nameof(inAltarChangedCallback))]
    bool inAltar { get; set; }

    float _maxLife;
    float _maxSpeed;
    Vector3 spawnPosition;
    void Start()
    {
        transform.forward = Vector3.right;

        spawnPosition = transform.position;
        _maxLife = _life;
        _maxSpeed = _speed;
    }
    public override void Spawned()
    {
        base.Spawned();
        CanvasLifebar lifebarManager = FindObjectOfType<CanvasLifebar>(); //esto no es optimo
        CanvasPointsbar pointsbarManager = FindObjectOfType<CanvasPointsbar>(); //esto no es optimo

        lifebarManager?.SpawnBar(this);
        pointsbarManager?.SpawnBar(this);

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
    static void LifeChangedCallback(Changed<PlayerModel> changed)
    {
        changed.Behaviour.OnUpdateLifebar(changed.Behaviour._life / changed.Behaviour._maxLife);
    }

    static void PointsChangedCallback(Changed<PlayerModel> changed)
    {
        Debug.Log("[PlayerModel - PointsChangedCallback]");
        changed.Behaviour.OnUpdatePointsbar(changed.Behaviour._points);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_GetHit(float dmg)
    {
        _life -= dmg;
        Debug.LogWarning("le sacaron vida a " + this);

        if (_life <= 0)
        {
            Dead();
        }
    }
    public void TakeDamage(float dmg)
    {
        RPC_GetHit(dmg);
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_AddPoints()
    {
        _points++;
        transform.position = spawnPosition;
        Debug.LogWarning("le agregue points a " + this);
        StartCoroutine(ShowVictoryScreenCoroutine());
    }
    public void EnterAltar()
    {
        inAltar = true;
    }
    static void inAltarChangedCallback(Changed<PlayerModel> changed)
    {
        if (changed.Behaviour.inAltar)
        {
            changed.Behaviour.RPC_AddPoints();
            changed.Behaviour.inAltar = false;
        }
    }

    void Dead()
    {
        Debug.LogWarning("Player " + this + " muerto");
        transform.position = spawnPosition;
        _life = _maxLife;
        StartCoroutine(ShowDeathScreenCoroutine());

        //Runner.Shutdown(); //en realidad deberia mostrarse un YOU DIED o algo asi
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerDestroyed();
    }

    public IEnumerator ShowDeathScreenCoroutine()
    {
        float screenDuration = GameManager.Instance.ShowDeathScreen();
        yield return new WaitForSeconds(screenDuration);
        GameManager.Instance.HideDeathScreen();
    }
    public IEnumerator ShowVictoryScreenCoroutine()
    {
        _speed = 0;
        float screenDuration = GameManager.Instance.ShowVictoryScreen();

        yield return new WaitForSeconds(screenDuration);

        _speed = _maxSpeed;
        GameManager.Instance.HideVictoryScreen();

    }
}
