using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace SilentPause;

public class Core : ModSystem
{
    private ICoreClientAPI clientAapi;
    
    public override void StartClientSide(ICoreClientAPI api)
    {
        clientAapi = api;
        ClientMain main = api.World as ClientMain;
        main.Platform.RegisterOnFocusChange(OnFocusChanged);
        api.Event.PauseResume += Event_PauseResume;
    }

    public override void Dispose()
    {
        clientAapi.Event.PauseResume -= Event_PauseResume;
    }

    private void OnFocusChanged(bool focus)
    {
        if (!clientAapi.IsGamePaused)
        {
            PauseOrResumeSounds(isPaused: focus);
        }
    }

    private void Event_PauseResume(bool isPaused)
    {
        PauseOrResumeSounds(isPaused);
    }

    private void PauseOrResumeSounds(bool isPaused)
    {
        ClientMain main = clientAapi.World as ClientMain;
        Queue<ILoadedSound> sounds = main.GetField<Queue<ILoadedSound>>("ActiveSounds");
        foreach (ILoadedSound sound in sounds)
        {
            if (isPaused)
            {
                sound.Pause();
                continue;
            }
            if (sound.IsPaused)
            {
                sound.Toggle(true);
            }
        }
    }
}
