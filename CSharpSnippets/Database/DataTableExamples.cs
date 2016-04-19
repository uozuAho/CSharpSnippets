using System;
using System.Data;

namespace CSharpSnippets.Database
{
    class DataTableExamples
    {
        public static void Run()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn { DataType = typeof(int), ColumnName = "id", AllowDBNull = false });
            dt.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "name", MaxLength = 10 });

            // good row
            var row = dt.NewRow();
            row["id"] = 1;
            row["name"] = "Bill";
            dt.Rows.Add(row);

            // long name
            //row = dt.NewRow();
            //row["id"] = 1;
            //row["name"] = "Bill5678901";
            //dt.Rows.Add(row);

            // null id
            //row = dt.NewRow();
            //row["name"] = "Bill";
            //dt.Rows.Add(row);

            // add a column after rows added - NOTE: you can add nulls to a not-null column this way
            dt.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "newcol", AllowDBNull = false});

            foreach (DataRow r in dt.Rows)
            {
                Console.WriteLine(string.Join(",", r.ItemArray));
            }
        }
    }
}
