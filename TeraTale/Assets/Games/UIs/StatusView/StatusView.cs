﻿using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour
{
    public Player target { private get; set; }
    [SerializeField]
    Image _hpBar = null;
    [SerializeField]
    Text _hpText = null;
    [SerializeField]
    Image _staminaBar = null;
    [SerializeField]
    Text _staminaText = null;
    [SerializeField]
    Text _levelText = null;
    [SerializeField]
    Text _nameText = null;
    [SerializeField]
    Text _moneyText = null;
    [SerializeField]
    Transform _buffsGroup = null;
    [SerializeField]
    Image _expBar = null;
    [SerializeField]
    Text _expText = null;

    void Update()
    {
        if (target == null)
            return;
        //if (target.GetType().IsSubclassOf(typeof(Player)));//따로처리
        _hpBar.fillAmount = target.hp / target.hpMax;
        _hpText.text = target.hp + "/" + target.hpMax + "(" + _hpBar.fillAmount * 100 + "%)";
        _staminaBar.fillAmount = target.stamina / target.staminaMax;
        _staminaText.text = target.stamina + "/" + target.staminaMax + "(" + _staminaBar.fillAmount * 100 + "%)";
        _levelText.text = target.level.ToString();
        _nameText.text = target.name;
        _expBar.fillAmount = target.exp / target.expMax;
        _expText.text = string.Format("{0:0.##}% ({1}/{2})", target.exp / target.expMax * 100, target.exp, target.expMax);
        _moneyText.text = target.money + "G";
        //_buffsGroup.AddChildren(_player.GetAllBuffs());
    }
}