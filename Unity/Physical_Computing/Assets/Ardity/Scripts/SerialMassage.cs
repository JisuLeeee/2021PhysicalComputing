using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SerialMassage : MonoBehaviour
{

    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [SerializeField] private GameObject LightManagement;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Preset == null)
            return;

        if (!Application.isPlaying)
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        //int number = int.Parse(msg);
        if (msg != null)
        {
            //Debug.Log("Arrived: " + msg);
            try
            {
                int number = int.Parse(msg);
                if (Preset == null)
                    return;
                else
                {
                    UpdateLighting(number / 24f);
                }
            }
            catch { }
        }
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }


    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }


    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }

        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    public void PlayBGM(int idx, float loudness)
    {
        //Debug.Log("BGM: " + idx);
        //Debug.Log("Amp: " + loudness);
        var v = (byte)vmap(loudness, 0, 0.12f, 0, 127);
        var s = System.Text.Encoding.ASCII.GetString(new[] { v });
        //Debug.Log("Amv: " + s);
        LightManagement.GetComponent<SerialController>().SendSerialMessage(idx.ToString() + s);
    }

    private static float vmap(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
