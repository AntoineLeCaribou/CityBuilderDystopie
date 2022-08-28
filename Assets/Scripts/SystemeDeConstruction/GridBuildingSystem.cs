using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    private Grid grid;

    public int largeur = 20;
    public int hauteur = 20;
    public int valeurEnCache;
    public string nomEnCache;
    public float tailleCellule = 20f;
    public Color couleurGrille = Color.white;

    public Transform parentCellules = null;
    public Transform parentLignes = null;
    public Transform parentBatiments = null;
    public Transform parentSol = null;

    public Material materialLigneGrille;
    public Material materialObjetPlacable;
    public Material materialObjetNonPlacable;

    public GameObject objetAPlacer;
    public GameObject ObjetDansLaMain;

    public Vector3 vecteurDecalageCamera;
    public Vector3 offsetBatimentCase;

    private void Awake()
    {
        Encyclopedie.Init();
        grid = new Grid(largeur, hauteur, tailleCellule, parentCellules);
        grid = TerrainGenerator.GenererTerrain(grid, largeur, hauteur, this);
        DessinerGrille();
        SetPrefabAPlacer(objetAPlacer, 1, "arbre");
    }

    public void SetPrefabAPlacer(GameObject objet, int valeur, string nom)
    {
        this.objetAPlacer = objet;
        Destroy(this.ObjetDansLaMain);
        this.ObjetDansLaMain = Instantiate(objet, Vector3.zero, Quaternion.identity);
        this.valeurEnCache = valeur;
        this.nomEnCache = nom;
    }

    private void Update()
    {
        bool isUI;
        Vector3 positionDeLaSourisDansLeMonde3D = Utils.GetMousePositionIn3DWorld(out isUI);

        if (isUI)
            return;

        int valeurCaseTouchee = grid.GetValue(positionDeLaSourisDansLeMonde3D);

        //Si on n'a rien touché dans le monde 3d
        if (valeurCaseTouchee == -1)
        {
            this.ObjetDansLaMain.gameObject.SetActive(false);
            return;
        }

        ////On a touché à une case !
        this.ObjetDansLaMain.gameObject.SetActive(true);
        this.ObjetDansLaMain.transform.position = grid.GetCoords(positionDeLaSourisDansLeMonde3D);

        //Si la case est constructible
        if (valeurCaseTouchee == 0)
        {
            for (int i = 0; i < this.ObjetDansLaMain.transform.childCount; i++)
            {
                this.ObjetDansLaMain.transform.GetChild(i).GetComponent<MeshRenderer>().material = materialObjetPlacable;
            }
        }
        //Sinon, si la case est déjà prise
        else
        {
            for (int i = 0; i < this.ObjetDansLaMain.transform.childCount; i++)
            {
                this.ObjetDansLaMain.transform.GetChild(i).GetComponent<MeshRenderer>().material = materialObjetNonPlacable;
            }
        }

        //On veut construire
        if (Input.GetMouseButton(0))
        {
            //Si la case est constructible
            if (valeurCaseTouchee == 0)
            {
                Vector2Int pos = Placer(objetAPlacer, positionDeLaSourisDansLeMonde3D, valeurEnCache, nomEnCache);
                TerrainGenerator.RefreshSol(grid, pos.x, pos.y, largeur, hauteur, this);

                SupprimerSol(pos.x - 1, pos.y);
                SupprimerSol(pos.x + 1, pos.y);
                SupprimerSol(pos.x, pos.y - 1);
                SupprimerSol(pos.x, pos.y + 1);

                TerrainGenerator.RefreshSol(grid, pos.x - 1, pos.y, largeur, hauteur, this);
                TerrainGenerator.RefreshSol(grid, pos.x + 1, pos.y, largeur, hauteur, this);
                TerrainGenerator.RefreshSol(grid, pos.x, pos.y - 1, largeur, hauteur, this);
                TerrainGenerator.RefreshSol(grid, pos.x, pos.y + 1, largeur, hauteur, this);
            }
            //Sinon, si la case est déjà prise
            else
            {
                Debug.Log("Impossible de construire ici ...");
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            Retirer(positionDeLaSourisDansLeMonde3D);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Si la case est occupée
            if (valeurCaseTouchee == 1)
            {
                //On tourne l'objet que l'on regarde
                GameObject objet = grid.GetObject(positionDeLaSourisDansLeMonde3D);
                Vector3 rotation = objet.transform.rotation.eulerAngles;
                rotation += new Vector3(0, 90, 0);
                objet.transform.rotation = Quaternion.Euler(rotation);
            }
            //Sinon, si la case est vide
            else
            {
                //On tourne l'objet de la main
                GameObject objet = this.ObjetDansLaMain;
                Vector3 rotation = objet.transform.rotation.eulerAngles;
                rotation += new Vector3(0, 90, 0);
                objet.transform.rotation = Quaternion.Euler(rotation);
            }
        }
    }

    #region Operations de batiments

    public Vector2Int Placer(GameObject objet, Vector3 position, int valeur, string nom, bool rotaRandom = false)
    {
        position = Utils.ConvertirVector3inXYZintoXZY(position);
        Vector2Int position2D = grid.GetGridCellFromWorldPosition(position);
        return Placer(objet, position2D.x, position2D.y, valeur, nom, rotaRandom);
    }

    public Vector2Int Placer(GameObject objet, int x, int y, int valeur, string nom = "vide", bool rotaRandom = false)
    {
        Quaternion rotation = this.ObjetDansLaMain.transform.rotation;
        if (rotaRandom)
            rotation = GetRandomRotation();

        GameObject nouvelObjet = Instantiate(objet, grid.GetCoords(x, y), rotation, parentBatiments);
        grid.SetValue(x, y, valeur);
        grid.SetObject(x, y, nouvelObjet);
        grid.SetNom(x, y, nom);
        SupprimerSol(x, y);

        return new Vector2Int(x, y);
    }

    public void PlacerSol(GameObject prefab, int x, int y, Quaternion rotation, bool Sol = false)
    {
        GameObject nouvelObjet;

        if (Sol == false)
            Instantiate(prefab, grid.GetCoords(x, y), rotation, parentBatiments);
        else
        {
            nouvelObjet = Instantiate(prefab, grid.GetCoords(x, y), rotation, parentSol);
            grid.AddSol(x, y, nouvelObjet);
        }
    }

    private void SupprimerSol(int x, int y)
    {
        if (x < 0 || y < 0 || x >= largeur || y >= hauteur)
            return;

        List<GameObject> sols = grid.GetSol(x, y);
        for (int i = sols.Count - 1; i >= 0; i--)
        {
            Destroy(sols[i]);
            sols.RemoveAt(i);
        }

        grid.SetSol(x, y, sols);
    }

    private Quaternion GetRandomRotation()
    {
        int nbAleatoire = Random.Range(0, 359+1);
        return Quaternion.Euler(new Vector3(0, nbAleatoire, 0));
    }

    public void Retirer(Vector3 position)
    {
        position = Utils.ConvertirVector3inXYZintoXZY(position);
        Vector2Int position2D = grid.GetGridCellFromWorldPosition(position);
        Retirer(position2D.x, position2D.y);
    }

    public void Retirer(int x, int y)
    {
        if (grid.GetValue(x, y) == 0)
            return;
            
        Destroy(grid.GetObject(x, y));
    
        List<GameObject> sols = grid.GetSol(x, y);
        for (int i = sols.Count-1; i >= 0 ; i--)
        {
            Destroy(sols[i]);
            sols.RemoveAt(i);
        }

        grid.SetSol(x, y, sols);
        grid.SetNom(x, y, "vide");
        grid.SetValue(x, y, 0);

        this.grid = TerrainGenerator.RefreshSol(grid, x, y, largeur, hauteur, this);

        SupprimerSol(x - 1, y);
        SupprimerSol(x + 1, y);
        SupprimerSol(x, y - 1);
        SupprimerSol(x, y + 1);

        TerrainGenerator.RefreshSol(grid, x - 1, y, largeur, hauteur, this);
        TerrainGenerator.RefreshSol(grid, x + 1, y, largeur, hauteur, this);
        TerrainGenerator.RefreshSol(grid, x, y - 1, largeur, hauteur, this);
        TerrainGenerator.RefreshSol(grid, x, y + 1, largeur, hauteur, this);
    }

    #endregion

    #region Operations de grille

    public void DessinerGrille()
    {
        Quaternion rotation90deg = Quaternion.Euler(new Vector3(90, 0, 0));

        for (int x = 0; x <= largeur; x++)
        {
            GameObject objet = new GameObject("x=" + x);
            objet.transform.position = Vector3.zero;
            objet.transform.rotation = rotation90deg;
            objet.transform.parent = parentLignes;
            LineRenderer lineRenderer = objet.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(x * tailleCellule, 0.01f, 0f));
            lineRenderer.SetPosition(1, new Vector3(x * tailleCellule, 0.01f, hauteur * tailleCellule));
            lineRenderer.material = materialLigneGrille;
            lineRenderer.alignment = LineAlignment.TransformZ;
            lineRenderer.startWidth = 0.5f;
        }

        for (int y = 0; y <= hauteur; y++)
        {
            GameObject objet = new GameObject("y=" + y);
            objet.transform.position = Vector3.zero;
            objet.transform.rotation = rotation90deg;
            objet.transform.parent = parentLignes;
            LineRenderer lineRenderer = objet.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(0f, 0.01f, y * tailleCellule));
            lineRenderer.SetPosition(1, new Vector3(largeur * tailleCellule, 0.01f, y * tailleCellule));
            lineRenderer.material = materialLigneGrille;
            lineRenderer.alignment = LineAlignment.TransformZ;
            lineRenderer.startWidth = 0.5f;
        }
    }

    public void ActiverGrille()
    {
        parentLignes.gameObject.SetActive(true);
    }

    public void DesactiverGrille()
    {
        parentLignes.gameObject.SetActive(false);
    }

    #endregion

}
