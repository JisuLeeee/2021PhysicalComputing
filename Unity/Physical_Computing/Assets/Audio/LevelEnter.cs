using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnter : MonoBehaviour
{
    public string bgmName = "";

    private GameObject CamObject;

    void Start()
    {
        CamObject = GameObject.Find("Main Camera");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "myplayer")
        {
            CamObject.GetComponent<SoundManager>().PlayBGM(bgmName);
        }
    }
}
