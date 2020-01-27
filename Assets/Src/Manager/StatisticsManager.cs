using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct kind {
    public DateTime time;//时间
    public float rental; //总额
    public float IAE; //收支
    public float residue; //剩余
    public string remark; //备注
}
public class StatisticsManager {

    private static StatisticsManager _instance;
    public static StatisticsManager instance {
        get {
            if (_instance == null) {
                _instance = new StatisticsManager ();
            }
            return _instance;
        }
    }
    private float _money;
    public float money {
        get { return _money; }
        set { _money = value; }
    }
    private Dictionary<int, List<kind>> _dataDic;
    public Dictionary<int, List<kind>> dataDic {
        get { return _dataDic; }
    }
    public void AddDataDic (int key, kind data) {
        if (!_dataDic.ContainsKey (key)) {
            List<kind> list = new List<kind> ();
            list.Add (data);
            _dataDic.Add (key, list);
        } else {

            List<kind> list = _dataDic[key];
            list.Add (data);
        }
    }

    public void init () {
        _money = StorageManager.Instance.storage.money;
        _dataDic = StorageManager.Instance.storage.dataDic;
    }
}