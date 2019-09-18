using UnityEngine;
using UnityEngine.UI;
using System;

public class EnergyBar : MonoBehaviour
{
	public Scrollbar energyBar;
	public float Energy = 100;

    //private JSONLoader jsonLoader;

    private JSONLoaderJuego0 jsonLoader;

    void Awake()
    {
        jsonLoader = GameObject.FindObjectOfType<JSONLoaderJuego0>();
    }

    void Update()
    {
        Hungry(0.4e-2f);
    }

    public void Hungry(float value)
	{
		Energy -= value;
		energyBar.size = Energy / 100f;
	}

    public void Comer(int indice)
    {
        Energy += (float)jsonLoader.darPorcentajeEnergiaAlimento(indice);
        energyBar.size = Energy / 100f;
    }

}

