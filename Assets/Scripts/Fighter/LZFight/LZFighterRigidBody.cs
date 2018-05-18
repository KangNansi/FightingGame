using UnityEngine;
using System.Collections;
using LZFight;
using System.Collections.Generic;
using LZFight.Scripting;

[RequireComponent(typeof(LZFighterAnimator))]
[RequireComponent(typeof(Rigidbody2D))]
public class LZFighterRigidBody : MonoBehaviour {
    private LZFighterAnimator animator;
    private Rigidbody2D body;

    public List<Force> forces = new List<Force>();
    

    // Use this for initialization
    void Start() {
        animator = GetComponent<LZFighterAnimator>();
        body = GetComponent<Rigidbody2D>();
        animator.fighter.AddScript(forces.ConvertAll<MiniScript>((s) => (MiniScript)s));
    }

    // Update is called once per frame
    void Update() {
        Vector2 velocity = animator.GetVelocity();
        body.velocity = velocity;
        //transform.position += (new Vector3(velocity.x, velocity.y, 0))*Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        foreach(ContactPoint2D contact in collision.contacts) {
            if(Vector2.Dot(contact.normal, Vector2.up) > 0.5f) {
                Debug.Log("Hit Ground");
                animator.fighter.ApplyEvent(LZFIGHTEREVENT.HITGROUND);
            }
        }
    }
}
