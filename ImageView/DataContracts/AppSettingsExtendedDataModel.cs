using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// AppSettings stored in a settings file instead of registry
    /// </summary>
    [Serializable]
    [DataContract(Name = "AppSettingsExtendedDataModel")]
    public class AppSettingsExtendedDataModel
    {
        public AppSettingsExtendedDataModel()
        {

        }
        /// <summary>
        /// Gets or sets the application settings UUID.
        /// </summary>
        /// <value>
        /// The application settings UUID.
        /// </value>
        [DataMember(Name = "AppSettingsUUID", Order = 1)]
        public Guid AppSettingsUUID { get; set; }

        /// <summary>
        /// Gets or sets the form state list.
        /// </summary>
        /// <value>
        /// The form state list.
        /// </value>
        [DataMember(Name = "FormStateDictionary", Order = 2, IsRequired = true, EmitDefaultValue = true)]
        public Dictionary<string, FormSizeAndPositionModel> FormStateDictionary { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show maximized image area].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show maximized image area]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "BookmarksShowMaximizedImageArea", Order = 3)]
        public bool BookmarksShowMaximizedImageArea { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show overlay window].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show overlay window]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "BookmarksShowOverlayWindow", Order = 4)]
        public bool BookmarksShowOverlayWindow { get; set; }
        
        public void InitFormDictionary()
        {
            if (FormStateDictionary == null)
            {
                FormStateDictionary = new Dictionary<string, FormSizeAndPositionModel>();
            }
        }
    }
}