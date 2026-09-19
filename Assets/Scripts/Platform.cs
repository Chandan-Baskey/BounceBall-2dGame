using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Platform : MonoBehaviour
{
    private const float StickDeadZone = 0.2f;

    [SerializeField] private float speed = 10f;

    private readonly List<RaycastResult> uiRaycastResults = new List<RaycastResult>();
    private Rigidbody2D rb;
    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction horizontalAction;
    private InputAction pointerPositionAction;
    private InputAction pointerPressAction;
    private EventSystem cachedEventSystem;
    private PointerEventData pointerEventData;
    private float moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        leftAction = new InputAction("Move Left", InputActionType.Button);
        leftAction.AddBinding("<Keyboard>/a");
        leftAction.AddBinding("<Keyboard>/leftArrow");
        leftAction.AddBinding("<Gamepad>/dpad/left");

        rightAction = new InputAction("Move Right", InputActionType.Button);
        rightAction.AddBinding("<Keyboard>/d");
        rightAction.AddBinding("<Keyboard>/rightArrow");
        rightAction.AddBinding("<Gamepad>/dpad/right");

        horizontalAction = new InputAction("Horizontal", InputActionType.Value, "<Gamepad>/leftStick/x");
        pointerPositionAction = new InputAction("Pointer Position", InputActionType.PassThrough, "<Mouse>/position");
        pointerPressAction = new InputAction("Pointer Press", InputActionType.Button, "<Mouse>/leftButton");
    }

    private void OnEnable()
    {
        leftAction.Enable();
        rightAction.Enable();
        horizontalAction.Enable();
        pointerPositionAction.Enable();
        pointerPressAction.Enable();
    }

    private void OnDisable()
    {
        leftAction.Disable();
        rightAction.Disable();
        horizontalAction.Disable();
        pointerPositionAction.Disable();
        pointerPressAction.Disable();
        moveDirection = 0f;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnDestroy()
    {
        leftAction.Dispose();
        rightAction.Dispose();
        horizontalAction.Dispose();
        pointerPositionAction.Dispose();
        pointerPressAction.Dispose();
    }

    private void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.IsPlaying)
        {
            moveDirection = 0f;
            return;
        }

        if (GameManager.instance.PointerReleaseRequired)
        {
            moveDirection = 0f;

            if (!IsAnyPointerPressed())
            {
                GameManager.instance.NotifyPointersReleased();
            }

            return;
        }

        moveDirection = ReadMoveDirection();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection * speed, 0f);
    }

    private float ReadMoveDirection()
    {
        bool digitalLeft = leftAction.IsPressed();
        bool digitalRight = rightAction.IsPressed();
        float analogValue = horizontalAction.ReadValue<float>();
        bool moveLeft = digitalLeft || analogValue < -StickDeadZone;
        bool moveRight = digitalRight || analogValue > StickDeadZone;

        AddMouseInput(ref moveLeft, ref moveRight);
        AddTouchInput(ref moveLeft, ref moveRight);

        // Simultaneous left and right input intentionally cancel each other.
        if (moveLeft == moveRight)
        {
            return 0f;
        }

        if (Mathf.Abs(analogValue) > StickDeadZone && !digitalLeft && !digitalRight)
        {
            return Mathf.Clamp(analogValue, -1f, 1f);
        }

        return moveLeft ? -1f : 1f;
    }

    private void AddMouseInput(ref bool moveLeft, ref bool moveRight)
    {
        if (!pointerPressAction.IsPressed())
        {
            return;
        }

        Vector2 pointerPosition = pointerPositionAction.ReadValue<Vector2>();
        if (IsPointerOverUi(pointerPosition, -1))
        {
            return;
        }

        SetScreenSide(pointerPosition, ref moveLeft, ref moveRight);
    }

    private void AddTouchInput(ref bool moveLeft, ref bool moveRight)
    {
        Touchscreen touchscreen = Touchscreen.current;
        if (touchscreen == null)
        {
            return;
        }

        foreach (UnityEngine.InputSystem.Controls.TouchControl touch in touchscreen.touches)
        {
            if (!touch.press.isPressed)
            {
                continue;
            }

            int touchId = touch.touchId.ReadValue();
            Vector2 position = touch.position.ReadValue();

            if (!IsPointerOverUi(position, touchId))
            {
                SetScreenSide(position, ref moveLeft, ref moveRight);
            }
        }
    }

    private static void SetScreenSide(Vector2 screenPosition, ref bool moveLeft, ref bool moveRight)
    {
        if (screenPosition.x < Screen.width * 0.5f)
        {
            moveLeft = true;
        }
        else
        {
            moveRight = true;
        }
    }

    private bool IsPointerOverUi(Vector2 screenPosition, int pointerId)
    {
        EventSystem currentEventSystem = EventSystem.current;
        if (currentEventSystem == null)
        {
            return false;
        }

        if (cachedEventSystem != currentEventSystem || pointerEventData == null)
        {
            cachedEventSystem = currentEventSystem;
            pointerEventData = new PointerEventData(currentEventSystem);
        }

        pointerEventData.Reset();
        pointerEventData.pointerId = pointerId;
        pointerEventData.position = screenPosition;
        uiRaycastResults.Clear();
        currentEventSystem.RaycastAll(pointerEventData, uiRaycastResults);
        return uiRaycastResults.Count > 0;
    }

    private bool IsAnyPointerPressed()
    {
        if (pointerPressAction.IsPressed())
        {
            return true;
        }

        Touchscreen touchscreen = Touchscreen.current;
        if (touchscreen == null)
        {
            return false;
        }

        foreach (UnityEngine.InputSystem.Controls.TouchControl touch in touchscreen.touches)
        {
            if (touch.press.isPressed)
            {
                return true;
            }
        }

        return false;
    }
}
