using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame
{
    public class FightManager : MonoBehaviour {
        public FighterController player1;
        public FighterController player2;
        public static float groundHeight = 0;
        public static float gravity = 1;
        public static float defaultTimeModifier = 1.0f;
        public static float timeModifier = 1.0f;

        bool bReset = false;
        float resetTimer = 0.0f;


	    // Use this for initialization
	    void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
            FighterController.Hit(player1, player2);
            if (!bReset && (player1.life <= 0 || player2.life <= 0))
            {
                Debug.Log("Match End");
                bReset = true;
                resetTimer = 0.0f;
            }
            else if (bReset)
            {
                resetTimer += Time.deltaTime;
                if(resetTimer > 2.0f)
                {
                    ResetMatch();
                }
            }
	    }

        void ResetMatch()
        {
            Debug.Log("Match Reload");
            bool bPlayer1Fall = (player1.life <= 0);
            bool bPlayer2Fall = (player2.life <= 0);
            player1.Reset();
            player2.Reset();
            if (bPlayer1Fall)
            {
                player1.Fighter.SetMove(player1.Fighter.GetUp);
            }

            if (bPlayer2Fall)
            {
                player2.Fighter.SetMove(player1.Fighter.GetUp);
            }
            bReset = false;
        }

        public void OnRenderObject()
        {
            float l1 = Mathf.Max(player1.life, 0.0f);
            float l2 = Mathf.Max(player2.life, 0.0f);
            float c1 = player1.ComboStrength;
            float c2 = player2.ComboStrength;

            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(0.05f, 0.05f, 0.05f));
            Material debug = HitBox.GetDebugMaterial();
            GL.PushMatrix();
            debug.SetPass(0);
            GL.MultMatrix(player1.transform.localToWorldMatrix*scale * Matrix4x4.Translate(new Vector3(-50, -35, -10)));
            GL.Begin(GL.QUADS);

            GL.Color(Color.red);
            GL.Vertex(Vector3.zero);
            GL.Vertex3(0, 20, 0);
            GL.Vertex3(100, 20, 0);
            GL.Vertex3(100, 0, 0);

            GL.Color(Color.green);
            GL.Vertex(Vector3.zero);
            GL.Vertex3(0, 20, 0);
            GL.Vertex3(l1, 20, 0);
            GL.Vertex3(l1, 0, 0);

            GL.Color(Color.blue);
            GL.Vertex3(l1 - c1, 0, 0);
            GL.Vertex3(l1 - c1, 20, 0);
            GL.Vertex3(l1, 20, 0);
            GL.Vertex3(l1, 0, 0);


            GL.End();
            GL.PopMatrix();


            GL.PushMatrix();
            debug.SetPass(0);
            GL.MultMatrix(player2.transform.localToWorldMatrix * scale * Matrix4x4.Translate(new Vector3(-50, -35, -10)));
            GL.Begin(GL.QUADS);

            GL.Color(Color.red);
            GL.Vertex(Vector3.zero);
            GL.Vertex3(0, 20, 0);
            GL.Vertex3(100, 20, 0);
            GL.Vertex3(100, 0, 0);

            GL.Color(Color.green);
            GL.Vertex(Vector3.zero);
            GL.Vertex3(0, 20, 0);
            GL.Vertex3(l2, 20, 0);
            GL.Vertex3(l2, 0, 0);

            GL.Color(Color.blue);
            GL.Vertex3(l2 - c2, 0, 0);
            GL.Vertex3(l2 - c2, 20, 0);
            GL.Vertex3(l2, 20, 0);
            GL.Vertex3(l2, 0, 0);

            GL.End();
            GL.PopMatrix();
        }

        private void OnDrawGizmos()
        {
            OnRenderObject();
        }

    }
}
