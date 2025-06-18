using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Xml.XPath;

namespace System
{
  internal static class Constants
  {
    #region " * Types                                     "

    #region " --> TYPE_NULLABLE                           "
    internal static readonly Type TYPE_NULLABLE = typeof(Nullable<>);
    #endregion

    #region " --> TYPE_NULLABLE_GUID                      "
    internal static readonly Type TYPE_NULLABLE_GUID = typeof(Guid?);
    #endregion

    #region " --> TYPE_NULLABLE_INT16                     "
    internal static readonly Type TYPE_NULLABLE_INT16 = typeof(Int16?);
    #endregion

    #region " --> TYPE_NULLABLE_INT32                     "
    internal static readonly Type TYPE_NULLABLE_INT32 = typeof(Int32?);
    #endregion

    #region " --> TYPE_NULLABLE_INT64                     "
    internal static readonly Type TYPE_NULLABLE_INT64 = typeof(Int64?);
    #endregion

    #region " --> TYPE_NULLABLE_UINT16                    "
    internal static readonly Type TYPE_NULLABLE_UINT16 = typeof(UInt16?);
    #endregion

    #region " --> TYPE_NULLABLE_UINT32                    "
    internal static readonly Type TYPE_NULLABLE_UINT32 = typeof(UInt32?);
    #endregion

    #region " --> TYPE_NULLABLE_UINT64                    "
    internal static readonly Type TYPE_NULLABLE_UINT64 = typeof(UInt64?);
    #endregion

    #region " --> TYPE_NULLABLE_DECIMAL                   "
    internal static readonly Type TYPE_NULLABLE_DECIMAL = typeof(Decimal?);
    #endregion

    #region " --> TYPE_NULLABLE_SINGLE                    "
    internal static readonly Type TYPE_NULLABLE_SINGLE = typeof(Single?);
    #endregion

    #region " --> TYPE_NULLABLE_DOUBLE                    "
    internal static readonly Type TYPE_NULLABLE_DOUBLE = typeof(Double?);
    #endregion

    #region " --> TYPE_NULLABLE_BOOLEAN                   "
    internal static readonly Type TYPE_NULLABLE_BOOLEAN = typeof(Boolean?);
    #endregion

    #region " --> TYPE_NULLABLE_DATEONLY                  "
    internal static readonly Type TYPE_NULLABLE_DATEONLY = typeof(DateOnly?);
    #endregion

    #region " --> TYPE_NULLABLE_DATETIME                  "
    internal static readonly Type TYPE_NULLABLE_DATETIME = typeof(DateTime?);
    #endregion

    #region " --> TYPE_NULLABLE_TIMESPAN                  "
    internal static readonly Type TYPE_NULLABLE_TIMESPAN = typeof(TimeSpan?);
    #endregion

    #region " --> TYPE_NULLABLE_BYTE                      "
    internal static readonly Type TYPE_NULLABLE_BYTE = typeof(Byte?);
    #endregion

    #region " --> TYPE_NULLABLE_RECTANGLE_F               "
    internal static readonly Type TYPE_NULLABLE_RECTANGLE_F = typeof(RectangleF?);
    #endregion

    #region " --> TYPE_DYNAMIC_OBJECT                     "
    internal static readonly Type TYPE_DYNAMIC_OBJECT = typeof(Dynamic.DynamicObject);
    #endregion

    #region " --> TYPE_GENERIC_LIST                       "
    internal static readonly Type TYPE_GENERIC_LIST = typeof(List<>);
    #endregion

    #region " --> TYPE_STRING                             "
    internal static readonly Type TYPE_STRING = typeof(String);
    #endregion

    #region " --> TYPE_MEMORY                             "
    internal static readonly Type TYPE_MEMORY = typeof(ReadOnlyMemory<Char>);
    #endregion

    #region " --> TYPE_GUID                               "
    internal static readonly Type TYPE_GUID = typeof(Guid);
    #endregion

    #region " --> TYPE_INT16                              "
    internal static readonly Type TYPE_INT16 = typeof(Int16);
    #endregion

    #region " --> TYPE_INT32                              "
    internal static readonly Type TYPE_INT32 = typeof(Int32);
    #endregion

    #region " --> TYPE_INT64                              "
    internal static readonly Type TYPE_INT64 = typeof(Int64);
    #endregion

    #region " --> TYPE_UINT16                             "
    internal static readonly Type TYPE_UINT16 = typeof(UInt16);
    #endregion

    #region " --> TYPE_UINT32                             "
    internal static readonly Type TYPE_UINT32 = typeof(UInt32);
    #endregion

    #region " --> TYPE_UINT64                             "
    internal static readonly Type TYPE_UINT64 = typeof(UInt64);
    #endregion

    #region " --> TYPE_DECIMAL                            "
    internal static readonly Type TYPE_DECIMAL = typeof(Decimal);
    #endregion

    #region " --> TYPE_SINGLE                             "
    internal static readonly Type TYPE_SINGLE = typeof(Single);
    #endregion

    #region " --> TYPE_DOUBLE                             "
    internal static readonly Type TYPE_DOUBLE = typeof(Double);
    #endregion

    #region " --> TYPE_BOOLEAN                            "
    internal static readonly Type TYPE_BOOLEAN = typeof(Boolean);
    #endregion

    #region " --> TYPE_DATEONLY                           "
    internal static readonly Type TYPE_DATEONLY = typeof(DateOnly);
    #endregion

    #region " --> TYPE_DATETIME                           "
    internal static readonly Type TYPE_DATETIME = typeof(DateTime);
    #endregion

    #region " --> TYPE_TIMESPAN                           "
    internal static readonly Type TYPE_TIMESPAN = typeof(TimeSpan);
    #endregion

    #region " --> TYPE_TYPE                               "
    internal static readonly Type TYPE_TYPE = typeof(Type);
    #endregion

    #region " --> TYPE_METHOD_INFO                        "
    internal static readonly Type TYPE_METHOD_INFO = typeof(MethodInfo);
    #endregion

    #region " --> TYPE_XPATHNAVIGATOR                     "
    internal static readonly Type TYPE_XPATHNAVIGATOR = typeof(XPathNavigator);
    #endregion

    #region " --> TYPE_BYTE                               "
    internal static readonly Type TYPE_BYTE = typeof(Byte);
    #endregion

    #region " --> TYPE_OBJECT                             "
    internal static readonly Type TYPE_OBJECT = typeof(Object);
    #endregion

    #region " --> TYPE_BYTES                              "
    internal static readonly Type TYPE_BYTES = typeof(Byte[]);
    #endregion

    #region " --> TYPE_ENUM                               "
    internal static readonly Type TYPE_ENUM = typeof(Enum);
    #endregion

    #region " --> TYPE_VERSION                            "
    internal static readonly Type TYPE_VERSION = typeof(Version);
    #endregion

    #region " --> TYPE_RECTANGLE_F                        "
    internal static readonly Type TYPE_RECTANGLE_F = typeof(RectangleF);
    #endregion

    #endregion

    #region " * String-Konstanten                         "

    #region " --> ERR_MSG_ARGUMENT_NULL                   "
    public static readonly String ERR_MSG_ARGUMENT_NULL = Sample.Extensions.Properties.ErrorMessages.ERR_MSG_ARGUMENT_NULL;
    #endregion

    #region " --> ERR_MSG_INVALID_ARGUMENT                "
    public static readonly String ERR_MSG_INVALID_ARGUMENT = Sample.Extensions.Properties.ErrorMessages.ERR_MSG_INVALID_ARGUMENT;
    #endregion

    #region " --> ERR_MSG_TIMEOUT                         "
    public static readonly String ERR_MSG_TIMEOUT = Sample.Extensions.Properties.ErrorMessages.ERR_MSG_TIMEOUT;
    #endregion

    #region " --> ERR_MSG_MISSING_RIGHTS                  "
    public static readonly String ERR_MSG_MISSING_RIGHTS = Sample.Extensions.Properties.ErrorMessages.ERR_MSG_MISSING_RIGHTS;
    #endregion

    #endregion
  }
}
