using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    bool isInWaterArea = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fishMovement();
    }
    void fishMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h > 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = 1;
            transform.localScale = theScale;
        }
        else if (h < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = -1;
            transform.localScale = theScale;
        }
        if (isInWaterArea)
        {
            rb.velocity = new Vector2(h, v) * speed;
        }
        else
        {
            rb.velocity = new Vector2(h * speed, rb.velocity.y);
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
            //transform.SetParent(null);
            Invoke("ClearParent", 0.1f);
            isInWaterArea = false;
        }
    }
    private void ClearParent()
    {
        transform.SetParent(null);
    }
}
