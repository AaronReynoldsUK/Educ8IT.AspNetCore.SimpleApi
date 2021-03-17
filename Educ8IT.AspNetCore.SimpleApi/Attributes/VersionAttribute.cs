// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class VersionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiVersion Version { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Deprecated { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        public VersionAttribute(string version)
        {
            Version = ApiVersion.Parse(version);
        }

        /// <summary>
        /// API verions
        /// </summary>
        /// <param name="major">APIs with the same name but different major versions are not interchangeable.</param>
        /// <param name="minor">If the name and major version number on two APIs are the same, 
        ///     but the minor version number is different, 
        ///     this indicates significant enhancement with the intention of backward compatibility.</param>
        /// <param name="build">A difference in build number represents a recompilation of the same source.</param>
        /// <param name="revision">APIs with the same name, major, and minor version numbers but different revisions are intended to be fully interchangeable.</param>
        public VersionAttribute(int major, int minor, int build = 0, int revision = 0)
        {
            Version = new ApiVersion(major, minor, build, revision);
            //Version = ApiVersion.Parse($"{major}.{minor}.{build}.{revision}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    [JsonObject]
    public class ApiVersion
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public int Major { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public int Minor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public int Build { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public int Revision { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VersionText
        {
            get
            {
                return this.ToString();
            }
            set
            {
                if (ApiVersion.TryParse(value, out ApiVersion apiVersion))
                {
                    this.Major = apiVersion.Major;
                    this.Minor = apiVersion.Minor;
                    this.Build = apiVersion.Build;
                    this.Revision = apiVersion.Revision;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ApiVersion() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        public ApiVersion(int major, int minor, int build = 0, int revision = 0)
        {
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
            this.Revision = revision;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        public ApiVersion(Version version)
        {
            this.Major = version.Major;
            this.Minor = version.Minor;
            this.Build = version.Build;
            this.Revision = version.Revision;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionString"></param>
        /// <returns></returns>
        public static ApiVersion Parse(string versionString)
        {
            if (versionString == null)
                throw new ArgumentNullException(nameof(versionString));

            if (Version.TryParse(versionString, out Version version))
            {
                return new ApiVersion(version);
            }
            else
            {
                throw new ArgumentException("Invalid version string provided");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionString"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string? versionString, [NotNullWhen(true)] out ApiVersion? result)
        {
            result = null;

            if (versionString == null)
                return false;

            if (Version.TryParse(versionString, out Version version))
            {
                result = new ApiVersion(version);
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(ApiVersion obj)
        {
            return (this.ToString() == obj.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EqualsSameMajorAndMinor(ApiVersion obj)
        {
            return (this.Major == obj.Major && this.Minor == obj.Minor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiVersion FromMajorAndMinor()
        {
            return new ApiVersion(this.Major, this.Minor);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasBuild
        {
            get { return Build > 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasRevision
        {
            get { return Revision > 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var __versionString = $"{Major}.{Minor}";
            if (Build > 0 || Revision > 0)
                __versionString += $".{Build}";
            if (Revision > 0)
                __versionString += $".{Revision}";

            return __versionString;
        }
    }
}
