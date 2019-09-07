using System;
using System.Reflection;

namespace ImageViewer.Models
{
    /// <summary>
    /// AppSettingsInRegistry
    /// </summary>
    public class RegistryAppSettings
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName { get; set; }
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the install date.
        /// </summary>
        /// <value>
        /// The install date.
        /// </value>
        public string InstallDate { get; set; }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="productName">Name of the product.</param>
        /// <param name="companyName">Name of the company.</param>
        /// <returns></returns>
        public static RegistryAppSettings CreateNew(string productName, string companyName)
        {
            var regAppSettings = new RegistryAppSettings
            {
                ProductName = productName, 
                InstallDate = DateTime.Now.ToLongDateString(),
                CompanyName = companyName,
                Version = Assembly.GetCallingAssembly().GetName().Version.ToString()
            };

            return regAppSettings;
        }
    }
}