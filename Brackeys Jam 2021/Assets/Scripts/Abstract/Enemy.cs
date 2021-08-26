using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public Rigidbody2D thisBody;
    protected bool isBeingKnocked;
    protected virtual void Start() {
        GameManager.Instance.enemyManager.currentEnemies.Add(this);

        if (GetComponentInChildren<Rigidbody2D>()) {
            thisBody = GetComponentInChildren<Rigidbody2D>();
        }

        isBeingKnocked = false;
    }
    public void GetKnockedBack(Vector2 direction, float strength, float delay = 0.7f) {
        StopCoroutine(KnockbackRoutine(direction, strength, delay));
        StartCoroutine(KnockbackRoutine(direction, strength, delay));
    }

    public IEnumerator KnockbackRoutine(Vector2 direction, float strength, float delay = 0.7f) {
        isBeingKnocked = true;
        thisBody.AddForce(direction*strength, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delay);
        thisBody.velocity = Vector2.zero;
        isBeingKnocked = false;
    }

}
