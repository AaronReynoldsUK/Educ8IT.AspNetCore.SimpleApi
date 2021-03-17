// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    public static class Formatting
    {
        #region Number to String formatting

        private static NumberFormatInfo NumberFormat = null;

        /// <summary>
        /// Formats a Double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nfi">can be null to use standard formatting</param>
        /// <param name="withGroupSeparator"></param>
        /// <returns></returns>
        public static string ToNumberFormat(this Double value, NumberFormatInfo nfi, bool withGroupSeparator)
        {
            if (nfi == null)
            {
                if (NumberFormat == null)
                {
                    NumberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    NumberFormat.NumberDecimalDigits = 2;
                    NumberFormat.NumberDecimalSeparator = ".";
                    NumberFormat.NumberGroupSeparator = withGroupSeparator ? "," : "";
                }
                nfi = NumberFormat;
            }

            string result = value.ToString("N", nfi);
            return result;
        }

        /// <summary>
        /// Formats a Double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nfi">can be null to use standard formatting</param>
        /// <param name="withGroupSeparator"></param>
        /// <returns></returns>
        public static string ToNumberFormat(this Decimal value, NumberFormatInfo nfi, bool withGroupSeparator)
        {
            var result = "";

            if (nfi == null)
            {
                if (NumberFormat == null)
                {
                    NumberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    NumberFormat.NumberDecimalDigits = 2;
                    NumberFormat.NumberDecimalSeparator = ".";
                    NumberFormat.NumberGroupSeparator = withGroupSeparator ? "," : "";
                }
                nfi = NumberFormat;
            }

            result = value.ToString("N", nfi);

            return result;
        }

        #endregion

        #region Date and Time formatting

        /// <summary>
        /// Formats a DateTime to a date string (dd/MM/yyyy)
        /// </summary>
        /// <param name="dtIn"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dtIn)
        {
            return dtIn.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Formats a DateTime to a date and time string (dd/MM/yyyy HH:mm)
        /// </summary>
        /// <param name="dtIn"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime dtIn)
        {
            return dtIn.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Formats a DateTime to a 12 hour time string (hh:mm tt)
        /// </summary>
        /// <param name="dtIn"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime dtIn)
        {
            return dtIn.ToString("hh:mm tt");
        }

        /// <summary>
        /// Formats a DateTime to a JSON DateTime string
        /// </summary>
        /// <param name="dtIn"></param>
        /// <returns></returns>
        public static string ToJsonDateTimeString(this DateTime dtIn)
        {
            return dtIn.ToString("yyyy-MM-ddTHH:mm:ssK");
        }

        #endregion

        #region File size formatting

        /// <summary>
        /// Formats file size e.g. "4.3TB"
        /// </summary>
        /// <param name="fileSizeInBytes"></param>
        /// <returns></returns>
        public static string ToFileSize(this long fileSizeInBytes)
        {
            long __kb = 1024;
            long __mb = 1024 * __kb;
            long __gb = 1024 * __mb;
            long __tb = 1024 * __gb;

            if (fileSizeInBytes / __tb > 1)
                return (fileSizeInBytes / __tb).ToString("0.#") + " TB";
            else if (fileSizeInBytes / __gb > 1)
                return (fileSizeInBytes / __gb).ToString("0.#") + " GB";
            else if (fileSizeInBytes / __mb > 1)
                return (fileSizeInBytes / __mb).ToString("0.#") + " MB";
            else if (fileSizeInBytes / __kb > 1)
                return (fileSizeInBytes / __kb).ToString("0.#") + " KB";
            else
                return fileSizeInBytes + " bytes";
        }

        #endregion

        #region Exception formatting

        /// <summary>
        /// Converts an Exception hierarchy into a HTML representation
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ToHtml(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div><h1>Exception</h1>\r\n");

            sb.AppendFormat("<div><b>Message:</b> {0}</div>\r\n",
                ex.Message);

            sb.AppendFormat("<div><b>Source:</b> {0}</div>\r\n",
                ex.Source ?? "None");

            sb.AppendFormat("<div><b>Help Link:</b> {0}</div>\r\n",
                ex.HelpLink ?? "None");

            sb.AppendFormat("<div><b>Stack Trace:</b> {0}</div>\r\n",
                ex.StackTrace ?? "None");

            sb.AppendFormat("<div><b>Target:</b> {0}</div>\r\n",
                (ex.TargetSite != null) ? ex.TargetSite.Name : "None");

            if (ex.Data != null)
            {
                sb.AppendFormat("<div><b>Data:</b><br />\r\n");

                foreach (DictionaryEntry de in ex.Data)
                    sb.AppendFormat("<b>{0}</b>={1}<br />\r\n",
                        de.Key, de.Value);

                sb.Append("</div>\r\n");
            }

            if (ex.InnerException != null && ex.InnerException.Message != ex.Message)
                sb.AppendFormat("<div><b>Inner Exception:</b><br />\r\n{0}</div>\r\n",
                    ex.InnerException.ToHtml());

            sb.Append("</div>\r\n");

            return sb.ToString();
        }

        #endregion

        #region NameValueCollection formatting


        /// <summary>
        /// Converts a NameValueCollection into a HTML representation
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string ToHtml(this NameValueCollection collection)
        {
            return collection.ToHtml("Generic Collection");
        }

        /// <summary>
        /// Converts a NameValueCollection into a named HTML representation
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static string ToHtml(this NameValueCollection collection, string collectionName)
        {
            if (collection == null)
                return null;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<div><h1>{0}</h1>\r\n", collectionName);

            foreach (string key in collection.Keys)
                sb.AppendFormat("<b>{0}</b>={1}<br />\r\n",
                    key, collection[key]);

            sb.Append("</div>\r\n");

            return sb.ToString();
        }

        #endregion
    }
}
