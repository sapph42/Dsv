using System.Data;

namespace sapphtools.ohno {
    
    public static class Dsv {
        private static string ArrayToDsvLine (string[] row) {
            if (row[0].Count(c => c=='"')==row[0].Length)
                throw new InvalidDataException("First element of a DSV row cannot contain only quotations marks");
            char delimiter = row[0].Where(c => c!='"').First();
            for (int i=0; i < row.Length; i++) {
                if (row[i].First()=='"' && row[i].Last()=='"')
                    continue;
                if (row[i].Any(c => c == '"') || row[i].Any(c => c == delimiter))
                    row[i] = '"' + row[i] + '"';
            }
            return string.Join(delimiter, row);
        }

        private static string[] DsvLineToArray (string line) {
            char delimiter = line.Where(c => c!='"').First();
            string[] initialSplit = line.Split(delimiter);
            string[] finalSplit;
            int currentindex = 0;
            bool openquotes = false;
            finalSplit = new string[initialSplit.Length];
            foreach (string item in initialSplit) {
                if (item=="\"") {
                    finalSplit[currentindex] += delimiter;
                    if (openquotes) {
                        openquotes = false;
                        currentindex++;
                    } else {
                        openquotes = true;
                    }
                    continue;
                }
                if (item.Count(c => c=='"') % 2 == 1) {
                    if (openquotes) {
                        if (item[0]=='"') {
                            finalSplit[currentindex] += item[1..];
                        } else {
                            openquotes = false;
                            finalSplit[currentindex] += item[0..^1];
                            currentindex++;
                        }
                        continue;
                    } else {
                        finalSplit[currentindex] += item[1..];
                        openquotes = true;
                        continue;
                    }
                } 
                finalSplit[currentindex] += item;
                if (!openquotes) {
                    currentindex++;
                }
            }
            if (openquotes)
                throw new InvalidDataException("An odd number of quote characters were in the line");
            Array.Resize(ref finalSplit, currentindex);
            return finalSplit;
        }

        public static string[] ToStringArr(DataTable table) {
            List<string> dsv = [ArrayToDsvLine(table.Columns.OfType<DataColumn>().Select(dc => dc.Caption).ToArray<string>())];
            foreach (DataRow row in table.Rows) {
                dsv.Add(ArrayToDsvLine(row.ItemArray.Cast<string>().ToArray<string>()));
            }
            return [.. dsv];
        }

        public static DataTable ToDataTable(string[] dsv, bool hasHeaders) {
            string[] firstRow = DsvLineToArray(dsv[0]);
            using DataTable output = new();
            if (hasHeaders) {
                for(int i=0; i < firstRow.Length; i++) {
                    output.Columns.Add(new DataColumn(firstRow[i]));
                }
            } else {
                for(int i=0; i < firstRow.Length; i++) {
                    output.Columns.Add(new DataColumn($"Column{i}"));
                }
                _ = output.Rows.Add(firstRow);
            }
            foreach(string row in dsv[1..]) {
                _ = output.Rows.Add(DsvLineToArray(row));
            }
            return output;
        }

        public static DataTable ToDataTable(string dsv) {
            return ToDataTable([dsv], false);
        }
    }
}
