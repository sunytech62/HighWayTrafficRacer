//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Handles mobile drag events to control the showroom camera.
/// </summary>
public class HR_UI_MobileDrag : MonoBehaviour, IDragHandler, IEndDragHandler {

    /// <summary>
    /// Reference to the showroom camera
    /// </summary>
    private HR_Camera_Showroom showroomCamera;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake() {

        showroomCamera = FindFirstObjectByType<HR_Camera_Showroom>();

    }

    /// <summary>
    /// Event handler for drag events
    /// </summary>
    /// <param name="data">Pointer event data</param>
    public void OnDrag(PointerEventData data) {

        showroomCamera.OnDrag(data);

    }

    /// <summary>
    /// Event handler for end drag events
    /// </summary>
    /// <param name="data">Pointer event data</param>
    public void OnEndDrag(PointerEventData data) {

    }

}
