using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightingGame
{
    public class FightCamera : MonoBehaviour {
        Vector3 basePosition;

        public float maxDistance = 3;
        float maxSeparation = 4;

        public FighterController p1;
        public FighterController p2;

        public new Camera camera;

	    // Use this for initialization
	    void Start () {
            basePosition = transform.position;
            Ray rayLeft = camera.ScreenPointToRay(new Vector3(0, 0, 0));
            Ray rayRight = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, 0, 0));
            Plane p = new Plane(Vector3.back, p1.transform.position);
            float rayDistance;
            float left = 0;
            float right = 0;
            if(p.Raycast(rayLeft, out rayDistance))
            {
                left = rayLeft.GetPoint(rayDistance).x;
            }
            if (p.Raycast(rayRight, out rayDistance))
            {
                right = rayRight.GetPoint(rayDistance).x;
            }
            maxSeparation = Mathf.Abs(right - left) / 2f;
            Debug.Log("sep:" + maxSeparation);
        }
	
	    // Update is called once per frame
	    void Update () {
            float center = Mathf.Min(p1.transform.position.x, p2.transform.position.x)
                            + Mathf.Abs(p1.transform.position.x - p2.transform.position.x)/2f;
            transform.position = new Vector3(Mathf.Max(Mathf.Min(center, basePosition.x+maxDistance), basePosition.x-maxDistance), transform.position.y, transform.position.z);
            
            

            p1.maxPosition = new Vector2(transform.position.x - maxSeparation, transform.position.x+maxSeparation);
            p2.maxPosition = new Vector2(transform.position.x - maxSeparation, transform.position.x+maxSeparation);
        }
    }
}
