using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBar : MonoBehaviour
{
	public Scrollbar energyBar;
	public float Energy = 100;

    void Update()
    {
        Hungry(0.4e-2f);
    }

    public void Hungry(float value)
	{
		Energy -= value;
		energyBar.size = Energy / 100f;
	}

}

