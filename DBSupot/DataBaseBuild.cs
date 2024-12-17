using System.Collections.Generic;

namespace SystemBased
{
    internal class DataBaseBuild
    {
        public string TableName { get; private set; }
        public List<string> IgnoreThis { get; private set; }
        public string DataBaseName {  get; private set; } = "DataBase";

        public DataBaseBuild()
        {
            IgnoreThis = null;
        }

        public DataBaseBuild SetTableName(string tableName)
        {
            TableName = tableName;
            return this;
        }

        public DataBaseBuild SetDataBaseName(string sataBaseName)
        {
            DataBaseName = sataBaseName;
            return this;
        }

        public DataBaseBuild SetIgnoreThis(List<string> ignoreThis, bool cleraFirsIgnore = false)
        {
            if (cleraFirsIgnore)
                IgnoreThis.Clear();
            IgnoreThis.AddRange(ignoreThis);
            return this;
        }

        public DataBase Build()
        {
            DataBase DB = new DataBase(TableName, DataBaseName);
            if(IgnoreThis != null)
                DB.SetIgnoreColumns(IgnoreThis, true);
            return DB;
        }
    }
}
