using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {

        SceneManager.LoadSceneAsync ("Main").completed += (AsyncOperation) => {
            string fileData = StorageManager.Instance.LoadFile ();
            if (fileData != "") {
                JObject data = JsonConvert.DeserializeObject<JObject> (fileData);
                StorageManager.Instance.InitData (data, 0);
            } else {
                StorageManager.Instance.InitData (null, 0);
            }
            StatisticsManager.instance.init();
        };

    }

    // Update is called once per frame
    void Update () {

    }
}