using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PrefabController : MonoBehaviour, IPointerDownHandler {

    public GameObject showText;
    public GameObject InputText;
    public Text inputText;
   
    private bool _input = false;

    private DetailController _controller;
  
    public void init (DetailController controller) {
        _controller = controller;
        _controller.controller._nowmoney = StatisticsManager.instance.money;

    }
    public void OnPointerDown (PointerEventData pointerEventData) {

        if (!_controller) return;
        _controller.controller._nowmoney = StatisticsManager.instance.money;
        _controller.ResidueText.text = _controller.controller._nowmoney.ToString ();
        _input = true;
        InputText.SetActive (_input);
    }

    public void change () {

        if (inputText.text == "" || inputText.text == null) {
            _controller.controller._temp = 0;
        } else {
            _controller.controller._temp = int.Parse (inputText.text);
        }
        _controller.ResidueText.text = (_controller.controller._nowmoney - _controller.controller._temp).ToString ();

    }
    public void end () {

        if (inputText.text == "" || inputText.text == null) {
            _controller.controller._temp = 0;
        } else {
            _controller.controller._temp = int.Parse (inputText.text);
        }
        _controller.controller._nowmoney = _controller.controller._nowmoney - _controller.controller._temp;
        _controller.ResidueText.text = _controller.controller._nowmoney.ToString ();
        _input = false;
        InputText.SetActive (_input);
        showText.GetComponent<Text> ().text = inputText.text;

    }
  
}