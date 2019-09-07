using System;

namespace ImageViewer.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IComparable{ImageView.Models.ApplicationVersion}" />
    public class ApplicationVersion : IComparable<ApplicationVersion>, IEquatable<ApplicationVersion>
    {
        /// <summary>
        /// Gets the major.
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        public int Major { get; private set; }
        /// <summary>
        /// Gets the minor.
        /// </summary>
        /// <value>
        /// The minor.
        /// </value>
        public int Minor { get; private set; }
        /// <summary>
        /// Gets the build.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        public int Build { get; private set; }
        /// <summary>
        /// Gets the revistion.
        /// </summary>
        /// <value>
        /// The revistion.
        /// </value>
        public int Revistion { get; private set; }

        /// <summary>
        /// Gets or sets the download URL.
        /// </summary>
        /// <value>
        /// The download URL.
        /// </value>
        public string DownloadUrl { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationVersion"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <exception cref="ArgumentException"></exception>
        public ApplicationVersion(string version)
        {
            var versionSegments = version.Split(".".ToCharArray());
            if (versionSegments.Length != 4)
                throw new ArgumentException();

            Major = int.Parse(versionSegments[0]);
            Minor = int.Parse(versionSegments[1]);
            Build = int.Parse(versionSegments[2]);
            Revistion = int.Parse(versionSegments[3]);
        }

        /// <summary>
        /// Parses the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static ApplicationVersion Parse(string version)
        {
            return new ApplicationVersion(version);
        }

        public static ApplicationVersion Parse(string version, string downloadUrl)
        {
            var appVersion = new ApplicationVersion(version) {DownloadUrl = downloadUrl};
            return appVersion;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int CompareTo(ApplicationVersion other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;
            int minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;
            int buildComparison = Build.CompareTo(other.Build);
            if (buildComparison != 0) return buildComparison;
            return Revistion.CompareTo(other.Revistion);
        }

        public bool Equals(ApplicationVersion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Major == other.Major && Minor == other.Minor && Build == other.Build && Revistion == other.Revistion;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ApplicationVersion) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Major;
                hashCode = (hashCode * 397) ^ Minor;
                hashCode = (hashCode * 397) ^ Build;
                hashCode = (hashCode * 397) ^ Revistion;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revistion}";
        }
    }
}
