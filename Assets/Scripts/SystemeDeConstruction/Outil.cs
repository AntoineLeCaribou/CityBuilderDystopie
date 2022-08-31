using System.Collections.Generic;
using UnityEngine;

public abstract class Outil
{
    public virtual void EstDansLeVide(GameObject objetDansLaMain)
    {
        return;
    }
    public virtual void EstSurLeTerrain(GameObject objetDansLaMain, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        return;
    }
    public virtual void UtilisationPrincipale(GameObject objetDansLaMain, GridBuildingSystem gridBuildingSystem, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        return;
    }
    public virtual void UtilisationSecondaire(GameObject objetDansLaMain)
    {
        return;
    }
    public virtual void OnDelete(GameObject objetDansLaMain)
    {
        return;
    }
    public virtual string GetUtilitePrincipale()
    {
        return "";
    }
    public virtual string GetUtiliteSecondaire()
    {
        return "";
    }
}

public class Selection : Outil
{
    //Cet outil sert juste à se promener sans faire de mauvaises actions
}

public class Rotation : Outil
{
    private Vector2Int anciennePosition = new Vector2Int(-1, -1);
    private GameObject ancienObjet = null;
    private Material[] anciensMateriaux;

    public override void EstSurLeTerrain(GameObject objetDansLaMain, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        //Si on veut tourner le vide alors ne rien faire
        if (idCaseTouchee == 0)
            return;

        //Si on traite le même objet qu'avant, ne rien faire
        if (anciennePosition.Equals(grille.GetGridCellFromWorldPosition(Utils.ConvertirVector3inXYZintoXZY(positionSourisDansLeMonde3D))))
            return;

        //----------------Donc c'est une nouvelle case-------------------

        //On recupere l'objet
        GameObject objet = grille.GetObject(positionSourisDansLeMonde3D);

        //On rend sa couleur à l'ancien objet
        if (ancienObjet != null)
            Utils.RendreMaterialsAuxEnfants(ancienObjet, anciensMateriaux);

        //On sauvegarde l'objet et la position
        ancienObjet = objet;
        anciennePosition = grille.GetGridCellFromWorldPosition(Utils.ConvertirVector3inXYZintoXZY(positionSourisDansLeMonde3D));

        //Si on a les fonds
        Material material;
        if (objet.GetComponent<ItemModification>().rotationnable == true)
        {
            material = MaterialGetter.objetEnRotation;
        }
        else //Sinon
        {
            material = MaterialGetter.objetNonPlacable;
        }

        anciensMateriaux = Utils.ChangerMaterialsDesEnfants(objet, material);

        return;
    }

    public override void UtilisationPrincipale(GameObject objetDansLaMain, GridBuildingSystem gridBuildingSystem, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        //Si la case est occupée
        if (idCaseTouchee > 0)
        {
            //On recupere l'objet
            GameObject objet = grille.GetObject(positionSourisDansLeMonde3D);

            //Si l'objet n'est pas tournable
            if (objet.GetComponent<ItemModification>().rotationnable == false)
            {
                //TODO: jouer son pour dire que c'est pas bon
                return;
            }

            //On tourne l'objet que l'on regarde
            Vector3 rotation = objet.transform.rotation.eulerAngles;
            rotation += new Vector3(0, 90, 0);
            objet.transform.rotation = Quaternion.Euler(rotation);
        }
    }

    public override void OnDelete(GameObject objetDansLaMain)
    {
        if (ancienObjet != null)
        {
            Utils.RendreMaterialsAuxEnfants(ancienObjet, anciensMateriaux);
            anciennePosition = new Vector2Int(-1, -1);
            ancienObjet = null;
        }
        return;
    }

    public override string GetUtilitePrincipale()
    {
        return "Tourner";
    }
}

public class Creer : Outil
{
    public override void EstDansLeVide(GameObject objetDansLaMain)
    {
        objetDansLaMain.SetActive(false);
        return;
    }

    public override void EstSurLeTerrain(GameObject objetDansLaMain, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        objetDansLaMain.SetActive(true);
        objetDansLaMain.transform.position = grille.GetCoords(positionSourisDansLeMonde3D);

        int idDuSolConstructiblePourCetObjet = objetDansLaMain.GetComponent<ShopItem>().placableSurTelID;
        float prix = objetDansLaMain.GetComponent<ShopItem>().prixDeLobjet;

        //Si c'est constructible et qu'on a les fonds
        if (idDuSolConstructiblePourCetObjet == idCaseTouchee && ressourceManager.GetArgent() >= prix)
        {
            for (int i = 0; i < objetDansLaMain.transform.childCount; i++)
            {
                objetDansLaMain.transform.GetChild(i).GetComponent<MeshRenderer>().material = MaterialGetter.objetPlacable;
            }
        }
        else //Sinon
        {
            for (int i = 0; i < objetDansLaMain.transform.childCount; i++)
            {
                objetDansLaMain.transform.GetChild(i).GetComponent<MeshRenderer>().material = MaterialGetter.objetNonPlacable;
            }
        }

        return;
    }

    public override void UtilisationPrincipale(GameObject objetDansLaMain, GridBuildingSystem gridBuildingSystem, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        ShopItem info = objetDansLaMain.GetComponent<ShopItem>();

        //Si je peux placer ma structure et que j'ai les fonds
        if (idCaseTouchee == info.placableSurTelID && ressourceManager.GetArgent() >= info.prixDeLobjet)
        {
            //On retire l'argent
            ressourceManager.RetirerArgent(info.prixDeLobjet);

            //On place la structure
            Vector2Int pos = gridBuildingSystem.Placer(gridBuildingSystem.objetAPlacer, positionSourisDansLeMonde3D, info.id, info.nomDeLobjet);

            //On met à jour le sol
            TerrainGenerator.RefreshSol(grille, pos.x, pos.y, gridBuildingSystem.largeur, gridBuildingSystem.hauteur, gridBuildingSystem);

            //On actualise les 4 cases frontalières
            gridBuildingSystem.SupprimerSol(pos.x - 1, pos.y);
            gridBuildingSystem.SupprimerSol(pos.x + 1, pos.y);
            gridBuildingSystem.SupprimerSol(pos.x, pos.y - 1);
            gridBuildingSystem.SupprimerSol(pos.x, pos.y + 1);
            TerrainGenerator.RefreshSol(grille, pos.x - 1, pos.y, gridBuildingSystem.largeur, gridBuildingSystem.hauteur, gridBuildingSystem);
            TerrainGenerator.RefreshSol(grille, pos.x + 1, pos.y, gridBuildingSystem.largeur, gridBuildingSystem.hauteur, gridBuildingSystem);
            TerrainGenerator.RefreshSol(grille, pos.x, pos.y - 1, gridBuildingSystem.largeur, gridBuildingSystem.hauteur, gridBuildingSystem);
            TerrainGenerator.RefreshSol(grille, pos.x, pos.y + 1, gridBuildingSystem.largeur, gridBuildingSystem.hauteur, gridBuildingSystem);

            return;
        }

        //Sinon je ne peux pas placer ma structure
        //jouer son d'erreur
    }

    public override void UtilisationSecondaire(GameObject objetDansLaMain)
    {
        Vector3 rotation = objetDansLaMain.transform.rotation.eulerAngles;
        rotation += new Vector3(0, 90, 0);
        objetDansLaMain.transform.rotation = Quaternion.Euler(rotation);
    }

    public override void OnDelete(GameObject objetDansLaMain)
    {
        objetDansLaMain.SetActive(false);
    }

    public override string GetUtilitePrincipale()
    {
        return "Placer";
    }

    public override string GetUtiliteSecondaire()
    {
        return "Tourner";
    }
}

public class Supprimer : Outil
{
    private Vector2Int anciennePosition = new Vector2Int(-1, -1);
    private GameObject ancienObjet = null;
    private Material[] anciensMateriaux;

    public override void EstSurLeTerrain(GameObject objetDansLaMain, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        //Si on veut supprimer le vide alors ne rien faire
        if (idCaseTouchee == 0)
            return;
        
        //Si on traite le même objet qu'avant, ne rien faire
        if (anciennePosition.Equals(grille.GetGridCellFromWorldPosition(Utils.ConvertirVector3inXYZintoXZY(positionSourisDansLeMonde3D))))
            return;

        //----------------Donc c'est une nouvelle case-------------------

        //On recupere l'objet
        GameObject objet = grille.GetObject(positionSourisDansLeMonde3D);

        //On rend sa couleur à l'ancien objet
        if (ancienObjet != null)
            Utils.RendreMaterialsAuxEnfants(ancienObjet, anciensMateriaux);

        //On sauvegarde l'objet et la position
        ancienObjet = objet;
        anciennePosition = grille.GetGridCellFromWorldPosition(Utils.ConvertirVector3inXYZintoXZY(positionSourisDansLeMonde3D));

        //On recupere le prix de destruction
        float prix_de_destruction = objet.GetComponent<ItemModification>().prixDeDestruction;

        //Si on a les fonds
        Material material;
        if (ressourceManager.GetArgent() >= prix_de_destruction)
        {
            material = MaterialGetter.objetNonPlacable;
        }
        else //Sinon
        {
            material = MaterialGetter.objetEnSupression;
        }

        anciensMateriaux = Utils.ChangerMaterialsDesEnfants(objet, material);

        return;
    }

    public override void UtilisationPrincipale(GameObject objetDansLaMain, GridBuildingSystem gridBuildingSystem, Grid grille, Vector3 positionSourisDansLeMonde3D, int idCaseTouchee, RessourceManager ressourceManager)
    {
        //Si on veut supprimer le vide alors ne rien faire
        if (idCaseTouchee == 0)
            return;

        //On recupere l'objet
        GameObject objet = grille.GetObject(positionSourisDansLeMonde3D);

        //On recupere le prix de destruction
        float prix_de_destruction = objet.GetComponent<ItemModification>().prixDeDestruction;

        //Si on a pas les moyens
        if (prix_de_destruction > ressourceManager.GetArgent())
        {
            //jouer son pour dire qu'on a pas les moyens
            Debug.Log("trop pauvre pour supprimer cet obstacle");
            return;
        }

        //Donc on paye le prix et on retire l'objet
        ressourceManager.RetirerArgent(prix_de_destruction);
        gridBuildingSystem.Retirer(positionSourisDansLeMonde3D);
    }

    public override void OnDelete(GameObject objetDansLaMain)
    {
        if (ancienObjet != null)
        {
            Utils.RendreMaterialsAuxEnfants(ancienObjet, anciensMateriaux);
            anciennePosition = new Vector2Int(-1, -1);
            ancienObjet = null;
        }
        return;
    }

    public override string GetUtilitePrincipale()
    {
        return "Détruire";
    }
}
