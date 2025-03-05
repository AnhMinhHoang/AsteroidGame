using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ScreenWrap : MonoBehaviour
{
    private Bounds screenBounds;
    private Rigidbody2D rb;
    public bool screenWrapping = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");

        for (int i = 0; i < boundaries.Length; i++)
        {
            boundaries[i].SetActive(!screenWrapping);
        }
    }

    private void Update()
    {
        //Get screen position of object in pixel
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        //Get the top right side of the screen in world units
        Vector2 maxWorldSide = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //Get the bottom left side of the screen in world units
        Vector2 minWorldSide = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f));

        //Check if the player is moving through side of screen and only occur if is moving toward
        //the side not oppotiste(For anything spawning outside of the screen)
        if(screenPos.x <= 0 && rb.linearVelocity.x < 0)
        {
            transform.position = new Vector2(maxWorldSide.x, transform.position.y);
        }
        else if (screenPos.x >= Screen.width && rb.linearVelocity.x > 0)
        {
            transform.position = new Vector2(minWorldSide.x, transform.position.y);
        }
        else if (screenPos.y <= 0 && rb.linearVelocity.y < 0)
        {
            transform.position = new Vector2(transform.position.x, maxWorldSide.y);
        }
        else if (screenPos.y >= Screen.height && rb.linearVelocity.y > 0)
        {
            transform.position = new Vector2(transform.position.x, minWorldSide.y);
        }
    }
}
