using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaisons : MonoBehaviour
{
    [SerializeField] private Image[] imagesAnimation;
    [SerializeField] private Color[] couleursEnMemoire;
    [SerializeField] private TMPro.TMP_Text texte;

    private Animator animator;

    private Sprite[] imagesEnMemoire;
    private string saisonEnCache;

    private void Awake()
    {
        CacherSaisons();

        imagesEnMemoire = new Sprite[4];
        imagesEnMemoire[0] = Resources.Load<Sprite>("Sprites/hiver");
        imagesEnMemoire[1] = Resources.Load<Sprite>("Sprites/printemps");
        imagesEnMemoire[2] = Resources.Load<Sprite>("Sprites/ete");
        imagesEnMemoire[3] = Resources.Load<Sprite>("Sprites/automne");

        animator = GetComponent<Animator>();
    }

    public void MettreAJourUISaisons(string ancienneSaison)
    {
        if (ancienneSaison == "Hiver")
        {
            imagesAnimation[0].sprite = imagesEnMemoire[0];
            imagesAnimation[1].sprite = imagesEnMemoire[1];
            imagesAnimation[2].sprite = imagesEnMemoire[2];
            imagesAnimation[3].sprite = imagesEnMemoire[3];
        }
        else if (ancienneSaison == "Printemps")
        {
            imagesAnimation[0].sprite = imagesEnMemoire[1];
            imagesAnimation[1].sprite = imagesEnMemoire[2];
            imagesAnimation[2].sprite = imagesEnMemoire[3];
            imagesAnimation[3].sprite = imagesEnMemoire[0];
        }
        else if (ancienneSaison == "Été")
        {
            imagesAnimation[0].sprite = imagesEnMemoire[2];
            imagesAnimation[1].sprite = imagesEnMemoire[3];
            imagesAnimation[2].sprite = imagesEnMemoire[0];
            imagesAnimation[3].sprite = imagesEnMemoire[1];
        }
        else if (ancienneSaison == "Automne")
        {
            imagesAnimation[0].sprite = imagesEnMemoire[3];
            imagesAnimation[1].sprite = imagesEnMemoire[0];
            imagesAnimation[2].sprite = imagesEnMemoire[1];
            imagesAnimation[3].sprite = imagesEnMemoire[2];
        }
    }

    public void JouerAnimation(string ancienneSaison, string saison)
    {
        SetTexte(ancienneSaison);
        saisonEnCache = saison;
        MettreAJourUISaisons(ancienneSaison);
        MontrerSaisons();
        animator.SetTrigger("Tourner");
    }

    private void SetTexte(string saison)
    {
        texte.SetText(saison);
    }

    public void SetTexteFromUI()
    {
        texte.SetText(saisonEnCache);
    }

    public void MontrerSaisons()
    {
        gameObject.SetActive(true);
    }

    public void CacherSaisons()
    {
        gameObject.SetActive(false);
    }
}
