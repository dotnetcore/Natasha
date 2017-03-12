
namespace Natasha.Core.Base
{
    public interface IPacket
    {
        void InStackAndPacket();
        void InStackAndUnPacket();
        void Packet();
        void UnPacket();
    }
}
