namespace Practices.SharePoint.Utilities {
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint.Utilities;

    public static class Picker {
        public static string GetFieldValues(this ClientPeoplePicker peoplePicker) {
            var resolvedEntities = peoplePicker.ResolvedEntities;
            string result = "";
            if (peoplePicker.Page.IsPostBack) {
                peoplePicker.Validate();
            }
            if (resolvedEntities.Count > 0) {
                if (!peoplePicker.AllowMultipleEntities) {
                    PickerEntity pickerEntity = (PickerEntity)resolvedEntities[0];
                    string text = pickerEntity.EntityData["SPUserID"] as string;
                    string text2 = pickerEntity.EntityData["SPGroupID"] as string;
                    if (!string.IsNullOrEmpty(text)) {
                        result = text;
                    } else if (!string.IsNullOrEmpty(text2)) {
                        result = text2;
                    } else {
                        result = "-1;#" + pickerEntity.Key;
                    }
                }
            } else {
                var list = new List<string>();
                foreach (PickerEntity pickerEntity2 in resolvedEntities) {
                    string item = "-1";
                    string text3 = pickerEntity2.EntityData["SPUserID"] as string;
                    string text4 = pickerEntity2.EntityData["SPGroupID"] as string;
                    if (!string.IsNullOrEmpty(text3)) {
                        item = text3;
                    } else if (!string.IsNullOrEmpty(text4)) {
                        item = text4;
                    }
                    list.Add(item);
                    list.Add(pickerEntity2.Key);
                }
                result = ConvertMultiColumnValueToString(list, false,false);
            }
            return result;
        }

        static string ConvertMultiColumnValueToString(List<string> subColumnValues, bool bAddLeadingTailingDelimiter, bool bPreserveEmpty) {
            bool flag = false;
            var stringBuilder = new StringBuilder(255);
            for (int i = 0; i < subColumnValues.Count; i++) {
                string text = subColumnValues[i];
                if (!string.IsNullOrEmpty(text)) {
                    text = text.Replace(";", ";;");
                }
                if (!string.IsNullOrEmpty(text)) {
                    flag = true;
                }
                if (bAddLeadingTailingDelimiter || i != 0) {
                    stringBuilder.Append(";#");
                }
                stringBuilder.Append(text);
            }
            if (flag || bPreserveEmpty) {
                if (bAddLeadingTailingDelimiter) {
                    stringBuilder.Append(";#");
                }
                return stringBuilder.ToString();
            }
            return string.Empty;
        }
    }
}
