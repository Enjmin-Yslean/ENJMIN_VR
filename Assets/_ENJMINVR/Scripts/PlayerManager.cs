using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour {

    private Player m_Player;
    public GameObject m_PlayerUIPanel;

    public float m_MovementPadding = 0.3f;

    public float m_Life = 3;
    public float m_MoveSpeed = 1;
    public float m_AngleSpeed = 1;

    public float m_DefaultRumbleTime = 0.1f;

    private bool m_Alive = true;
    public bool isAlive { get { return m_Alive; } }

    void Awake()
    {
        ShowCanvas(false);
    }
	// Use this for initialization
	void Start ()
    {
        m_Player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {
        float leftYAxis = Input.GetAxis("Left_Y_Axis");
        float AngleSign = Vector3.Cross(m_Player.transform.forward, m_Player.hmdTransform.forward).y < 0 ? -1 : 1;
        float angle = AngleSign * Quaternion.Angle(m_Player.transform.rotation, m_Player.hmdTransform.rotation);

        if (Mathf.Abs(leftYAxis) > m_MovementPadding)
        {
            Turn(angle);
            Move(m_Player.transform.forward * leftYAxis, Mathf.Abs(leftYAxis * m_MoveSpeed)); //Vitesse toujours positive
        }
    }

    void Move(Vector3 direction, float speed)
    {
        if(m_Alive)
            m_Player.transform.position += new Vector3 (direction.x, m_Player.transform.position.y, direction.z).normalized * speed * Time.deltaTime;
    }

    void Turn(float angle)
    {
        Quaternion rot = Quaternion.Euler(0, m_Player.hmdTransform.rotation.eulerAngles.y, 0);
        m_AngleSpeed = Quaternion.Angle(m_Player.transform.rotation, rot);
        m_Player.transform.rotation = Quaternion.RotateTowards(m_Player.transform.rotation, rot, Time.deltaTime * m_AngleSpeed) ;
    }

    public void Hurt(int damage)
    {
        m_Life -= damage;
        if (m_Life <= 0)
        {
            GameOver();
        }
        StartCoroutine(Rumble(m_DefaultRumbleTime));
    }

    void GameOver()
    {
        m_Life = 0;
        m_Alive = false;
        m_Player.headCollider.enabled = false;
        ShowCanvas("YOU DIED");
    }

#region Vibrations
    float m_RumbleTimer = 0;

    IEnumerator Rumble(float time = 0.1f)
    {
        GamePad.SetVibration(0, 1.0f, 1.0f);
        while (m_RumbleTimer < time)
        {
            m_RumbleTimer += Time.deltaTime;
            yield return null;
        }

        m_RumbleTimer = 0;
        GamePad.SetVibration(0, 0, 0 );
        yield return null;
    }
    
    float m_PulseTimer = 0;

    IEnumerator TriggerHapticPulse(int time = 500)
    {
        while (m_PulseTimer < time)
        {
            m_Player.leftController.TriggerHapticPulse();
            m_Player.rightController.TriggerHapticPulse();



            m_PulseTimer += Time.deltaTime;
            yield return null;
        }
        m_PulseTimer = 0;
        yield return null;
    }
#endregion

    void ShowCanvas(bool show = true)
    {
        ShowCanvas("", show);
    }

    void ShowCanvas( string text = "", bool show = true)
    {
        m_PlayerUIPanel.gameObject.SetActive(show);
        Text tempText = m_PlayerUIPanel.GetComponentInChildren<Text>();
        tempText.text = text;
    }
}
