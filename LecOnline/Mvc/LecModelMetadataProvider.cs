// -----------------------------------------------------------------------
// <copyright file="LecModelMetadataProvider.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Extended model metadata with support for the help text.
    /// </summary>
    public class LecModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        /// <summary>
        /// Gets the metadata for the specified property.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">The type of the model.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The metadata for the property.</returns>
        protected override ModelMetadata CreateMetadata(
           IEnumerable<Attribute> attributes,
           Type containerType,
           Func<object> modelAccessor,
           Type modelType,
           string propertyName)
        {
            var modelMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            foreach (var attribute in attributes)
            {
                if (attribute is HelpPopupAttribute)
                {
                    var helpPopupAttribute = (HelpPopupAttribute)attribute;
                    modelMetadata.AdditionalValues["Help"] = helpPopupAttribute.GetHelpText();
                }
            }

            return modelMetadata;
        }
    }
}
