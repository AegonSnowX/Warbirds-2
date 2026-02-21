using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AAGunController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Transform barrelPivot;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Rigidbody2D projectilePrefab;

    [Header("Firing")]
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private bool barrelForwardIsUp;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (!TryGetPointerDownPosition(out Vector2 touchScreenPosition))
        {
            return;
        }

        if (targetCamera == null || barrelPivot == null || firePoint == null || projectilePrefab == null)
        {
            Debug.LogWarning("AAGunController: Assign camera, barrelPivot, firePoint, and projectilePrefab in Inspector.");
            return;
        }

        if (IsTouchOverUi())
        {
            return;
        }

        Vector2 touchWorldPosition = targetCamera.ScreenToWorldPoint(touchScreenPosition);
        RotateBarrelToPoint(touchWorldPosition);
        FireTowards(touchWorldPosition);
    }

    private bool TryGetPointerDownPosition(out Vector2 touchPosition)
    {
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                touchPosition = touch.position.ReadValue();
                return true;
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            touchPosition = Mouse.current.position.ReadValue();
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    private bool IsTouchOverUi()
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        if (Touchscreen.current != null)
        {
            int touchId = Touchscreen.current.primaryTouch.touchId.ReadValue();
            return EventSystem.current.IsPointerOverGameObject(touchId);
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    private void RotateBarrelToPoint(Vector2 targetPoint)
    {
        Vector2 from = barrelPivot.position;
        Vector2 direction = targetPoint - from;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (barrelForwardIsUp)
        {
            angle -= 90f;
        }

        barrelPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FireTowards(Vector2 targetPoint)
    {
        Vector2 from = firePoint.position;
        Vector2 direction = (targetPoint - from).normalized;
        Rigidbody2D projectile = Instantiate(projectilePrefab, from, Quaternion.identity);
        projectile.linearVelocity = direction * projectileSpeed;
    }
}
