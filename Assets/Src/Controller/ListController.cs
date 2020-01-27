using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour {
    public GameObject DetailPrefab;
    public Transform CreatePos;
    public Text money;

    public int _nowmoney;
    public int _temp = 0;
    public Button SaveBtn;
    private DetailController child;

    private List<GameObject> list = new List<GameObject> ();
    // Start is called before the first frame update
    void Start () {
        SaveBtn.onClick.AddListener (enter);
        init ();

    }
    public void init () {
        money.text = "总额：" + StatisticsManager.instance.money.ToString ();
        for (int i = 0; i < StatisticsManager.instance.dataDic.Keys.Count + 1; i++) {

            GameObject obj = ObjectPoolManager.Instance.GetPrefabObject (DetailPrefab, CreatePos.transform);

            child = obj.GetComponent<DetailController> ();
            child.init (this, i);
            list.Add (obj);
        }
    }
    public void RecoverEmptyBox () {
        for (int i = 0; i < list.Count; i++) {
            int prefabID = list[i].GetInstanceID ();
            ObjectPoolManager.Instance.RecoverPrefabObject (prefabID, list[i]);
        }
        list.Clear();
    }
    public void enter () {
        if (child.greenText.text == null || child.greenText.text == "" || child.greenText.text == "收支") {
            Debug.Log ("未输入收支金额");
            return;
        }
        StatisticsManager.instance.money = _nowmoney;

        kind data = new kind ();
        data.IAE = _temp;
        data.residue = int.Parse (child.ResidueText.text);
        data.rental = _nowmoney;
        data.time = DateTime.Now;
        data.remark = child.InputremarkText.text;
        StatisticsManager.instance.AddDataDic (child.selfindex, data);
        StorageManager.Instance.SyncData ();
        RecoverEmptyBox ();
        init ();
    }
}