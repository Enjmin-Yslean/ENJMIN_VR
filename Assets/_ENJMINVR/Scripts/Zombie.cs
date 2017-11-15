using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Zombie : MonoBehaviour {

	public Animator m_Animator;

	public float m_Life = 1;
	public float m_WalkSpeed = 1;
	public float m_AttackSpeed = 1;

    public float m_VisionDistance = 10;
    public float m_AttackZone = 1.5f;

    public PlayerManager m_PlayerManager;
    private Player m_Player;
    private float m_DistanceToPlayer = 0;
    private Transform m_ActualTarget = null;

    private bool m_IsHuntingPlayer = false;
    private bool m_IsAttacking = false;
    private bool m_IsMoving = false;

	// Use this for initialization
	void Start ()
    {
        m_Player = Player.instance;
        Stop();
    }
	
	// Update is called once per frame
	void Update () 
	{
        m_DistanceToPlayer = Vector3.Distance(transform.position, m_Player.hmdTransform.position);

        if (Input.GetKeyDown(KeyCode.Space))
            Attack();
        
        if (IsSeeingPlayer() && !m_IsHuntingPlayer) //Voir le joueur
        {
            StartHunting(m_Player.hmdTransform);
        }
        else if (!IsSeeingPlayer() && m_IsHuntingPlayer)
            Stop();

    }

    void SetTarget(Transform target)
    {
        m_ActualTarget = target;
    }

    void Move(Transform target)
    {
        if (target != null)
        {
            //Debug.Log("Move");
            StopWalking();
            m_IsMoving = true;
            m_Animator.SetTrigger("Move");
            SetTarget(target);
            StartCoroutine(Walk());
        }
    }

    IEnumerator Walk()
    {
        while (m_DistanceToPlayer > m_AttackZone)
        {
            LookAtTarget();
            transform.localPosition += transform.forward * m_WalkSpeed * Time.deltaTime;
            yield return null;
        }
    }

    void StopWalking()
    {
        StopCoroutine(Walk());
    }

    void StartHunting(Transform target)
    {
        StopCoroutine(Hunt());
        m_IsHuntingPlayer = true;
        SetTarget(target);
        StartCoroutine(Hunt());
    }

    IEnumerator Hunt()
    {
        Move(m_Player.hmdTransform);

        while (m_IsHuntingPlayer)
        {
            //Debug.Log("Hunt");
            if (m_DistanceToPlayer <= m_AttackZone && !m_IsAttacking)
            {
                Attack();
                yield return null;
            }
            else if (!m_IsAttacking && !m_IsMoving)
            {
                Move(m_ActualTarget);
                yield return null;
            }
            yield return null;
        }
        yield return null;
    }

    void Attack()
    {
        if (m_PlayerManager.isAlive)
        {
            //Debug.Log("Attack");
            StopWalking();
            m_IsMoving = false;
            LookAtTarget();
            m_Animator.SetTrigger("Attack");
            m_PlayerManager.Hurt(1);
            m_IsAttacking = true;
        }
    }

    void Stop()
    {
        //Debug.Log("Stop");
        m_IsMoving = false;
        StopAllCoroutines();
        m_IsHuntingPlayer = false;
        m_Animator.SetTrigger("Idle");
    }

    void LookAtTarget()
    {
        if(m_ActualTarget != null)
            transform.LookAt(new Vector3 (m_ActualTarget.position.x, transform.position.y, m_ActualTarget.position.z));
    }

    bool IsSeeingPlayer()
    {
        if (m_Player != null)
        {
            RaycastHit hit;
            Vector3 ToPlayerDir = (m_Player.hmdTransform.position - transform.position).normalized;
            Physics.Raycast(transform.position, ToPlayerDir, out hit, m_VisionDistance);
            if (hit.collider != m_Player.headCollider)
                return false;
            else
                return true;
        }
        else return false;
    }

    void OnDrawGizmos()
    {
        if(m_Player != null)
            Gizmos.DrawLine(transform.position, m_Player.hmdTransform.position);
    }

	#region ZombieAnimation

	public void EndAttack()
	{
        m_IsAttacking = false;
	}
	#endregion
}
