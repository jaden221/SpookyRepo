/*
    I couldn't get the new Input System to work with the camera controller. Do you think you could implement it? NVM FIXED IT
    - sso
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    #region Variables

    [Header("Customisable")]
    [SerializeField] private float smoothing = 10f;
    [SerializeField] private float cameraMoveAmount = 8f;
    #region Camera Positions
    /*
    [SerializeField] private Vector2 upPos;
    [SerializeField] private Vector2 downPos;
    [SerializeField] private Vector2 leftPos;
    [SerializeField] private Vector2 rightPos;
    */
    #endregion

    [Space(5)]

    [Header("Input")]
    [SerializeField] private InputActionReference camContRef;
    private InputAction camcont;

    [Space(5)]

    [Header("Physics")]
    [SerializeField] private Vector2 totInput;

    #endregion

    #region Processes

    private void Awake()
    {
        camcont = camContRef;
    }

    void LateUpdate()
    {
        UpdatePositionUponInput();
    }

    void OnEnable()
    {
        camcont.Enable();
    }

    void OnDisable()
    {
        camcont.Disable();
    }

    #endregion

    #region Function Which Moves The Camera Based On Player Input

    private void UpdatePositionUponInput()
    {
        totInput = camcont.ReadValue<Vector2>().normalized;

        transform.localPosition = Vector2.Lerp(transform.localPosition, totInput * cameraMoveAmount, smoothing * Time.deltaTime);

        // unused
        /*if (Input.GetKey(KeyCode.UpArrow)) transform.localPosition = Vector2.Lerp(transform.localPosition, upPos, smoothing * Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow)) transform.localPosition = Vector2.Lerp(transform.localPosition, downPos, smoothing * Time.deltaTime);
        else if (Input.GetKey(KeyCode.LeftArrow)) transform.localPosition = Vector2.Lerp(transform.localPosition, leftPos, smoothing * Time.deltaTime);
        else if (Input.GetKey(KeyCode.RightArrow)) transform.localPosition = Vector2.Lerp(transform.localPosition, rightPos, smoothing * Time.deltaTime);
        else transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(0, 0), smoothing * Time.deltaTime);*/
    }

    #endregion
}
