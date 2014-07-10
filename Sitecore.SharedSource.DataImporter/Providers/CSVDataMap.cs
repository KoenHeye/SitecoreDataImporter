using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using System.IO;

namespace Sitecore.SharedSource.DataImporter.Providers {
	public class CSVDataMap : BaseDataMap {
        
		public CSVDataMap(Database db, string connectionString, Item importItem)
            : base(db, connectionString, importItem) {
        }

		/// <summary>
		/// uses the query field to retrieve file data
		/// </summary>
		/// <returns></returns>
        public override IEnumerable<object> GetImportData() {

			if (!File.Exists(Query))
				return Enumerable.Empty<object>();

			byte[] bytes = GetFileBytes(Query);
            string data = Encoding.Default.GetString(bytes);

			//split urls by breaklines
			List<string> lines = SplitString(data, "\n");
			
			return lines;
        }
		
		/// <summary>
		/// There is no custom data for this type
		/// </summary>
		/// <param name="newItem"></param>
		/// <param name="importRow"></param>
		public override void ProcessCustomData(ref Item newItem, object importRow) {
		}

		/// <summary>
		/// gets a field value from an item
		/// </summary>
		/// <param name="importRow"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		protected override string GetFieldValue(object importRow, string fieldName) {
			
			var item = importRow as string;
			List<string> lines = SplitString(item, ",");
			
			int pos;
			string s = string.Empty;
			if(int.TryParse(fieldName, out pos) && (lines[pos] != null))
				s = lines[pos];
			return s;
		}
		
		protected List<string> SplitString(string str, string splitter){
			return str.Split(new [] { splitter }, StringSplitOptions.None).ToList();
		}

		protected byte[] GetFileBytes(string filePath) {
			//open the file selected
			var f = new FileInfo(filePath);
			FileStream s = f.OpenRead();
			var bytes = new byte[s.Length];
			s.Position = 0;
			s.Read(bytes, 0, int.Parse(s.Length.ToString(CultureInfo.InvariantCulture)));
			return bytes;
		}
    }
}
