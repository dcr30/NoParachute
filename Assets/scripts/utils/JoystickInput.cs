﻿using UnityEngine;
using UnityEngine.UI;

public class JoystickInput : MonoBehaviour
{
    public static Vector2 input;
    public static bool isEnabled = true;
    // Максимальное смещение джойстика относительно места нажатия
    public float maxJoystickDistance = 70f;
    // Ввод с различных источников
    public bool enableTouchInput = true;
    public bool enableMouseInput = true;
    public bool enableKeyboardInput = true;
    // Отображение джойстика на экране
    public Image joystickImage;

    private bool isTouching;
    private int fingerId;
    private Vector2 startTouchPosition;

	void Start ()
    {
        joystickImage.enabled = false;
    }

    private void JoystickMove(Vector2 position)
    {
        var delta = position - startTouchPosition;
        // TODO: Чувствительность из настроек
        delta = Vector2.ClampMagnitude(delta, maxJoystickDistance);
        input = delta / maxJoystickDistance;

        joystickImage.rectTransform.position = startTouchPosition + delta;
        joystickImage.color = new Color(1f, 1f, 1f, delta.magnitude / maxJoystickDistance);
    }

    private void JoystickBegin(Vector2 position)
    {
        startTouchPosition = position;
        isTouching = true;
        joystickImage.enabled = true;
        joystickImage.color = new Color(1f, 1f, 1f, 0f);
    }

    private void JoystickEnd(Vector2 position)
    {
        isTouching = false;
        input = Vector2.zero;
        joystickImage.enabled = false;
    }

	void Update()
    {
        if (!isEnabled)
        {
            input = Vector2.zero;
            return;
        }
        // Ввод с клавиатуры 
        if (enableKeyboardInput)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        }
        // Тач
        if (enableTouchInput && Input.touchSupported)
        {
            foreach (var touch in Input.touches)
            {
                if (isTouching && touch.phase == TouchPhase.Moved && touch.fingerId == fingerId)
                {
                    JoystickMove(touch.position);
                }
                else if (isTouching && touch.phase == TouchPhase.Ended && touch.fingerId == fingerId)
                {
                    JoystickEnd(touch.position);
                }
                else if (touch.phase == TouchPhase.Began)
                {
                    fingerId = touch.fingerId;
                    JoystickBegin(touch.position);
                }
            }
        }
        // Мышь
        if (enableMouseInput)
        {
            if (isTouching && Input.GetMouseButton(0))
            {
                JoystickMove(Input.mousePosition);
            }
            else if (isTouching && Input.GetMouseButtonUp(0))
            {
                JoystickEnd(Input.mousePosition);
            }
            else if (!isTouching && Input.GetMouseButtonDown(0))
            {
                JoystickBegin(Input.mousePosition);
            }
        }
	}


}
