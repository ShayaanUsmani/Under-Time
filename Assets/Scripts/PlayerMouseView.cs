﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseView : MonoBehaviour
{
    private float mouseSensitivity = 100f;
    private float xRot;

    public Camera cam;

    // lock the mouse so it stays put while the player tries to look around
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // using the input mouse movement, allow the player to look around, clamping rotation by
    // the x axis to prevent player from rotating their view farther than straight upwards or
    // downwards.
    // Note that when looking up and down, the camera attached on the player moves, not the player
    void Update()
    {
        float mX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= mY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRot * Vector3.right);

        transform.Rotate(Vector3.up * mX);
    }
}
