using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private TextMesh[,] textArray;
    private GameObject[,] objectArray;
    private List<GameObject>[,] solArray;
    private string[,] nomArray;

    #region Constructeur
    public Grid(int width, int height, float cellSize, Transform parentCellules)
    {
        //Initialisation des paramètres de la grille
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        //On initialise le tableau à 2 dimensions
        gridArray = new int[width, height];
        textArray = new TextMesh[width, height];
        objectArray = new GameObject[width, height];
        solArray = new List<GameObject>[width, height];
        nomArray = new string[width, height];

        //On calcule le décalage nécessaire à l'obtention des coordonnées du milieu d'une cellule
        Vector3 offsetMilieuCase = GetOffsetMilieuCellule(width, height);

        //On calculer l'objet quaternion permettant d'effectuer une rotation de 90°
        Vector3 VecteurDeRotation = new Vector3(90, 0, 0);
        Quaternion QuaternionDeRotation = Quaternion.Euler(VecteurDeRotation);

        //On instancie mes champs textes qui seront dans les cellules
        for (int x = 0; x < gridArray.GetLength(0); x++) {  
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                //On instancie la matrice de liste des sols
                solArray[x, y] = new List<GameObject>();
                //On instancie tous les noms en "vide"
                nomArray[x, y] = "vide";
                //On ajoute les textes au milieu des cellules
                TextMesh texte = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), parentCellules, GetWorldPositionFromGridCell(x, y) + offsetMilieuCase, 80, Color.white, TextAnchor.MiddleCenter);
                //On effectue une rotation pour que le texte se retrouve à plat
                texte.gameObject.transform.rotation = QuaternionDeRotation;
                //On renomme les textes
                texte.transform.name = x + "," + y;
                //On stocke le texte dans un tableau
                textArray[x, y] = texte;
                //On désactive le texte
                texte.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region Conversion Monde 3D <-> Grille

    private Vector3 GetWorldPositionFromGridCell(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize;
    }

    public Vector2Int GetGridCellFromWorldPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);
        return new Vector2Int(x, y);
    }

    #endregion

    #region Utils

    public Vector3 GetOffsetMilieuCellule(int width, int height)
    {
        return new Vector3(width, 0, height) * .5f;
    }

    #endregion

    #region Setters

    public void SetValue(int x, int y, int valeur)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = valeur;
            textArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 position, int valeur)
    {
        position = Utils.ConvertirVector3inXYZintoXZY(position);
        Vector2Int position2D = GetGridCellFromWorldPosition(position);
        SetValue(position2D.x, position2D.y, valeur);
    }

    public void SetObject(int x, int y, GameObject objet)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            objectArray[x, y] = objet;
        }
    }

    public void SetObject(Vector3 position, GameObject objet)
    {
        position = Utils.ConvertirVector3inXYZintoXZY(position);
        Vector2Int position2D = GetGridCellFromWorldPosition(position);
        SetObject(position2D.x, position2D.y, objet);
    }

    #endregion

    #region Getters

    public int GetValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return gridArray[x, y];
        }
        return -1;
    }
    
    public int GetValue(Vector3 position)
    {
        position = Utils.ConvertirVector3inXYZintoXZY(position);
        Vector2Int position2D = GetGridCellFromWorldPosition(position);
        return GetValue(position2D.x, position2D.y);
    }

    public Vector3 GetCoords(int x, int y)
    {
        return new Vector3(x * cellSize, 0, y * cellSize) + GetOffsetMilieuCellule(this.width, this.height);
    }

    public Vector3 GetCoords(Vector3 position)
    {
        position = Utils.ConvertirVector3inXYZintoXZY(position);
        Vector2Int position2D = GetGridCellFromWorldPosition(position);
        return GetCoords(position2D.x, position2D.y);
    }

    public GameObject GetObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return objectArray[x, y];
        }
        return null;
    }

    public GameObject GetObject(Vector3 position)
    {
        position = Utils.ConvertirVector3inXYZintoXZY(position);
        Vector2Int position2D = GetGridCellFromWorldPosition(position);
        return GetObject(position2D.x, position2D.y);
    }

    #endregion

    public void AddSol(int x, int y, GameObject sol)
    {
        solArray[x, y].Add(sol);
    }

    public List<GameObject> GetSol(int x, int y)
    {
        return solArray[x, y];
    }

    public void SetSol(int x, int y, List<GameObject> liste)
    {
        solArray[x, y] = liste;
    }

    public void ResetSol(int x, int y)
    {
        solArray[x, y] = new List<GameObject>();
    }

    public void SetNom(int x, int y, string nom)
    {
        nomArray[x, y] = nom;
    }

    public string GetNom(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return nomArray[x, y];
        }
        return "hors terrain";
    }
}
