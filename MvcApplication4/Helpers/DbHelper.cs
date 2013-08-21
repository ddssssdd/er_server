using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Helpers
{
    public class DbHelper
    {
        private DbContext _db;
        private String _connectionString;
        
        public DbHelper(DbContext db)
        {
            _db = db;
            _connectionString = _db.Database.Connection.ConnectionString;
        }
        public DataTable tables()
        {
            using (var connection = _db.Database.Connection)
            {
                connection.Open();
                return connection.GetSchema("Tables");
            }

        }
        public List<FieldDefine> columns(String tablename)
        {
            using (var connection = _db.Database.Connection)
            {
                connection.Open();
                DataTable dt = connection.GetSchema("Columns", new string[] { null, null, tablename, null });
                List<FieldDefine> fields = new List<FieldDefine>();
                foreach (DataRow dr in dt.Rows)
                {
                    FieldDefine field = new FieldDefine();
                    field.Name = dr["COLUMN_NAME"].ToString();
                    field.Index = int.Parse(dr["ORDINAL_POSITION"].ToString());
                    field.Type = dr["DATA_TYPE"].ToString();
                    
                    if (!( dr["CHARACTER_MAXIMUM_LENGTH"] is DBNull))
                        field.Length = int.Parse(dr["CHARACTER_MAXIMUM_LENGTH"].ToString());
                    field.Nullable = dr["IS_NULLABLE"].ToString().ToLower().Equals("yes");
                    fields.Add(field);
                }
                return fields;
            }
        }
        public List<FieldDefine> executeSqlToSchema(String sql)
        {
            _db.Database.Connection.Open();
            IDbCommand command = _db.Database.Connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            IDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo);
            DataTable dt =reader.GetSchemaTable();
            List<FieldDefine> fields = new List<FieldDefine>();
            foreach(DataRow dr in dt.Rows)
            {
                FieldDefine field = new FieldDefine();
                field.Name = dr["ColumnName"].ToString();
                field.Index = int.Parse( dr["ColumnOrdinal"].ToString());
                field.Type = dr["DataTypeName"].ToString();
                field.Length = int.Parse(dr["ColumnSize"].ToString());
                field.Nullable = bool.Parse(dr["AllowDBNull"].ToString());
                fields.Add(field);
            }
            return fields;
        }
        public DataTable sqlSchema(String sql)
        {
            _db.Database.Connection.Open();
            IDbCommand command = _db.Database.Connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            IDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo);
            return reader.GetSchemaTable();
            
        }
        public DataTable data(String tablename,int count=10)
        {
            _db.Database.Connection.Open();
            IDbCommand command = _db.Database.Connection.CreateCommand();
            command.CommandText = String.Format("select top {1} * from {0} ", tablename,count);
            command.CommandType = CommandType.Text;
            IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
        
    }
    public class FieldDefine
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
        public bool Nullable { get; set; }
    }
    [Table("sys.all_objects")]
    public class SqlObject
    {
        public String name { get; set; }
        public int? object_id { get; set; }
        public int? principal_id { get; set; }
        public int? schema_id { get; set; }
        public int? parent_object_id { get; set; }
        public String type { get; set; }
        public String type_desc { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? modify_date { get; set; }
        public bool is_ms_shipped { get; set; }
        public bool is_published { get; set; }
        public bool is_schema_published { get; set; }
    }
    [Table("sys.columns")]
    public class SqlColumn
    {
        public int? object_id { get; set; }
        public String name { get; set; }
        public int? column_id { get; set; }
        public byte system_type_id { get; set; }
        public int? user_type_id { get; set; }
        public short max_length { get; set; }
        public byte precision { get; set; }
        public byte scale { get; set; }
        public String collation_name { get; set; }
        public bool is_nullable { get; set; }
        public bool is_ansi_padded { get; set; }
        public bool is_rowguidcol { get; set; }
        public bool is_identity { get; set; }
        public bool is_computed { get; set; }
        public bool is_filestream { get; set; }
        public bool is_replicated { get; set; }
        public bool is_non_sql_subscribed { get; set; }
        public bool is_merge_published { get; set; }
        public bool is_dts_replicated { get; set; }
        public bool is_xml_document { get; set; }
        public int? xml_collection_id { get; set; }
        public int? default_object_id { get; set; }
        public int? rule_object_id { get; set; }
        public bool is_sparse { get; set; }
        public bool is_column_set { get; set; }
        public int number { get; set; }
    }
    [Table("sys.tables")]
    public class SqlTable
    {
        public String name { get; set; }
        public int? object_id { get; set; }
        public int? principal_id { get; set; }
        public int? schema_id { get; set; }
        public int? parent_object_id { get; set; }
        public String type { get; set; }
        public String type_desc { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? modify_date { get; set; }
        public bool is_ms_shipped { get; set; }
        public bool is_published { get; set; }
        public bool is_schema_published { get; set; }
        public int? lob_data_space_id { get; set; }
        public int? filestream_data_space_id { get; set; }
        public int? max_column_id_used { get; set; }
        public bool lock_on_bulk_load { get; set; }
        public bool uses_ansi_nulls { get; set; }
        public bool is_replicated { get; set; }
        public bool has_replication_filter { get; set; }
        public bool is_merge_published { get; set; }
        public bool is_sync_tran_subscribed { get; set; }
        public bool has_unchecked_assembly_data { get; set; }
        public int? text_in_row_limit { get; set; }
        public bool large_value_types_out_of_row { get; set; }
        public bool is_tracked_by_cdc { get; set; }
        public int lock_escalation { get; set; }
        public String lock_escalation_desc { get; set; }
    }
    [Table("sys.procedures")]
    public class SqlProcedure
    {
        public String name { get; set; }
        public int? object_id { get; set; }
        public int? principal_id { get; set; }
        public int? schema_id { get; set; }
        public int? parent_object_id { get; set; }
        public String type { get; set; }
        public String type_desc { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? modify_date { get; set; }
        public bool is_ms_shipped { get; set; }
        public bool is_published { get; set; }
        public bool is_schema_published { get; set; }
        public bool is_auto_executed { get; set; }
        public bool is_execution_replicated { get; set; }
        public bool is_repl_serializable_only { get; set; }
        public bool skips_repl_constraints { get; set; }        
    }
    [Table("sys.views")]
    public class SqlView
    {
        public String name { get; set; }
        public int? object_id { get; set; }
        public int? principal_id { get; set; }
        public int? schema_id { get; set; }
        public int? parent_object_id { get; set; }
        public String type { get; set; }
        public String type_desc { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? modify_date { get; set; }
        public bool is_ms_shipped { get; set; }
        public bool is_published { get; set; }
        public bool is_schema_published { get; set; }
        public bool is_replicated { get; set; }
        public bool has_replication_filter { get; set; }
        public bool has_opaque_metadata { get; set; }
        public bool has_unchecked_assembly_data { get; set; }
        public bool with_check_option { get; set; }
        public bool is_date_correlation_view { get; set; }
        public bool is_tracked_by_cdc { get; set; }
    }
    [Table("sys.triggers")]
    public class SqlTrigger
    {
        public String name { get; set; }
        public int? object_id { get; set; }
        public byte parent_class { get; set; }
        public String parent_class_desc { get; set; }
        public int? parent_id { get; set; }
        public String type { get; set; }
        public String type_desc { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? modify_date { get; set; }
        public bool is_ms_shipped { get; set; }
        public bool is_disabled { get; set; }
        public bool is_not_for_replication { get; set; }
        public bool is_instead_of_trigger { get; set; }
    }
    [Table("sys.all_sql_modules")]
    public class SqlModule
    {
        public int? object_id { get; set; }
        public String definition { get; set; }
        public bool uses_ansi_nulls { get; set; }
        public bool uses_quoted_identifier { get; set; }
        public bool is_schema_bound { get; set; }
        public bool uses_database_collation { get; set; }
        public bool is_recompiled { get; set; }
        public bool null_on_null_input { get; set; }
        public int? execute_as_principal_id { get; set; }
    }
}