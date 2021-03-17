// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Educ8IT.AspNetCore.SimpleApi.Documentation
{
    /// <summary>
    /// 
    /// </summary>
    public static class Annotation
    {
        /// <summary>
        /// 
        /// ... Modify this to support more data annotation attributes.
        /// </summary>
        public static readonly IDictionary<Type, Func<object, string>> AnnotationTextGenerator = new Dictionary<Type, Func<object, string>>
        {
            { typeof(RequiredAttribute), a => "Required" },
            { typeof(RangeAttribute), a =>
                {
                    RangeAttribute range = (RangeAttribute)a;
                    return String.Format(CultureInfo.CurrentCulture, "Range: inclusive between {0} and {1}", range.Minimum, range.Maximum);
                }
            },
            { typeof(MaxLengthAttribute), a =>
                {
                    MaxLengthAttribute maxLength = (MaxLengthAttribute)a;
                    return String.Format(CultureInfo.CurrentCulture, "Max length: {0}", maxLength.Length);
                }
            },
            { typeof(MinLengthAttribute), a =>
                {
                    MinLengthAttribute minLength = (MinLengthAttribute)a;
                    return String.Format(CultureInfo.CurrentCulture, "Min length: {0}", minLength.Length);
                }
            },
            { typeof(StringLengthAttribute), a =>
                {
                    StringLengthAttribute strLength = (StringLengthAttribute)a;
                    return String.Format(CultureInfo.CurrentCulture, "String length: inclusive between {0} and {1}", strLength.MinimumLength, strLength.MaximumLength);
                }
            },
            { typeof(DataTypeAttribute), a =>
                {
                    DataTypeAttribute dataType = (DataTypeAttribute)a;
                    return String.Format(CultureInfo.CurrentCulture, "Data type: {0}", dataType.CustomDataType ?? dataType.DataType.ToString());
                }
            },
            { typeof(RegularExpressionAttribute), a =>
                {
                    RegularExpressionAttribute regularExpression = (RegularExpressionAttribute)a;
                    return String.Format(CultureInfo.CurrentCulture, "Matching regular expression pattern: {0}", regularExpression.Pattern);
                }
            },
            {
                typeof(AdditionalInformationAttribute), a =>
                {
                    AdditionalInformationAttribute additionalInformationAttribute = (AdditionalInformationAttribute)a;
                    return additionalInformationAttribute.Text;
                }
            },
            {
                typeof (ApiPropertyAttribute), a =>
                {
                     ApiPropertyAttribute apiPropertyAttribute = (ApiPropertyAttribute)a;
                    return apiPropertyAttribute.DeprecationAdvice;
                }
            },
            {
                typeof(DefaultValueAttribute), a=>
                {
                    DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)a;
                    return String.Format("Default Value = {0}", defaultValueAttribute.Value);
                }
            }
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public static void GenerateAnnotations(this ApiParameterItem parameter)
        {
            List<ParameterAnnotation> annotations = new List<ParameterAnnotation>();

            IEnumerable<Attribute> attributes = parameter.ParameterInfo.GetCustomAttributes();
            foreach (Attribute attribute in attributes)
            {
                Func<object, string> textGenerator;
                if (AnnotationTextGenerator.TryGetValue(attribute.GetType(), out textGenerator))
                {
                    annotations.Add(
                        new ParameterAnnotation
                        {
                            AnnotationAttribute = attribute,
                            Documentation = textGenerator(attribute)
                        });
                }
            }

            // Rearrange the annotations
            annotations.Sort((x, y) =>
            {
                // Special-case RequiredAttribute so that it shows up on top
                if (x.AnnotationAttribute is RequiredAttribute)
                {
                    return -1;
                }
                if (y.AnnotationAttribute is RequiredAttribute)
                {
                    return 1;
                }

                // Sort the rest based on alphabetic order of the documentation
                return String.Compare(x.Documentation, y.Documentation, StringComparison.OrdinalIgnoreCase);
            });

            foreach (ParameterAnnotation annotation in annotations)
            {
                if (!String.IsNullOrEmpty(annotation.Documentation))
                    parameter.Annotations.Add(annotation);
            }
        }
    }
}
