using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRessourceManager : MonoBehaviour
{

    public TMP_Text textArgent;
    public TMP_Text textHabitants;

    private void Awake()
    {
        UpdateCompteurHabitants(1);
        UpdateCompteurArgent(1000);
    }

    public void UpdateCompteurArgent(float argent)
    {
        argent = (int)argent;

        if (argent < 0)
        {
            textArgent.SetText("<color=#FF6262>" + Utils.NombreToNombreAvecEspace(argent.ToString()));
            return;
        }

        textArgent.SetText(Utils.NombreToNombreAvecEspace(argent.ToString()) + " €");
    }

    public void UpdateCompteurHabitants(int nbHabitants)
    {
        textHabitants.SetText(nbHabitants.ToString());
    }
}
