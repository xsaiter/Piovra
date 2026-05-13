using Npgsql;

namespace Piovra.Pgsql;

public sealed class RowReader(NpgsqlDataReader reader) {
    readonly Dictionary<string, int> _mapColumn = CreateMapColumn(reader);

    static Dictionary<string, int> CreateMapColumn(NpgsqlDataReader reader) {
        return Enumerable.Range(0, reader.FieldCount)
            .ToDictionary(reader.GetName, _ => _, StringComparer.OrdinalIgnoreCase);
    }

    public bool HasRows => reader.HasRows;
    public bool Read() => reader.Read();
    public Task<bool> ReadAsync(CancellationToken ct = default) => reader.ReadAsync(ct);

    public T? Field<T>(string column, Func<NpgsqlDataReader, int, T> get) {
        return ExtractField(column, get, canNull: true);
    }

    public T Field_Must<T>(string column, Func<NpgsqlDataReader, int, T> get) {
        return Requires.NotNull(ExtractField<T>(column, get, canNull: false));
    }

    T? ExtractField<T>(string column, Func<NpgsqlDataReader, int, T> get, bool canNull) {
        if (_mapColumn.TryGetValue(column, out int i)) {
            if (reader.IsDBNull(i)) {
                if (canNull) {
                    return default;
                }
                throw new InvalidOperationException(
                    message: $"Column '{column}' contains a DBNull value, but value was required.");
            }
            return get(reader, i);
        }
        throw new ArgumentException(
            message: $"Column '{column}' was not found.",
            paramName: nameof(column));
    }

    public string? Str(string column) => Field(column, (r, i) => r.GetString(i));
    public string Str_Must(string column) => Field_Must(column, (r, i) => r.GetString(i));
    public string Str_Empty(string column) => Str(column) ?? string.Empty;

    public int? Int32(string column) => Field(column, (r, i) => r.GetInt32(i));
    public int Int32_Must(string column) => Field_Must(column, (r, i) => r.GetInt32(i));

    public long? Int64(string column) => Field(column, (r, i) => r.GetInt64(i));
    public long Int64_Must(string column) => Field_Must(column, (r, i) => r.GetInt64(i));
}
