using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class GamePadInput : MonoBehaviour
{
    [SerializeField] private UnityEvent Press_EnterEvent = new UnityEvent();
    [SerializeField] private UnityEvent Press_BackEvent = new UnityEvent();
    [SerializeField] private UnityEvent Press_LeftEvent = new UnityEvent();
    [SerializeField] private UnityEvent Press_RightEvent = new UnityEvent();
    [SerializeField] private UnityEvent Press_UpEvent = new UnityEvent();
    [SerializeField] private UnityEvent Press_DownEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Yボタン
        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            Debug.Log("Button Yが押された！");
        }

        //Xボタン
        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            Debug.Log("Button Xが押された！");
        }

        //Aボタン
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Button Aが押された！");
            Press_EnterEvent.Invoke();
        }

        //Bボタン
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            Debug.Log("Button Bが押された！");
            Press_BackEvent.Invoke();
        }

        //左ボタン
        if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            Debug.Log("Button 左が押された！");
            Press_LeftEvent.Invoke();
        }

        //右ボタン
        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            Debug.Log("Button 右が押された！");
            Press_RightEvent.Invoke();
        }

        //上ボタン
        if (Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            Debug.Log("Button 上が押された！");
            Press_UpEvent.Invoke();

        }

        //下ボタン
        if (Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            Debug.Log("Button 下が押された！");
            Press_DownEvent.Invoke();
        }

        if (Gamepad.current.leftStick.ReadValue().x > 0.5)
        {
            Debug.Log("左スティックを右に倒した");
        }

        if (Gamepad.current.leftStick.ReadValue().x < -0.5)
        {
            Debug.Log("左スティックを左に倒した");
        }

        if (Gamepad.current.leftStick.ReadValue().y > 0.5)
        {
            Debug.Log("左スティックを上に倒した");
        }

        if (Gamepad.current.leftStick.ReadValue().y < -0.5)
        {
            Debug.Log("左スティックを下に倒した");
        }
    }
}
