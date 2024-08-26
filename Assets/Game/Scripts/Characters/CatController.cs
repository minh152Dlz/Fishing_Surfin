using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{
    private static CatController instance;
    public static CatController Instance { get { return instance; } }
    public CatState state;
    [SerializeField] private FixedJoystick joystick;
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    bool isInWaterArea = false;
    [SerializeField] private Transform catPos;
    [SerializeField] private GameObject DeathPanel;

    public Image timeBar;
    public float timeInWater =3f;
    bool facingRight = true;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CatState.die)
        {
            return;
        }
        CheckWater();
        CatMovement();
    }
    void CheckWater()
    {
        if (isInWaterArea)
        {
            timeInWater += Time.deltaTime;

            if (timeInWater >= 3f)
            {
                rb.gravityScale = Mathf.Lerp(0, 20f, (timeInWater - 3f) / 1f);
            }
        }
        else
        {
            timeInWater = 0;
        }

        timeBar.fillAmount = Mathf.Clamp(timeInWater / 3f, 0, 1);
    }
    void CatMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if ((h > 0 || joystick.Direction.x > 0) && !facingRight)
        {
            Flip();
        }
        else if ((h < 0 || joystick.Direction.x < 0) && facingRight)
        {
            Flip();
        }
        if (isInWaterArea)
        {
            //rb.velocity = new Vector2(h, v) * speed;
            rb.velocity = new Vector2(joystick.Direction.x, joystick.Direction.y) * speed;
        }
        else
        {
            //rb.velocity = new Vector2(h * speed, rb.velocity.y);
            rb.velocity = new Vector2(joystick.Direction.x * speed, rb.velocity.y);
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1f;
        transform.localScale = theScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            state = CatState.die;
            DeathPanel.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (timeInWater == 0f)
            {
               rb.gravityScale = 0;
            }

            transform.SetParent(collision.transform);
            isInWaterArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            rb.gravityScale = 1;
            Invoke("ClearParent", 0.1f);
            isInWaterArea = false;
            timeInWater = 0f;
        }
    }
    private void ClearParent()
    {
        transform.SetParent(catPos);
    }
}
public enum CatState
{
    alive,
    die
}
