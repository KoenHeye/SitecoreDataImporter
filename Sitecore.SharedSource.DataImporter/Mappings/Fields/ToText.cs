using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.SharedSource.DataImporter.Extensions;
using Sitecore.Data.Items;
using System.Web;
using Sitecore.Data.Fields;
using System.Data;
using System.Collections;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields
{
    /// <summary>
    /// this stores the plain text import value as is into the new field
    /// </summary>
    public class ToText : BaseMapping, IBaseField
    {
		
		#region Properties 

        /// <summary>
        /// name field delimiter
        /// </summary>
		public char[] comSplitr = { ',' };

        /// <summary>
        /// the existing data fields you want to import
        /// </summary>
        public IEnumerable<string> ExistingDataNames { get; set; }

        /// <summary>
        /// the delimiter you want to separate imported data with
        /// </summary>
        public string Delimiter { get; set; }

        /// <summary>
        /// Use in combination with multiple fiels and a delimiter.
        /// If set to true, empty values won't be concatenated.
        /// </summary>
        public bool IgnoreEmptyValues { get; set; }

        #endregion Properties
		
		#region Constructor

		public ToText(Item i) : base(i) {
            //store fields
            ExistingDataNames = i.Fields["From What Fields"].Value.Split(comSplitr, StringSplitOptions.RemoveEmptyEntries);
			Delimiter = i.Fields["Delimiter"].Value;
		    
		    var cbf = (CheckboxField) i.Fields["Ignore Empty Values"];
		    if (cbf != null)
		        IgnoreEmptyValues = cbf.Checked;
		}

		#endregion Constructor
		
		#region Methods

		public virtual void FillField(BaseDataMap map, ref Item newItem, string importValue) {
            //store the imported value as is
            Field f = newItem.Fields[NewItemField];
            if(f != null)
                f.Value = importValue;
		}

        #endregion Methods

        #region IBaseField Methods

        /// <summary>
        /// returns a string list of fields to import
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetExistingFieldNames()
        {
            return ExistingDataNames;
        }

        /// <summary>
        /// return the delimiter to separate imported values with
        /// </summary>
        /// <returns></returns>
        public string GetFieldValueDelimiter()
        {
            return Delimiter;
        }

        public bool GetIgnoreEmptyValues()
        {
            return IgnoreEmptyValues;
        }

        #endregion IBaseField Methods
    }
}
