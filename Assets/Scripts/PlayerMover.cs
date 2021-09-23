using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover:MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _speed;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float  _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] private Transform _headChecker;

    [Header(("Animation"))]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;

    private float _direction;
    private bool _Jump;
    private bool _crawl;
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
        Debug.Log(message: "Hp raised" + hpPoints); 
    }

    public void AddMp(int mpPoints)
    {
        Debug.Log(message: "Mp raised" + mpPoints);
    }

    public void AddCoin(int coinPoints)
    {
        Debug.Log(message: "Coin raised" + coinPoints);
    }
}
 