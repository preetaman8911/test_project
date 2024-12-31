using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingObject : MonoBehaviour
{

    private Material _mat;
    private Color[] _colors = { Color.yellow, Color.red };
    private float _flashSpeed = 0.1f;
    private float _lengthOfTimeToFlash = 1f;

    public void Awake()
    {

        _mat = GetComponent<MeshRenderer>().material;
        _mat.color = Color.red;

    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Player))
        {
            StartCoroutine(Flash(_lengthOfTimeToFlash, _flashSpeed, collision));
            collision.gameObject.transform.GetChild(7).gameObject.SetActive(true);
        }
    }
    IEnumerator Flash(float time, float intervalTime, Collision collision)
    {
        float elapsedTime = 0f;
        int index = 0;
        while (elapsedTime < time)
        {
            //_mat.SetColor("_Color", _colors[index % 2]);

            elapsedTime += Time.deltaTime;
            index++;
            collision.gameObject.transform.GetChild(7).gameObject.SetActive(false);
            yield return new WaitForSeconds(intervalTime);
            Destroy(gameObject, intervalTime);
        }
    }
}