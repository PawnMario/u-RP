using System;
using System.Collections.Generic;
using System.Text;

using GTANetworkAPI;

namespace GTAV_RP.Player
{
    class Data : IDisposable
    {
        public Database.User data = null;

        public Client client = null;
        public Character.Data character = null;

        public Data(Client player, Database.User userData)
        {
            client = player;
            data = userData;
        }

        public void Dispose()
        {
            if (character != null)
            {
                character.Dispose();
                character = null;
            }
        }

        public Character.Data CurrentCharacter
        {
            get
            {
                return character;
            }
        }

        private ulong accountId = Database.User.INVALID_ID;

        public ulong AccountId
        {
            get
            {
                return accountId;
            }
        }

        public void SetCharacter(Character.Data Char)
        {
            character = Char;

            Character.Appearance appearance = NAPI.Util.FromJson<Character.Appearance>(character.data.appearance);
            foreach (var face in appearance.facefeature)    NAPI.Player.SetPlayerFaceFeature(client, face.faceIndex, face.faceScale);

            Dictionary<int, ComponentVariation> clothes = new Dictionary<int, ComponentVariation>();
            foreach (var component in appearance.components)    clothes.Add(component.componentId, new ComponentVariation { Drawable = component.drawableId, Texture = component.textureId });

            NAPI.Player.SetPlayerClothes(client, clothes);

            if (appearance.headoverlay != null)
            {
                foreach (var headoverlay in appearance.headoverlay) NAPI.Player.SetPlayerHeadOverlay(client, headoverlay.overlayId, new HeadOverlay { Index = (byte)headoverlay.index, Color = (byte)headoverlay.firstColor, Opacity = (byte)headoverlay.opacity });
            }

            HeadBlend genetic = new HeadBlend
            {
                ShapeFirst = (byte)appearance.genID,
                ShapeSecond = (byte)appearance.genID,
                SkinFirst = (byte)appearance.genID,
                SkinSecond = (byte)appearance.genID
            };

            NAPI.Player.SetPlayerHeadBlend(client, genetic);
            //character.Spawn();
        }

        /// <summary>
        /// Save player data.
        /// </summary>
        public void Save()
        {
            if (character != null)
            {
                character.Save();
            }
        }

        public void InitAuth(ulong AccountId)
        {
            accountId = AccountId;
            client.SetSharedData("AccountId", accountId);
        }

        public static implicit operator Client(Data player)
        {
            return player.client;
        }

        public static implicit operator NetHandle(Data player)
        {
            return player.client.Handle;
        }
    }
}
