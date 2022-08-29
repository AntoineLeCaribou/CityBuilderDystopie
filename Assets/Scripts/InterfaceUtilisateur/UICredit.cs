using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICredit : MonoBehaviour
{
    public Animator animateur;

    private Credit credit;

    [SerializeField]
    private GameObject parentFenetreLimiteCredit;
    [SerializeField]
    private GameObject parentFenetreFaireCredit;


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

    private void Awake()
    {
        credit = GetComponent<Credit>();
    }

    public void InitCapital(bool tuto, float capital, float min, float max)
    {
        sliderCapital.maxValue = max;
        sliderCapital.minValue = min;
        sliderCapital.value = capital;
        
        if (tuto == true)
            sliderCapital.interactable = false;
        else
            sliderCapital.interactable = true;
        
        minCapital.SetText(Utils.NombreToNombreAvecEspace(min.ToString()) + " €");
        maxCapital.SetText(Utils.NombreToNombreAvecEspace(max.ToString()) + " €");
        finalCapital.SetText(Utils.NombreToNombreAvecEspace(capital.ToString()) + " €");
    }

    public void UpdateFinalCapital()
    {
        float value = sliderCapital.value;
        finalCapital.SetText(Utils.NombreToNombreAvecEspace(value.ToString()) + " €");
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
        
        minDuree.SetText(Utils.NombreToMoisEtAnnee(min));
        maxDuree.SetText(Utils.NombreToMoisEtAnnee(max));
        finalDuree.SetText(Utils.NombreToMoisEtAnnee(duree));
    }

    public void UpdateFinalDuree()
    {
        float value = sliderDuree.value;
        finalDuree.SetText(Utils.NombreToMoisEtAnnee(value));
    }

    public void InitFraisDeDossier(float montant, float pourcentage)
    {
        fraisDeDossier.SetText("Frais de dossier (<color=#FF6262>" + pourcentage.ToString() + "%</color> de l'emprunt) : <color=#FF6262>" + Utils.NombreToNombreAvecEspace(montant.ToString()) + " €");
    }

    public void InitPourcentageSupplementaire(float montant, float pourcentage)
    {
        supplement.SetText("Supplément à rembourser (<color=#FF6262>" + pourcentage.ToString() + "%</color> de l'emprunt) : <color=#FF6262>" + Utils.NombreToNombreAvecEspace(montant.ToString()) + " €");
    }

    public void InitRemboursement(float total, float parMois)
    {
        remboursementTotal.SetText("Montant total à rembourser : <color=#FF6262>" + Utils.NombreToNombreAvecEspace(total.ToString()) + " €");
        remboursementParMois.SetText("Somme à rembourser / mois : <color=#FF6262>" + Utils.NombreToNombreAvecEspace(parMois.ToString()) + " €");
    }

    public void InitBoutonSouscription(float montant, bool PasPossible)
    {
        textBoutonSouscription.SetText("Souscrire\n[<color=#FF6262>" + Utils.NombreToNombreAvecEspace(montant.ToString()) + " €</color>]");
        textBoutonSouscription.transform.parent.GetComponent<Button>().interactable = !PasPossible;
    }

    public void OuvrirFenetreCredit()
    {
        animateur.SetBool("Ouverture", true);
        credit.VerifierSiTropDeCredits();
    }

    public void SetLimiteCredit()
    {
        parentFenetreFaireCredit.SetActive(false);
        parentFenetreLimiteCredit.SetActive(true);
    }

    public void SetFenetreFaireUnCredit()
    {
        parentFenetreFaireCredit.SetActive(true);
        parentFenetreLimiteCredit.SetActive(false);
    }

    public void FermerFenetreCredit()
    {
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
