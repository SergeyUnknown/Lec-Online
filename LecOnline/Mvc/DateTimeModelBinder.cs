// -----------------------------------------------------------------------
// <copyright file="DateTimeModelBinder.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <summary>
    /// Model binder for the <see cref="DateTime"/> data type.
    /// </summary>
    public class DateTimeModelBinder : IModelBinder
    {
        /// <summary>
        /// Custom format to use for the formatting.
        /// </summary>
        private readonly string[] customFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeModelBinder"/> class.
        /// </summary>
        /// <param name="customFormat">Custom format to use.</param>
        public DateTimeModelBinder(params string[] customFormat)
        {
            this.customFormat = customFormat;
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller 
        /// context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The bound value.</returns>
        object IModelBinder.BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (string.IsNullOrWhiteSpace(value.AttemptedValue) && bindingContext.ModelMetadata.IsNullableValueType && bindingContext.ModelMetadata.ConvertEmptyStringToNull)
            {
                return null;
            }

            return DateTime.ParseExact(value.AttemptedValue, this.customFormat, CultureInfo.CurrentUICulture, DateTimeStyles.AssumeUniversal);
        }
    }
}
