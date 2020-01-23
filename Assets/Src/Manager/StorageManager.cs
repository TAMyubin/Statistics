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
public class Storage
{
       public long money;

    public string lastOnlineTime; //最后登录时间
    public string lastUpdateAwardTime; //上次刷新奖励时间
    public int activeDay;//活跃天数
}

public class StorageManager
{
    string filepath = null;
    private static StorageManager _instance;
    public static StorageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StorageManager();
            }
            return _instance;
        }
    }

    private Storage _storage;
    //    private int _version = 0;
    private bool _isAddDay = false;

    public Storage storage { get { return _storage; } }

    private StorageManager()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass jClass = new AndroidJavaClass("com.mogame.sdk.LWSDKUtils");
        filepath = jClass.CallStatic<string>("GetFilesPath", activity) + "/data";
#else
        filepath = Application.persistentDataPath + "/data";
#endif
    }

    public event Action OnMoneyChange;

    public Task<bool> SyncData()
    {
        _setLastOnlineTime();
        return Task<bool>.Factory.StartNew(() =>
        {
            try
            {
                if (_storage == null) return false;


                // _storage.signInDay = SignInManager.Instance.signInDay;

                JObject jObject = JObject.FromObject(_storage);
                Save(jObject);
                Debug.Log("Storage Save!");
                return true;
            }
            catch (Exception err)
            {
                Debug.LogException(err);
                return false;
            }
        });
    }

    public void InitData(JObject data, int version)
    {
        if (data != null)
        {
            Storage storage = data.ToObject<Storage>();
            _storage = storage;
    

            // if(_storage.heroWeaponDic==null){
            //     _storage.heroWeaponDic = new Dictionary<int, List<int>>();

            // }
        }
        else
        {
            _storage = new Storage();
            _storage.money = 50;
         
        
       

         
            _storage.lastOnlineTime = DateTime.Now.ToString();
        
            _storage.activeDay = 1;
            _storage.lastUpdateAwardTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).ToString();

           



        }
        _setActiveDay();
    }

  

    public void Save(JObject data)
    {
        FileStream sw;
        FileInfo t = new FileInfo(filepath);

        sw = t.OpenWrite();
        sw.SetLength(0);
        byte[] buffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(data));
        sw.Write(buffer, 0, buffer.Length);
        sw.Close();
        sw.Dispose();
    }

    public string LoadFile()
    {
        FileStream fsread = null;
        string data = "";
        try
        {
            fsread = File.OpenRead(filepath);
            Debug.Log("filepath" + filepath);
        }
        catch (Exception e)
        {
            Debug.Log("没存档文件: " + e);
            return "";
        }
        byte[] buffer = new byte[1024 * 1024 * 2];
        int r = fsread.Read(buffer, 0, buffer.Length);
        data = Encoding.Default.GetString(buffer, 0, r);
        fsread.Close();
        fsread.Dispose();
        return data;
    }

    /// <summary>
    /// 设定最后登录时间
    /// </summary>
    private void _setLastOnlineTime()   
    {
        _setActiveDay();

        DateTime dt = DateTime.Now;
        DateTime lastTime = new DateTime();
        if (_storage == null || _storage.lastOnlineTime == null)
        {
            return;
        }
        DateTime.TryParse(_storage.lastOnlineTime, out lastTime);
        _storage.lastOnlineTime = DateTime.Compare(dt, lastTime) > 0 ? dt.ToString() : _storage.lastOnlineTime;
    }

    /// <summary>
    /// 设置活跃天数
    /// </summary>
    private void _setActiveDay()
    {
        if (_storage == null || _storage.lastOnlineTime == null || _isAddDay)
        {
            return;
        }
        DateTime dt = DateTime.Now;
        DateTime lastTime = new DateTime();

        DateTime.TryParse(_storage.lastOnlineTime, out lastTime);

        int dtDay = dt.DayOfYear;
        int lastDay = lastTime.DayOfYear;

        if (dtDay - lastDay >= 1 && !_isAddDay)
        {
            _isAddDay = true;
            _storage.activeDay++;
        }
    }

    /// <summary>
    /// 获得离线时间
    /// </summary>
    public int GetDaysDiffer()
    {
        if (_storage == null || _storage.lastOnlineTime == null)
        {
            return 0;
        }
        DateTime dt;
        DateTime.TryParse(_storage.lastOnlineTime, out dt);
        DateTime currentTime = DateTime.Now;      // 获取当前时间
        TimeSpan ts = currentTime.Subtract(dt);   // 与当前日期的时间之差
        int seconds = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
        Debug.Log("离线了：" + seconds + "秒");
        return seconds;
    }
}
