using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private Rigidbody2D RigidbodyObj;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrenght = 1f;
    private bool grounded;
    private bool climbing;

    public GameObject panelGameOver;
    public bool finalScene;
    public GameObject panelWin;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ajoute le component RigidbodyObj au player
        RigidbodyObj = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
        panelGameOver.SetActive(false);
        if(finalScene)
        panelWin.SetActive(false);
    }

    private void OnEnable() 
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable() 
    {
        CancelInvoke();
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            // Mise en place de la nomination du layer
            if (hit.layer == LayerMask.NameToLayer("Ground")) 
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);
                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    private void Update()
    {
        CheckCollision();
 
        if (climbing) {
            // Direction sur l'axe verticale avec une vitesse du deplacement
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
            // Direction du saut
            // Grounded va permettre de checker si le player est bien en contact avec le sol
        } else if (grounded && Input.GetButtonDown("Jump")) {
            direction = Vector2.up * jumpStrenght;
        } else {
            direction += Physics2D.gravity * Time.deltaTime;
        }
        
        // Direction sur l'axe horizontale avec une vitesse du deplacement
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        
        if (grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

       // Faire bouger son player a gauche et a droite
       if (direction.x > 0f) {
           transform.eulerAngles = Vector3.zero;
       } else if (direction.x < 0f) {
           transform.eulerAngles = new Vector3(0f, 180f, 0f);
       }
    }

    private void FixedUpdate()
    {
        RigidbodyObj.MovePosition(RigidbodyObj.position + direction * Time.fixedDeltaTime);
    }

    // Animation sprites
    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else
        {
            spriteIndex++;

            if (spriteIndex >= runSprites.Length) {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    // Lorsque le player rentre en collision avec un objet
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();
            panelWin.SetActive(true);
        } 
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelFailed();
            panelGameOver.SetActive(true);
        }
    }
}
