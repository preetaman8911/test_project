using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
   
    void Start()
    {
        animator = transform.parent.transform.GetChild(2).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == Constants.Player)
        {

            
            GameManager.instance.pathMoveSpeed = 0;
            other.gameObject.GetComponent<PlayerMovement>().moveSpeed = 0f;
            other.gameObject.SetActive(false);
            animator.Play(Constants.GotTreasure);
            transform.parent.transform.GetChild(2).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            var transposer = GameManager.instance.virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            GameManager.instance.virtualCamera.m_Follow = transform.parent.transform.GetChild(2);
               // Change the offset
             transposer.m_TrackedObjectOffset = new Vector3(x: 0, y:0, z: 0);


        }

    }
}
