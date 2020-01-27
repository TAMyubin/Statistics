using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DetailController : MonoBehaviour {
    public Text redText;
    public Text greenText;
    public Text blueText;
    public Text ResidueText;
    public Text remarkText;
    public Text InputremarkText;

    private int _selfIndex;
    public int selfindex {
        get { return _selfIndex; }
        set { _selfIndex = value; }
    }
    private ListController _controller;
    public ListController controller {
        get { return _controller; }
    }
    private PrefabController _child;

    public void init (ListController controller, int index) {
        _selfIndex = index;
        _controller = controller;

    }
    // Start is called before the first frame update
    void Start () {
        if (_controller == null) return;
        if (selfindex >= StatisticsManager.instance.dataDic.Count) {
            redText.text = DateTime.Now.ToString ();
            // OKBtn.gameObject.SetActive (true);
            InputremarkText.transform.parent.gameObject.SetActive (true);
            _child = greenText.gameObject.GetComponentInParent<PrefabController> ();
            _child.init (this);

        } else {
            List<kind> list = StatisticsManager.instance.dataDic[selfindex];
            redText.text = list[list.Count - 1].time.ToString ();
            greenText.text = list[list.Count - 1].IAE.ToString ();
            blueText.text = list[list.Count - 1].residue.ToString ();
            remarkText.text = list[list.Count - 1].remark;
        }

    }

    // Update is called once per frame
    void Update () {

    }

}