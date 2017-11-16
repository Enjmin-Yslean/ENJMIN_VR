using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour {

    public Image coeur;
    public GameObject panel;
    public int m_NbHeart;

    private int actual;
    private List<Image> coeurs = new List<Image>();

	// Use this for initialization
	void Start ()
    {
        //Reset_UI();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PerteCoeur(int newHeartNb)
    { 
        Destroy(coeurs[newHeartNb]);
        coeurs.RemoveAt(newHeartNb);
        m_NbHeart = newHeartNb;
    }

    public void InitializeUI(int nbHeart)
    {
        m_NbHeart = nbHeart;

        if (coeurs.Count != 0)
        {
            for (int i = 0; i <= coeurs.Count - 1; i++)
            {
                Destroy(coeurs[i]);
            }
            coeurs.Clear();
        }
        

        for (int i = 0; i <= nbHeart - 1; i++)
        {
            coeurs.Add(Instantiate(coeur, panel.transform));
        }
    }
}
