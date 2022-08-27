using System.Collections.Generic;
using UnityEngine;

public class Astar {

    public static List<Vector2Int> GetShortestPathFromOriginToDestination(int largeur, int hauteur, int xdebut, int ydebut, int xfin, int yfin, Grid grille)
    {
        //On initialise les listes
        List<AstarNode> Open_list = new List<AstarNode>();
        List<AstarNode> Closed_list = new List<AstarNode>();

        //On créé la case d'arrivée
        AstarNode caseDeFin = new AstarNode(xfin, yfin);

        //On créé la case départ
        AstarNode caseDeDebut = new AstarNode(xdebut, ydebut);
        caseDeDebut.g = 0;
        caseDeDebut.h = CalculateDistance(caseDeDebut, caseDeFin);
        caseDeDebut.f = caseDeDebut.g + caseDeDebut.h;

        //On ajoute la case départ à la liste
        Open_list.Add(caseDeDebut);

        //Tant qu'il nous reste des cases à explorer, on continue
        while (Open_list.Count > 0)
        {
            //On recupere la meilleure node de la liste ouverte
            AstarNode currentNode = GetBestAstarNodeWithLowestF(Open_list);

            //Si notre case est la case de fin, on a presque terminé !
            if (currentNode.Equals(caseDeFin))
            {
                Debug.Log("chemin trouvé !");
                return TraitementInverse(currentNode);
            }

            //On supprime cette node de la liste ouverte
            Open_list.Remove(currentNode);

            //On ajoute notre node à la liste fermée
            Closed_list.Add(currentNode);

            //On récupère tous ses voisins
            List<AstarNode> voisins = new List<AstarNode>();
            voisins.Add(new AstarNode(currentNode.x - 1, currentNode.y));
            voisins.Add(new AstarNode(currentNode.x, currentNode.y - 1));
            voisins.Add(new AstarNode(currentNode.x + 1, currentNode.y));
            voisins.Add(new AstarNode(currentNode.x, currentNode.y + 1));

            //On traite tous les voisins
            for (int i = 0; i < voisins.Count; i++)
            {
                AstarNode voisin = voisins[i];

                //Si le voisin est dans la liste fermée, on passe au prochain voisin
                if (Closed_list.Contains(voisin))
                {
                    continue;
                }

                //Si les coordonnées sont hors-piste, on passe au prochain voisin
                if (currentNode.x < 0 || currentNode.y < 0 || currentNode.x >= largeur || currentNode.y >= hauteur)
                {
                    continue;
                }
                
                //Si la case correspond à un obstacle, on passe au prochain voisin
                if (grille.GetValue(voisin.x, voisin.y) == 1)
                {
                    continue;
                }
                
                //Si le voisin est plus court que le parent ou que le voisin n'est pas dans la liste ouverte
                int nouveauCoutG = currentNode.g + 1;
                if (nouveauCoutG < voisin.g || !Open_list.Contains(voisin))
                {
                    voisin.parent = currentNode;
                    voisin.g = nouveauCoutG;
                    voisin.h = CalculateDistance(voisin, caseDeFin);
                    voisin.f = voisin.g + voisin.h;

                    //Si le voisin n'est pas dans la liste ouverte
                    if (!Open_list.Contains(voisin))
                    {
                        Open_list.Add(voisin);
                    }
                }
            }
        }

        Debug.Log("il n'existe pas de chemin menant à l'arrivée ...");

        return null;
    }

    
    private static AstarNode GetBestAstarNodeWithLowestF(List<AstarNode> listeDeNodes)
    {
        AstarNode meilleureNode = listeDeNodes[0];
        List<AstarNode> meilleuresNode = new List<AstarNode>();
        meilleuresNode.Add(meilleureNode);

        for (int i = 1; i < listeDeNodes.Count; i++)
        {
            AstarNode nodeActuelle = listeDeNodes[i];
            if (nodeActuelle.f == meilleureNode.f)
            {
                meilleuresNode.Add(nodeActuelle);
            }
            else if (nodeActuelle.f < meilleureNode.f)
            {
                meilleuresNode.Clear();
                meilleuresNode.Add(nodeActuelle);
            }
        }
        Debug.Log("taille liste meilleures nodes = " + meilleuresNode.Count);
        return meilleuresNode[0];
    }

    private static List<Vector2Int> TraitementInverse(AstarNode nodeParent)
    {
        List<Vector2Int> liste = new List<Vector2Int>();

        liste.Add(nodeParent.GetVector2());

        if (nodeParent.parent != null)
            liste.AddRange(TraitementInverse(nodeParent.parent));

        return liste;
    }
    /*
    private static AstarNode GetNodeAvecCoteLePlusGrand(List<AstarNode> listeDeNodes)
    {
        int plusGrandMaxDeCote = listeDeNodes[0].MaxDeCote();
        AstarNode meilleurNode = listeDeNodes[0];

        for (int i = 0; i < listeDeNodes.Count; i++)
        {
            AstarNode currentNode = listeDeNodes[i];
            if (currentNode.MaxDeCote() < plusGrandMaxDeCote)
            {
                plusGrandMaxDeCote = currentNode.MaxDeCote();
                meilleurNode = currentNode;
            }
        }
        Debug.Log("le meilleur est donc ("+ meilleurNode.x + ", " + meilleurNode.y+")");
        return meilleurNode;
    }
    */

    private static int CalculateDistance(AstarNode nodeA, AstarNode nodeB)
    {
        return Mathf.Abs(nodeA.x - nodeB.x) + Mathf.Abs(nodeA.y - nodeB.y);
    }
}
