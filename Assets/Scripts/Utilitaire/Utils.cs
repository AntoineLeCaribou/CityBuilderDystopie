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

    public static string NombreToNombreAvecEspace(string nombre)
    {
        string res = "";
        string[] separationVirgule = nombre.Split(",");

        int nbDeLettresParcourues = 0;
        for (int i = separationVirgule[0].Length - 1; i >= 0; i--)
        {
            res = separationVirgule[0][i] + res;

            nbDeLettresParcourues++;

            if (nbDeLettresParcourues % 3 == 0)
                res = " " + res;
        }

        if (separationVirgule.Length == 2)
            res += "," + separationVirgule[1];

        return res;
    }

    public static string NombreToMoisEtAnnee(float nbDeMois)
    {

        //si c'est moins d'un an
        if (nbDeMois < 12)
            return nbDeMois + " mois";

        //sinon si c'est juste pile un an
        else if (nbDeMois == 12)
            return "1 an";

        //sinon si c'est une certaine année pile
        else if (nbDeMois % 12 == 0)
            return (nbDeMois / 12).ToString() + " ans";

        //sinon (c'est plus d'un an avec des mois)
        else
        {
            float moisEnPlus = nbDeMois % 12;
            nbDeMois -= moisEnPlus;
            float nbAnnee = nbDeMois / 12;

            if (nbAnnee == 1)
                return "1 an et " + moisEnPlus + " mois";

            return nbAnnee + " ans et " + moisEnPlus + " mois";
        }
    }
}
