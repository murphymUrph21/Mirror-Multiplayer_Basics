﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Player_Movement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    private Camera mainCamera;

    #region Server


    [Command]
    private void Cmd_Move(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(navMeshHit.position);
    }

    #endregion




    #region Client


    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        mainCamera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) { return; }
        if (!Input.GetMouseButtonDown(1)) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

       if(!Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity)) { return; }

        Cmd_Move(raycastHit.point);

    }

    #endregion



}
