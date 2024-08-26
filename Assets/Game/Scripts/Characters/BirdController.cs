using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    private static BirdController instance;
    public static BirdController Instance { get { return instance; } }
    public BirdState state;
    [SerializeField] private FixedJoystick joystick;
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    bool isInWaterArea = false;
    [SerializeField] private Transform birdPos;
    [SerializeField] private GameObject DeathPanel;

    public Image timeBar;
    public Image skillBar;
    public float timeInWater = 2f;
    bool facingLeft = true;

    //skill
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private float skillDuration = 2f;
    private bool isUsingAbility = false;
    private bool isCooldown = false;
    private float skillCooldownTimer = 0f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (state == BirdState.die)
        {
            return;
        }

        CheckWater();
        BirdMovement();
        UpdateSkillBar();
    }

    public void UseAbility()
    {
        if (!isUsingAbility && !isCooldown)
        {
            StartCoroutine(BirdFly());
        }
    }

    void CheckWater()
    {
        if (isInWaterArea)
        {
            timeInWater += Time.deltaTime;

            if (timeInWater >= 2f)
            {
                rb.gravityScale = Mathf.Lerp(0, 12f, (timeInWater - 3f) / 1f);
            }
        }
        else
        {
            timeInWater = 0;
        }

        timeBar.fillAmount = Mathf.Clamp(timeInWater / 3f, 0, 1);
    }

    void BirdMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if ((h > 0 || joystick.Direction.x > 0) && facingLeft)
        {
            Flip();
        }
        else if ((h < 0 || joystick.Direction.x < 0) && !facingLeft)
        {
            Flip();
        }

        if (isInWaterArea)
        {
            rb.velocity = new Vector2(joystick.Direction.x, joystick.Direction.y) * speed;
        }
        else
        {
            rb.velocity = new Vector2(joystick.Direction.x * speed, rb.velocity.y);
        }
    }

    private IEnumerator BirdFly()
    {
        isUsingAbility = true;
        skillBar.fillAmount = 0;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(joystick.Direction.x, joystick.Direction.y) * speed;
        yield return new WaitForSeconds(skillDuration);

        if (!isInWaterArea)
        {
            rb.gravityScale = 1;
            rb.velocity = new Vector2(joystick.Direction.x * speed, rb.velocity.y);
        }

        isUsingAbility = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        skillCooldownTimer = cooldownTime;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    private void UpdateSkillBar()
    {
        if (isCooldown)
        {
            skillCooldownTimer -= Time.deltaTime;
            skillBar.fillAmount = Mathf.Clamp(skillCooldownTimer / cooldownTime, 0, 1);
        }
        else
        {
            skillBar.fillAmount = 0;
        }
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1f;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            state = BirdState.die;
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
        transform.SetParent(birdPos);
    }
}

public enum BirdState
{
    alive,
    die
}
