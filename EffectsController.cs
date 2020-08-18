using System.Collections;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Avateering;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class EffectsController : MonoBehaviour
{

    //Jugador1
    public GameObject[] effectsStickman_1_1;
    Collider boxCollider;
    int tiempoDeEspera = 10;
    public ShuffleEffects shuffle;
    bool salto = false;
    GameObject saltoObj;
    public JumpFBX jugador1FBX;

    private void OnEnable()
    {
        
        Reset();
        StartCoroutine(Stickmen1());
        instruccionUI.SetActive(false);
        boxCollider = GetComponentInChildren<BoxCollider>();
        boxCollider.enabled = true;
       
    }
    private void OnDisable()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
        boxCollider.enabled = false;
        saltoObj = null;
        StopCoroutine(Stickmen1());
    }
    public string CutString(string input)
    {
        string[] retVal = input.Split((' '));
        return retVal[0];
    }
    public bool ReturnIzq(string input)
    {
        string[] retVal = input.Split(' ');
        if (retVal[1] == "Izq")
        {
            return true;
        }
        else
            return false;
    }
    public bool ReturnDer(string input)
    {
        string[] retVal = input.Split(' ');
        if (retVal[1] == "Der")
        {
            return true;
        }
        else
            return false;
    }
    private void Update()
    {
        if (salto)
        {
            if (jugador1FBX.JumpHeight > 0)
            {
                Salto();
            }
       }
    }
    public IEnumerator Stickmen1()
    {
        effectsStickman_1_1 = shuffle.ShuffleEffect1();

        for (int i = 0; i < effectsStickman_1_1.Length;)
        {
            string temp = CutString(effectsStickman_1_1[i].name);
            if (temp == "Salto")
            {
                salto = true;
                effectsStickman_1_1[i].gameObject.SetActive(true);
                saltoObj = effectsStickman_1_1[i].gameObject;
                saltoObj.transform.GetChild(0).GetComponent<Animator>().SetBool("Salto", false);
       
                yield return new WaitForSeconds(5);
                saltoObj.transform.GetChild(1).gameObject.SetActive(true);
                effectsStickman_1_1[i].gameObject.SetActive(false);
                
                salto = false;
                i++;
            }else if (ReturnIzq(effectsStickman_1_1[i].name) && (temp != "Aro" && temp != "Rayos"))
            {
                effectsStickman_1_1[i].SetActive(true);
                effectsStickman_1_1[i + 1].SetActive(true);
                Animator anim = effectsStickman_1_1[i].gameObject.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    float tiempo = GetLenghtOfClip(effectsStickman_1_1[i].name, anim);
                    yield return new WaitForSeconds(tiempo);
                }
                else
                {
                    yield return new WaitForSeconds(tiempoDeEspera);
                }

                effectsStickman_1_1[i].SetActive(false);
                effectsStickman_1_1[i + 1].SetActive(false);
                i += 2;
            }else if (ReturnIzq(effectsStickman_1_1[i].name) && temp == "Aro")
            {
                for (int j = i; j < (i + 4); j++)
                {
                    effectsStickman_1_1[j].SetActive(true);
                }
                yield return new WaitForSeconds(tiempoDeEspera);
                for (int k = i; k < (i + 4); k++)
                {
                    effectsStickman_1_1[k].SetActive(false);
                }
                i += 4;
            } else if (ReturnIzq(effectsStickman_1_1[i].name) && temp == "Rayos")
            {
                for (int j = i; j < (i + 6); j++)
                {
                    effectsStickman_1_1[j].SetActive(true);
                }
                yield return new WaitForSeconds(tiempoDeEspera);
                for (int k = i; k < (i + 6); k++)
                {
                    effectsStickman_1_1[k].SetActive(false);
                }
                i += 6;
            }
            else
            {
                effectsStickman_1_1[i].SetActive(true);

                Animator anim = effectsStickman_1_1[i].gameObject.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    float tiempo = GetLenghtOfClip(effectsStickman_1_1[i].name, anim);
                    yield return new WaitForSeconds(tiempo);
                }
                else
                {
                    yield return new WaitForSeconds(tiempoDeEspera);
                }
                effectsStickman_1_1[i].SetActive(false);
                i++;
            }
        }
        StartCoroutine(Stickmen1());
        StopCoroutine(Stickmen1());
    }
    public void Salto()
    {
        StartCoroutine(SaltoCorutine());
    }
    IEnumerator SaltoCorutine() 
    {
        saltoObj.transform.GetChild(0).GetComponent<Animator>().SetBool("Salto", false);
        saltoObj.transform.GetChild(0).GetComponent<Animator>().SetBool("Salto", true);
        
        saltoObj.transform.GetChild(1).gameObject.SetActive(false);

        string tempName = saltoObj.transform.GetChild(0).name;
        Animator animPlayer = saltoObj.transform.GetChild(0).GetComponent<Animator>();
        float tiempo = GetLenghtOfClip(tempName, animPlayer);
        yield return new WaitForSeconds(tiempo);
        
        saltoObj.transform.GetChild(1).gameObject.SetActive(true);
        saltoObj.gameObject.SetActive(false);
        salto = false;
    }
    public void Reset()
    {
        if (effectsStickman_1_1!=null)
        {
            for (int i = 0; i < effectsStickman_1_1.Length; i++)
            {
                effectsStickman_1_1[i].SetActive(false);
            }
        }
        effectsStickman_1_1 = null;
    }
    public float GetLenghtOfClip(string clipName, Animator anim)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        if (anim != null)
        {
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
        }
        return 0;
    }
}
