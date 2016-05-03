using System;
using System.Data;

namespace Uozu.Utils.Database
{
    public class MockDataReader : IDataReader
    {
        private int _currentRow = -1;
        private readonly DataTable _data;

        public MockDataReader(DataTable data)
        {
            _data = data;
        }

        public object this[string name]
        {
            get
            {
                return _data.Rows[_currentRow][name];
            }
        }

        public object this[int i]
        {
            get
            {
                return _data.Rows[_currentRow][i];
            }
        }

        public int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int FieldCount
        {
            get
            {
                return _data.Columns.Count;
            }
        }

        public bool IsClosed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int RecordsAffected
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Close()
        {
        }

        public void Dispose()
        {
        }

        public bool GetBoolean(int i)
        {
            return (bool)_data.Rows[_currentRow][i];
        }

        public byte GetByte(int i)
        {
            return (byte)_data.Rows[_currentRow][i];
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return (char)_data.Rows[_currentRow][i];
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)_data.Rows[_currentRow][i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)_data.Rows[_currentRow][i];
        }

        public double GetDouble(int i)
        {
            return (double)_data.Rows[_currentRow][i];
        }

        public Type GetFieldType(int i)
        {
            return _data.Rows[_currentRow][i].GetType();
        }

        public float GetFloat(int i)
        {
            return (float)_data.Rows[_currentRow][i];
        }

        public Guid GetGuid(int i)
        {
            return (Guid)_data.Rows[_currentRow][i];
        }

        public short GetInt16(int i)
        {
            return (short)_data.Rows[_currentRow][i];
        }

        public int GetInt32(int i)
        {
            return (int)_data.Rows[_currentRow][i];
        }

        public long GetInt64(int i)
        {
            return (long)_data.Rows[_currentRow][i];
        }

        public string GetName(int i)
        {
            return _data.Columns[i].ColumnName;
        }

        public int GetOrdinal(string name)
        {
            return _data.Columns[name].Ordinal;
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            return (string)_data.Rows[_currentRow][i];
        }

        public object GetValue(int i)
        {
            return _data.Rows[_currentRow][i];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return _data.Rows[_currentRow][i].Equals(null);
        }

        public bool NextResult()
        {
            if (_currentRow == _data.Rows.Count - 1)
                return false;
            _currentRow += 1;
            return true;
        }

        public bool Read()
        {
            if (_currentRow == _data.Rows.Count - 1)
                return false;
            _currentRow += 1;
            return true;
        }
    }
}
