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
        public static float timeModifier = 1.0f;
        public Slider p1life;
        public Slider p2life;


	    // Use this for initialization
	    void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
            FighterController.Hit(player1, player2);
	    }

        public void OnRenderObject()
        {
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
            GL.Vertex3(player1.life, 20, 0);
            GL.Vertex3(player1.life, 0, 0);


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
            GL.Vertex3(player2.life, 20, 0);
            GL.Vertex3(player2.life, 0, 0);

            GL.End();
            GL.PopMatrix();
        }

        private void OnDrawGizmos()
        {
            OnRenderObject();
        }

    }
}
