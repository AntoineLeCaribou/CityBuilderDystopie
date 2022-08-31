using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeConstruction : MonoBehaviour
{
    public UIOutils uiOutils;
    public BookAnimator grosLivre;

    public bool modeConstruction;

    public GameObject lignes;
    public GameObject hitboxHerbe;

    public Animator animateur;
    [Header("Gestion du temps")]
    public int etatEnCache;

    public UIBoutonsVitesseDefilement vitesseDefilement;

    public Toggle caseGrille;
    public bool etatGrille = false;

    // Start is called before the first frame update
    void Start()
    {
        etatEnCache = 0;
        SwitchModeConstruction();

        caseGrille.isOn = etatGrille;

        caseGrille.onValueChanged.AddListener((valeur) => SetEtatGrille(valeur));
    }

    private void SetEtatGrille(bool etat)
    {
        etatGrille = etat;
        if (modeConstruction)
            lignes.SetActive(etatGrille);
    }

    public void SwitchModeConstruction()
    {
        modeConstruction = !modeConstruction;
        if (!modeConstruction)
        {
            grosLivre.SetConstruction(false);
            uiOutils.Cacher();
            animateur.SetBool("Ouverture", false);
            lignes.SetActive(false);
            hitboxHerbe.layer = 2;

            vitesseDefilement.ReactiverBoutons();
            vitesseDefilement.ActiverBoutons(etatEnCache);
        }
        else if (modeConstruction)
        {
            grosLivre.SetConstruction(true);
            uiOutils.Afficher();
            animateur.SetBool("Ouverture", true);
            lignes.SetActive(etatGrille);
            hitboxHerbe.layer = 0;
            
            etatEnCache = vitesseDefilement.etat;
            vitesseDefilement.ActiverBoutons(0);
            vitesseDefilement.DesactiverBoutons();
        }
    }
}
