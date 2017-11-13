using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerMoveManager : MonoBehaviour {

    private Player m_Player;

    public float m_MovementPadding = 0.3f;

    public float m_Life = 3;
    public float m_MoveSpeed = 1;
    public float m_AngleSpeed = 1;

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
        Debug.Log(leftYAxis);
        if (Mathf.Abs(leftYAxis) > m_MovementPadding)
        {
            Turn(angle);
            Move(m_Player.transform.forward * leftYAxis, leftYAxis * m_MoveSpeed);
        }
    }

    void Move(Vector3 direction, float speed)
    {
        m_Player.transform.position += new Vector3 (direction.x, m_Player.transform.position.y, direction.z).normalized * speed * Time.deltaTime;
    }

    void Turn(float angle)
    {
        Quaternion rot = Quaternion.Euler(0, m_Player.hmdTransform.rotation.eulerAngles.y, 0);
        m_AngleSpeed = Quaternion.Angle(m_Player.transform.rotation, rot);
        m_Player.transform.rotation = Quaternion.RotateTowards(m_Player.transform.rotation, rot, Time.deltaTime * m_AngleSpeed) ;
    }

    void OnDrawGizmos()
    {
        if(m_Player != null)
            Gizmos.DrawLine(transform.position, new Vector3(0, m_Player.hmdTransform.rotation.eulerAngles.y, 0));
    }
}
