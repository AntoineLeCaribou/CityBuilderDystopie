using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuage : MonoBehaviour
{
    [Header("Paramètres")]
    public Vector3 limit = new Vector3(1000f, 0f, 0f);
    public Vector3 respawn = new Vector3(-800, 0f, 0f);
    public Vector3 mouvement = new Vector3(10f, 0f, 0f);

    private void FixedUpdate()
    {
        Vector3 position = gameObject.transform.position;

        gameObject.transform.position = position + mouvement;

        if (limit.x != 0 && position.x > limit.x)
            gameObject.transform.position = new Vector3(respawn.x, position.y, position.z);

        else if (limit.y != 0 && position.y > limit.y)
            gameObject.transform.position = new Vector3(position.x, respawn.y, position.z);

        else if (limit.z != 0 && position.z > limit.z)
            gameObject.transform.position = new Vector3(position.x, position.y, respawn.z);
    }
}
