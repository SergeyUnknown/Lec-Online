﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LecOnline {
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
    internal class MailStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MailStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LecOnline.MailStrings", typeof(MailStrings).Assembly);
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
        ///   Looks up a localized string similar to Для вас был создана учетная запись в LecOnline.ru. 
        ///
        ///Перед тем как начать пользоваться услугами, пожалуйста [установите ваш пароль в системе]({0})
        ///
        ///Если ваш почтовый клиент не распознает ссылки,вы можете скопировать ссылку ниже, и вставить ее в браузер
        ///
        ///{0}.
        /// </summary>
        internal static string MailCreateAccountBody {
            get {
                return ResourceManager.GetString("MailCreateAccountBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Создание учетной записи в LecOnline.ru.
        /// </summary>
        internal static string MailCreateAccountSubject {
            get {
                return ResourceManager.GetString("MailCreateAccountSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Администратор отправил вам ссылку на смену пароля в системе LecOnline.ru.
        ///
        ///Теперь вы можете [изменить ваш пароль в системе]({0})
        ///
        ///Если ваш почтовый клиент не распознает ссылки,вы можете скопировать ссылку ниже, и вставить ее в браузер
        ///
        ///{0}
        ///
        ///Если вы считаете что данное сообщение пришло вам по ошибке, пожалуйста свяжитесь со службой поддержки, отправив
        ///email на адрес support@lec-online.ru.
        /// </summary>
        internal static string ResetPasswordBody {
            get {
                return ResourceManager.GetString("ResetPasswordBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сброс пароля для учетной зависи в LecOnline.ru.
        /// </summary>
        internal static string ResetPasswordSubject {
            get {
                return ResourceManager.GetString("ResetPasswordSubject", resourceCulture);
            }
        }
    }
}
