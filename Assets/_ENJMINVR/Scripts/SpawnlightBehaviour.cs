using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnlightBehaviour : MonoBehaviour {

    public GameObject m_Spawnlight;
    public PlayerManager m_PlayerManager;
    public ParticleSystem m_ParticleSystem;

    public float m_TimeBeforeBlinking = 2f;
    public float m_BlinkingTime = 3f;

    void Start()
    {
        StartCoroutine(Behaviour());
    }

    private float m_Timer = 0;

    IEnumerator Behaviour()
    {
        yield return new WaitForSeconds(m_TimeBeforeBlinking);

        while(m_Timer < m_BlinkingTime)
        {
            if(m_Timer > m_BlinkingTime/10 && m_Timer < m_BlinkingTime / 9)
            {
                m_Spawnlight.SetActive(false);
            }
            if(m_Timer> m_BlinkingTime / 3 && m_Timer < m_BlinkingTime / 2.8f)
            {
                m_Spawnlight.SetActive(true);
            }
            if(m_Timer > m_BlinkingTime / 2.7f && m_Timer < m_BlinkingTime / 2.4f)
            {
                m_Spawnlight.SetActive(false);
            }
            if(m_Timer > m_BlinkingTime / 2 && m_Timer < m_BlinkingTime / 1.7f)
            {
                m_Spawnlight.SetActive(true);
            }
            if (m_Timer > m_BlinkingTime / 1.5f && m_Timer < m_BlinkingTime / 1.4f)
            {
                m_Spawnlight.SetActive(false);
                m_ParticleSystem.Play();
            }
             m_Timer += Time.deltaTime;
            yield return null;
        }
        m_PlayerManager.TurnLight(true);
    }
}
