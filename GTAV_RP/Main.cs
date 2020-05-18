using System;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using BCrypt;
using System.Collections.Generic;

using GTAV_RP.Offer;

namespace GTAV_RP
{
    public class Main : Script, IDisposable
    {
        private Player.Manager playerManager = null;
        private Database.Context database = null;

        byte index = 0;

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            NAPI.Server.SetAutoSpawnOnConnect(false);

            database = new Database.Context();
            Utils.Log("GTAV_RP STARTED");

            playerManager = new Player.Manager();
        }

        [ServerEvent(Event.ResourceStop)]
        public void OnResourceStop()
        {
            Dispose();
        }

        public void Dispose()
        {
            playerManager.Dispose();
            database.Dispose();
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Client Player)
        {
            // Pokazujemy okno logowania, zamrażamy gracza...
            NAPI.ClientEvent.TriggerClientEvent(Player, "LOG_IN_SHOW", true);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            playerManager.unregisterPlayer(player);
        }

        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerSpawn(Client player)
        {
            Player.Data user = playerManager.findPlayerByHandle(player);
            NAPI.Chat.SendChatMessageToPlayer(player, $"Witaj, {user.data.name} (UID: {user.data.id})! Rozpoczęto grę na postaci {user.CurrentCharacter.data.name} {user.CurrentCharacter.data.surname}.");
        }

        [RemoteEvent("OnPlayerLogin")]
        public void OnPlayerLogin(Client player, string login, string password)
        {
            Database.User user = database.findUserByName(login);
            if (login.Length < 3)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "LOG_IN_ERROR", "Konto nie zostało odnalezione w bazie danych.");
                return;
            }
            if (user == null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "LOG_IN_ERROR", "Konto nie zostało odnalezione w bazie danych.");
                return;
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.password);

            if (!isPasswordCorrect)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "LOG_IN_ERROR", "Wprowadzone hasło jest nieprawidłowe.");
                return;
            }

            playerManager.registerPlayer(player, user);
            NAPI.ClientEvent.TriggerClientEvent(player, "LOG_IN_SHOW", false);
        }

        [RemoteEvent("OnSpawnPlayer")]
        public void OnSpawnPlayer(Client player)
        {
            Player.Data user = playerManager.findPlayerByHandle(player);
            var characters = playerManager.getUserCharacters(user);

            ArrayList charData = new ArrayList();
            foreach (Character.Data character in characters)
            {
                charData.Add(new Database.Character { id = character.data.id, name = character.data.name, surname = character.data.surname });
            }
            string jsonCharacters = NAPI.Util.ToJson(charData);
            NAPI.Util.ConsoleOutput(jsonCharacters);

            NAPI.ClientEvent.TriggerClientEvent(player, "selector:show", true, jsonCharacters);
        }

        [RemoteEvent("OnPlayerCharacterSelect")]
        public void OnPlayerCharacterSelect(Client player, long charID)
        {
            Player.Data user = playerManager.findPlayerByHandle(player);
            Character.Data character = playerManager.getCharacterById(charID);

            user.SetCharacter(character);
            NAPI.Player.SpawnPlayer(player, new Vector3(-53.788506, 86.40988, 73.99321));
        }

        [RemoteEvent("OnCreateCharacter")]
        public void OnCreateCharacter(Client player, string data)
        {
            Player.Data user = playerManager.findPlayerByHandle(player);
            Database.Character charData = NAPI.Util.FromJson<Database.Character>(data);

            Character.Data newCharacter = new Character.Data(user, charData);
            OnPlayerCharacterSelect(player, newCharacter.data.id);
        }

        [Command("test")]
        public void CMD_TEST(Client sender, int id)
        {

            Player.Data player = playerManager.findPlayerByHandle(sender);
            NAPI.Chat.SendChatMessageToPlayer(sender, $"Witaj! {player.data.name} masz ID: {player.data.id}");

            NAPI.Util.ConsoleOutput($"{sender.Position.X}, {sender.Position.Y}, {sender.Position.Z} HEADING: {sender.Heading}");

            NAPI.Player.SetPlayerHeadOverlay(player, id, new HeadOverlay { Index = index, Opacity = 1.0f, Color = 0 });
        }
    }
}
