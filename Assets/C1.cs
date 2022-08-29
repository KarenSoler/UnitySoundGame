using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1 : MonoBehaviour
{
    [Range(20, 20000)]
    public float Frecuencia;
    public float FrecuenciaMuestreo = 44100.0f;
    public float TiempoSegundos = 2.0f;

    int TimeIndex = 0;

    AudioSource fuente;


    // Start is called before the first frame update
    void Start()
    {
        fuente = gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.spatialBlend = 0;
        fuente.Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!fuente.isPlaying)
            {
                TimeIndex = 0;
                fuente.Play();
                selector = 0;
            }
            else
            {
                fuente.Stop();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!fuente.isPlaying)
            {
                TimeIndex = 0;
                fuente.Play();
                selector = 1;
            }
            else
            {
                fuente.Stop();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!fuente.isPlaying)
            {
                TimeIndex = 0;
                fuente.Play();
                selector = 2;
            }
            else
            {
                fuente.Stop();
            }
        }
    }

    int selector = 0;
    void OnAudioFilterRead(float[] data, int channels)
    {
        if(selector == 0)
        { 
            for (int i=0; i<data.Length; i += channels)
            {
                data[i] = CreateSeno(TimeIndex, Frecuencia, FrecuenciaMuestreo);
            
                if (channels == 2)
                data[i+1] = CreateSeno(TimeIndex, Frecuencia, FrecuenciaMuestreo);
                TimeIndex++;

                if (TimeIndex >= (FrecuenciaMuestreo*TiempoSegundos))
                TimeIndex = 0;
            }
        }
        else if (selector == 1)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
               data[i] = CreateSquare(TimeIndex, Frecuencia, FrecuenciaMuestreo);

               if (channels == 2)
                   data[i+1] = CreateSquare(TimeIndex, Frecuencia, FrecuenciaMuestreo);
               TimeIndex++;

               if (TimeIndex >= (FrecuenciaMuestreo * TiempoSegundos))
                   TimeIndex = 0;
            }
        }
        else if (selector == 2)
        {
            int Tm = (int)(Frecuencia / FrecuenciaMuestreo);
            for (int i = 0; i < data.Length; i += channels)
            {
                data[i] = CreateTriangle(TimeIndex, Frecuencia, FrecuenciaMuestreo, Tm);

                if (channels == 2)
                    data[i+1] = CreateTriangle(TimeIndex, Frecuencia, FrecuenciaMuestreo, Tm);
                TimeIndex++;

                if (TimeIndex >= Tm)
                    TimeIndex = 0;
            }
        }
    }

    public float CreateSeno(int TimeIndex, float Frecuencia, float FrecuenciaMuestreo)
    {
        return Mathf.Sin(2*Mathf.PI*Frecuencia*TimeIndex/FrecuenciaMuestreo);
    }
    public float CreateSquare(int TimeIndex, float Frecuencia, float FrecuenciaMuestreo)
    {
        if(Mathf.Sin(2*Mathf.PI*Frecuencia*TimeIndex/FrecuenciaMuestreo) >0)
            return 1;
        else if(Mathf.Sin(2 * Mathf.PI * Frecuencia * TimeIndex / FrecuenciaMuestreo) <0)
            return -1;
        else
            return 0;
    }
    public float CreateTriangle(int TimeIndex, float Frecuencia, float FrecuenciaMuestreo, int Tm)
    {
        //Para hallar la pendiente de la 1ra recta:
        float m1 = 1 / ((Tm / 4.0f));
        //Para hallar la peniente de la 2da recta:
        float m2 = -2 / ((Tm * (3 / 4.0f)) - ((Tm / 4.0f)));
        //Para hallar la pendiente de la 3ra recta:
        float m3 = 1 / (Tm-((Tm*3)/4.0f));
        
        //Para hallar el intercepto de la 1ra recta:
        float b1 = 1 - (m1 * (Tm / 4));
        //Para hallar el intercepto de la 2da recta:
        float b2 = 1 - (m2 * (Tm / 4));
        //Para hallar el intercepto de la 3ra recta:
        float b3 = 0 - (m2 * Tm);

        if (TimeIndex < (Tm / 4)) return (m1 * TimeIndex + b1);
        else if (TimeIndex > (Tm / 4) && TimeIndex <= ((Tm * 3) / 4)) return (m2 * TimeIndex + b2);
        else return (m3*TimeIndex+b3);
    }
}
