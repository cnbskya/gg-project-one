using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask ignoreLayers;

    private void Update()
    {
        ClickGridPart();
    }

    private void ClickGridPart()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        RaycastHit hit = CastRay();

        if (hit.transform == null) return;
        if (!hit.collider.TryGetComponent(out IClickable gridPart)) return;
            
        gridPart.Click();
    }

    #region Helper Funcs

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCam.farClipPlane);

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCam.nearClipPlane);

        Vector3 worldMousePosFar = mainCam.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = mainCam.ScreenToWorldPoint(screenMousePosNear);

        Vector3 rayDir = worldMousePosFar - worldMousePosNear;
        Ray rayToDir = new Ray(worldMousePosNear, rayDir);

        Debug.DrawRay(rayToDir.origin, rayToDir.direction * 200F, Color.red);

        Physics.Raycast(rayToDir, out RaycastHit hitInfo, 200F, ignoreLayers);

        return hitInfo;
    }

    #endregion
    
}
