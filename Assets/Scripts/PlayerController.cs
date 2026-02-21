using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;

    private bool moveLeftPressed;
    private bool moveRightPressed;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate()
    {
        float input = 0f;

        if (moveLeftPressed)
        {
            input -= 1f;
        }

        if (moveRightPressed)
        {
            input += 1f;
        }

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
            return;
        }

        transform.Translate(Vector3.right * (input * moveSpeed * Time.fixedDeltaTime));
    }

    public void OnLeftButtonDown()
    {
        moveLeftPressed = true;
    }

    public void OnLeftButtonUp()
    {
        moveLeftPressed = false;
    }

    public void OnRightButtonDown()
    {
        moveRightPressed = true;
    }

    public void OnRightButtonUp()
    {
        moveRightPressed = false;
    }
}
