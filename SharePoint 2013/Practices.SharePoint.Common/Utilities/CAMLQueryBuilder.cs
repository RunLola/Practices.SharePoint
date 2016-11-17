namespace Practices.SharePoint.Utilities {
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ms467521(v=office.15).aspx
    /// </summary>
    public class CAMLQueryBuilder {
        protected IList<Filter> filters = new List<Filter>();
        protected IList<Sorter> sorters = new List<Sorter>();

        #region Filtering

        /// <summary>
        /// Logical Joins
        /// </summary>
        protected enum FilterChainingOperator {
            And,
            Or
        }

        /// <summary>
        /// Comparison Operators
        /// </summary>
        protected enum FilterQueryOperator {
            /// <summary>
            /// Searches for a string at the start of a column that holds Text or Note field type values.
            /// </summary>
            BeginsWith,
            /// <summary>
            /// Searches for a string anywhere within a column that holds Text or Note field type values.
            /// </summary>
            Contains,
            /// <summary>
            /// Used in queries to compare the dates in a recurring event with a specified DateTime value, to determine whether they overlap.
            /// </summary>
            DateRangesOverlap,
            /// <summary>
            /// Arithmetic operator that means "equal to" and is used within a query.
            /// </summary>
            Eq,
            /// <summary>
            /// Arithmetic operator that means "not equal to" and is used in queries.
            /// </summary>
            Neq,
            /// <summary>
            /// Arithmetic operator that means "less than" and is used in queries in views. This element is used similarly to the Eq and Gt elements.
            /// </summary>
            Lt,
            /// <summary>
            /// Arithmetic operator that means "less than or equal to." The Leq element is used in view queries similarly to the Eq and Geq elements.
            /// </summary>
            Leq,
            /// <summary>
            /// Arithmetic operator that means "greater than." This element is used similarly to the Eq and Lt elements.
            /// </summary>
            Gt,
            /// <summary>
            /// Arithmetic operator that means "greater than or equal to."  This element can be used within a Where element in a query.
            /// </summary>
            Geq,
            /// <summary>
            /// If the specified field is a Lookup field that allows multiple values, specifies that the Value element is included in the list item for the field that is specified by the FieldRef element.
            /// </summary>
            Include,
            /// <summary>
            /// If the specified field is a Lookup field that allows multiple values, specifies that the Value element is excluded from the list item for the field that is specified by the FieldRef element.
            /// </summary>
            NotIncludes,
            /// <summary>
            /// Specifies whether the value of a list item for the field specified by the FieldRef element is equal to one of the values specified by the Values element.
            /// </summary>
            In,
            /// <summary>
            /// Used within a query to return items that are empty (Null).
            /// </summary>
            IsNull,
            /// <summary>
            /// Used within a query to return items that are not empty (Null).
            /// </summary>
            IsNotNull,
            /// <summary>
            /// 
            /// </summary>
            Lookup
        }

        protected enum FilterFieldType {
            Text,
            Integer,
            Number,
            Guid,
            Boolean,
            DateTime,
            Lookup,
            Computed,
            ModStat,
            File
        }

        /// <summary>
        /// Class that holds filtering expressions that can be used by the <see cref="CAMLQueryBuilder"/>.
        /// </summary>
        protected class Filter {
            public string FieldName {
                get;
                set;
            }

            public object FieldValue {
                get;
                set;
            }

            public FilterFieldType FieldType {
                get;
                set;
            }

            public FilterQueryOperator QueryOperator {
                get;
                set;
            }

            public FilterChainingOperator ChainingOperator {
                get;
                set;
            }

            /// <summary>
            /// The filter expression to use when building a query. 
            /// </summary>
            public string Expression {
                get {
                    switch (QueryOperator) {
                        case FilterQueryOperator.IsNull:
                        case FilterQueryOperator.IsNotNull:
                            return string.Format("<{0}><FieldRef Name='{1}'/></{0}>", QueryOperator, FieldName);
                        case FilterQueryOperator.Lookup:
                            switch (FieldType) {
                                case FilterFieldType.Integer:
                                    return string.Format("<{0}><FieldRef Name='{1}' LookupId='TRUE'/><Value Type='{2}'>{3}</Value></{0}>",
                                        FilterQueryOperator.Eq, FieldName, FilterFieldType.Lookup, FieldValue);
                                default:
                                    return string.Format("<{0}><FieldRef Name='{1}'/><Value Type='{2}'>{3}</Value></{0}>",
                                        FilterQueryOperator.Eq, FieldName, FilterFieldType.Lookup, FieldValue);
                            }
                        case FilterQueryOperator.In:
                            string[] fieldValues = (string[])FieldValue;
                            StringBuilder builder = new StringBuilder();
                            switch (FieldType) {
                                case FilterFieldType.Lookup:
                                    builder.AppendFormat("<{0}>", QueryOperator);
                                    builder.AppendFormat("<FieldRef Name='{0}' LookupId='TRUE'/>", FieldName);
                                    builder.Append("<Values>");
                                    foreach (var fieldValue in fieldValues) {
                                        builder.AppendFormat("<Value Type='{0}'>{1}</Value>", FilterFieldType.Integer, fieldValue);
                                    }
                                    builder.Append("</Values>");
                                    builder.AppendFormat("</{0}>", QueryOperator);
                                    break;
                                default:
                                    var cycle = fieldValues.Length / 500;
                                    //if (cycle > 2) {
                                    //    cycle = 2;
                                    //}
                                    var mod = fieldValues.Length % 500;
                                    if (cycle > 0) {
                                        if (mod > 0) {
                                            for (int i = 0; i < cycle; i++) {
                                                builder.AppendFormat("<{0}>", FilterChainingOperator.Or);
                                            }
                                            for (int i = 0; i < cycle; i++) {
                                                builder.AppendFormat("<{0}>", QueryOperator);
                                                builder.AppendFormat("<FieldRef Name='{0}'/>", FieldName);
                                                builder.Append("<Values>");
                                                for (int j = 0; j < 500; j++) {
                                                    var fieldValue = fieldValues[i * 500 + j];
                                                    builder.AppendFormat("<Value Type='{0}'>{1}</Value>", FieldType, fieldValue);
                                                }
                                                builder.Append("</Values>");
                                                builder.AppendFormat("</{0}>", QueryOperator);
                                                if (i > 0) {
                                                    builder.AppendFormat("</{0}>", FilterChainingOperator.Or);
                                                }
                                            }
                                            builder.AppendFormat("<{0}>", QueryOperator);
                                            builder.AppendFormat("<FieldRef Name='{0}'/>", FieldName);
                                            builder.Append("<Values>");
                                            for (int i = 0; i < mod; i++) {
                                                var fieldValue = fieldValues[cycle * 500 + i];
                                                builder.AppendFormat("<Value Type='{0}'>{1}</Value>", FieldType, fieldValue);
                                            }
                                            builder.Append("</Values>");
                                            builder.AppendFormat("</{0}>", QueryOperator);
                                            builder.AppendFormat("</{0}>", FilterChainingOperator.Or);
                                        } else {
                                            for (int i = 0; i < cycle - 1; i++) {
                                                builder.AppendFormat("<{0}>", FilterChainingOperator.Or);
                                            }
                                            for (var i = 0; i < cycle; i++) {
                                                builder.AppendFormat("<{0}>", QueryOperator);
                                                builder.AppendFormat("<FieldRef Name='{0}'/>", FieldName);
                                                builder.Append("<Values>");
                                                for (int j = 0; j < 500; j++) {
                                                    var fieldValue = fieldValues[i * 500 + j];
                                                    builder.AppendFormat("<Value Type='{0}'>{1}</Value>", FieldType, fieldValue);
                                                }
                                                builder.Append("</Values>");
                                                builder.AppendFormat("</{0}>", QueryOperator);
                                                if (i > 0) {
                                                    builder.AppendFormat("</{0}>", FilterChainingOperator.Or);
                                                }
                                            }
                                        }
                                    } else {
                                        builder.AppendFormat("<{0}>", QueryOperator);
                                        builder.AppendFormat("<FieldRef Name='{0}'/>", FieldName);
                                        builder.Append("<Values>");
                                        foreach (var fieldValue in fieldValues) {
                                            builder.AppendFormat("<Value Type='{0}'>{1}</Value>", FieldType, fieldValue);
                                        }
                                        builder.Append("</Values>");
                                        builder.AppendFormat("</{0}>", QueryOperator);
                                    }
                                    break;
                            }
                            return builder.ToString();
                        default:
                            return string.Format("<{0}><FieldRef Name='{1}'/><Value Type='{2}'>{3}</Value></{0}>",
                                QueryOperator, FieldName, FieldType, FieldValue);
                    }
                }
            }
        }

        #region Methods

        protected CAMLQueryBuilder AddFilter(Filter filter) {
            filters.Add(filter);
            return this;
        }

        #region <IsNull>

        public CAMLQueryBuilder AddIsNull(string fieldName) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Text,
                FieldValue = null,
                QueryOperator = FilterQueryOperator.IsNull,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        #endregion

        #region <IsNotNull>

        public CAMLQueryBuilder AddIsNotNull(string fieldName) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Text,
                FieldValue = null,
                QueryOperator = FilterQueryOperator.IsNotNull,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        #endregion

        #region <Contains>

        public CAMLQueryBuilder AddContains(string fieldName, string fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Text,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Contains,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        public CAMLQueryBuilder OrContains(string fieldName, string fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Text,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Contains,
                ChainingOperator = FilterChainingOperator.Or
            });
        }

        #endregion

        #region <Eq>

        public CAMLQueryBuilder AddEqual(string fieldName, int fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Integer,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        public CAMLQueryBuilder AddEqual(string fieldName, string fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Text,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        public CAMLQueryBuilder AddEqual(string fieldName, Guid fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Guid,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        public CAMLQueryBuilder AddEqual(string fieldName, bool fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Boolean,
                FieldValue = fieldValue ? "1" : "0",
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        public CAMLQueryBuilder OrEqual(string fieldName, int fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Integer,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.Or
            });
        }

        public CAMLQueryBuilder OrEqual(string fieldName, string fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Text,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.Or
            });
        }

        public CAMLQueryBuilder OrEqual(string fieldName, Guid fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Guid,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.Or
            });
        }

        public CAMLQueryBuilder OrEqual(string fieldName, bool fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Boolean,
                FieldValue = fieldValue ? "1" : "0",
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.Or
            });
        }

        #region [Me]

        public CAMLQueryBuilder AddCurrentUser(string fieldName) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldValue = "<UserID Type=\"Integer\" />",
                FieldType = FilterFieldType.Integer,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        #endregion

        #region [File]

        public CAMLQueryBuilder AddFileLeafRef(string fieldValue) {
            return AddFilter(new Filter {
                FieldName = "FileLeafRef",
                FieldType = FilterFieldType.File,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Eq,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        #endregion

        #endregion

        #region <In>

        public CAMLQueryBuilder AddIn(string fieldName, Guid[] fieldValues) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Guid,
                FieldValue = Array.ConvertAll(fieldValues, fieldValue => fieldValue.ToString()),
                QueryOperator = FilterQueryOperator.In,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        #region LookupIn

        public CAMLQueryBuilder AddLookupIn(string fieldName, int[] fieldValues) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Lookup,
                FieldValue = Array.ConvertAll(fieldValues, fieldValue => fieldValue.ToString()),
                QueryOperator = FilterQueryOperator.In,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        #endregion

        #endregion
        
        #region Lookup

        public CAMLQueryBuilder AddLookup(string fieldName, int fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Integer,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Lookup,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        public CAMLQueryBuilder AddLookup(string fieldName, string fieldValue) {
            return AddFilter(new Filter {
                FieldName = fieldName,
                FieldType = FilterFieldType.Text,
                FieldValue = fieldValue,
                QueryOperator = FilterQueryOperator.Lookup,
                ChainingOperator = FilterChainingOperator.And
            });
        }

        #endregion

        #endregion

        #endregion

        #region Sortering

        public enum SorterDirection {
            ASC,
            DESC
        }

        /// <summary>
        /// Class that holds sorting expressions that can be used by the <see cref="CAMLQueryBuilder"/>.
        /// </summary>
        protected class Sorter {
            public string FieldName { protected get; set; }
            public SorterDirection Direction { protected get; set; }

            /// <summary>
            /// The sorter expression to use when building a query. 
            /// </summary>
            public string Expression {
                get {
                    string direction = string.Empty;
                    if (Direction == SorterDirection.DESC)
                        direction = "Ascending='FALSE'";
                    return string.Format("<FieldRef Name='{0}' {1}/>", FieldName, direction);
                }
            }
        }

        #region Methods

        protected CAMLQueryBuilder AddSorter(Sorter sorter) {
            sorters.Add(sorter);
            return this;
        }

        public CAMLQueryBuilder AddSorting(string fieldName) {
            return AddSorter(new Sorter() {
                FieldName = fieldName,
                Direction = SorterDirection.ASC
            });
        }

        public CAMLQueryBuilder AddSorting(string fieldName, SorterDirection direction) {
            return AddSorter(new Sorter() {
                FieldName = fieldName,
                Direction = direction
            });
        }

        #endregion

        #endregion

        public string Build() {
            StringBuilder queryString = new StringBuilder();
            if (filters.Count > 0) {
                queryString.Append("<Where>");
                if (filters.Count > 1) {
                    for (var i = filters.Count - 1; i >0 ; i--) {
                        queryString.AppendFormat("<{0}>", filters[i].ChainingOperator);
                    }
                }
                for (var i = 0; i < filters.Count; i++) {
                    queryString.Append(filters[i].Expression);
                    if (i > 0) {
                        queryString.AppendFormat("</{0}>", filters[i].ChainingOperator);
                    }
                }
                queryString.Append("</Where>");
            }
            if (sorters.Count > 0) {
                queryString.Append("<OrderBy>");
                for (int i = 0; i < sorters.Count; i++) {
                    queryString.Append(sorters[i].Expression);
                }
                queryString.Append("</OrderBy>");
            }
            return queryString.ToString();
        }
    }
}