using System;
using System.IO;
using System.Reflection;
using MySql.Data.MySqlClient;
using GTANetworkAPI;
using System.Collections;
using System.Data.Common;

namespace GTAV_RP.Database
{
    public class ConfigureConnection
    {
        public string fileName { get; set; }
        public string host { get; set; }
        public string password { get; set; }
        public string user { get; set; }
        public string database { get; set; }

        public ConfigureConnection()
        {
            this.fileName = "MySQL.json";
            this.host = "127.0.0.1";
            this.password = "";
            this.user = "root";
            this.database = "gtavrp";
        }
    }

    public class Context : IDisposable
    {

        private bool isConnectionSetUp = false;

        public static Context Instance = null;
        private MySqlConnection connection;

        public Context()
        {
            ConfigureConnection sql = new ConfigureConnection();
            String FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), sql.fileName);

            if (File.Exists(FilePath))
            {
                Utils.Log($"[MYSQL] Odnaleziono plik ({sql.fileName}). Trwa łączenie z bazą danych...");

                String SQLData = File.ReadAllText(FilePath);
                sql = NAPI.Util.FromJson<ConfigureConnection>(SQLData);

                String SQLConnection = $"SERVER={sql.host};PASSWORD={sql.password};UID={sql.user};DATABASE={sql.database};";
                connection = new MySqlConnection(SQLConnection);

                try
                {
                    connection.Open();
                    Utils.Log($"[MYSQL] Pomyślnie połączono z bazą danych!\n-----------\nHOST: {sql.host}\nDATABASE: {sql.database}\n----------\n");
                    isConnectionSetUp = true;
                }
                catch (Exception ex)
                {
                    Utils.Log($"[MYSQL] Wystąpił problem podczas próby połączenia z bazą danych:\n{ex}");
                }
            }
            else
            {
                Utils.Log($"[MYSQL] Nie odnaleziono pliku konfiguracyjnego bazy danych ({sql.fileName}).");
                String SQLData = NAPI.Util.ToJson(sql);

                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(SQLData);
                }

                Utils.Log("[MYSQL] Plik konfiguracyjny bazy danych został utworzony.");
                Utils.Log($"[MYSQL] Wprowadź prawidłowe dane do pliku i uruchom serwer ponownie... (FilePath: {FilePath})");
                // EXIT SERVER HERE
            }

            Instance = this;
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
            connection = null;

            Instance = null;
        }

        private MySqlCommand buildUpdateCommand(object Obj)
        {
            Type type = Obj.GetType();

            DbTable tableData = type.GetCustomAttribute<DbTable>();
            if (tableData == null)
            {
                throw new Exception("Tried to build update command using the object without DBTable attribute defined. Type: " + type.AssemblyQualifiedName);
            }

            if (tableData.name.Length == 0)
            {
                throw new Exception("Tried to build update command using the object which table name is zero length. Type: " + type.AssemblyQualifiedName);
            }

            string where = "";

            MySqlCommand command = new MySqlCommand("", connection);

            command.CommandText = "UPDATE " + tableData.name + " ";

            bool isFirstField = true;
            foreach (FieldInfo field in type.GetFields())
            {
                DbField dbField = field.GetCustomAttribute<DbField>();
                if (dbField == null)
                {
                    continue;
                }

                if (dbField.skipUpdate)
                {
                    continue;
                }

                string fieldName = dbField.name;
                string paramName = "@" + fieldName;
                object value = field.GetValue(Obj);

                command.Parameters.AddWithValue(paramName, value);

                if (dbField.isUpdateKey)
                {
                    if (where.Length > 0)
                    {
                        throw new Exception("Unsupported duplicate update key detected name = " + fieldName + ", where value = " + where);
                    }
                    else
                    {
                        where = " WHERE " + fieldName + " = " + paramName;
                    }
                    continue;
                }

                if (isFirstField)
                {
                    command.CommandText += "SET ";
                    isFirstField = false;
                }
                else
                {
                    command.CommandText += ", ";
                }

                command.CommandText += dbField.name + " = " + paramName;
            }

            command.CommandText += where;
            return command;
        }

        private void deserializeData(object Obj, DbDataReader Reader)
        {
            foreach (FieldInfo field in Obj.GetType().GetFields())
            {
                DbField dbField = field.GetCustomAttribute<DbField>();
                if (dbField == null)
                {
                    continue;
                }

                field.SetValue(Obj, Reader[dbField.name]);
            }
        }

        public User findUserByName(string name)
        {
            string query = "SELECT `member_id`, `name`, `members_pass_hash` FROM `core_members` WHERE name = @Login LIMIT 1";
            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Login", name);
            MySqlDataReader reader = command.ExecuteReader();
            
            if(!reader.Read())
            {
                reader.Close();
                return null;
            }
            User userData = new User();
            deserializeData(userData, reader);
            reader.Close();
            return userData;
        }

        public ArrayList getUserCharacters(ulong userId)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM rp_characters WHERE gid=@gid", connection);
            command.Parameters.AddWithValue("@gid", userId);

            ArrayList characters = new ArrayList();

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Character character = new Character();
                deserializeData(character, reader);

                characters.Add(character);
            }
            reader.Close();
            return characters;
        }

        public bool createCharacter(ref Character CharacterData)
        {
            if (CharacterData.id != Character.INVALID_ID)
            {
                throw new Exception("Tried to create character from data of already exising character.");
            }

            MySqlCommand command = new MySqlCommand("INSERT INTO rp_characters (gid, name, surname, appearance) VALUES(@gid, @name, @surname, @appearance)", connection);
            command.Parameters.AddWithValue("@gid", CharacterData.gid);
            command.Parameters.AddWithValue("@name", CharacterData.name);
            command.Parameters.AddWithValue("@surname", CharacterData.surname);
            command.Parameters.AddWithValue("@appearance", CharacterData.appearance);
            //command.Parameters.AddWithValue("@owner", CharacterData.owner);
            //command.Parameters.AddWithValue("@is_male", CharacterData.is_male);
            //command.Parameters.AddWithValue("@skin", CharacterData.skin);

            object id = command.ExecuteScalar();
            if (id == null)
            {
                return false;
            }

            CharacterData.id = (long)id;
            return true;
        }

        public bool updateCharacter(Character data)
        {
            MySqlCommand command = buildUpdateCommand(data);
            return command.ExecuteNonQuery() == 1;
        }
    }
}
