// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ThirdParty;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// Common data-type conversion extension methods
    /// </summary>
    public static class Conversion
    {
        /// <summary>
        /// Converts an object to a specific string representation
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToString(object dataIn, EConversionFormat format)
        {
            if (format == EConversionFormat.STRING)
                return dataIn.ToString();
            else
            {
                switch (format)
                {
                    case EConversionFormat.CURRENCY:
                        // We should change this to more globalised
                        var __currency = dataIn.ToDecimal();
                        if (__currency.HasValue)
                            return __currency.Value.ToString("#.##");
                        break;

                    case EConversionFormat.DATE:
                        DateTime? __date = dataIn.ToDate();
                        if (__date.HasValue)
                            return __date.Value.ToDateString();
                        break;

                    case EConversionFormat.DATETIME:
                        DateTime? __dateTime = dataIn.ToDate();
                        if (__dateTime.HasValue)
                            return __dateTime.Value.ToString();
                        break;

                    case EConversionFormat.TIME:
                        DateTime? __time = dataIn.ToDate();
                        if (__time.HasValue)
                            return __time.Value.ToTimeString();
                        break;
                }
            }

            return String.Empty;
        }

        #region Integers

        /// <summary>
        /// Convert numeric object data / strings to Int32 with a fall-back value.
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this object dataIn, Int32 defaultValue)
        {
            Int32 result = defaultValue;

            if (dataIn != null)
            {
                if (dataIn is Decimal dataAsDecimal)
                {
                    result = Convert.ToInt32(dataAsDecimal);
                }
                else if (dataIn is Double dataAsDouble)
                {
                    result = Convert.ToInt32(dataAsDouble);
                }
                else if (dataIn is Single dataAsSingle)
                {
                    result = Convert.ToInt32(dataAsSingle);
                }
                else if (!Int32.TryParse(dataIn.ToString(), out result))
                    result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Convert numeric object data / strings to Int32 or nullable.
        /// </summary>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public static Int32? ToInt32(this object dataIn)
        {
            Int32? result = default;

            if (dataIn != null)
            {
                if (dataIn is Decimal dataAsDecimal)
                {
                    result = Convert.ToInt32(dataAsDecimal);
                }
                else if (dataIn is Double dataAsDouble)
                {
                    result = Convert.ToInt32(dataAsDouble);
                }
                else if (dataIn is Single dataAsSingle)
                {
                    result = Convert.ToInt32(dataAsSingle);
                }
                else if (Int32.TryParse(dataIn.ToString(), out Int32 tmpResult))
                    result = tmpResult;
            }

            return default;
        }

        /// <summary>
        /// Convert numeric object data / strings to Int64 with a fall-back value.
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this object dataIn, Int64 defaultValue)
        {
            Int64 result = defaultValue;

            if (dataIn != null)
            {
                if (dataIn is Decimal dataAsDecimal)
                {
                    result = Convert.ToInt64(dataAsDecimal);
                }
                else if (dataIn is Double dataAsDouble)
                {
                    result = Convert.ToInt64(dataAsDouble);
                }
                else if (dataIn is Single dataAsSingle)
                {
                    result = Convert.ToInt64(dataAsSingle);
                }
                else if (!Int64.TryParse(dataIn.ToString(), out result))
                    result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Convert numeric object data / strings to Int64 or nullable.
        /// </summary>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public static Int64? ToInt64(this object dataIn)
        {
            Int64? result = default;

            if (dataIn != null)
            {
                if (dataIn is Decimal dataAsDecimal)
                {
                    result = Convert.ToInt64(dataAsDecimal);
                }
                else if (dataIn is Double dataAsDouble)
                {
                    result = Convert.ToInt64(dataAsDouble);
                }
                else if (dataIn is Single dataAsSingle)
                {
                    result = Convert.ToInt64(dataAsSingle);
                }
                else if (Int64.TryParse(dataIn.ToString(), out Int64 tmpResult))
                    result = tmpResult;
            }

            return default;
        }

        #endregion

        #region HEX conversion

        /// <summary>
        /// Converts a Nullable Int64 to Hex value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHex(this Int64? value)
        {
            if (value.HasValue)
                return value.Value.ToString("X");
            else
                return null;
        }

        /// <summary>
        /// Converts a Hex value into a Nullable Int64
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns></returns>
        public static Int64? FromHex(this string hexValue)
        {
            try
            {
                return Convert.ToInt64(hexValue, 16);
            }
            catch { }

            return default(Int64?);
        }


        #endregion

        #region Other object/string to number conversions

        /// <summary>
        /// Converts an object/string to Decimal
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object dataIn, Decimal defaultValue)
        {
            Decimal result = defaultValue;

            if (dataIn != null)
                if (!Decimal.TryParse(dataIn.ToString(), out result))
                    result = defaultValue;

            return result;
        }

        /// <summary>
        /// Converts an object/string to a Nullable Decimal
        /// </summary>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public static Decimal? ToDecimal(this object dataIn)
        {
            Decimal? result = default(Decimal?);

            if (dataIn != null)
                if (Decimal.TryParse(dataIn.ToString(), out Decimal tmp))
                    result = tmp;

            return result;
        }

        /// <summary>
        /// Converts an object/string to Double
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Double ToDouble(this object dataIn, Double defaultValue)
        {
            Double result = defaultValue;

            if (dataIn != null)
                if (!Double.TryParse(dataIn.ToString(), out result))
                    result = defaultValue;

            return result;
        }

        /// <summary>
        /// Converts an object/string to a Nullable Double
        /// </summary>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public static Double? ToDouble(this object dataIn)
        {
            Double? result = default(Double?);

            if (dataIn != null)
                if (Double.TryParse(dataIn.ToString(), out Double tmp))
                    result = tmp;

            return result;
        }

        /// <summary>
        /// Converts an object/string to Single
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Single ToSingle(this object dataIn, Single defaultValue)
        {
            Single result = defaultValue;

            if (dataIn != null)
                if (!Single.TryParse(dataIn.ToString(), out result))
                    result = defaultValue;

            return result;
        }

        /// <summary>
        /// Converts an object/string to a Nullable Single
        /// </summary>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public static Single? ToSingle(this object dataIn)
        {
            Single? result = default(Single?);

            if (dataIn != null)
                if (Single.TryParse(dataIn.ToString(), out Single tmp))
                    result = tmp;

            return result;
        }

        #endregion

        #region Boolean conversions

        /// <summary>
        /// Converts an object to a Boolean
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBool(this object dataIn, bool defaultValue)
        {
            bool result = defaultValue;

            if (dataIn != null)
                if (!Boolean.TryParse(dataIn.ToString(), out result))
                    result = defaultValue;

            return result;
        }

        #endregion

        #region Guid conversion

        /// <summary>
        /// Converts a string representation of a Guid to a Guid
        /// </summary>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public static Guid? ToGuid(this string dataIn)
        {
            return Guid.TryParse(dataIn, out Guid guid)
                ? guid : default;
        }

        #endregion

        #region Date Conversion

        /// <summary>
        /// Converts an object to a Nullable DateTime
        /// </summary>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this object dataIn)
        {
            return (dataIn != null)
                ? (DateTime.TryParse(dataIn.ToString(), out DateTime tmp)
                    ? tmp 
                    : default)
                : default;
        }

        /// <summary>
        /// Converts a Date and Time string into a DateTime. Expects the date to be (dd/MM/yyyy).
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? GetDate(string date, string time)
        {
            DateTime? dtOut = default;

            if (String.IsNullOrEmpty(date))
                return dtOut;

            if (String.IsNullOrEmpty(time))
                time = "09:00 AM";

            var enGb = new System.Globalization.CultureInfo("en-GB");

            DateTime dt;
            if (DateTime.TryParseExact(String.Format("{0} {1}", date, time), "dd/MM/yyyy h:mm tt", enGb, System.Globalization.DateTimeStyles.AdjustToUniversal, out dt))
                dtOut = dt;

            return dtOut;
        }

        #endregion

        #region NameValueCollection conversion

        /// <summary>
        /// Converts a <see cref="NameValueCollection"/> to a Dictionary of string, string
        /// </summary>
        /// <param name="nvc"></param>
        /// <param name="handleMultipleValuesPerKey"></param>
        /// <returns></returns>
        public static Dictionary<string, object> NvcToDictionary(this NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values.Length == 1)
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a <see cref="NameValueCollection"/> to an XML-serialisable Dictionary of string, string
        /// </summary>
        /// <param name="nvc"></param>
        /// <param name="handleMultipleValuesPerKey"></param>
        /// <returns></returns>
        public static XmlSerializableDictionary<string, object> NvcToSerializableDictionary(this NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new XmlSerializableDictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values.Length == 1)
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }

            return result;
        }

        #endregion

        #region Parameter Conversion

        public static object ConvertToType(string dataIn, Type destinationType)
        {
            if (destinationType == typeof(Guid?))
                return dataIn.ToGuid();
            else if (destinationType == typeof(Guid))
                return dataIn.ToGuid() ?? Guid.Empty;

            else if (destinationType == typeof(Int64?))
                return dataIn.ToInt64();
            else if (destinationType == typeof(Int64))
                return dataIn.ToInt64(0);

            else if (destinationType == typeof(Int32?))
                return dataIn.ToInt32();
            else if (destinationType == typeof(Int32))
                return dataIn.ToInt32(0);

            else if (destinationType == typeof(Decimal?))
                return dataIn.ToDecimal();
            else if (destinationType == typeof(Decimal))
                return dataIn.ToDecimal(0M);

            else if (destinationType == typeof(Double?))
                return dataIn.ToDouble();
            else if (destinationType == typeof(Double))
                return dataIn.ToDouble(0D);

            else if (destinationType == typeof(Single?))
                return dataIn.ToSingle();
            else if (destinationType == typeof(Single))
                return dataIn.ToSingle(0F);

            //else if (destinationType == typeof(Boolean?))
            //    return dataIn.ToBool(false);
            else if (destinationType == typeof(Boolean))
                return dataIn.ToBool(false);

            else if (destinationType == typeof(String))
                return dataIn;

            else if (destinationType.BaseType == typeof(Enum))
                return Enum.Parse(destinationType, dataIn);

            else return null;
        }

        #endregion
    }
}
