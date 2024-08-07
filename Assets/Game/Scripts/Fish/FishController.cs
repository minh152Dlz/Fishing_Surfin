using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    private static FishController instance;
    public static FishController Instance { get { return instance; } }
    public FishState state;
    [SerializeField] private FloatingJoystick joystick;
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    bool isInWaterArea = false;
    [SerializeField] private Transform fishPos;
    [SerializeField] private GameObject DeathPanel;
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
        if (state == FishState.die)
        {
            return;
        }
        fishMovement();
    }
    void fishMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h > 0 || joystick.Direction.x > 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = 0.7f;
            transform.localScale = theScale;
        }
        else if (h < 0 || joystick.Direction.x < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = -0.7f;
            transform.localScale = theScale;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            state = FishState.die;
            DeathPanel.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            rb.gravityScale = 0;
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
        }
    }
    private void ClearParent()
    {
        transform.SetParent(fishPos);
    }
}
public enum FishState
{
    alive,
    die
}
