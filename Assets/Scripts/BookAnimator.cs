using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimator : MonoBehaviour
{

    private Animator animator;

    [SerializeField] private GameObject parentFenetreCredit;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        DesactiverToutesLesFenetres();
    }

    public void SetConstruction(bool etat)
    {
        animator.SetBool("Construction", etat);
    }

    public void SetMontrer(bool etat)
    {
        animator.SetBool("Montrer", etat);
    }

    public void OuvrirLivre()
    {
        SetMontrer(true);
    }

    public void FermerLivre()
    {
        SetMontrer(false);
    }

    public void ActiverFenetreCredit()
    {
        DesactiverToutesLesFenetres();
        parentFenetreCredit.SetActive(true);
    }

    public void DesactiverToutesLesFenetres()
    {
        parentFenetreCredit.SetActive(false);
    }
}
