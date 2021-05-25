    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;
    public Room currentRoom;
    public float moveSpeedWhenRoomChange;
    void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (currentRoom == null)
        {
            return;
        }

        var targetPosition = GetCameraTargetPosition();

        transform.position =
            Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
        {
            return Vector3.zero;
        }

        var targetPosition = currentRoom.GetRoomCentre();
        targetPosition.z = transform.position.z;
        return targetPosition;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
