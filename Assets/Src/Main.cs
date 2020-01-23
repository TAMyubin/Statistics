using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class Main : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        string fileData = StorageManager.Instance.LoadFile ();
        if (fileData != "") {
            JObject data = JsonConvert.DeserializeObject<JObject> (fileData);
            StorageManager.Instance.InitData (data, 0);
        } else {
            StorageManager.Instance.InitData (null, 0);
        }

    }

    // Update is called once per frame
    void Update () {

    }
}