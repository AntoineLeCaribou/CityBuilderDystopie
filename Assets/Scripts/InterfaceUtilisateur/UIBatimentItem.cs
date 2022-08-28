using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBatimentItem : MonoBehaviour
{
    public GridBuildingSystem gridBuildingSystem;
    public Button[] listeBoutons;
    public GameObject[] listeObjets;
    public int[] valeurs;
    public string[] noms;

    private void Awake()
    {
        for (int x = 0; x < listeObjets.Length; x++)
        {
            int copieX = x;
            listeBoutons[copieX].onClick.AddListener(() => DefinirNouveauBatiment(listeObjets[copieX], valeurs[copieX], noms[copieX]));
            listeBoutons[copieX].onClick.AddListener(() => ResetColorButtons());
            listeBoutons[copieX].onClick.AddListener(() => listeBoutons[copieX].Select());
            continue;
        }

        listeBoutons[0].Select();
        listeBoutons[0].onClick.Invoke();
    }

    public void DefinirNouveauBatiment(GameObject objet, int valeur, string nom)
    {
        gridBuildingSystem.SetPrefabAPlacer(objet, valeur, nom);
    }

    private void ResetColorButtons()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
