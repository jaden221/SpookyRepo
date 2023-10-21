using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour
{
    #region Variables

    [Tooltip("These Are Changable Elements Which Will Effect Gameplay")]
    [Header("Customisable")]
    [SerializeField] private float speed = 5f;
    [SerializeField] public bool canFollowPlayer = true;

    [Space(5)]

    [Header("GameObjects")]
    [SerializeField] private GameObject player;

    #endregion

    #region Processes

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        PositionToPlayerPosition();
    }

    #endregion

    #region Function Which Lerps The Main Camera To The Player Reference Position

    private void PositionToPlayerPosition()
    {
        if (!canFollowPlayer) return;

        Vector3 plrPos = new Vector3(player.transform.position.x, player.transform.position.y, -10 /* required for camera position to stay behind sprites */);
        Vector3 camPos = new Vector3(transform.position.x, transform.position.y, -10 /* required for camera position to stay behind sprites */);

        transform.position = Vector3.Lerp(camPos, plrPos, speed * Time.deltaTime);
    }

    #endregion
}
