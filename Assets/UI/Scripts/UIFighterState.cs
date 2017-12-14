using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame
{
    public class UIFighterState : MonoBehaviour
    {
        public FighterController fighter;
        public Image LifeBar;
        public Image ComboBar;
        public Image GuardBar;
        public Image StunBar;
        public UIVictoryManager victoryBoxes;

        // Use this for initialization
        void Start()
        {
            victoryBoxes.SetFighter(fighter);
        }

        // Update is called once per frame
        void Update()
        {
            LifeBar.fillAmount = fighter.Life;
            LifeBar.color = Color.HSVToRGB((fighter.Life * 128) / 360f, 1f, 1f);
            ComboBar.fillAmount = fighter.ComboStrength;
            ComboBar.rectTransform.anchoredPosition = new Vector3(LifeBar.rectTransform.rect.width * (1f - fighter.Life), 0, 0);

            GuardBar.fillAmount = fighter.Guard;
            StunBar.fillAmount = fighter.Stun;
        }
    }
}