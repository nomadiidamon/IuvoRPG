using System;
using System.Runtime.InteropServices;
using UnityEngine;

public interface IPlayerHandler
{
    Context playerContext { get; set; }
    ContextPlayerHandlerKey HandlerKey { get; }

    public void UpdateHandlerInContext()
    {
        IPlayerHandler handler = this;
        if (handler != null && playerContext != null)
        {
            Type toAdd = handler.GetType();
            var key = handler.HandlerKey;
            
            playerContext.Set<Type>(key, toAdd);
        }
    }
}