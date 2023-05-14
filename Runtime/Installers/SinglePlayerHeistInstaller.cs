using Reflex.Core;
using UnityEngine;

namespace Mirzipan.Heist.Installers
{
    public class SinglePlayerHeistInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            HeistInstaller.InstallCommon(descriptor);
            HeistInstaller.InstallNullNetwork(descriptor);
            HeistInstaller.InstallClient(descriptor);
            HeistInstaller.InstallServer(descriptor);
        }
    }
}