using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCommandes : MonoBehaviour
{
    public GameObject panel;
    public GameObject EasterEgg;

    private Animator animator;

    public Button boutonPasOk;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Ok()
    {
        animator.SetTrigger("Fermer");
    }

    public void PasOk()
    {
        boutonPasOk.gameObject.SetActive(false);
        EasterEgg.SetActive(true);
    }
}
