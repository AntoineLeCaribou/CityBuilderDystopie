using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeModifier : MonoBehaviour
{
    private int heureLever;
    private int minuteLever;

    private int heureCoucher;
    private int minuteCoucher;

    private int heureZenith;
    private int minuteZenith;

    private int heureActuelle;
    private int minuteActuelle;

    private int jour;
    private int mois;

    private int[,] leverDesMois;
    private int[,] coucherDesMois;
    private int[,] zenithDesMois;

    [Header("R�f�rences")]
    public TMP_Text textAMettreAJour;
    public Light lumiereDirectionnelle;
    public Camera cam;

    [Header("Couleurs du ciel")]
    public Color orange;
    public Color nuit;
    public Color zenith;
    public Color bleufonce;

    [Header("Intensit� lumi�re naturelle")]
    [Range(0f,2f)]
    public float intensiteMaxSoleil = 1.5f;
    [Range(0f, 2f)]
    public float intensiteMaxLune = 0.5f;

    [Header("Angle de la camera")]
    public float angleLumiere = -90;

    private int tempsZenith;
    private int tempsLever;
    private int tempsCoucher;
    private int tempsZenithLunaire;
    private int tempsZenithLunaireApresMinuit;

    private bool premierJour;

    private float _time;
    public float _interval = 0.1f;

    // Start is called before the first frame update
    void Awake()
    {
        cam.backgroundColor = nuit;
        premierJour = true;
        _time = 0f;

        leverDesMois = new int[12,2];
        coucherDesMois = new int[12,2];
        zenithDesMois = new int[12, 2];

        //Janvier - 8h19 - 49.9 s
        leverDesMois[0, 0] = 8; leverDesMois[0, 1] = 45;
        coucherDesMois[0, 0] = 17; coucherDesMois[0, 1] = 4;
        zenithDesMois[0, 0] = 12; zenithDesMois[0, 1] = 54;

        //Fevrier - 9h27 - 56.7 s
        leverDesMois[1, 0] = 8; leverDesMois[1, 1] = 21;
        coucherDesMois[1, 0] = 17; coucherDesMois[1, 1] = 48;
        zenithDesMois[1, 0] = 13; zenithDesMois[1, 1] = 04;

        //Mars - 11h01 - 66.1 s
        leverDesMois[2, 0] = 7; leverDesMois[2, 1] = 32;
        coucherDesMois[2, 0] = 18; coucherDesMois[2, 1] = 33;
        zenithDesMois[2, 0] = 13; zenithDesMois[2, 1] = 02;

        //Avril - 12h53 - 77.3 s
        leverDesMois[3, 0] = 7; leverDesMois[3, 1] = 28;
        coucherDesMois[3, 0] = 20; coucherDesMois[3, 1] = 21;
        zenithDesMois[3, 0] = 13; zenithDesMois[3, 1] = 54;

        //Mai - 14h35 - 87.5 s
        leverDesMois[4, 0] = 6; leverDesMois[4, 1] = 30;
        coucherDesMois[4, 0] = 21; coucherDesMois[5, 1] = 05;
        zenithDesMois[4, 0] = 13; zenithDesMois[5, 1] = 47;

        //Juin - 15h53 - 95.3 s
        leverDesMois[5, 0] = 5; leverDesMois[5, 1] = 52;
        coucherDesMois[5, 0] = 21; coucherDesMois[5, 1] = 45;
        zenithDesMois[5, 0] = 13; zenithDesMois[5, 1] = 48;

        //Juillet - 16h05 - 96.5 s
        leverDesMois[6, 0] = 5; leverDesMois[6, 1] = 52;
        coucherDesMois[6, 0] = 21; coucherDesMois[6, 1] = 57;
        zenithDesMois[6, 0] = 13; zenithDesMois[6, 1] = 54;

        //Aout - 15h04 - 90.4 s
        leverDesMois[7, 0] = 6; leverDesMois[7, 1] = 25;
        coucherDesMois[7, 0] = 21; coucherDesMois[7, 1] = 29;
        zenithDesMois[7, 0] = 13; zenithDesMois[7, 1] = 57;

        //Septembre - 13h25 - 80.5 s
        leverDesMois[8, 0] = 7; leverDesMois[8, 1] = 08;
        coucherDesMois[8, 0] = 20; coucherDesMois[8, 1] = 33;
        zenithDesMois[8, 0] = 13; zenithDesMois[8, 1] = 50;

        //Octobre - 11h38 - 69.8 s
        leverDesMois[9, 0] = 7; leverDesMois[9, 1] = 51;
        coucherDesMois[9, 0] = 19; coucherDesMois[9, 1] = 29;
        zenithDesMois[9, 0] = 13; zenithDesMois[9, 1] = 40;

        //Novembre - 9h52 - 59.2 s
        leverDesMois[10, 0] = 7; leverDesMois[10, 1] = 38;
        coucherDesMois[10, 0] = 17; coucherDesMois[10, 1] = 30;
        zenithDesMois[10, 0] = 12; zenithDesMois[10, 1] = 34;

        //Decembre - 8h32 - 57.2 s
        leverDesMois[11, 0] = 8; leverDesMois[11, 1] = 24;
        coucherDesMois[11, 0] = 16; coucherDesMois[11, 1] = 56;
        zenithDesMois[11, 0] = 12; zenithDesMois[11, 1] = 40;

        Init();
    }

    private void Update()
    {
        if (_interval == 0f)
            return;

        _time += Time.deltaTime;
        while (_time >= _interval)
        {  
            ProchaineMinute();
            _time -= _interval;
        }
    }

    private void Init()
    {
        mois = 8;
        ProchainJour();
    }

    private void ResetHeure()
    {
        if (premierJour)
        {
            heureActuelle = 8;
            minuteActuelle = 0;
            premierJour = false;
        }

        heureLever = leverDesMois[mois - 1, 0];
        minuteLever = leverDesMois[mois - 1, 1];

        heureCoucher = coucherDesMois[mois - 1, 0];
        minuteCoucher = coucherDesMois[mois - 1, 1];

        heureZenith = zenithDesMois[mois - 1, 0];
        minuteZenith = zenithDesMois[mois - 1, 1];

        tempsZenith = heureZenith * 60 + minuteZenith;
        tempsLever = heureLever * 60 + minuteLever;
        tempsCoucher = heureCoucher * 60 + minuteCoucher;
        tempsZenithLunaire = tempsCoucher + (tempsLever + (24 * 60 - tempsCoucher)) / 2;
        tempsZenithLunaireApresMinuit = tempsZenithLunaire - (24 * 60);

        tempsZenithLunaire = 24 * 60; //minuit
    }

    private void ProchaineMinute()
    {
        minuteActuelle++;
        if (minuteActuelle == 60)
        {
            minuteActuelle = 0;
            heureActuelle++;
        }

        UpdateTempsDuReveil();
        UpdateLuminosite();
        UpdateAngleLumiere();
        UpdateColorCamera();


        if (heureActuelle == 24)
        {
            heureActuelle = 0;
            ProchainJour();
        }
    }

    private void ProchainJour()
    {
        jour++;
        if (jour == 31)
        {
            jour = 1;
            mois++;
        }

        if (mois == 13)
        {
            mois = 1;
        }

        lumiereDirectionnelle.transform.rotation = Quaternion.Euler(new Vector3(50, -140, 0));

        ResetHeure(); //On recupere l'heure de lever et celle de coucher
    }

    private void UpdateTempsDuReveil()
    {
        textAMettreAJour.SetText(this.ToString());
    }

    private void UpdateLuminosite()
    {
        float intensite = 0;
        float pourcentage = 0;

        int tempsActuel = heureActuelle * 60 + minuteActuelle;


        //si on est apr�s le coucher et avant le zenith lunaire (apr�s minuit)
        if (tempsActuel < tempsZenithLunaireApresMinuit)
        {
            //On calcule le pourcentage (entre 0.5f et 1.0f)
            pourcentage = (tempsActuel - tempsZenithLunaire) / (float)(tempsZenithLunaireApresMinuit - tempsZenithLunaire) ;
            intensite = Mathf.Clamp(pourcentage * intensiteMaxLune, 0.1f, intensiteMaxLune);
        }
        //sinon, si on est apr�s le zenith lunaire et avant le lever
        else if (tempsActuel < tempsLever)
        {
            //On calcule le pourcentage (entre 1.0f et 0.0f)
            pourcentage = Mathf.Abs((tempsActuel - tempsZenithLunaireApresMinuit) / (float)(tempsLever - tempsZenithLunaireApresMinuit) - 1);
            intensite = Mathf.Clamp(pourcentage * intensiteMaxLune, 0.1f, intensiteMaxLune);
        }
        //sinon, si on est avant le zenith
        else if (tempsActuel < tempsZenith)
        {
            //On calcule le pourcentage (entre 0.0f et 1.0f)
            pourcentage = (tempsActuel - tempsLever) / (float)(tempsZenith - tempsLever);
            intensite = Mathf.Clamp(pourcentage * intensiteMaxSoleil, 0.1f, intensiteMaxSoleil);
        }
        //sinon, si on est apr�s le zenith et avant le coucher
        else if (tempsActuel < tempsCoucher)
        {
            //On calcule le pourcentage (entre 1.0f et 0.0f)
            pourcentage = Mathf.Abs((tempsActuel - tempsZenith) / (float)(tempsCoucher - tempsZenith) - 1);
            intensite = Mathf.Clamp(pourcentage * intensiteMaxSoleil, 0.1f, intensiteMaxSoleil);
        }
        //sinon, si on est apr�s le coucher et avant le zenith lunaire (avant minuit)
        else if (tempsActuel < tempsZenithLunaire)
        {
            //On calcule le pourcentage (entre 0.0f et 0.5f)
            pourcentage = (tempsActuel - tempsCoucher) / (float)(tempsZenithLunaire - tempsCoucher);
            intensite = Mathf.Clamp(pourcentage * intensiteMaxLune, 0.1f, intensiteMaxLune);
        }

        lumiereDirectionnelle.intensity = intensite;
    }

    private void UpdateAngleLumiere()
    {
        float division = 0;
        int tempsActuel = heureActuelle * 60 + minuteActuelle;
        Vector3 vecteur = Vector3.zero;

        //si on est apr�s le coucher et avant le zenith lunaire (apr�s minuit)
        if (tempsActuel < tempsZenithLunaireApresMinuit)
        {
            //On calcule le pourcentage (entre 0.5f et 1.0f)
            division = (tempsActuel) / (float)(tempsZenithLunaireApresMinuit);
            vecteur = Vector3.Lerp(new Vector3(50, -140, 0), new Vector3(50, -180, 0), division);
        }
        //sinon, si on est apr�s le zenith lunaire et avant le lever
        else if (tempsActuel < tempsLever)
        {
            //On calcule le pourcentage (entre 1.0f et 0.0f)
            division = (tempsActuel - tempsZenithLunaireApresMinuit) / (float)(tempsLever - tempsZenithLunaireApresMinuit);
            vecteur = Vector3.Lerp(new Vector3(50, -180, 0), new Vector3(50, -270, 0), division);
        }
        //sinon, si on est avant le zenith
        else if (tempsActuel < tempsZenith)
        {
            //On calcule le pourcentage (entre 1.0 et 0.0f)
            division = (tempsActuel - tempsLever) / (float)(tempsZenith - tempsLever);
            vecteur = Vector3.Lerp(new Vector3(50, -270, 0), new Vector3(50, -360, 0), division);
        }
        //sinon, si on est entre le zenith et le coucher
        else if (tempsActuel < tempsCoucher)
        {
            //On calcule le pourcentage (entre 0.0 et 1.0f)
            division = (tempsActuel - tempsZenith) / (float)(tempsCoucher - tempsZenith);
            vecteur = Vector3.Lerp(new Vector3(50, -360, 0), new Vector3(50, -450, 0), division);
        }
        //sinon, si on est apr�s le coucher et avant le zenith lunaire (avant minuit)
        else if (tempsActuel < tempsZenithLunaire)
        {
            //On calcule le pourcentage (entre 0.0 et 1.0f)
            division = (tempsActuel - tempsCoucher) / (float)(tempsZenithLunaire - tempsCoucher);
            vecteur = Vector3.Lerp(new Vector3(50, -450, 0), new Vector3(50, -500, 0), division);
        }

        lumiereDirectionnelle.transform.rotation = Quaternion.Euler(vecteur);
    }

    private void UpdateColorCamera()
    {
        int tempsActuel = heureActuelle * 60 + minuteActuelle;

        //si on est avant le zenith
        if (tempsActuel < tempsZenith )
        {
            float division = (tempsActuel - tempsLever) / (float)(tempsZenith - tempsLever);
            cam.backgroundColor = Color.Lerp(nuit, zenith, division);
        }
        //sinon, si on est apr�s
        else
        {
            float division = (tempsActuel - tempsZenith) / (float)(tempsCoucher - tempsZenith);
            cam.backgroundColor = Color.Lerp(zenith, nuit, division);
        }
    }

    #region util

    public override string ToString()
    {
        return AjouterZeroNombre(heureActuelle.ToString()) + " : "+ AjouterZeroNombre(minuteActuelle.ToString());
    }

    private static string AjouterZeroNombre(string nombre)
    {
        if (nombre.Length == 1)
        {
            return "0" + nombre;
        }
        return nombre;
    }

    #endregion
}
