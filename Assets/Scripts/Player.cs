using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float ThrustSpeed = 1.0f;
    public float TurnSpeed = 1.0f;
    public float InvulnerabilityTime = 2.0f;
    [SerializeField] private float shootDelay = 0.2f;

    [SerializeField]
    private Bullet bulletPrefab;
    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private bool _backing;
    private float _turnDirection;
    private float lastShootTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _backing = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            _turnDirection = 1.0f;
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            _turnDirection = -1.0f;
        }
        else{
            _turnDirection = 0.0f;
        }

        if((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        && Time.time >= lastShootTime + shootDelay){
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if(_thrusting){
            _rigidbody.AddForce(transform.up * ThrustSpeed);
        }

        if(_backing){
            _rigidbody.AddForce(transform.up * -ThrustSpeed / 2);
        }

        if(_turnDirection != 0.0f){
            _rigidbody.AddTorque(_turnDirection * TurnSpeed);
        }
    }

    private void Shoot()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Shoot);
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Projectile(transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Death);
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            GameManager.Instance.PlayerDied(this);
        }
    }

    private void OnEnable() 
    {
        TurnOffColiision();    
        Invoke(nameof(TurnOnCollision), InvulnerabilityTime);
    }

    private void TurnOffColiision()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
    }

    private void TurnOnCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
