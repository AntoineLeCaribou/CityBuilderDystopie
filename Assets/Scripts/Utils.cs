using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{

    public static Vector3 GetMousePositionIn3DWorld(out bool isUi)
    {
        isUi = EventSystem.current.IsPointerOverGameObject();

        Vector3 mousePositionOn2DScreen = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePositionOn2DScreen);

        //Par défaut on considère le vecteur3 en (-1,0,-1)
        Vector3 mousePositionIn3DWorld = new Vector3(-1, 0, -1);

        int mask = 1 << 2; //tout sauf 2 (ignore raycast)

        //Si le rayon a touché un truc
        if (Physics.Raycast(ray, out RaycastHit hitData))
        {
            mousePositionIn3DWorld = hitData.point;
        }

        return new Vector3(mousePositionIn3DWorld.x, 0, mousePositionIn3DWorld.z);
    }

    public static Vector3 ConvertirVector3inXYZintoXZY(Vector3 position)
    {
        return new Vector3(position.x, position.z, position.y);
    }
}
