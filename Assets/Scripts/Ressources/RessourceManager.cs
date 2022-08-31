using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    public UIRessourceManager uiRessourceManager;
    public UIBatimentItem uiBatimentItem;

    private float argent;
    private int habitants;

    private void Awake()
    {
        argent = 1000;
        habitants = 1;
    }

    // Start is called before the first frame update
    private void Start()
    {
        uiRessourceManager.UpdateCompteurArgent(argent);
        uiRessourceManager.UpdateCompteurHabitants(habitants);
    }

    public void AjouterArgent(float argent)
    {
        this.argent += argent;
        uiRessourceManager.UpdateCompteurArgent(this.argent);
        uiBatimentItem.RefreshPrixBoutons();
    }

    public void RetirerArgent(float argent)
    {
        this.argent -= argent;
        uiRessourceManager.UpdateCompteurArgent(this.argent);
        uiBatimentItem.RefreshPrixBoutons();
    }

    public void AjouterHabitant(int habitants)
    {
        this.habitants += habitants;
        uiRessourceManager.UpdateCompteurHabitants(this.habitants);
    }

    public void RetirerHabitant(int habitants)
    {
        this.habitants -= habitants;

        if (this.habitants < 1)
            this.habitants = 1;

        uiRessourceManager.UpdateCompteurHabitants(this.habitants);
    }

    public float GetArgent()
    {
        return this.argent;
    }

    public int GetHabitant()
    {
        return this.habitants;
    }
}
