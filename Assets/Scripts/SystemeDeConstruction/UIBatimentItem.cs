using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBatimentItem : MonoBehaviour
{
    [SerializeField]
    private UIOutils uiOutils;

    [SerializeField]
    private RessourceManager ressourceManager;
    [SerializeField]
    private GridBuildingSystem gridBuildingSystem;

    [SerializeField]
    private Button[] listeBoutons;
    [SerializeField]
    private GameObject[] listeObjets;

    private Color[] couleursBoutons;

    [SerializeField]
    private Color couleurBoutonActif;

    private void Start()
    {
        couleursBoutons = new Color[listeObjets.Length];

        for (int x = 0; x < listeObjets.Length; x++)
        {
            int copieX = x;

            //On récupère la couleur de base
            couleursBoutons[copieX] = listeBoutons[copieX].GetComponent<Image>().color;

            //On recupère le item shop
            ShopItem scriptItemShop = listeObjets[copieX].GetComponent<ShopItem>();

            //Si l'objet n'as pas le script shopitem
            if (scriptItemShop == null)
            {
                listeBoutons[copieX].transform.parent.gameObject.SetActive(false);
                continue;
            }

            //Donc c'est bien un item du shop

            //On change la couleur de la carte graphiquement
            listeBoutons[copieX].transform.GetChild(0).GetComponent<Image>().color = scriptItemShop.couleurDeLobjet;
            //On change le nom de l'objet
            listeBoutons[copieX].transform.GetChild(1).GetComponent<TMPro.TMP_Text>().SetText(scriptItemShop.nomDeLobjet);

            RefreshPrixBoutons();

            //On ajoute les intéractions
            listeBoutons[copieX].onClick.AddListener(() => DefinirNouveauBatiment(listeObjets[copieX], scriptItemShop.id, scriptItemShop.nomDeLobjet, scriptItemShop.prixDeLobjet));
            listeBoutons[copieX].onClick.AddListener(() => SelectionnerBouton(copieX));
            
            continue;
        }

        //On recupère le item shop
        ShopItem info = listeObjets[0].GetComponent<ShopItem>();

        //On active le bouton 0
        SelectionnerBouton(0);
        DefinirNouveauBatiment(listeObjets[0], info.id, info.nomDeLobjet, info.prixDeLobjet);
    }

    public void RefreshPrixBoutons()
    {
        for (int x = 0; x < listeObjets.Length; x++)
        {
            //On recupère le item shop
            ShopItem scriptItemShop = listeObjets[x].GetComponent<ShopItem>();

            //On change le prix
            string couleurRouge;
            if (scriptItemShop.prixDeLobjet > ressourceManager.GetArgent())
            {
                couleurRouge = "<color=#FF6262>";
            }
            else
            {
                couleurRouge = "";
            }
            listeBoutons[x].transform.GetChild(2).GetComponent<TMPro.TMP_Text>().SetText(couleurRouge + Utils.NombreToNombreAvecEspace(scriptItemShop.prixDeLobjet.ToString()) + " €");
        }
    }

    private void DefinirNouveauBatiment(GameObject objet, int id, string nom, float prix)
    {
        uiOutils.ActiverBouton(2);
        gridBuildingSystem.SetPrefabAPlacer(objet, id, nom, prix);
    }

    private void ResetColorBoutons()
    {
        for (int x = 0; x < listeObjets.Length; x++)
        {
            listeBoutons[x].GetComponent<Image>().color = couleursBoutons[x];
        }
    }

    private void SelectionnerBouton(int i)
    {
        ResetColorBoutons();
        listeBoutons[i].GetComponent<Image>().color = couleurBoutonActif;
    }
}