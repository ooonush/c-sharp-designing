using System;
using System.Collections.Generic;

namespace Generics.Tables
{
    public abstract class TableIndexer<TRow, TCol, TData>
    {
        protected readonly Table<TRow, TCol, TData> _table;

        protected TableIndexer(Table<TRow, TCol, TData> table)
        {
            _table = table;
        }

        public abstract TData this[TRow row, TCol col] { get; set; }
    }

    public class OpenTableIndexer<TRow, TCol, TValue> : TableIndexer<TRow, TCol, TValue>
    {
        public OpenTableIndexer(Table<TRow, TCol, TValue> table) : base(table)
        {
        }

        public override TValue this[TRow row, TCol col]
        {
            get => _table.TryGetValue(row, col, out TValue value) ? value : default;
            set => _table[row, col] = value;
        }
    }
    
    public class ExistedTableIndexer<TRow, TCol, TValue> : TableIndexer<TRow, TCol, TValue>
    {
        public ExistedTableIndexer(Table<TRow, TCol, TValue> table) : base(table)
        {
        }

        public override TValue this[TRow row, TCol col]
        {
            get
            {
                if (_table.HasColumnAndRow(row, col))
                {
                    return _table[row, col];
                }
                throw new ArgumentException();
            }
            set
            {
                if (_table.HasColumnAndRow(row, col))
                {
                    _table[row, col] = value;
                    return;
                }
                throw new ArgumentException();
            }
        }
    }

    public class Table<TRow, TCol, TValue>
    {
        public readonly OpenTableIndexer<TRow, TCol, TValue> Open;
        public readonly ExistedTableIndexer<TRow, TCol, TValue> Existed;

        public readonly HashSet<TRow> Rows = new HashSet<TRow>();
        public readonly HashSet<TCol> Columns = new HashSet<TCol>();
        private readonly Dictionary<(TRow, TCol), TValue> _data = new Dictionary<(TRow, TCol), TValue>();

        public Table()
        {
            Open = new OpenTableIndexer<TRow, TCol, TValue>(this);
            Existed = new ExistedTableIndexer<TRow, TCol, TValue>(this);
        }

        public bool HasColumnAndRow(TRow row, TCol col)
        {
            return Rows.Contains(row) && Columns.Contains(col);
        }

        public bool TryGetValue(TRow row, TCol col, out TValue data)
        {
            return _data.TryGetValue((row, col), out data);
        }

        public TValue this[TRow row, TCol col]
        {
            get => _data[(row, col)];
            set
            {
                if (!Rows.Contains(row))
                {
                    AddRow(row);
                }
                if (!Columns.Contains(col))
                {
                    AddColumn(col);
                }
                
                _data[(row, col)] = value;
            }
        }

        public void AddColumn(TCol col)
        {
            if (Columns.Contains(col))
            {
                return;
            }
            Columns.Add(col);
            foreach (TRow row in Rows)
            {
                this[row, col] = default;
            }
        }

        public void AddRow(TRow row)
        {
            if (Rows.Contains(row))
            {
                return;
            }
            Rows.Add(row);
            foreach (TCol col in Columns)
            {
                this[row, col] = default;
            }
        }
    }
}