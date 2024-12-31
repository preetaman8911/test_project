using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] PathManager pathManager;

    public float triggerDistance = 10f;
    AudioManager audioManager;
    Animator animator;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag(Constants.AudioManager).GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Find the nearest joker box and play animation
        GameObject box = GetTheNearestJokerBox();
        if (box)
        {

            box.GetComponent<Animator>().Play(Constants.JokerBoxAnime);
            // Show the gilters when box is opened.
            Transform particleObj = box.transform.GetChild(0);

            if (particleObj.GetComponent<ParticleSystem>() != null)
            {
                ParticleSystem particle = particleObj.GetComponent<ParticleSystem>();

                // check if particles are playing, if not then play
                if (!particle.isPlaying)
                {
                   
                    audioManager.PlaySFX(audioManager.jokerBox);
                    particle.Play();
                }
            }
        }
       
        
    }


    GameObject GetTheNearestJokerBox()
    {
        
        GameObject[] boxes = GameObject.FindGameObjectsWithTag(Constants.JokerBox);

        foreach(GameObject box in boxes)
        {
            float distance = Vector3.Distance(transform.position, box.transform.position);
            
            if (triggerDistance >= distance)
            {
                return box;
            }
        }

        return null;
    }

    private void OnTriggerExit(Collider other)
    {
     
        if (other.gameObject.CompareTag(Constants.CenterPoint) && gameObject.CompareTag(Constants.Player))
        {
            
            pathManager.InstantiateEnv(new Vector3(x: 0, y: 0, z: GameManager.instance.SpwanPoint.z));
            
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      
        // check if colliion is coins then update the score and destory the coin object
        if (!transform.GetChild(0).gameObject.GetComponent<MagnetPower>().isMagnetActive && other.gameObject.CompareTag(Constants.Coins) )
        {
           
            GameManager.instance.updateCoinScore();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag(Constants.Power))
        {
            Destroy(other.gameObject);
            StartCoroutine(startRunning());

        }
        if (other.gameObject.CompareTag(Constants.Magnet))
        {
            Destroy(other.gameObject);
            StartCoroutine(onMagnetPower());

        }
    }

    IEnumerator onMagnetPower()
    {

        // active the magnet object

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.GetComponent<MagnetPower>().ActivateMagnetPower(10f);
        yield return new WaitForSeconds(10f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

   


    IEnumerator startRunning()
    {
        GameManager.instance.pathMoveSpeed = 18f;
        transform.GetComponent<PlayerMovement>().moveSpeed = 18f;
        animator.SetBool(Constants.Is_Walk, true);
        yield return new WaitForSeconds(5f);
        animator.SetBool(Constants.Is_Walk, false);
        GameManager.instance.pathMoveSpeed = 12f;
        transform.GetComponent<PlayerMovement>().moveSpeed = 12f;
    }


}
