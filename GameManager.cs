using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{

    public Vector3 SpwanPoint;
    public int pathCount;
    public static GameManager instance;
    public float pathMoveSpeed = 8f;
    public int coinCollected = 0;
    public float distance = 0;
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] TextMeshProUGUI scoreText_TMP;
    [SerializeField] TextMeshProUGUI distanceText_TMP;
    [SerializeField] AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioManager.PlayBGM(audioManager.backgroundMusic,isLoop:true,isStop:false);

         

    }

    // Update is called once per frame
    void Update()
    {
        //distance = Mathf.Tr;
        distanceText_TMP.text = distance.ToString();
    }

    public void updateCoinScore()
    {
        coinCollected += 1;
        scoreText_TMP.text = coinCollected.ToString();
    }

}
