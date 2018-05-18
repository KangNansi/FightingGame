using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame
{
    public class LZUIFighterState : MonoBehaviour
    {
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
        public LZUIVictoryBoxContainer victoryBoxContainer;



        // Use this for initialization
        void Start()
        {
            lifeMaskStart = lifeMask.transform.position;
            lifeStartPosition = LifeBar.transform.position;
            comboStartPosition = ComboBar.transform.position;
            //UnderBar.material = fighter.GetComponent<SpriteRenderer>().material;

        }

        // Update is called once per frame
        void Update()
        {
            
            //LifeBar.fillAmount = fighter.Life;
            //LifeBar.color = Color.HSVToRGB((fighter.Life * 128) / 360f, 1f, 1f);
            //ComboBar.fillAmount = fighter.ComboStrength;
            //ComboBar.rectTransform.anchoredPosition = new Vector3(LifeBar.rectTransform.rect.width * (1f - fighter.Life), 0, 0);

            
            //ChargeBar.fillAmount = fighter.TeabagCharge;
        }

        public void Setup(int nbVictory) {
            victoryBoxContainer.Setup(nbVictory);
        }

        public void SetLife(float life, float combo) {
            lifeMask.anchoredPosition = new Vector3((1f - life) * LifeBar.rectTransform.rect.width, 0, 0);
            comboMask.anchoredPosition = new Vector3(((1f - life) + combo) * LifeBar.rectTransform.rect.width, 0, 0);
            LifeBar.rectTransform.position = lifeStartPosition;
            ComboBar.rectTransform.position = comboStartPosition;
        }

        public void SetStun(float stun) {
            StunBar.fillAmount = stun;
        }

        public void SetGuard(float guard) {
            GuardBar.fillAmount = guard;
        }

        public void AddVictory() {
            victoryBoxContainer.AddVictory();
        }


    }
}