﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LecOnline.Core {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RequestActions {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RequestActions() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LecOnline.Core.RequestActions", typeof(RequestActions).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Файл {0} удален.
        /// </summary>
        internal static string FileDeleted {
            get {
                return ResourceManager.GetString("FileDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Файл {0} загружен.
        /// </summary>
        internal static string FileUploaded {
            get {
                return ResourceManager.GetString("FileUploaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Совещание завершено.
        /// </summary>
        internal static string MeetingFinished {
            get {
                return ResourceManager.GetString("MeetingFinished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Назначено совещание.
        /// </summary>
        internal static string MeetingSet {
            get {
                return ResourceManager.GetString("MeetingSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Совещание начато.
        /// </summary>
        internal static string MeetingStarted {
            get {
                return ResourceManager.GetString("MeetingStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заявка создана.
        /// </summary>
        internal static string RequestCreated {
            get {
                return ResourceManager.GetString("RequestCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Данные заявка изменены.
        /// </summary>
        internal static string RequestInformationUpdate {
            get {
                return ResourceManager.GetString("RequestInformationUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заявка принята на рассмотрение комитетом.
        /// </summary>
        internal static string RequestReviewAccepted {
            get {
                return ResourceManager.GetString("RequestReviewAccepted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заявка отклонена комитетом.
        /// </summary>
        internal static string RequestReviewRejected {
            get {
                return ResourceManager.GetString("RequestReviewRejected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заявка отправлен в комитет.
        /// </summary>
        internal static string RequestSubmitted {
            get {
                return ResourceManager.GetString("RequestSubmitted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Исследование разрешено.
        /// </summary>
        internal static string StudyAccepted {
            get {
                return ResourceManager.GetString("StudyAccepted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Исследование отклонено.
        /// </summary>
        internal static string StudyRejected {
            get {
                return ResourceManager.GetString("StudyRejected", resourceCulture);
            }
        }
    }
}