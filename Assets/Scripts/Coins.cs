using UnityEngine;
public class Coins : MonoBehaviour
{
    [SerializeField] private float MaxLifeTime = 30f;

    private void Start()
    {
        Destroy(gameObject, MaxLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.CoinCollected);
            GameManager.Instance.CoinCollected(this);
            Destroy(gameObject);
        }
    }
}
