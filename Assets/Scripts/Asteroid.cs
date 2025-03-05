using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float Speed = 50.0f;
    [SerializeField] private float MaxLifeTime = 30.0f;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float coinSpawnChance = 0.05f;
    [SerializeField] private Coins coinPrefab;
    [HideInInspector] public float speed = 1.0f;
    
    public float MinSize = 0.5f;
    public float MaxSize = 1.5f;
    public float Size = 1.0f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    private void Awake(){
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value *360.0f);
        transform.localScale = Vector3.one * Size;

        //Bigger size mean heavier mean it slower
        //set speed to 0. if want to faster and >1 if slower
        _rigidbody.mass = Size * speed;
        //Destroy after lifetime
        Destroy(gameObject, MaxLifeTime);
    }
    public void SetTrajectory(Vector2 directional){
        _rigidbody.AddForce(directional * Speed);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Bullet")
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.AsteroidDestroyed);
            if((Size * 0.5f) >= MinSize)
            {
                CreateSplit();
                CreateSplit();
            }

            if (Random.value < coinSpawnChance)
            {
                CoinSpawn();
            }
            
            GameManager.Instance.AsteroidDestroy(this);
            Destroy(gameObject);
        }
    }

    private Asteroid CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid halfAsteroid = Instantiate(this, position, transform.rotation);
        halfAsteroid.Size = Size * 0.5f;
        halfAsteroid.SetTrajectory(Random.insideUnitCircle.normalized * Speed * 2.0f);
    
        return halfAsteroid;
    }

    private Coins CoinSpawn()
    {
        return Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }
}
