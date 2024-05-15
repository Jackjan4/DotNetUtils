using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Roslan.DotNETUtils.Data {
    public static class DataTableUtils {


        /// <summary>
        /// Filter DataTable and return the content of the first found field
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        ///
        /// <param name="table">zu durchsuchende Tabelle</param>
        /// <param name="filter">gesuchte Einträge</param>
        /// <param name="field">Feldname, dessen Inhalt zurückgeliefert werden soll</param>
        public static object FilterDataTable(DataTable table, string filter, string sorting, string field) {
            object result = null; // Rückgabewert
            DataView dvDatensicht = new DataView(table); // Datensichtobjekt

            // Filter an Dataview übergeben
            dvDatensicht.RowFilter = filter;
            dvDatensicht.Sort = sorting;

            // Datensicht durchsuchen
            foreach (DataRowView zeile in dvDatensicht) {
                // Leere Felder nicht berücksichtigen
                if (!Convert.IsDBNull(zeile[field])) {
                    result = zeile[field];
                    break;
                }
            }

            // Ausgabe zurückliefern
            return result;
        }


        /// <summary>
        /// Ändert den Inhalt einer DataTable
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <param name="tabelle">zu ändernde Tabelle</param>
        /// <param name="filter">gesuchte Einträge</param>
        /// <param name="field">Feldnamen</param>
        /// <param name="content">Inhalte</param>
        public static void ReplaceDataTableEntries(DataTable table, string filter, List<string> fields, List<string> content) {
            DataRow[] drInhalte;              // Datensatzobjekt

            // Nur hinzufügen, wenn gleiche Anzahl Felder und Inhalte
            if (fields.Count == content.Count) {
                drInhalte = table.Select(filter);
                foreach (DataRow drInhalt in drInhalte) {
                    // Alle Felder / Inhalte durchlaufen
                    for (int i = 0; i < fields.Count; i++) {

                        if (content[i] == "") {
                            drInhalt[fields[i]] = DBNull.Value;
                        } else {
                            drInhalt[fields[i]] = content[i];
                        }
                    }
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
        public static string ConvertDataTableToCSV(DataTable table, string separator = ",") {
            string result = null;
            TextWriter writer = new StringWriter();                                  // Dateiobjekt
            DataRow[] drZeilen = table.Select();

            // Kopfzeile schreiben
            StringBuilder strZeile = new StringBuilder();               // Textobjekt
            for (int i = 0; i < table.Columns.Count; i++) {
                strZeile.Append(separator + table.Columns[i].ToString());
            }
            writer.WriteLine(strZeile.ToString().Substring(2));

            foreach (DataRow drZeile in drZeilen) {
                StringBuilder striZeile = new StringBuilder();              // Textobjekt
                for (int i = 0; i < table.Columns.Count; i++) {
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
            DataTable result = new DataTable();
            string[] lines = strCsv.Split(new[] { "\r\n" }, StringSplitOptions.None);

            if (lines.Length < 1) {
                return null; // TODO: Exception?
            }

            string[] colNames = lines[0].Split(new[] { separator }, StringSplitOptions.None);
            int countCol = colNames.Length;
            
            // Create Columns in DataTable
            foreach (string colName in colNames) {
                result.Columns.Add(colName);
            }

            foreach(string strRow in lines.Skip(1)) {
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
