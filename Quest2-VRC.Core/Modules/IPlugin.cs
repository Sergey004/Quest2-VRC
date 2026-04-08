using System;

namespace Quest2_VRC
{
    // Plugin interface for all modules
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        void Init(); // Called after load
        void Start(); // Called to start plugin logic
        void Stop(); // Called to stop plugin logic
    }
}
