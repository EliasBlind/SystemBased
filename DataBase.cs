using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace SystemBased
{
    internal class DataBase
    {
        public readonly string TableName;
        public List<string> Сolumns { get; private set; }
        public List<string> AutoIncrementingColumns { get; private set; }
        public List<string> PrimaryKey { get; private set; }
        public int Rows 
        {
            get
            {
                return sqlRequest<int>($"SELECT COUNT(*) FROM {TableName}");
            }
            private set { }
        }

        private readonly List<string> _parameters = new List<string>();
        private readonly SqlConnection _sqlConnection;
        
        public DataBase(string tableName, string dataBaseName = "DataBase")
        {
            TableName = tableName;
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[dataBaseName].ConnectionString);
            _sqlConnection.Open();
            Сolumns = getСolumn<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}';");
            AutoIncrementingColumns = getСolumn<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{TableName}' AND COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1;");
            PrimaryKey = getСolumn<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = '{TableName}' AND CONSTRAINT_NAME = (SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = '{TableName}' AND CONSTRAINT_TYPE = 'PRIMARY KEY');");
            _parameters.CreateParameters(Сolumns, AutoIncrementingColumns);
        }

        public void Write(List<object> data)
        {
            List<string> columns = Сolumns.DeliteItems(AutoIncrementingColumns);
            SqlCommand command = new SqlCommand($"INSERT INTO {TableName} ({columns.Unification(", ")}) VALUES ({_parameters.Unification(", ")})", _sqlConnection);
            for(int i = 0; i < columns.Count; i++)
                command.Parameters.AddWithValue(_parameters[i], data[i] ?? DBNull.Value);
            command.ExecuteNonQuery();
        }

        public List<T> GetСolumn<T>(string columnName)
        {
            return getСolumn<T>($"SELECT {columnName} FROM {TableName}");
        }

        public List<T> GetСolumnCustomRequest<T>(string request, int column = 0)
        {
            return getСolumn<T>(request, column);
        }

        public void SetIgnoreColumns(List<string> ignores, bool cleraFirsIgnore = false)
        {
            if (cleraFirsIgnore)
                AutoIncrementingColumns.Clear();
            AutoIncrementingColumns.AddRange(ignores);
        }

        public void SetMesh<T, U>(T primaryKey, string columnName, U value, int primaryKeyIndex = 0)
        {
            setMash(primaryKey, columnName, value, primaryKeyIndex);
        }

        public List<object> GetRow<T>(T primaryKey)
        {
            return getRow(primaryKey);
        }

        public void DeleteRow<T>(T primaryKey, int primaryKeyIndex = 0)
        {
            string primaryKeyParameter = $"@{PrimaryKey[primaryKeyIndex]}";
            string query = $"DELETE FROM {TableName} WHERE {PrimaryKey[primaryKeyIndex]} = {primaryKeyParameter}";
            using (SqlCommand command = new SqlCommand(query, _sqlConnection))
            {
                command.Parameters.AddWithValue(primaryKeyParameter, primaryKey);
                command.ExecuteNonQuery();
            }
        }

        private void setMash<T, U>(T primaryKey, string columnName, U value, int primaryKeyIndex = 0)
        {
            string primaryKeyParametrs = $"@{PrimaryKey[primaryKeyIndex]}";
            string stringValue = "@value";
            using (SqlCommand command = new SqlCommand($"UPDATE {TableName} SET {columnName} = {stringValue} WHERE {PrimaryKey[primaryKeyIndex]} = {primaryKeyParametrs};", _sqlConnection))
            {
                command.Parameters.AddWithValue(primaryKeyParametrs, primaryKey);
                command.Parameters.AddWithValue(stringValue, value);
                command.ExecuteNonQuery();
            }
        }

        private List<object> getRow<T>(T primaryKey, int primaryKeyIndex = 0)
        {
            List<object> data = new List<object>();
            string primaryKeyParametrs = $"@{PrimaryKey[primaryKeyIndex]}";
            using (SqlCommand command = new SqlCommand($"SELECT * FROM {TableName} WHERE {PrimaryKey[primaryKeyIndex]} = {primaryKeyParametrs}", _sqlConnection))
            {
                command.Parameters.AddWithValue(primaryKeyParametrs, primaryKey);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            data.Add(reader.IsDBNull(i) ? null : reader.GetValue(i));
                        }
                    }
                }
            }
            return data;
        }

        private List<T> getСolumn<T>(string request, int column = 0)
        {
            List<T> data = new List<T>();
            using (SqlCommand command = new SqlCommand(request, _sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T value = reader.GetValue(column) == DBNull.Value ? default : (T)reader.GetValue(column);
                        data.Add(value);
                    }
                }
            }
            return data;
        }

        private T sqlRequest<T>(string request)
        {
            T value;
            SqlCommand command = new SqlCommand(request, _sqlConnection);
            value = (T)command.ExecuteScalar();
            return value;
        }
    }

    public static class ListExtensions
    {
        public static string Unification(this List<string> data, string add = " ")
        {
            return string.Join(add, data);
        }

        public static List<string> DeliteItems(this List<string> data, List<string> deliteThis)
        {
            List<string> result = new List<string>(data);
            foreach (string item in deliteThis)
            {
                result.Remove(item);
            }
            return result;
        }

        public static void CreateParameters(this List<string> data, List<string> referense, List<string> ignore = null)
        {
            if(data == null) 
                data = new List<string>();
            else
                data.Clear();
            if(ignore == null)
                ignore = new List<string>();
            for (int i = 0; i < referense.Count; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!ignore.Contains(referense[i])) 
                {
                    stringBuilder.Append("@");
                    stringBuilder.Append(referense[i]);
                    data.Add(stringBuilder.ToString());
                }
            }
        }
    }
}
