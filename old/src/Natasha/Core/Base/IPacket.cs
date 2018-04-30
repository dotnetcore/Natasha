
namespace Natasha.Core
{
    public interface IPacket
    {
        void InStackAndPacket();
        void InStackAndUnPacket();
        void Packet();
        void UnPacket();
    }
}
