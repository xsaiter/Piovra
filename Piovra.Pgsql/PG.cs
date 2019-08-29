using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace Piovra.Pgsql {
    public static class PG {
        public static async Task<int> PerformNonQuery(this NpgsqlConnection conn, string sql, object param = null) {
            using (var cmd = conn.CreateCommand()) {
                PrepareCmd(cmd, sql, param);
                var result = await cmd.ExecuteNonQueryAsync();
                return result;

            }
        }

        public static async Task<T> PerformScalar<T>(this NpgsqlConnection conn, string sql, object param = null) {
            using (var cmd = conn.CreateCommand()) {
                PrepareCmd(cmd, sql, param);
                var result = await cmd.ExecuteScalarAsync();
                return (T)result;
            }
        }

        public static async Task<List<T>> PerformQuery<T>(this NpgsqlConnection conn, string sql, object param = null)
        where T : new() {
            var result = new List<T>();

            using (var cmd = conn.CreateCommand()) {
                PrepareCmd(cmd, sql, param);

                using (var r = await cmd.ExecuteReaderAsync()) {
                    if (r.HasRows) {
                        var properties = typeof(T).GetProperties();
                        var columns = r.GetColumnSchema();

                        while (r.Read()) {
                            var obj = new T();

                            foreach (var column in columns) {
                                var name = column.ColumnName;
                                var property = properties.FirstOrDefault(x => x.Name.SameIgnoreCase(name));
                                if (property != null) {
                                    var i = r.GetOrdinal(name);
                                    if (!r.IsDBNull(i)) {
                                        var value = r.GetValue(i);
                                        property.SetValue(obj, value);
                                    }
                                }
                            }

                            result.Add(obj);
                        }
                    }
                }
            }

            return result;
        }

        static void PrepareCmd(NpgsqlCommand cmd, string sql, object param = null) {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            if (param != null) {
                var properties = param.GetType().GetProperties();

                foreach (var property in properties) {
                    var p = cmd.CreateParameter();

                    p.ParameterName = $"@{property.Name}";
                    var value = property.GetValue(param);

                    if (value != null) {
                        p.NpgsqlValue = value;
                        p.NpgsqlDbType = GetDbTypeFrom(property);
                    } else {
                        p.NpgsqlValue = null;
                    }

                    cmd.Parameters.Add(p);
                }
            }
        }

        static NpgsqlDbType GetDbTypeFrom(PropertyInfo property) {
            var t = property.PropertyType;
            if (mapTypes.ContainsKey(t)) {
                return mapTypes[t];
            }
            throw new Exception($"unexpected type: {t}");
        }

        public static async Task RunAllSqlFilesInDirectory(string path, int timeout, string connString, int step = 100) {
            var files = Directory.GetFiles(path);

            using (var conn = await SmartConn.New(connString)) {
                foreach (var file in files) {
                    var lines = File.ReadAllLines(file);

                    var skip = 0;
                    List<string> cc;

                    while ((cc = lines.Skip(skip).Take(step).ToList()).Any()) {
                        skip += cc.Count;

                        var sb = new StringBuilder();

                        cc.ForEach(c => sb.AppendLine(c));

                        using (var cmd = conn.CreateCommand()) {
                            cmd.CommandText = sb.ToString();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = timeout;

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        public static async Task CopyFromFile(NpgsqlConnection conn, FileInfo file, string copyFromCommand) {
            using (var stream = file.OpenRead()) {
                using (var writer = conn.BeginTextImport(copyFromCommand)) {
                    using (var reader = new StreamReader(stream)) {
                        while (!reader.EndOfStream) {
                            var line = await reader.ReadLineAsync();
                            await writer.WriteLineAsync(line);
                        }
                    }
                }
            }
        }

        static readonly Dictionary<Type, NpgsqlDbType> mapTypes = new Dictionary<Type, NpgsqlDbType> { { typeof(int), NpgsqlDbType.Integer },
            { typeof(int?), NpgsqlDbType.Integer },
            { typeof(long), NpgsqlDbType.Bigint },
            { typeof(long?), NpgsqlDbType.Bigint },
            { typeof(double), NpgsqlDbType.Double },
            { typeof(double?), NpgsqlDbType.Double },
            { typeof(bool), NpgsqlDbType.Boolean },
            { typeof(bool?), NpgsqlDbType.Boolean },
            { typeof(DateTime), NpgsqlDbType.Date },
            { typeof(DateTime?), NpgsqlDbType.Date },
            { typeof(string), NpgsqlDbType.Text }
        };

        public class SmartConn : Sql.Core.SmartConn<NpgsqlConnection> {
            public SmartConn(Config cfg) : base(cfg) { }
        }
    }
}