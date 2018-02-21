using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame
{
    public class UIFighterState : MonoBehaviour
    {
        public FighterController fighter;
        public Image UnderBar;
        public Image LifeBar;
        public Vector3 lifeStartPosition;
        public RectTransform lifeMask;
        public Vector3 lifeMaskStart;
        public Image ComboBar;
        Vector3 comboStartPosition;
        public RectTransform comboMask;
        Vector3 comboMaskStart;
        public Image GuardBar;
        public Image StunBar;
        public Image ChargeBar;
        public UIVictoryManager victoryBoxes;



        // Use this for initialization
        void Start()
        {
            lifeMaskStart = lifeMask.transform.position;
            lifeStartPosition = LifeBar.transform.position;
            comboStartPosition = ComboBar.transform.position;
            //UnderBar.material = fighter.GetComponent<SpriteRenderer>().material;

            victoryBoxes.SetFighter(fighter);
        }

        // Update is called once per frame
        void Update()
        {
            lifeMask.anchoredPosition = new Vector3((1f-fighter.Life) * LifeBar.rectTransform.rect.width, 0, 0);
            comboMask.anchoredPosition = new Vector3(((1f - fighter.Life) + fighter.ComboStrength) * LifeBar.rectTransform.rect.width, 0, 0);
            LifeBar.rectTransform.position = lifeStartPosition;
            ComboBar.rectTransform.position = comboStartPosition;
            //LifeBar.fillAmount = fighter.Life;
            //LifeBar.color = Color.HSVToRGB((fighter.Life * 128) / 360f, 1f, 1f);
            //ComboBar.fillAmount = fighter.ComboStrength;
            //ComboBar.rectTransform.anchoredPosition = new Vector3(LifeBar.rectTransform.rect.width * (1f - fighter.Life), 0, 0);

            GuardBar.fillAmount = fighter.Guard;
            StunBar.fillAmount = fighter.Stun;
            //ChargeBar.fillAmount = fighter.TeabagCharge;
        }


    }
}