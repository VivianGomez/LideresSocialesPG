using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONWriter : MonoBehaviour
{
    public int diaActual;
    public List<int> cantidadAlimentos;
    public int porcentajeEnergia;

    // Start is called before the first frame update
    void Start()
    {
        diaActual = 1;
        cantidadAlimentos = new List<int>();
        cantidadAlimentos.Add(1);
        cantidadAlimentos.Add(2);
        cantidadAlimentos.Add(3);
        porcentajeEnergia = 100;
    }
    
    private JSONWriter CreateSaveGameObject()
    {
        GameObject p = GameObject.Find("Main Camera");
        JSONWriter save = p.GetComponent<JSONWriter>();
        print(cantidadAlimentos.Count);
        for (int i=0; i<cantidadAlimentos.Count; i++)
        {
            save.cantidadAlimentos.Add(i);
        }

        save.diaActual = diaActual;
        save.porcentajeEnergia = porcentajeEnergia;

        return save;
    }
    
    public void SaveAsJSON()
    {
        JSONWriter save = CreateSaveGameObject();
        string json = JsonUtility.ToJson(save);

        print("Saving as JSON: ");
    }
}
