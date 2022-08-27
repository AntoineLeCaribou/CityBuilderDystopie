using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoutonsVitesseDefilement : MonoBehaviour
{
    public int etat = 0;

    public List<GameObject> listeBoutons;
    private List<Color> couleursDeBase;
    public List<Color> couleursActive;
    public List<float> vitesse;

    public TimeModifier timeModifier;

    // Start is called before the first frame update
    void Awake()
    {
        couleursDeBase = new List<Color>();

        foreach (GameObject bouton in listeBoutons)
        {
            couleursDeBase.Add(bouton.GetComponent<Image>().color);
        }

        for (int i = 0; i < listeBoutons.Count; i++)
        {
            int tempInt = i;
            listeBoutons[i].GetComponent<Button>().onClick.AddListener(() => ActiverBoutons(tempInt));
        }

        ActiverBoutons(etat);
    }

    private void ResetBoutons()
    {
        int i = 0;
        foreach (GameObject bouton in listeBoutons)
        {
            bouton.GetComponent<Image>().color = couleursDeBase[i];
            i++;
        }
    }

    public void ActiverBoutons(int i)
    {
        ResetBoutons();
        listeBoutons[i].GetComponent<Image>().color = couleursActive[i];
        timeModifier._interval = vitesse[i];
        etat = i;
    }

    public void ReactiverBoutons()
    {
        foreach (GameObject bouton in listeBoutons)
        {
            Button button = bouton.GetComponent<Button>();
            button.interactable = true;
        }
    }

    public void DesactiverBoutons()
    {
        foreach (GameObject bouton in listeBoutons)
        {
            Button button = bouton.GetComponent<Button>();
            button.interactable = false;
        }
    }
}
