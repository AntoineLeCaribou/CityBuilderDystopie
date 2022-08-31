using UnityEngine;

public class MaterialGetter : MonoBehaviour
{
    public static Material objetEnRotation;
    public static Material objetEnSupression;
    public static Material objetPlacable;
    public static Material objetNonPlacable;

    public static void Init()
    {
        objetEnRotation = Resources.Load<Material>("Materials/vert transparent");
        objetEnSupression = Resources.Load<Material>("Materials/noir transparent");
        objetPlacable = Resources.Load<Material>("Materials/bleu transparent");
        objetNonPlacable = Resources.Load<Material>("Materials/rouge transparent");
    }
}
