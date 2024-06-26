using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Game.PacketProcessors;

public class CashShopOnOpenPacketProcessor : IGamePacketProcessor
{
    public GameServerPacketEnum Type => GameServerPacketEnum.CashShopOnOpen;
    
    public async Task Process(GameClient client, byte[] packetData)
    {
        //TODO: Verify what more can we send here.
        var packetWriter = new PacketWriter();
        packetWriter.Type(3412);
        client.Send(packetWriter);
    }

}