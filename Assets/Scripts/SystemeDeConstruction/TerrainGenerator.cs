using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public static Texture2D noiseTex;
    public static Color[] pix;
    public static Renderer rend;

    public static Grid GenererTerrain(Grid grille, int largeur, int hauteur, GridBuildingSystem systemeDeBuild)
    {
        float vraieSeed = Random.Range(1.1f, 5.0f);

        //generation du perlin noise
        float[,] matriceDeBruit = PerlinNoise.GenererPerlinNoise(largeur, hauteur, vraieSeed);
        PerlinNoise.ArrayToTexture(largeur, hauteur, matriceDeBruit);

        //generation des rivieres
        grille = AjouterRiviere(grille, largeur, hauteur, systemeDeBuild);

        //generation de la foret
        grille = AjouterVegetation(grille, largeur, hauteur, matriceDeBruit, systemeDeBuild);

        //generation du sol
        grille = AjouterSol(grille, largeur, hauteur, systemeDeBuild);

        return grille;
    }

    private static Grid AjouterRiviere(Grid grille, int largeur, int hauteur, GridBuildingSystem systemeDeBuild)
    {
        int nbAleatoire = Random.Range(1, 2+1);

        int xdebut = 0;
        int ydebut = 0;

        int mouvementx = 0;
        int mouvementy = 0;

        int mouvement1x = 0;
        int mouvement1y = 0;

        int mouvement2x = 0;
        int mouvement2y = 0;

        int cheminEmprunte = 0;

        if (nbAleatoire == 1) {
            xdebut = 0;
            ydebut = Random.Range(1, hauteur-1);
            mouvementx = 1;
            mouvement1y = 1;
            mouvement2y = -1;
        }

        else if (nbAleatoire == 2)
        {
            xdebut = Random.Range(1, largeur-1);
            ydebut = 0;
            mouvementy = 1;
            mouvement1x = 1;
            mouvement2x = -1;
        }

        systemeDeBuild.Placer(Encyclopedie.Riviere[0], xdebut, ydebut, 2, "eau", false);

        int currentx = xdebut;
        int currenty = ydebut;

        currentx += mouvementx;
        currenty += mouvementy;

        while (currentx > 0 && currenty > 0 && currentx < largeur && currenty < hauteur)
        {
            if (grille.GetValue(currentx, currenty) == 0)
            {
                systemeDeBuild.Placer(Encyclopedie.Riviere[0], currentx, currenty, 2, "eau", false);
            }
            
            nbAleatoire = Random.Range(1, 5+1);

            // = 3 chance sur 5 d'aller tout droit
            if (nbAleatoire >= 3)
            {
                if (cheminEmprunte == 0)
                    continue;

                currentx += mouvementx;
                currenty += mouvementy;
                cheminEmprunte = 0;

                if (grille.GetValue(currentx, currenty) > 0 || GetNbVoisinsRiviere(currentx, currenty, grille) >= 2)
                {
                    currentx -= mouvementx;
                    currenty -= mouvementy;
                }
            }
            else
            {
                if (cheminEmprunte == 1)
                    continue;

                //1 chance sur 5 d'aller à gauche
                if (nbAleatoire == 2)
                {
                    currentx += mouvement1x;
                    currenty += mouvement1y;
                    cheminEmprunte = 1;

                    if (grille.GetValue(currentx, currenty) > 0 || GetNbVoisinsRiviere(currentx, currenty, grille) >= 2)
                    {
                        currentx -= mouvement1x;
                        currenty -= mouvement1y;
                    }
                }
                //1 chance sur 5 d'aller à droite
                else
                {
                    if (cheminEmprunte == 2)
                        continue;

                    currentx += mouvement2x;
                    currenty += mouvement2y;
                    cheminEmprunte = 2;

                    if (grille.GetValue(currentx, currenty) > 0 || GetNbVoisinsRiviere(currentx, currenty, grille) >= 2)
                    {
                        currentx -= mouvement2x;
                        currenty -= mouvement2y;
                    }
                }
            }
        }

        if (grille.GetValue(currentx, currenty) == 0)
        {
            systemeDeBuild.Placer(Encyclopedie.Riviere[0], currentx, currenty, 2, "eau", false);
        }

        return grille;
    }

    private static Grid AjouterSol(Grid grille, int largeur, int hauteur, GridBuildingSystem systemeDeBuild)
    {
        for (int x = 0; x < largeur; x++)
        {
            for (int y = 0; y < hauteur; y++)
            {
                grille = RefreshSol(grille, x, y, largeur, hauteur, systemeDeBuild);
            }
        }

        return grille;
    }

    public static Grid RefreshSol(Grid grille, int x, int y, int largeur, int hauteur, GridBuildingSystem systemeDeBuild)
    {
        if (x < 0 || y < 0 || x >= largeur || y >= hauteur)
            return grille;

        GameObject prefab;
        Quaternion rot;
        float rotationY = 0f;

        bool bas = y == 0;
        bool droite = x == largeur - 1;
        bool gauche = x == 0;
        bool haut = y == hauteur - 1;

        bool basGauche = bas && gauche;
        bool basDroite = bas && droite;
        bool hautGauche = haut && gauche;
        bool hautDroite = haut && droite;

        //Si on a affaire à de la rivière
        if (grille.GetNom(x, y) == "eau")
        {
            //Mur externe coin
            if (basGauche || basDroite || hautGauche || hautDroite)
            {
                prefab = Encyclopedie.Riviere[3];

                if (basGauche)
                    rotationY = 0;
                else if (basDroite)
                    rotationY = -180;
                else if (hautDroite)
                    rotationY = -270;
                else
                    rotationY = 0;

                rot = Quaternion.Euler(new Vector3(0, rotationY, 0));
                systemeDeBuild.PlacerSol(prefab, x, y, rot, true);
            }
            //Mur externe
            if (gauche || droite || haut || bas)
            {
                prefab = Encyclopedie.Riviere[2];

                if (bas)
                    rotationY = 0;
                else if (droite)
                    rotationY = -90;
                else if (haut)
                    rotationY = -180;
                else
                    rotationY = -270;

                rot = Quaternion.Euler(new Vector3(0, rotationY, 0));
                systemeDeBuild.PlacerSol(prefab, x, y, rot, true);
            }

            //Mur interne
            prefab = Encyclopedie.Riviere[1];

            if (grille.GetValue(x + 1, y) != 2 && grille.GetValue(x + 1, y) != -1)
            {
                rotationY = -90;
                rot = Quaternion.Euler(new Vector3(0, rotationY, 0));
                systemeDeBuild.PlacerSol(prefab, x, y, rot, true);
            }

            if (grille.GetValue(x - 1, y) != 2 && grille.GetValue(x - 1, y) != -1)
            {
                rotationY = -270;
                rot = Quaternion.Euler(new Vector3(0, rotationY, 0));
                systemeDeBuild.PlacerSol(prefab, x, y, rot, true);
            }

            if (grille.GetValue(x, y + 1) != 2 && grille.GetValue(x, y + 1) != -1)
            {
                rotationY = -180;
                rot = Quaternion.Euler(new Vector3(0, rotationY, 0));
                systemeDeBuild.PlacerSol(prefab, x, y, rot, true);
            }

            if (grille.GetValue(x, y - 1) != 2 && grille.GetValue(x, y - 1) != -1)
            {
                rotationY = 0;
                rot = Quaternion.Euler(new Vector3(0, rotationY, 0));
                systemeDeBuild.PlacerSol(prefab, x, y, rot, true);
            }

            return grille;
        }

        //Si on a affaire à du sol normal
        if (grille.GetNom(x, y) != "eau")
        {
            if (basGauche || basDroite || hautGauche || hautDroite)
            {
                prefab = Encyclopedie.SolHerbe[0];

                if (basGauche)
                    rotationY = -90;
                else if (basDroite)
                    rotationY = -180;
                else if (hautDroite)
                    rotationY = -270;
                else
                    rotationY = -360;
            }
            else if (gauche || droite || haut || bas)
            {
                prefab = Encyclopedie.SolHerbe[1];

                if (bas)
                    rotationY = 0;
                else if (droite)
                    rotationY = -90;
                else if (haut)
                    rotationY = -180;
                else
                    rotationY = -270;
            }
            else
            {
                prefab = Encyclopedie.SolHerbe[2];
                rotationY = 0;
            }

            Quaternion rotation = Quaternion.Euler(new Vector3(0, rotationY, 0));
            systemeDeBuild.PlacerSol(prefab, x, y, rotation, true);
        }

        //Si on a affaire à une route
        if (grille.GetNom(x, y) == "route")
        {
            Destroy(grille.GetObject(x, y));

            int nbVoisins = GetNbVoisinsRoute(x, y, grille);

            bool rGauche = grille.GetNom(x - 1, y) == "route";
            bool rDroite = grille.GetNom(x + 1, y) == "route";
            bool rHaut   = grille.GetNom(x, y + 1) == "route";
            bool rBas    = grille.GetNom(x, y - 1) == "route";

            if (nbVoisins == 1)
            {
                //Cul de sac
                prefab = Encyclopedie.Route[5];

                if (rHaut)
                    rotationY = 0;
                else if (rDroite)
                    rotationY = 90;
                else if (rBas)
                    rotationY = 180;
                else if (rGauche)
                    rotationY = 270;
            }
            else if (nbVoisins == 2)
            {
                //route en I
                if ((rGauche && rDroite) || (rHaut && rBas))
                {
                    prefab = Encyclopedie.Route[1];

                    if (rGauche && rDroite)
                    {
                        rotationY = 90f;
                    }
                    else if (rHaut && rBas)
                    {
                        rotationY = 0f;
                    }
                }
                //route en L
                else
                {
                    prefab = Encyclopedie.Route[2];

                    if (rHaut && rDroite)
                    {
                        rotationY = 0f;
                    }
                    else if (rBas && rDroite)
                    {
                        rotationY = 90f;
                    }
                    else if (rBas && rGauche)
                    {
                        rotationY = 180f;
                    }
                    else if (rHaut && rGauche)
                    {
                        rotationY = 270f;
                    }
                }
            }
            else if (nbVoisins == 3)
            {
                //Route en T
                prefab = Encyclopedie.Route[3];

                if (rHaut && rBas && rDroite)
                {
                    rotationY = 0f;
                }
                else if (rBas && rDroite && rGauche)
                {
                    rotationY = 90f;
                }
                else if (rBas && rGauche && rHaut)
                {
                    rotationY = 180f;
                }
                else if (rHaut && rGauche && rDroite)
                {
                    rotationY = 270f;
                }
            }
            else if (nbVoisins == 4)
            {
                //Route en +
                prefab = Encyclopedie.Route[4];
            }
            else
            {
                //Defaut (vide)
                prefab = Encyclopedie.Route[0];
            }

            Quaternion rotation = Quaternion.Euler(new Vector3(0, rotationY, 0));
            GameObject nouvelleRoute = Instantiate(prefab, grille.GetCoords(x, y), rotation);
            grille.SetObject(x, y, nouvelleRoute);
        }

        return grille;
    }

    private static int GetNbVoisinsRiviere(int x, int y, Grid grille)
    {
        int i = 0;

        if (grille.GetValue(x + 1, y) > 0)
            i++;

        if (grille.GetValue(x - 1, y) > 0)
            i++;

        if (grille.GetValue(x, y + 1) > 0)
            i++;

        if (grille.GetValue(x, y - 1) > 0)
            i++;

        return i;
    }

    private static int GetNbVoisinsRoute(int x, int y, Grid grille)
    {
        int i = 0;

        if (grille.GetNom(x + 1, y) == "route")
            i++;

        if (grille.GetNom(x - 1, y) == "route")
            i++;

        if (grille.GetNom(x, y + 1) == "route")
            i++;

        if (grille.GetNom(x, y - 1) == "route")
            i++;

        return i;
    }

    private static Grid AjouterVegetation(Grid grille, int largeur, int hauteur, float[,] matriceDeBruit, GridBuildingSystem systemeDeBuild)
    {
        for (int x = 0; x < largeur; x++)
        {
            for (int y = 0; y < hauteur; y++)
            {
                if (grille.GetValue(x, y) > 0)
                    continue;

                //Forêts
                if (matriceDeBruit[x, y] > 0.55f)
                {
                    systemeDeBuild.Placer(Encyclopedie.GetObjetAleatoire(Encyclopedie.Arbres), x, y, 1, "arbre", true);
                }

                //Buissons
                if (matriceDeBruit[x, y] < 0.15f)
                {
                    systemeDeBuild.Placer(Encyclopedie.GetObjetAleatoire(Encyclopedie.Buissons), x, y, 1, "buisson", true);
                }

                //Rochers
                if (matriceDeBruit[x, y] >= 0.15f && matriceDeBruit[x, y] < 0.3f)
                {
                    systemeDeBuild.Placer(Encyclopedie.GetObjetAleatoire(Encyclopedie.Rochers), x, y, 1, "rocher", true);
                }
            }
        }

        return grille;
    }
}
