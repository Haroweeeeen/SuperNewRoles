using System.Collections.Generic;
using AmongUs.GameOptions;

namespace SuperNewRoles.MapOption;

public static class RandomMap
{
    public static void Prefix()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        if (MapOption.IsRandomMap)
        {
            var rand = new System.Random();
            List<byte> RandomMaps = new();
            if (MapOption.ValidationSkeld) RandomMaps.Add(0);
            if (MapOption.ValidationMira) RandomMaps.Add(1);
            if (MapOption.ValidationPolus) RandomMaps.Add(2);
            if (MapOption.ValidationAirship) RandomMaps.Add(4);
            if (RandomMaps.Count <= 0) { return; }
            var MapsId = RandomMaps[rand.Next(RandomMaps.Count)];
            GameOptionsManager.Instance.CurrentGameOptions.SetByte(ByteOptionNames.MapId, MapsId);
            CachedPlayer.LocalPlayer.PlayerControl.RpcSyncSettings(GameOptionsManager.Instance.gameOptionsFactory.ToBytes(GameOptionsManager.Instance.CurrentGameOptions));
        }
        return;
    }
}
