using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace SilentPause;

public class Core : ModSystem
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.Event.PauseResume += (isPaused) => Event_PauseResume(isPaused, api);
    }

    private void Event_PauseResume(bool isPaused, ICoreClientAPI api)
    {
        Queue<ILoadedSound> sounds = (api.World as ClientMain)?.GetField<Queue<ILoadedSound>>("ActiveSounds");
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
