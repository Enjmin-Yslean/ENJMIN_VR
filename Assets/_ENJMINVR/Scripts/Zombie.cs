using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

	public Animator m_Animator;

	public float m_Life = 1;
	public float m_WalkSpeed = 1;
	public float m_AttackSpeed = 1;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			m_Animator.SetTrigger("Attack");
		}

        Move(Camera.current.transform);
	}

    void Move(Transform target)
    {
        transform.LookAt(target);
        transform.localPosition += Vector3.forward * m_WalkSpeed;
    }


	#region ZombieAnimation

	public void EndAttack()
	{
		m_Animator.Play ("Walk");
	}
	#endregion
}
