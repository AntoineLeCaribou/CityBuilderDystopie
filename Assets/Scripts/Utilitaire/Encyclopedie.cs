using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedie : MonoBehaviour
{

    public static GameObject[] Arbres;
    public static GameObject[] Buissons;
    public static GameObject[] Rochers;

    public static GameObject[] SolHerbe;

    public static GameObject[] Riviere;
    public static GameObject[] Route;


    public static void Init()
    {
        Arbres = new GameObject[2];
        Arbres[0] = Resources.Load<GameObject>("Prefabs/arbre1");
        Arbres[1] = Resources.Load<GameObject>("Prefabs/arbre2");

        Buissons = new GameObject[2];
        Buissons[0] = Resources.Load<GameObject>("Prefabs/buisson1");
        Buissons[1] = Resources.Load<GameObject>("Prefabs/buisson2");

        Rochers = new GameObject[2];
        Rochers[0] = Resources.Load<GameObject>("Prefabs/rocher1");
        Rochers[1] = Resources.Load<GameObject>("Prefabs/rocher2");

        SolHerbe = new GameObject[4];
        SolHerbe[0] = Resources.Load<GameObject>("Prefabs/CoinHerbe");
        SolHerbe[1] = Resources.Load<GameObject>("Prefabs/CoteHerbe");
        SolHerbe[2] = Resources.Load<GameObject>("Prefabs/Normalherbe");

        Riviere = new GameObject[4];
        Riviere[0] = Resources.Load<GameObject>("Prefabs/riviere");
        Riviere[1] = Resources.Load<GameObject>("Prefabs/murInterneRiviere");
        Riviere[2] = Resources.Load<GameObject>("Prefabs/murExterneRiviere");
        Riviere[3] = Resources.Load<GameObject>("Prefabs/murCoinRiviere");

        Route = new GameObject[6];
        Route[0] = Resources.Load<GameObject>("Prefabs/routeDeBase");
        Route[1] = Resources.Load<GameObject>("Prefabs/routeEnI");
        Route[2] = Resources.Load<GameObject>("Prefabs/routeEnL");
        Route[3] = Resources.Load<GameObject>("Prefabs/routeEnT");
        Route[4] = Resources.Load<GameObject>("Prefabs/routeEn+");
        Route[5] = Resources.Load<GameObject>("Prefabs/routeCulDeSac");
    }

    public static GameObject GetObjetAleatoire(GameObject[] famille)
    { 
        if (famille.GetLength(0) == 0)
        {
            Debug.Log("liste vide");
            return null;
        }

        if (famille.GetLength(0) == 1)
        {
            return famille[0];
        }

        int tailleFamille = famille.GetLength(0);
        int indexElementFamille = Random.Range(0, tailleFamille);

        return famille[indexElementFamille];
    }
}
