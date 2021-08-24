using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class EnemyScript : Enemy
{
    SAP2DAgent agent;
    protected override void Start() {

        base.Start();

        agent = GetComponent<SAP2DAgent>();
        agent.Target = GameManager.Instance.playerManager.playerScript.gameObject.transform;
        // agent.CanMove = false;
    }

    void Update()
    {
        
    }
}
