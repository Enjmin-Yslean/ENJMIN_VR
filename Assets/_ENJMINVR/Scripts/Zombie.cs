using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Zombie : MonoBehaviour {

	public Animator m_Animator;

	public float m_Life = 1;
	public float m_WalkSpeed = 1;
	public float m_AttackSpeed = 1;

    public float m_AttackZone = 1.5f;

    private Player m_Player;
    private float m_DistanceToPlayer = 0;

	// Use this for initialization
	void Start ()
    {
        m_Player = Player.instance;
    }
	
	// Update is called once per frame
	void Update () 
	{
        m_DistanceToPlayer = Vector3.Distance(transform.position, m_Player.hmdTransform.position);

        if (Input.GetKeyDown(KeyCode.Space))
            Attack(Player.instance.hmdTransform);
        
        if(Input.GetKeyDown(KeyCode.Alpha0))
            Move(m_Player.hmdTransform);
    }

    void Move(Transform target)
    {
        StopAllCoroutines();
        m_Animator.SetTrigger("Move");
        StartCoroutine(Walk(target));
    }

   IEnumerator Walk(Transform target)
    {
        while (m_DistanceToPlayer > m_AttackZone)
        {
            LookAt(target);
            transform.localPosition += transform.forward * m_WalkSpeed * Time.deltaTime;
            
            yield return null;
        }

        Attack(Player.instance.hmdTransform);
        yield return null;
    }

    void Attack(Transform target)
    {
        StopAllCoroutines();
        LookAt(target);
        m_Animator.SetTrigger("Attack");
    }

    void LookAt(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, transform.localPosition.y, target.position.z));
    }


	#region ZombieAnimation

	public void EndAttack()
	{
        m_Animator.SetTrigger("Idle");
	}
	#endregion
}
