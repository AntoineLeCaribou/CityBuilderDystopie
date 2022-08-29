using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBreakMenu : MonoBehaviour
{

    private GameObject parentBreakMenu;
    private bool isActif;

    private void Awake()
    {
        parentBreakMenu = gameObject;

        parentBreakMenu.SetActive(false);
        isActif = false;
    }

    public void SwitchMenuPause()
    {
        isActif = !isActif;

        if (isActif)
        {
            parentBreakMenu.SetActive(true);
            return;
        }

        parentBreakMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Parametres()
    {
        print("params");
    }
}
