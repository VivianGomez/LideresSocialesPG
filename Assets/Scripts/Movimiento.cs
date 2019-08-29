using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    Vector3 escala;
    float escalaX;
    float escalaY;
    bool permiteMoverse = true;

    void Start()
    {
        escala = transform.localScale;
        escalaX = escala.x;
        escalaY = escala.y;
    }

    void Update()
    {
        GameObject player = GameObject.Find("Personaje");

            transform.Translate(Input.GetAxis("Horizontal") * 5f * Time.deltaTime, 0f, 0f);
            transform.Translate(0f, Input.GetAxis("Vertical") * 5f * Time.deltaTime, 0f);

            if (Input.GetAxis("Horizontal") < 0)
            {
                escala.x = -escalaX;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                escala.x = escalaX;
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                escala.y = escalaY;
            }
            transform.localScale = escala;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Colisión con " + col.name);
    }
}