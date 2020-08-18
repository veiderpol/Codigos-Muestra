using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ShuffleEffects : MonoBehaviour
{
    public GameObject[] effectsPlayer1;
    public GameObject[] shuffleEffects;
    public GameObject[] effects;
    public List<int> list;
    public List<string> stringEffects;

    void Start()
    {
        shuffleEffects = new GameObject[20];
    }
    public string CutString(string input)
    {
        string[] retVal = input.Split((' '));
        return retVal[0];
    }
    public bool ReturnIzq(string input)
    {
        string[] retVal = input.Split(' ');
        if (retVal[1] != "")
        {
            if (retVal[1] == "Izq")
            {
                return true;
            }
            else
                return false;
        }else return false;
        
    }
    public bool ReturnDer(string input)
    {
        string[] retVal = input.Split(' ');
        if (retVal[1] != "")
        {

            if (retVal[1] == "Der")
            {
                return true;
            }
            else
                return false;
        }
        else return false;
    }

    public GameObject[] ShuffleEffect1()
    {
        List<int> possible = new List<int> {0,1,2,3,4,5,6,7,8,9,11,13,15,17,19,21,23,25,27,29,35,39,43,47,48,
                                            49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,
                                            71,72,73,74,76,78};
        list = new List<int>();
        stringEffects = new List<string>();
        for (int i = 0; i < 20;)
        {
            if (possible.Count == 0)
            {
                ShuffleEffect1();
                break;
            }
            int index = Random.Range(0, possible.Count);
            
            int rand = possible[index];

            string temp = CutString(effects[rand].name);
            if (i <= 13 && ((temp == "Rayos") && (ReturnIzq(effects[rand].name))))
            {
                int aux = i;
                for (int j = rand; j < (rand + 6); j++)
                {
                    shuffleEffects[aux] = effects[j];
                    aux++;
                }
                possible.RemoveAt(index);

                i += 6;
                stringEffects.Add(temp);
            }
            else if (i <= 15 && (temp == "Aro") && ReturnIzq(effects[rand].name) && !stringEffects.Contains(temp))
            {
                int aux = i;
                for (int j = rand; j < (rand + 4); j++)
                {
                    shuffleEffects[aux] = effects[j];
                    aux++;
                }
                possible.RemoveAt(index);

                i += 4;
                stringEffects.Add(temp);

            }
            else if (i <= 17 && (ReturnIzq(effects[rand].name) && !stringEffects.Contains(temp) && ((temp != "Aro") && (temp != "Rayos"))))
            {
                shuffleEffects[i] = effects[rand];
                shuffleEffects[i + 1] = effects[rand + 1];
                possible.RemoveAt(index);

                i += 2;
                stringEffects.Add(temp);
            }
            else if (!ReturnDer(effects[rand].name) && !ReturnIzq(effects[rand].name) && !stringEffects.Contains(temp))
            {
                shuffleEffects[i] = effects[rand];
                possible.RemoveAt(index);
                i++;
                stringEffects.Add(temp);

            }
            else{
                possible.RemoveAt(index);
                continue;
            }
        }
        return shuffleEffects;
    }
}