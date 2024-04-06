﻿using System.Data;

namespace sapphtools.ohno
{
    public static class Dsv {
        private static string ArrayToDsvLine (string[] row) {
            if (row[0].Count(c => c=='"')==row[0].Length)
                throw new InvalidDataException("First element of a DSV row cannot contain only quotations marks");
            char delimiter = row[0].Where(c => c!='"').First();
            for (int i=0; i < row.Length; i++) {
                if (row[i].First()=='"' && row[i].Last()=='"')
                    continue;
                if (row[i].Count(c => c=='"') > 0 || row[i].Count(c => c==delimiter) > 0)
                    row[i] = '"' + row[i] + '"';
            }
            return string.Join(delimiter, row);
        }

        private static string[] DsvLineToArray (string line) {
            char delimiter = line.Where(c => c!='"').First();
            string[] initialSplit = line.Split(delimiter);
            string[] finalSplit;
            List<string> finalsplit = [];
            int currentindex = 0;
            bool openquotes = false;
            finalSplit = new string[initialSplit.Count()];
            foreach (string item in initialSplit) {
                if (item.Count(c => c=='"') % 2 == 1) {
                    if (openquotes) {
                        openquotes = false;
                    } else {
                        currentindex++;
                        finalSplit[currentindex] = "";
                        openquotes = true;
                    }
                } else {
                    if (!openquotes) {
                        currentindex++;
                    }
                }
                finalSplit[currentindex] += item;
            }
            if (openquotes)
                throw new InvalidDataException("An odd number of quote characters were in the line");
            Array.Resize(ref finalSplit, currentindex+1);
            return finalSplit;
        }

        public static string[] ToDataTable(DataTable table) {
            List<string> dsv = [ArrayToDsvLine(table.Columns.OfType<DataColumn>().Select(dc => dc.Caption).ToArray<string>())];
            foreach (DataRow row in table.Rows) {
                dsv.Add(ArrayToDsvLine((string[])row.ItemArray));
            }
            return dsv.ToArray<string>();
        }

        public static DataTable ToDataTable(string[] dsv, bool hasHeaders) {
            string[] firstRow = DsvLineToArray(dsv[0]);
            DataColumn[] columns = new DataColumn[firstRow.Length];
            DataTable output = new();
            if (hasHeaders) {
                for(int i=0; i < firstRow.Length; i++) {
                    columns[i] = new DataColumn(firstRow[i]);
                }
                foreach (DataColumn col in columns)
                    output.Columns.Add(col);
            } else {
                for(int i=0; i < firstRow.Length; i++) {
                    columns[i] = new DataColumn($"Column{i}");
                }
                foreach (DataColumn col in columns)
                    output.Columns.Add(col);
                output.Rows.Add(firstRow);
            }
            foreach(string row in dsv[1..]) {
                output.Rows.Add(DsvLineToArray(row));
            }
            return output;
        }

        public static DataTable ToDataTable(string dsv) {
            return ToDataTable(new string[]{dsv}, false);
        }
    }
}