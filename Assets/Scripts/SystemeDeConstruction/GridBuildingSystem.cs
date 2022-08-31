using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    private Outil outil;

    public RessourceManager ressourceManager;

    private Grid grid;

    public int largeur = 20;
    public int hauteur = 20;
    public float tailleCellule = 20f;

    private int idEnCache;
    private string nomEnCache;
    private float prixEnCache;

    public Transform parentCellules = null;
    public Transform parentLignes = null;
    public Transform parentBatiments = null;
    public Transform parentSol = null;

    public Material materialLigneGrille;
    public Material materialObjetPlacable;
    public Material materialObjetNonPlacable;

    public GameObject objetAPlacer;
    public GameObject objetDansLaMain;

    public Vector3 vecteurDecalageCamera;
    public Vector3 offsetBatimentCase;

    private Vector3 offsetMain = new(0f, 0.05f, 0f);

    private void Awake()
    {
        Encyclopedie.Init();
        MaterialGetter.Init();

        grid = new Grid(largeur, hauteur, tailleCellule, parentCellules);
        grid = TerrainGenerator.GenererTerrain(grid, largeur, hauteur, this);

        DessinerGrille();
        SetPrefabAPlacer(objetAPlacer, 1, "arbre", 999);
    }

    public void SetOutil(Outil outil)
    {
        //On active le trigger de supression de l'outil
        if (this.outil != null)
            this.outil.OnDelete(this.objetDansLaMain);

        this.outil = outil;
    }

    public void SetPrefabAPlacer(GameObject objet, int id, string nom, float prix)
    {
        this.objetAPlacer = objet;
        Destroy(this.objetDansLaMain);
        this.objetDansLaMain = Instantiate(objet, Vector3.zero + offsetMain, Quaternion.identity);

        this.idEnCache = id;
        this.nomEnCache = nom;
        this.prixEnCache = prix;

        this.objetDansLaMain.SetActive(false);
    }

    private void Update()
    {
        bool isUI;
        Vector3 positionDeLaSourisDansLeMonde3D = Utils.GetMousePositionIn3DWorld(out isUI);

        if (isUI)
            return;

        int idCaseTouchee = grid.GetValue(positionDeLaSourisDansLeMonde3D);

        //si on vise le vide
        if (idCaseTouchee == -1)
        {
            outil.EstDansLeVide(objetDansLaMain);
            return;
        }

        //donc on vise le terrain
        outil.EstSurLeTerrain(objetDansLaMain, grid, positionDeLaSourisDansLeMonde3D, idCaseTouchee, ressourceManager);

        //si on clic gauche
        if (Input.GetMouseButtonDown(0))
        {
            outil.UtilisationPrincipale(objetDansLaMain, this, grid, positionDeLaSourisDansLeMonde3D, idCaseTouchee, ressourceManager);
        }
        //si on appuie sur la touche R
        if (Input.GetKeyDown(KeyCode.R))
        {
            outil.UtilisationSecondaire(objetDansLaMain);
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
        Quaternion rotation = this.objetDansLaMain.transform.rotation;
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

    public void SupprimerSol(int x, int y)
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
        int nbAleatoire = Random.Range(0, 3+1);
        return Quaternion.Euler(new Vector3(0, nbAleatoire*90, 0));
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
