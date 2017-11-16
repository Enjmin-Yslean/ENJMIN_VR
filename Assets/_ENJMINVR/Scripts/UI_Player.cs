using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour {

    public Image coeur;
    public GameObject panel;
    private int m_NbHeart;
    public Text m_FPSText;

    private List<Image> coeurs = new List<Image>();

	// Use this for initialization
	void Start ()
    {
        //Reset_UI();
        m_FPSText.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.F))
        {
            m_FPSText.gameObject.SetActive(!m_FPSText.gameObject.activeInHierarchy);
        }
        if (m_FPSText.gameObject.activeInHierarchy && Time.frameCount % 45 == 0)
            FPSCounter();

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

    void FPSCounter()
    {
        float fps = Mathf.Round(1 / Time.deltaTime);
        m_FPSText.text = "FPS : " + fps;
    }
}
