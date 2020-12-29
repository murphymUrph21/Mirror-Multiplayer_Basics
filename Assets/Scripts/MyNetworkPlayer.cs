using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdated))] // from the server garaunteed
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;

    [HideInInspector]
    public string DisplayName
    {
        get { return displayName; }
    }

    #region Server


    [Server]
    public void SetDisplayName(string newName)
    {
        displayName = newName;
    }

    [Server] // called only by the server on the server
    public void SetDisplayColor(Color newColor)
    {
        displayColor = newColor;
    }

    [Command] // method called by client on server
    private void CmdSetDisplayName(string newDisplayName)
    {
        var username_Verified = Filter_Username(newDisplayName);
        if(username_Verified == Verified.all_Checks_Passed)
        {
            Rpc_SetDisplayName(newDisplayName);

            SetDisplayName(newDisplayName);
        }
        else
        {
            Debug.Log($"Error {username_Verified.ToString()}");
        }

    }


    #endregion


    #region Client Callbacks


    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("My");
    }


    [ClientRpc] // server calls this method on the client
    private void Rpc_SetDisplayName(string newDisplayName)
    {
        Debug.Log("Server works");
    }

    #endregion


    #region Local Methods


    public enum Verified
    {
        empty,
        too_short,
        too_long,
        innapropriate,
        all_Checks_Passed
    }

    private Verified Filter_Username(string userName)
    {
        if (string.IsNullOrEmpty(userName)) return Verified.empty;
        if (userName.Length <= 4) return Verified.too_short;
        if (userName.Length >= 16) return Verified.too_long;
        if (userName.Contains("fuck")) return Verified.innapropriate;
        return Verified.all_Checks_Passed;
    }


    #endregion
}
