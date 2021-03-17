// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class AdditionalInformationAttribute : Attribute, IAdditionalInformationInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        public AdditionalInformationAttribute(string Text)
        {
            this.Text = Text;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAdditionalInformationInterface
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; }
    }
}
