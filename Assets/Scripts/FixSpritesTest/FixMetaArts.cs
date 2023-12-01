using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.UIElements;

public class FixMetaArts : MonoBehaviour
{

    public int posXToAdd = 0;
    public int posYToAdd = 0;
    public int HeightToAdd = 0;
    public int WidthToAdd = 0;

    public string path;
    private string[] metaDataToModify;


    public void isWhatIWant(string key,int index, int thingToAdd)
    {
        if (metaDataToModify[index].Contains(key) && metaDataToModify[index][metaDataToModify[index].ToList().IndexOf(key[0]) - 2] == ' ' && metaDataToModify[index][metaDataToModify[index].ToList().IndexOf(key[0]) - 1] == ' ')
        {
            string resultString = Regex.Match(metaDataToModify[index], @"\d+").Value;
            metaDataToModify[index] = new String(metaDataToModify[index].Where(c => !Char.IsNumber(c)).ToArray());
            metaDataToModify[index] += Int32.Parse(resultString) + thingToAdd;


        }
    }

    [Button]
    public void FixMeta()
    {
        metaDataToModify = System.IO.File.ReadAllLines(path);
        for (int i = 0; i < metaDataToModify.Length; i++)
        {

            isWhatIWant("x: ", i, posXToAdd);
            isWhatIWant("y: ", i, posYToAdd);
            isWhatIWant("width: ", i , WidthToAdd);
            isWhatIWant("height: ", i, HeightToAdd);
        }   
        System.IO.File.WriteAllLines(path, metaDataToModify);
    }
}
