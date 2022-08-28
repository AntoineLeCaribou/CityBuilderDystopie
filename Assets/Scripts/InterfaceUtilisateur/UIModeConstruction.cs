using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeConstruction : MonoBehaviour
{
    public bool modeConstruction;

    public GameObject lignes;
    public GameObject hitboxHerbe;

    public Animator animateur;
    [Header("Gestion du temps")]
    public int etatEnCache;

    public UIBoutonsVitesseDefilement vitesseDefilement;

    public Toggle caseGrille;
    private bool etatGrille;

    // Start is called before the first frame update
    void Start()
    {
        etatEnCache = 0;
        SwitchModeConstruction();

        etatGrille = true;
        caseGrille.isOn = true;

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
        if (modeConstruction)
        {
            animateur.SetBool("Ouverture", false);
            modeConstruction = false;
            lignes.SetActive(false);
            hitboxHerbe.layer = 2;

            vitesseDefilement.ReactiverBoutons();
            vitesseDefilement.ActiverBoutons(etatEnCache);
        }
        else if (!modeConstruction)
        {
            animateur.SetBool("Ouverture", true);
            modeConstruction = true;
            lignes.SetActive(etatGrille);
            hitboxHerbe.layer = 0;
            
            etatEnCache = vitesseDefilement.etat;
            vitesseDefilement.ActiverBoutons(0);
            vitesseDefilement.DesactiverBoutons();
        }
    }
}
