using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using EZCameraShake;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VFXScript : MonoBehaviour
{

    public VisualEffect VFX;

    //public Material Monol;
    Renderer rend;
    float increase = 0f;

    public bool Activation = false;
    //bool deactivate = false;

    public Volume postPros;
    float ChromIncrease = 0f;

    public Animator camanim;

    void Start()
    {
        VFX.GetComponent<VisualEffect>();

        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_lerpy", increase);
        /*Monol = GetComponent<MeshRenderer>().sharedMaterial;
        Monol.SetFloat("lerpy", 5);
        Debug.Log(Monol);*/
    }

    void Update()
    {
        if (Activation == true)
        {
            StartCoroutine(MonoSeq());
            Activation = false;
        }
        /*if (Activation == true)
        {
            increase = increase+0.01f;
        }

        if (increase >= 3.0f && Activation == true)
        {
            VFX.SendEvent("EventP");
            //Monol.SetFloat("lerpy", increase);
            Activation = false;
            deactivate = true;
        }
        
        if (deactivate == true && increase >= 0f)
        {
            increase = increase - 0.01f;
        }*/

        //rend.material.SetFloat("_lerpy", increase);
    }

    IEnumerator MonoSeq()
    {
        bool done = false;
        Debug.Log("phase0");
        camanim.SetBool("animtrig", true);
        yield return new WaitForSeconds(3.5f);
        Debug.Log("phase1");
        VFX.SendEvent("PhaseOne");
        while (increase <= 3.0f)
        {
            if (increase >= 1.5f && !done)
            {
                done = true;
                CameraShaker.Instance.ShakeOnce(2f, 4f, 0.2f, 1f);
                Debug.Log("bruh");
            }
            rend.material.SetFloat("_lerpy", increase);
            increase += (0.5f * Time.deltaTime);

            VolumeProfile posprofile = postPros.profile;
            if (!posprofile.TryGet<ChromaticAberration>(out var Chrom))
            {
                Chrom = posprofile.Add<ChromaticAberration>(true);
            }
            Chrom.intensity.value = ChromIncrease;
            ChromIncrease += ((0.5f + Random.Range(-0.4f, 0.4f)) * Time.deltaTime);

            yield return null;
        }
        yield return new WaitForSeconds(1);
        Debug.Log("phase2");
        VFX.SendEvent("PhaseTwo");
        yield return new WaitForSeconds(0.5f);
        CameraShaker.Instance.ShakeOnce(2f, 4f, 1f, 2f);
        yield return new WaitForSeconds(4.5f);
        Debug.Log("phase3");
        while (increase >= 0.0f)
        {
            rend.material.SetFloat("_lerpy", increase);
            increase -= (0.5f * Time.deltaTime);
            //Debug.Log(increase);

            VolumeProfile posprofile = postPros.profile;
            if (!posprofile.TryGet<ChromaticAberration>(out var Chrom))
            {
                Chrom = posprofile.Add<ChromaticAberration>(true);
            }
            Chrom.intensity.value = ChromIncrease;
            ChromIncrease -= ((0.5f + Random.Range(-0.4f, 0.4f)) * Time.deltaTime);

            yield return null;
        }
        Debug.Log("Done");
    }
}
