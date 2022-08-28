using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICredit : MonoBehaviour
{
    public Animator animateur;

    [SerializeField]
    private GameObject parentFenetreCredit;

    [SerializeField]
    private Slider sliderCapital;
    [SerializeField]
    private TMP_Text minCapital;
    [SerializeField]
    private TMP_Text maxCapital;
    [SerializeField]
    private TMP_Text finalCapital;

    [SerializeField]
    private Slider sliderDuree;
    [SerializeField]
    private TMP_Text minDuree;
    [SerializeField]
    private TMP_Text maxDuree;
    [SerializeField]
    private TMP_Text finalDuree;

    [SerializeField]
    private TMP_Text fraisDeDossier;

    [SerializeField]
    private TMP_Text supplement;

    [SerializeField]
    private TMP_Text remboursementTotal;

    [SerializeField]
    private TMP_Text remboursementParMois;

    [SerializeField]
    private TMP_Text textBoutonSouscription;

    public void InitCapital(bool tuto, float capital, float min, float max)
    {
        sliderCapital.maxValue = max;
        sliderCapital.minValue = min;
        sliderCapital.value = capital;
        
        if (tuto == true)
            sliderCapital.interactable = false;
        else
            sliderCapital.interactable = true;
        
        minCapital.SetText(min.ToString() + " €");
        maxCapital.SetText(max.ToString() + " €");
        finalCapital.SetText(capital.ToString() + " €");
    }

    public void UpdateFinalCapital()
    {
        float value = sliderCapital.value;
        finalCapital.SetText(value.ToString() + " €");
    }

    public void InitMois(bool tuto, float duree, float min, float max)
    {
        sliderDuree.maxValue = max;
        sliderDuree.minValue = min;
        sliderDuree.value = duree;
        
        if (tuto == true)
            sliderDuree.interactable = false;
        else
            sliderDuree.interactable = true;
        
        minDuree.SetText(min.ToString() + " mois");
        maxDuree.SetText(max.ToString() + " mois");
        finalDuree.SetText(duree.ToString() + " mois");
    }

    public void UpdateFinalDuree()
    {
        float value = sliderDuree.value;
        finalDuree.SetText(value.ToString() + " mois");
    }

    public void InitFraisDeDossier(float montant, float pourcentage)
    {
        fraisDeDossier.SetText("Frais de dossier (<color=#FF6262>" + pourcentage.ToString() + "%</color> de l'emprunt) : <color=#FF6262>" + montant.ToString() + " €");
    }

    public void InitPourcentageSupplementaire(float montant, float pourcentage)
    {
        supplement.SetText("Supplément à rembourser (<color=#FF6262>" + pourcentage.ToString() + "%</color> de l'emprunt) : <color=#FF6262>" + montant.ToString() + " €");
    }

    public void InitRemboursement(float total, float parMois)
    {
        remboursementTotal.SetText("Montant total à rembourser : <color=#FF6262>" + total.ToString() + " €");
        remboursementParMois.SetText("Somme à rembourser / mois : <color=#FF6262>" + parMois.ToString() + " €");
    }

    public void InitBoutonSouscription(float montant, bool PasPossible)
    {
        textBoutonSouscription.SetText("Souscrire\n" + montant.ToString() + " €");
        textBoutonSouscription.transform.parent.GetComponent<Button>().interactable = !PasPossible;
    }

    public void OuvrirFenetreCredit()
    {
        //parentFenetreCredit.SetActive(true);
        animateur.SetBool("Ouverture", true);
    }

    public void FermerFenetreCredit()
    {
        //parentFenetreCredit.SetActive(false);
        animateur.SetBool("Ouverture", false);
    }

    public float GetMontant()
    {
        return this.sliderCapital.value;
    }

    public float GetDuree()
    {
        return this.sliderDuree.value;
    }
}
