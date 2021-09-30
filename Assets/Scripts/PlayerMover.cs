using System;
using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;




[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    

    [SerializeField] private float _speed;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] private Transform _headChecker;

    [Header(("Animation"))]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;

    [SerializeField] private int _maxHp;
    private int _currentHp;

    [Header("UI")]
    [SerializeField] private TMP_Text _coinAmountText;
    [SerializeField] private Slider _hpBar;

    private float _direction;
    private bool _Jump;
    private bool _crawl;
    

    private int _coinsAmount;
    public int CoinsAmount
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinAmountText.text = value.ToString();
        }
    }

    private int CurrentHp
    {
        get => _currentHp;
         
        set
        {
            if (value > _maxHp)
                value = _maxHp; 
            _currentHp = value;
            _hpBar.value = value;
        }
    }


    private void Start()
    {
        CoinsAmount = 0;
        _hpBar.maxValue = _maxHp;
        CurrentHp = _maxHp;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");

        _animator.SetFloat(_runAnimatorKey, value: Mathf.Abs(_direction));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _Jump = true;
         
        }

        if (_direction>0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false; 
        }
        else if(_direction<0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }

        _crawl = (Input.GetKey(KeyCode.C));
        

    }
     private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(x: _direction * _speed, _rigidbody.velocity.y);

        bool canJump = (bool)Physics2D.OverlapCircle((Vector2)_groundChecker.position, _groundCheckerRadius,(int)_whatIsGround);
        bool canStand =!Physics2D.OverlapCircle((Vector2)_headChecker.position, _headCheckerRadius , (int)_whatIsGround);

        _headCollider.enabled = !_crawl && canStand;

        if (_Jump&&canJump)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _Jump = false; 
        }


        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crouchAnimatorKey, !_headCollider.enabled); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
    }

    public void AddHp(int hpPoints)
    {

        int missingHp = _maxHp - CurrentHp;
        int pointToAdd = missingHp > hpPoints ? hpPoints : missingHp;
        StartCoroutine(RestoreHp(pointToAdd));
    }

    private IEnumerator RestoreHp(int pointToAdd)
    {
        while (pointToAdd != 0)
        {
            pointToAdd--;
            CurrentHp++;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void AddMp(int mpPoints)
    {
        Debug.Log(message: "Mp raised" + mpPoints);
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (_currentHp <= 0) 
        {
            gameObject.SetActive(false);
            Invoke(nameof(ReloadScene), 1f);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
 