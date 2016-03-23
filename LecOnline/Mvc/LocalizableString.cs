﻿// -----------------------------------------------------------------------
// <copyright file="LocalizableString.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Mvc
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime;
    using LecOnline.Properties;

    /// <summary>
    /// Represents localized strings.
    /// </summary>
    internal class LocalizableString
    {
        /// <summary>
        /// Name of the property which should be localized.
        /// </summary>
        private string propertyName;

        /// <summary>
        /// Gets or sets value of the property to be localized.
        /// </summary>
        private string propertyValue;

        /// <summary>
        /// Gets or sets 
        /// </summary>
        private Type resourceType;

        /// <summary>
        /// Function which returns cached result.
        /// </summary>
        private Func<string> cachedResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableString"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public LocalizableString(string propertyName)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Value
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.propertyValue;
            }

            set
            {
                if (this.propertyValue != value)
                {
                    this.ClearCache();
                    this.propertyValue = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type that contains the resources for the value.
        /// </summary>
        public Type ResourceType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.resourceType;
            }

            set
            {
                if (this.resourceType != value)
                {
                    this.ClearCache();
                    this.resourceType = value;
                }
            }
        }

        /// <summary>
        /// Get localizable values.
        /// </summary>
        /// <returns>Localized value.</returns>
        public string GetLocalizableValue()
        {
            if (this.cachedResult == null)
            {
                if (this.propertyValue == null || this.resourceType == null)
                {
                    this.cachedResult = () => this.propertyValue;
                }
                else
                {
                    PropertyInfo property = this.resourceType.GetProperty(this.propertyValue);
                    bool propertyNotFound = false;
                    if (!this.resourceType.IsVisible || property == null || property.PropertyType != typeof(string))
                    {
                        propertyNotFound = true;
                    }
                    else
                    {
                        MethodInfo getMethod = property.GetGetMethod();
                        if (getMethod == null || !getMethod.IsPublic || !getMethod.IsStatic)
                        {
                            propertyNotFound = true;
                        }
                    }

                    if (propertyNotFound)
                    {
                        string exceptionMessage = string.Format(
                            CultureInfo.CurrentCulture, 
                            Resources.LocalizableString_LocalizationFailed, 
                            this.propertyName,
                            this.resourceType.FullName,
                            this.propertyValue);
                        this.cachedResult = () => { throw new InvalidOperationException(exceptionMessage); };
                    }
                    else
                    {
                        this.cachedResult = () => (string)property.GetValue(null, null);
                    }
                }
            }

            return this.cachedResult();
        }

        /// <summary>
        /// Clear cached result.
        /// </summary>
        private void ClearCache()
        {
            this.cachedResult = null;
        }
    }
}
