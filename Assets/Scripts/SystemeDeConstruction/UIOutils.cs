using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOutils : MonoBehaviour
{

    [SerializeField] private GameObject parentTextAction1;
    [SerializeField] private GameObject parentTextAction2;

    private Animator animator;

    [SerializeField] private GridBuildingSystem gridBuildingSystem;

    [SerializeField] private Button[] boutons;

    private Outil[] outils = { new Selection(), new Rotation(), new Creer(), new Supprimer() };

    private Color boutonNonActif;
    private Color imageNonActif;

    [SerializeField] private Color boutonActif;
    [SerializeField] private Color[] couleursImageActif;

    private void Awake()
    {
        CacherActions();

        animator = GetComponent<Animator>();

        boutonNonActif = boutons[0].GetComponent<Image>().color;
        imageNonActif = boutons[0].transform.GetChild(0).GetComponent<Image>().color;
        Cacher();
        ActiverBouton(0);

        for (int x = 0; x < boutons.Length; x++)
        {
            int copiex = x;
            boutons[copiex].onClick.AddListener(() => ActiverBouton(copiex));
        }
    }

    public void Afficher()
    {
        //gameObject.SetActive(true);
        MontrerActions();
        animator.SetBool("Afficher", true);
    }

    public void Cacher()
    {
        //gameObject.SetActive(false);
        CacherActions();
        animator.SetBool("Afficher", false);
    }

    private void DesactiverTousLesBoutons()
    {
        for (int i = 0; i < boutons.Length; i++)
        {
            boutons[i].GetComponent<Image>().color = boutonNonActif;
            boutons[i].transform.GetChild(0).GetComponent<Image>().color = imageNonActif;
        }
    }

    public void ActiverBouton(int i)
    {
        DesactiverTousLesBoutons();
        boutons[i].GetComponent<Image>().color = boutonActif;
        boutons[i].transform.GetChild(0).GetComponent<Image>().color = couleursImageActif[i];
        gridBuildingSystem.SetOutil(outils[i]);
        RefreshTexteActions(outils[i]);
    }

    private void CacherActions()
    {
        parentTextAction1.SetActive(false);
        parentTextAction2.SetActive(false);
    }

    private void MontrerActions()
    {
        parentTextAction1.SetActive(true);
        parentTextAction2.SetActive(true);
    }

    private void RefreshTexteActions(Outil outil)
    {
        string txt = outil.GetUtilitePrincipale();
        if (txt == "")
            parentTextAction1.transform.GetChild(1).gameObject.SetActive(false);
        else
            parentTextAction1.transform.GetChild(1).gameObject.SetActive(true);
        parentTextAction1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText(txt);

        txt = outil.GetUtiliteSecondaire();
        if (txt == "")
            parentTextAction2.transform.GetChild(1).gameObject.SetActive(false);
        else
            parentTextAction2.transform.GetChild(1).gameObject.SetActive(true);
        parentTextAction2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText(txt);
    }
}
