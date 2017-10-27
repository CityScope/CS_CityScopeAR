using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine.UI;

public class RNCSimpleJsonParser : MonoBehaviour
{
    [Header("RNC data")]
    public TextAsset _jsonFile;
    private JSONNode _parsedJson;
    public Dictionary<int, GameObject> _dictNameObj = new Dictionary<int, GameObject>();
    [Header("Visulizing")]
    [Range(0.03f, 1f)]
    public float _objScale = 0.03f;
    public Material _baseMaterial;

    void Start()
    {
        _parsedJson = JSON.Parse(_jsonFile.text);
        RncAgents();
    }
    void RncAgents()
    {
        for (int _hr = 0; _hr < 23; _hr++)
        {
            for (int i = 0; i < _parsedJson["dates"]["2016-08-20"]["hours"][_hr]["C"]["personId"].Count; i++)
            {
                float _lat = _parsedJson["dates"]["2016-08-20"]["hours"][_hr]["C"]["lat"][i];
                float _lon = _parsedJson["dates"]["2016-08-20"]["hours"][_hr]["C"]["lon"][i];
                int _pid = _parsedJson["dates"]["2016-08-20"]["hours"][_hr]["C"]["personId"][i];

                var _newlat = (_lat - 42.5f) * 1000;
                var _newlon = (_lon - 1.5f) * 1000;

                if (_dictNameObj == null || _dictNameObj.ContainsKey(_pid) != true) // new object 
                {
                    var _newClObj = GameObject.CreatePrimitive(PrimitiveType.Cube); //make obj
                    _newClObj.transform.parent = transform;
                    _newClObj.transform.GetComponent<Renderer>().material = _baseMaterial;
                    _newClObj.transform.GetComponent<Renderer>().material.color = Color.gray; // Color.HSVToRGB(_tmpColor, 1, 1);
                    _newClObj.transform.localScale = new Vector3(_objScale, _objScale, _objScale);
                    _newClObj.transform.position = new Vector3(_newlat, _hr, _newlon); //compensate for scale shift due to height  
                    _newClObj.name = "NEW: " + _lat.ToString() + " " + _lon.ToString() + " " + _parsedJson["dates"]["2016-08-20"]["hours"][_hr]["C"]["personId"][i].ToString();
                    _dictNameObj.Add(_parsedJson["dates"]["2016-08-20"]["hours"][_hr]["C"]["personId"][i], _newClObj);
                }
                else if ((_dictNameObj != null && _dictNameObj.ContainsKey(_pid))) //obj already exist 
                {
                    var _exClObj = GameObject.CreatePrimitive(PrimitiveType.Cube); //make obj again 
                    _exClObj.transform.parent = transform;
                    float _tmpCol = (1.0f * _hr) / 24;
                    _exClObj.transform.GetComponent<Renderer>().material = _baseMaterial;
                    _exClObj.transform.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, _tmpCol);
                    _exClObj.transform.localScale = new Vector3(2 * _objScale, 2 * _objScale, 2 * _objScale);
                    _exClObj.transform.localPosition = new Vector3(_newlat, _hr, _newlon);
                    _exClObj.name = "EX: " + _lat.ToString() + " " + _lon.ToString() + " " + _parsedJson["dates"]["2016-08-20"]["hours"][_hr]["C"]["personId"][i].ToString();
                }
            }
        }
    }
}