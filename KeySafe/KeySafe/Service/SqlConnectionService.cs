﻿using System.Collections.Generic;
using System.Data.SQLite;
using KeySafe.Model;

namespace KeySafe.Service
{
    public class SqliteConnectionService
    {
        private static SQLiteConnection connection;
        private string connectionString;

        public SqliteConnectionService(string path, string password)
        {
            connectionString = $"Data Source={path};Password={password};";
        }

        public SqliteConnectionService(string path)
        {
            connectionString = $"Data Source={path};";
        }

        public List<KeyDirectory> GetKeyDirectorys()
        {
            List<KeyDirectory> keyDirectorys = new List<KeyDirectory>();

            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "SELECT ID, Name, ParentID FROM KeyDirectorys";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    // command.Parameters.AddWithValue("@id", "2");
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        var keyDirektory = new KeyDirectory();
                        while (dataReader.Read())
                        {
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("ID")))
                                keyDirektory.ID = dataReader.GetInt32(dataReader.GetOrdinal("ID"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Name")))
                                keyDirektory.Name = dataReader.GetString(dataReader.GetOrdinal("Name"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("ParentID")))
                                keyDirektory.ParentID = dataReader.GetInt32(dataReader.GetOrdinal("ParentID"));

                            // Objekt wird der Objektliste hinzugefuegt
                            keyDirectorys.Add(keyDirektory);
                        }
                    }
                }
            }
            return keyDirectorys;
        }


        public int? GetKeyDirectoryID(string name)
        {

            int? directoryId = null;
            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "SELECT ID FROM KeyDirectorys WHERE Name = @name";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@name", name);
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("ID")))
                                directoryId = dataReader.GetInt32(dataReader.GetOrdinal("ID"));
                           
                    }
                }
            }
            return directoryId;
        }


        public List<KeyDirectory> GetMainDirectorys()
        {
            List<KeyDirectory> keyDirectorys = new List<KeyDirectory>();

            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "SELECT ID, Name, ParentID FROM KeyDirectorys WHERE ParentID IS NULL";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {

                        using (SQLiteDataReader dataReader = command.ExecuteReader())
                        {

                            while (dataReader.Read())
                            {
                                var keyDirektory = new KeyDirectory();
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ID")))
                                    keyDirektory.ID = dataReader.GetInt32(dataReader.GetOrdinal("ID"));
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("Name")))
                                    keyDirektory.Name = dataReader.GetString(dataReader.GetOrdinal("Name"));
                                // Objekt wird der Objektliste hinzugefuegt
                                keyDirectorys.Add(keyDirektory);
                            }
                        }
                    
                }
            }
            return keyDirectorys;
        }


        public List<KeyDirectory> GetParentDirectorys(int id)
        {
            List<KeyDirectory> keyDirectoryList = new List<KeyDirectory>();

            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "SELECT ID, Name, ParentID FROM KeyDirectorys WHERE ParentID = @id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@id", id);
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        
                        while (dataReader.Read())
                        {
                                var keyDirektory = new KeyDirectory();
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ID")))
                                    keyDirektory.ID = dataReader.GetInt32(dataReader.GetOrdinal("ID"));
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("Name")))
                                    keyDirektory.Name = dataReader.GetString(dataReader.GetOrdinal("Name"));
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("ParentID")))
                                    keyDirektory.ParentID = dataReader.GetInt32(dataReader.GetOrdinal("ParentID"));

                            // Objekt wird der Objektliste hinzugefuegt
                            keyDirectoryList.Add(keyDirektory);
                        }
                    }
                }
            }
            return keyDirectoryList;
        }

        /// <summary>
        /// Neuer Ordner erstellen
        /// </summary>
        /// <param name="keyDirectory"></param>
        public void SetKeyDirectorys(string name, int id)
        {
            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "INSERT INTO KeyDirectorys(Name, ParentID) VALUES(@name, @parentId)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@parentId", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Neuer Ordner erstellen
        /// </summary>
        /// <param name="keyDirectory"></param>
        public void SetKeyDirectorys(string name)
        {
            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "INSERT INTO KeyDirectorys(Name) VALUES(@name)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@name", name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDirectory(string name)
        {
            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "DELETE FROM KeyDirectorys WHERE Name = @name";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@name", name); 
                    command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Gibt die SchluesselDaten von dem entsprechenden Ordner zurueck
        /// </summary>
        /// <param name="DirectoryID"> ID des Ordners in der DB</param>
        /// <returns> Liste der SchlüsselDaten</returns>
        public List<Model.Key> GetKeys(int DirectoryID)
        {
            List<Model.Key> keys = new List<Model.Key>();

            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "SELECT ID, Title, Username, Password, Url, Notes, DirectoryID FROM Keys WHERE DirectoryID = @id;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@id", DirectoryID);
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        
                        while (dataReader.Read())
                        {
                            var key = new Model.Key();
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("ID")))
                                key.ID = dataReader.GetInt32(dataReader.GetOrdinal("ID"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Title")))
                                key.Title = dataReader.GetString(dataReader.GetOrdinal("Title"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Username")))
                                key.Username = dataReader.GetString(dataReader.GetOrdinal("Username"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Password")))
                                key.Password = dataReader.GetString(dataReader.GetOrdinal("Password"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Url")))
                                key.Url = dataReader.GetString(dataReader.GetOrdinal("Url"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Notes")))
                                key.Notes = dataReader.GetString(dataReader.GetOrdinal("Notes"));
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("DirectoryID")))
                                key.DirectoryID = dataReader.GetInt32(dataReader.GetOrdinal("DirectoryID"));

                            // Objekt wird der Objektliste hinzugefuegt
                            keys.Add(key);
                        }
                    }
                }
            }
            return keys;
        }

        /// <summary>
        /// Einen neuen Key anlegen
        /// </summary>
        /// <param name="key"> Der Key der In die Datenbank geschrieben soll</param>
        public void SetKey(Model.Key key)
        {
            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "INSERT INTO Keys(Title, Username, Password, Url, Notes, DirectoryID) VALUES(@title, @username, @password, @url, @notes, @directoryId)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@title", key.Title);
                    command.Parameters.AddWithValue("@username", key.Username);
                    command.Parameters.AddWithValue("@password", key.Password);
                    command.Parameters.AddWithValue("@url", key.Url);
                    command.Parameters.AddWithValue("@notes", key.Notes);
                    command.Parameters.AddWithValue("@directoryId", key.DirectoryID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteKey(string name)
        {
            using (connection = new SQLiteConnection(connectionString))
            {
                // Connection öffnen
                connection.Open();

                // SQL Querry 
                string sql = "DELETE FROM Keys WHERE Title = @name";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // SQL injections vermeiden mit Parameter
                    command.Parameters.AddWithValue("@name", name);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
