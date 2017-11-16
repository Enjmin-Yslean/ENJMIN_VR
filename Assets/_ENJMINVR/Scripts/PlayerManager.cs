using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour {

    private Player m_Player;
    public GameObject m_PlayerUIPanel;
    public GameObject m_HeadCube;
    public GameObject m_SpotLight;
    public UI_Player m_PlayerUI;

    public float m_MovementPadding = 0.3f;

    public int m_Life = 3;
    public float m_MoveSpeed = 1;
    private float m_AngleSpeed = 1;
    public AnimationCurve m_AngleSpeedCurve;

    public float m_DefaultRumbleTime = 0.1f;
    public float m_HeadCubeBlinkDuration = 0.3f;

    private bool m_Alive = true;
    public bool isAlive { get { return m_Alive; } }

    private bool m_Invincible = false;

    void Awake()
    {
        InitializePlayer();
    }
	// Use this for initialization
	void Start ()
    {
        m_Player = Player.instance;
    }

    void InitializePlayer()
    {
        m_HeadCube.SetActive(false);
        ShowCanvas(false);
        m_SpotLight.SetActive(false);
        m_PlayerUI.InitializeUI(m_Life);
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
        if (m_Alive)
        {
            float yRotation = m_Player.hmdTransform.rotation.eulerAngles.y;
            Quaternion rot = Quaternion.Euler(0, yRotation, 0);
            m_AngleSpeed = m_AngleSpeedCurve.Evaluate(Quaternion.Angle(m_Player.transform.rotation, rot)/180) * 180;
            m_Player.transform.rotation = Quaternion.RotateTowards(m_Player.transform.rotation, rot, Time.deltaTime * m_AngleSpeed);
        }
    }

    public void Hurt(int damage)
    {
        if (!m_Invincible)
        {
            m_Life -= damage;
            m_PlayerUI.PerteCoeur(m_Life);
            if (m_Life <= 0)
            {
                GameOver();
            }
            StartCoroutine(Rumble(m_DefaultRumbleTime));
            StartCoroutine(HeadCubeBlink());
        }
    }

    void GameOver()
    {
        m_Life = 0;
        m_Alive = false;
        m_Player.headCollider.enabled = false;
        ShowCanvas("YOU DIED");
    }

    public void Victory()
    {
        m_Invincible = true;

        m_Player.headCollider.enabled = false;

        ShowCanvas("YOU ESCAPED");
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

    float m_HeadCubeTimer = 0;

    IEnumerator HeadCubeBlink()
    {
        m_HeadCube.SetActive(true);
        while (m_HeadCubeTimer < m_HeadCubeBlinkDuration)
        {
            m_HeadCubeTimer += Time.deltaTime;
            yield return null;
        }

        m_HeadCubeTimer = 0;
        m_HeadCube.SetActive(false);
        yield return null;
    }

    public void TurnLight(bool onOff)
    {
        m_SpotLight.SetActive(onOff);
    }
}
