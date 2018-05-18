using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LZFight.Scripting;
using CameraScript;

namespace FightingGame
{
    public class LZFightManager : MonoBehaviour {

        public class Player {
            public Life life;
            public Combo combo;

            public void Setup(LZFighterAnimator fighter) {
                life = fighter.fighter.GetComponent<Life>();
                combo = fighter.fighter.GetComponent<Combo>();
            }

            public int victoryCount = 0;
        }

        public LZFighterAnimator player1;
        public LZFighterAnimator player2;

        public LZUIFighterState p1State;
        public LZUIFighterState p2State;

        public RectTransform fight;
        public RectTransform p1win;
        public RectTransform p2win;

        private Vector3 p1position;
        private Vector3 p2position;

        private Player p1 = new Player();
        private Player p2 = new Player();

        public int roundTime = 99;
        private float time = 0f;

        private bool p1R, p2R, start;

        public Vector3 camOffset;
        FollowMultiple camFollow;
        public float camFollowThreshold;

        private void Awake() {
            player1.onInitialize += () => {
                p1.Setup(player1);
                p1.combo.onConfirm += () => {
                    p2.combo.Apply();
                };
                p1R = true;
            };
            player2.onInitialize += () => {
                p2.Setup(player2);
                p2.combo.onConfirm += () => {
                    p1.combo.Apply();
                };
                p2R = true;
            };
            p1position = player1.transform.position;
            p2position = player2.transform.position;
            StartCoroutine(StartMatch());
            p1State.Setup(2);
            p2State.Setup(2);
            Camera.main.LimitX(-3, 3);
            camFollow = Camera.main.FollowMultiple(player1.transform, player2.transform);
            player1.transform.ClampToCamera(Camera.main, camFollowThreshold);
            player2.transform.ClampToCamera(Camera.main, camFollowThreshold);

            fight.gameObject.SetActive(false);
            p1win.gameObject.SetActive(false);
            p2win.gameObject.SetActive(false);
        }

        private void Update() {
            //camFollow.offset = camOffset;
            if (!p1R || !p2R || !start) {
                return;
            }

            if(player1.transform.position.x > player2.transform.position.x) {
                player1.fighter.invertHorizontal = true;
                player2.fighter.invertHorizontal = false;
            }
            else {
                player2.fighter.invertHorizontal = true;
                player1.fighter.invertHorizontal = false;
            }

            if(p1.life.CurrentLife <= 0) {
                p2.victoryCount++;
                StartCoroutine(RoundEnd(2));
                p2State.AddVictory();
            }
            if (p2.life.CurrentLife <= 0) {
                p1.victoryCount++;
                StartCoroutine(RoundEnd(1));
                p1State.AddVictory();
            }

            p1State.SetLife(p1.life.NormalizedLife, p1.combo.NormalizedCombo);
            p2State.SetLife(p2.life.NormalizedLife, p2.combo.NormalizedCombo);
        }

        private IEnumerator RoundEnd(int player) {
            BlockPlayers();

            bool waitImage = false;
            if(player == 1) {
                yield return Victory(p1win);
            }
            else {
                yield return Victory(p2win);
            }

            yield return new WaitForSeconds(1f);
            // Round End Coroutine
            Debug.Log("Round End");
            StartCoroutine(StartMatch());
            //p1State.Setup(2);
            //p2State.Setup(2);
        }

        private IEnumerator Victory(RectTransform player)
        {
            player.gameObject.SetActive(true);
            player.MoveTo(Vector3.left * 5000);
            yield return new WaitForSeconds(0.2f);
            player.MoveTo(Vector3.zero, 0.3f);
            yield return new WaitForSeconds(2f);
            player.MoveTo(Vector3.right * 5000, 0.3f);
            yield return new WaitForSeconds(1f);
        }

        private void BlockPlayers() {
            start = false;
            player1.fighter.blockInput = true;
            player2.fighter.blockInput = true;
        }

        private void UnblockPlayers() {
            start = true;
            player1.fighter.blockInput = false;
            player2.fighter.blockInput = false;
        }


        private void ResetMatch() {
            player1.Initialize();
            player2.Initialize();
            time = 0f;
            player1.transform.position = p1position;
            player2.transform.position = p2position;
            BlockPlayers();
        }

        private IEnumerator StartMatch() {
            ResetMatch();
            BlockPlayers();

            yield return new WaitForSeconds(0.5f);
            fight.gameObject.SetActive(true);
            fight.MoveTo(Vector3.up * 5000);
            fight.MoveTo(Vector3.zero, 0.3f);
            yield return new WaitForSeconds(1f);
            fight.MoveTo(Vector3.down * 5000, 0.3f);

            yield return new WaitForSeconds(1.5f);
            UnblockPlayers();

        }

    }
}
