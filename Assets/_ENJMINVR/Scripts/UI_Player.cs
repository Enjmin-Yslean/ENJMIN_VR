using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour {

    public Image coeur;
    public GameObject panel;
    public int nbCoeur = 3;

    private int actual;
    private List<Image> coeurs = new List<Image>();

	// Use this for initialization
	void Start ()
    {
        Reset_UI();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PerteCoeur()
    {
        coeurs.Remove(coeurs[nbCoeur-1]);
        Destroy(coeurs[nbCoeur - 1]);
        nbCoeur -= 1;
        GameOver();
    }

    public bool GameOver ()
    {
        if (nbCoeur != 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Reset_UI()
    {
        actual = nbCoeur;
        if (coeurs.Count != 0)
        {
            for (int i = 0; i <= coeurs.Count; i++)
            {
                Destroy(coeurs[i-1]);
            }
        }
        
        coeurs.Clear();
        for (int i = 0; i <= nbCoeur - 1; i++)
        {
            coeurs.Add(Instantiate(coeur, panel.transform));
        }
    }
}
