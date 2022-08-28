using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Sensibilité")]
    public Vector2 cameraSensitivity = new Vector2(300f, 300f);
    public float flySpeed = 200f;
    public float moletteSpeed = 800f;
    public float scrollSpeed = 5000f;

    private float xRotation;

    [Header("Références")]
    public GameObject CameraRig;

    //initialisation 
    private void Start()
    {
        xRotation = transform.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Si on maintient clique droit
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity.x * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity.y * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            CameraRig.transform.Rotate(Vector3.up * mouseX, Space.World);
        }

        //Si on maintient la molette
        else if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 mouvementMolette = transform.right * mouseX * -1 + transform.up * mouseY * -1;
            CameraRig.GetComponent<CharacterController>().Move(mouvementMolette * moletteSpeed * Time.deltaTime);
        }

        float axeX = Input.GetAxisRaw("Horizontal");
        float axeY = Input.GetAxisRaw("Vertical");
        float axeMolette = Input.GetAxis("Mouse ScrollWheel");

        float vitesseDeScroll = scrollSpeed;
        float vitesseDeVol = flySpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            vitesseDeVol *= 2;
            vitesseDeScroll *= 2;
        }

        Vector3 mouvementTouches = transform.right * axeX * vitesseDeVol + transform.forward * (axeY * vitesseDeVol + axeMolette * vitesseDeScroll);
        CameraRig.GetComponent<CharacterController>().Move(mouvementTouches * Time.deltaTime);
    }
}
