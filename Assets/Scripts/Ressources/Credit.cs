using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{

    public UICredit uiCredit;
    public RessourceManager ressourceManager;

    private float minimumCredit = 1000;
    private float maximumCredit = 1000000;
    private float capital = 100000;

    private float fraisDeDossierEnPourcentage = 1;
    private float fraisDeDossier = 1000;

    private float margeEnPourcentage = 1.5f;
    private float marge = 1000;

    private float minimumMois = 6;
    private float maximumMois = 36;
    private float dureeDuCreditEnMois = 12;
    private float sommeARembourserParMois = 1;

    private int nbCreditEnCours = 0;
    private int nbCreditMax = 1;

    private float sommeTotaleARembourser = 0;

    public void Start()
    {
        Init();
    }

    private void Init()
    {
        uiCredit.InitCapital(true, capital, minimumCredit, maximumCredit);
        uiCredit.InitMois(true, dureeDuCreditEnMois, minimumMois, maximumMois);
        MettreAJourCredit();
    }

    public void MettreAJourCredit()
    {
        capital = uiCredit.GetMontant();
        dureeDuCreditEnMois = uiCredit.GetDuree();

        marge = capital * (margeEnPourcentage / 100f);
        fraisDeDossier = capital * (fraisDeDossierEnPourcentage / 100f);
        sommeTotaleARembourser = capital + marge;
        sommeARembourserParMois = sommeTotaleARembourser / dureeDuCreditEnMois;
        MettreAJourGraphiquementCredit();
    }

    private void MettreAJourGraphiquementCredit()
    {
        uiCredit.InitFraisDeDossier(fraisDeDossier, fraisDeDossierEnPourcentage);
        uiCredit.InitPourcentageSupplementaire(marge, margeEnPourcentage);
        uiCredit.InitRemboursement(sommeTotaleARembourser, sommeARembourserParMois);

        bool tropDeCredits = (nbCreditEnCours >= nbCreditMax) || (ressourceManager.GetArgent() < fraisDeDossier);
        uiCredit.InitBoutonSouscription(fraisDeDossier, tropDeCredits);
    }

    public void AccepterCredit()
    {
        nbCreditEnCours += 1;

        capital = uiCredit.GetMontant();
        dureeDuCreditEnMois = uiCredit.GetDuree();

        MettreAJourCredit();

        ressourceManager.RetirerArgent(fraisDeDossier);
        ressourceManager.AjouterArgent(capital);
    }
}
