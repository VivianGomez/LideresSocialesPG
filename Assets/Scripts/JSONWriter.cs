using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEditor;

public class JSONWriter : MonoBehaviour
{
    public GameData infoPartida = new GameData(1, new int[] {0,0,0,0}, 100);
    JsonData jsonCreado;

    
    // Start is called before the first frame update
    void Start()
    {
        jsonCreado = JsonMapper.ToJson(infoPartida);
        File.WriteAllText(Application.dataPath + "/Gamedata.json", jsonCreado.ToString());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void reescribirJSON(int dia, int[] lista, int porcentajeBarra)
    {
        infoPartida = new GameData(dia, lista, porcentajeBarra);

        jsonCreado = JsonMapper.ToJson(infoPartida);
        File.WriteAllText(Application.dataPath + "/Gamedata.json", jsonCreado.ToString());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }  


}

public class GameData
{
    public int diaActual;
    public int[] cantidadAlimentos;
    public int porcentajeEnergia;

    public GameData(int dia, int[] cantidad, int porcentaje)
    {
        this.diaActual = dia;
        this.cantidadAlimentos = cantidad;
        this.porcentajeEnergia= porcentaje;

    }
}
