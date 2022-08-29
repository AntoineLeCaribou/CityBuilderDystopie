using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCommandes : MonoBehaviour
{
    public GameObject panel;
    public GameObject EasterEgg;

    public Button boutonPasOk;

    public void Ok()
    {
        panel.SetActive(false);
    }

    public void PasOk()
    {
        boutonPasOk.gameObject.SetActive(false);
        EasterEgg.SetActive(true);
    }
}
