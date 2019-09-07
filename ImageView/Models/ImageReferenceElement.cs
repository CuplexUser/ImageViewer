using System;
using ImageViewer.DataContracts;

namespace ImageViewer.Models
{
    public class ImageReferenceElement : ImageReference, IComparable<ImageReferenceElement>, IEquatable<ImageReferenceElement>
    {
        public string SizeInKb => Math.Round(Size/1024d, 1) + " kB";

        public int CompareTo(ImageReferenceElement other)
        {
            return string.CompareOrdinal(CompletePath, other.CompletePath);
        }

        public bool Equals(ImageReferenceElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Directory, other.Directory) && string.Equals(FileName, other.FileName) &&
                   string.Equals(CompletePath, other.CompletePath) && Size == other.Size &&
                   CreationTime.Equals(other.CreationTime) &&
                   LastWriteTime.Equals(other.LastWriteTime) && LastAccessTime.Equals(other.LastAccessTime);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Directory?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (FileName?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (CompletePath?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Size.GetHashCode();
                hashCode = (hashCode*397) ^ CreationTime.GetHashCode();
                hashCode = (hashCode*397) ^ LastWriteTime.GetHashCode();
                hashCode = (hashCode*397) ^ LastAccessTime.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ImageReferenceElement) obj);
        }

        public static bool operator ==(ImageReferenceElement c1, ImageReferenceElement c2)
        {
            if (ReferenceEquals(c1, c2))
                return true;

            if (((object) c1 == null) || ((object) c2 == null))
                return false;

            return c1.Equals(c2);
        }

        public static bool operator !=(ImageReferenceElement c1, ImageReferenceElement c2)
        {
            return !(c1 == c2);
        }
    }
}