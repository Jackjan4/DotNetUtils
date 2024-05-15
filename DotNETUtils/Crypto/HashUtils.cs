using System.Security.Cryptography;
using System.Text;

namespace Roslan.DotNETUtils.Crypto
{
    public class HashUtils
    {


        private HashUtils() {

        }


        /// === Removal of Sha256(...) methods === 
        /// Since .NET 5 there is now a static function inside the most common hash function classes that enable easy use so our library functions are not needed anymore
        /// https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1850
        /// CA1850
    }
}
