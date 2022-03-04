using System.Collections.Generic;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Access
{
    public class SoundAccessAutocomplete
    {
        private SoundAccess _soundAccess;

        public string GetAutoComplete(string key)
        {
            if (_soundAccess == null)
            {
                _soundAccess = GameObject.FindObjectOfType<SoundAccess>();
            }

            if (key == null || key.Length == 0)
            {
                key = "/";
            }

            List<string> possibilites = new List<string>();
            int          slashesYet   = key.Split('/').Length - 1;

            if (!key.StartsWith("/"))
            {
                return "";
            }


            string    pathYet  = "/";
            string[]  keySplit = key.Split('/');
            Transform curPar   = _soundAccess.transform;
            for (int i = 1; i < keySplit.Length; i++)
            {
                List<Transform> possChildren = new List<Transform>();

                for (int j = 0; j < curPar.childCount; j++)
                {
                    if (curPar.GetChild(j).name.StartsWith(keySplit[i]))
                    {
                        possChildren.Add(curPar.GetChild(j));
                    }
                }

                if (possChildren.Count >= 2)
                {
                    string result = "";
                    for (int j = 0; j < possChildren.Count; j++)
                    {
                        result += pathYet + possChildren[j].name + "\n";
                    }

                    return result;
                }
                else if (possChildren.Count == 1)
                {
                    if (i + 1 == keySplit.Length)
                    {
                        return pathYet + possChildren[0].name;
                    }
                    else
                    {
                        curPar  =  possChildren[0];
                        pathYet += possChildren[0].name + "/";
                    }
                }
                else
                {
                    return "";
                }
            }

            return "Error parsing";
        }
    }
}