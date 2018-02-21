using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class UIVictoryManager : MonoBehaviour
    {
        public GameObject UIVBoxPrefab;
        FighterController fighter;
        List<UIVictoryBox> boxlist = new List<UIVictoryBox>();
        int boxNumber = 0;
        int victories = 0;

        // Use this for initialization
        void Start()
        {
            FightManager.addVictory += AddVictory;
            Setup(FightManager.matchNumber);
        }

        private void OnDestroy()
        {
            FightManager.addVictory -= AddVictory;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init()
        {
            victories = 0;
            foreach (UIVictoryBox vb in boxlist)
            {
                vb.Desactivate();
            }
        }

        public void SetFighter(FighterController controller)
        {
            fighter = controller;
        }

        public void AddVictory(FightingGame.FighterController controller)
        {
            if (controller == fighter && victories < boxlist.Count)
            {
                boxlist[victories].Activate();
                victories++;
            }
        }

        void Setup(int vnumber)
        {
            while (boxlist.Count != vnumber)
            {
                if (boxlist.Count < vnumber)
                {
                    AddBox();
                }
                else
                {
                    Destroy(boxlist[boxlist.Count - 1]);
                    boxlist.RemoveAt(boxlist.Count - 1);
                }
            }
            Init();
        }

        void AddBox()
        {
            boxlist.Add(Instantiate(UIVBoxPrefab).GetComponent<UIVictoryBox>());
            UIVictoryBox newBox = boxlist[boxlist.Count - 1];
            newBox.Desactivate();
            RectTransform newBoxTransform = newBox.GetComponent<RectTransform>();
            if (newBoxTransform != null)
            {
                newBoxTransform.SetParent(GetComponent<RectTransform>());
				newBoxTransform.localScale = Vector3.one;
                newBoxTransform.localPosition = new Vector2(-(newBoxTransform.rect.width + 4) * (boxlist.Count), 0);
            }
        }
    }
}