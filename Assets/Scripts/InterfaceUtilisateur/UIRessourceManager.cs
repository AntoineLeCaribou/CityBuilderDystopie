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
        if (argent < 0)
        {
            textArgent.SetText("<color=#FF6262>" + argent.ToString() + " €");
            return;
        }

        textArgent.SetText(argent.ToString() + " €");
    }

    public void UpdateCompteurHabitants(int nbHabitants)
    {
        textHabitants.SetText(nbHabitants.ToString());
    }
}
