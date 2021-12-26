using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }

    public BgmType[] BGMList;

    private AudioSource BGM;
    private string NowBGMname = "";
    private int NowIdx = 0;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    private float currentUpdateTime = 0f;
    private float[] clipSampleData;
    private float clipLoudness;

    [SerializeField] private GameObject LightManagement;

    void Start()
    {
        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        if (BGMList.Length > 0) PlayBGM(BGMList[0].name);
        clipSampleData = new float[sampleDataLength];
    }

    // Update is called once per frame
    void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            BGM.clip.GetData(clipSampleData, BGM.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
            LightManagement.GetComponent<SerialMassage>().PlayBGM(NowIdx, clipLoudness);
        }

    }

    public void PlayBGM(string name)
    {
        if (NowBGMname.Equals(name)) return;

        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                BGM.clip = BGMList[i].audio;
                BGM.Play();
                NowBGMname = name;
                NowIdx = i;
            }
    }
}