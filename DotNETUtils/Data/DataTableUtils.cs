using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Roslan.DotNetUtils.Data {
    public static class DataTableUtils {



        /// <summary>
        /// Filter DataTable and return the content of the first found field
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <param name="table">The DataTable to be filtered</param>
        /// <param name="filter">gesuchte Einträge</param>
        /// <param name="field">Feldname, dessen Inhalt zurückgeliefert werden soll</param>
        public static object FilterDataTable(DataTable table, string filter, string sorting, string field) {
			var dataView = new DataView(table) {
				// Filter an Dataview übergeben
				RowFilter = filter,
				Sort = sorting
			};

			// DataView durchsuchen und Ausgabe zurückliefern
			return (from DataRowView zeile 
                    in dataView 
                    where !Convert.IsDBNull(zeile[field]) 
                    select zeile[field]).FirstOrDefault();
        }



        /// <summary>
        /// Ändert den Inhalt einer DataTable
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <param name="table">zu ändernde Tabelle</param>
        /// <param name="filter">gesuchte Einträge</param>
        /// <param name="fields">Feldnamen</param>
        /// <param name="content">Inhalte</param>
        public static void ReplaceDataTableEntries(DataTable table, string filter, IList<string> fields, IList<string> content) {
            // Nur hinzufügen, wenn gleiche Anzahl Felder und Inhalte
            if (fields.Count != content.Count) 
                return;

            DataRow[] drInhalte = table.Select(filter);
            foreach (var drInhalt in drInhalte) {
                // Alle Felder / Inhalte durchlaufen
                for (var i = 0; i < fields.Count; i++) {
                    drInhalt[fields[i]] = content[i] == "" ? (object)DBNull.Value : (object)content[i];
                }
            }
        }



        /// <summary>
        /// Speichert den Inhalt einer Datatable als CSV.
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <param name="table">Datatable, welches gespeichert werden soll</param>
        /// <param name="separator">Wonach sollen die Spalten getrennt werden (Normalerweise ein Semikolon)</param>
        /// <returns></returns>
        public static string ConvertDataTableToCsv(DataTable table, string separator = ",") {
            string result = null;
            TextWriter writer = new StringWriter();                                  // Dateiobjekt

            // Kopfzeile schreiben
            var strZeile = new StringBuilder();               // Textobjekt
            for (var i = 0; i < table.Columns.Count; i++) {
                strZeile.Append(separator + table.Columns[i].ToString());
            }
            writer.WriteLine(strZeile.ToString().Substring(2));

            foreach (DataRow drZeile in table.Select()) {
                var striZeile = new StringBuilder();              // Textobjekt
                for (var i = 0; i < table.Columns.Count; i++) {
                    striZeile.Append(separator + drZeile[i].ToString());
                }
                writer.WriteLine(striZeile.ToString().Substring(2));
            }

            writer.Flush();
            result = writer.ToString();
            writer.Close();

            return result;
        }



        /// <summary>
        /// Erstellt einen DataTable aus einer CSV-Datei (Standard Trennzeichen Komma , ).
        /// Rückgabewert: Datatable mit dem Inhalt der CSV-Datei.
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <returns></returns>
        public static DataTable ConvertCsvToDataTable(string strCsv, string separator = ",") {
            var result = new DataTable();
            string[] lines = strCsv.Split(new[] { "\r\n" }, StringSplitOptions.None);

            if (lines.Length < 1) {
                return null; // TODO: Exception?
            }

            string[] colNames = lines[0].Split(new[] { separator }, StringSplitOptions.None);
            var countCol = colNames.Length;

            // Create Columns in DataTable
            foreach (string colName in colNames) {
                result.Columns.Add(colName);
            }

            foreach (string strRow in lines.Skip(1)) {
                string[] rowValues = strRow.Split(new[] { separator }, StringSplitOptions.None);

                DataRow newRow = result.NewRow();
                for (int i = 0; i < countCol; i++) {
                    newRow[i] = rowValues[i];
                }

                result.Rows.Add(newRow);
            }

            return result;
        }
    }
}
