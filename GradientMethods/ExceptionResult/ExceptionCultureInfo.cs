using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GradientMethods.ExceptionResult
{
    internal class ExceptionCultureInfo : IEquatable<ExceptionCultureInfo>
    {
        private readonly string ThreeLetterISOLanguageName;
        private readonly string TwoLetterISOLanguageName;

        private ExceptionCultureInfo()
        {

        }

        public ExceptionCultureInfo(CultureInfo culture)
        {
            this.TwoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
            this.ThreeLetterISOLanguageName = culture.ThreeLetterISOLanguageName;
        }

        public bool Equals([AllowNull]ExceptionCultureInfo other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.TwoLetterISOLanguageName.Equals(other.TwoLetterISOLanguageName)
             && this.ThreeLetterISOLanguageName.Equals(other.ThreeLetterISOLanguageName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hashTwoLetterISOLanguageName = this.TwoLetterISOLanguageName == null ? 0 : this.TwoLetterISOLanguageName.GetHashCode();
            int hashThreeLetterISOLanguageName = this.ThreeLetterISOLanguageName == null ? 0 : this.ThreeLetterISOLanguageName.GetHashCode();

            return hashTwoLetterISOLanguageName ^ hashThreeLetterISOLanguageName;
        }
    }
}
