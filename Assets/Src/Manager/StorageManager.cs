using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
public class Storage {
    public float money;
    public Dictionary<int, List<kind>> dataDic;
}

public class StorageManager {
    string filepath = null;
    private static StorageManager _instance;
    public static StorageManager Instance {
        get {
            if (_instance == null) {
                _instance = new StorageManager ();
            }
            return _instance;
        }
    }

    private Storage _storage;
    //    private int _version = 0;
    private bool _isAddDay = false;

    public Storage storage { get { return _storage; } }

    private StorageManager () {
// #if UNITY_ANDROID && !UNITY_EDITOR
//         AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
//         AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject> ("currentActivity");
 
//         filepath = jClass.CallStatic<string> ("GetFilesPath", activity) + "/data";
// #else
        filepath = Application.persistentDataPath + "/data";
// #endif
    }

    public Task<bool> SyncData () {
        
        return Task<bool>.Factory.StartNew (() => {
            try {
                if (_storage == null) return false;

                // _storage.signInDay = SignInManager.Instance.signInDay;
                _storage.money = StatisticsManager.instance.money;
                _storage.dataDic = StatisticsManager.instance.dataDic;
                JObject jObject = JObject.FromObject (_storage);
                Save (jObject);
                Debug.Log ("Storage Save!");
                return true;
            } catch (Exception err) {
                Debug.LogException (err);
                return false;
            }
        });
    }

    public void InitData (JObject data, int version) {
        if (data != null) {
            Storage storage = data.ToObject<Storage> ();
            _storage = storage;

            // if(_storage.heroWeaponDic==null){
            //     _storage.heroWeaponDic = new Dictionary<int, List<int>>();

            // }
        } else {
            _storage = new Storage ();
            _storage.money = 0;
            _storage.dataDic = new Dictionary<int, List<kind>> ();

    
        }

    }

    public void Save (JObject data) {
        FileStream sw;
        FileInfo t = new FileInfo (filepath);

        sw = t.OpenWrite ();
        sw.SetLength (0);
        byte[] buffer = Encoding.Default.GetBytes (JsonConvert.SerializeObject (data));
        sw.Write (buffer, 0, buffer.Length);
        sw.Close ();
        sw.Dispose ();
    }

    public string LoadFile () {
        FileStream fsread = null;
        string data = "";
        try {
            fsread = File.OpenRead (filepath);
            Debug.Log ("filepath" + filepath);
        } catch (Exception e) {
            Debug.Log ("没存档文件: " + e);
            return "";
        }
        byte[] buffer = new byte[1024 * 1024 * 2];
        int r = fsread.Read (buffer, 0, buffer.Length);
        data = Encoding.Default.GetString (buffer, 0, r);
        fsread.Close ();
        fsread.Dispose ();
        return data;
    }

}