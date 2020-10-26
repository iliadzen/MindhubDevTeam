using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappened.Tests
{
    [TestFixture]
    public class Sha256HasherTests
    {
        [Test]
        public void MakeSaltedHash_EmptySource_HashLength()
        {
            // Arrange
            IHasher hasher = new Sha256Hasher();
            string source = "";
            int expectedHashLength = 88;

            // Act
            int actualHashLength = hasher.MakeSaltedHash(source).Length;
            
            // Assert
            Assert.AreEqual(expectedHashLength, actualHashLength);
        }
        
        [Test]
        public void MakeSaltedHash_LongSource_HashLength()
        {
            // Arrange
            IHasher hasher = new Sha256Hasher();
            string source = "I can write anything I want here and the test will pass because all SHA256 hashes have the same length";
            int expectedHashLength = 88;

            // Act
            int actualHashLength = hasher.MakeSaltedHash(source).Length;
            
            // Assert
            Assert.AreEqual(expectedHashLength, actualHashLength);
        }
        
        [Test]
        public void VerifySaltedHash_FixedMatching_Verified()
        {
            // Arrange
            IHasher hasher = new Sha256Hasher();
            string source = "I like ya cut, G";
            string hash = "PZkwO23xVpMz/MB2zolkH1SfvLx45XD/NHYKXBBHLvK+pVqPtQKE1CJ4qXU92abingRURSMmv0WVQ5M60FGCGQ==";
            
            // Act
            bool isVerified = hasher.VerifySaltedHash(source, hash);
            
            // Assert
            Assert.IsTrue(isVerified);
        }
        
        [Test]
        public void VerifySaltedHash_FixedMismatching_NotVerified()
        {
            // Arrange
            IHasher hasher = new Sha256Hasher();
            string source = "This phrase is anything but 'I like ya cut, G'";
            string hash = "PZkwO23xVpMz/MB2zolkH1SfvLx45XD/NHYKXBBHLvK+pVqPtQKE1CJ4qXU92abingRURSMmv0WVQ5M60FGCGQ==";
            
            // Act
            bool isVerified = hasher.VerifySaltedHash(source, hash);
            
            // Assert
            Assert.IsFalse(isVerified);
        }
        
        [Test]
        public void VerifySaltedHash_ComputedMatching_Verified()
        {
            // Arrange
            IHasher hasher = new Sha256Hasher();
            string source = "s3cr37p455w0rd!7ru57n01";
            string hash = hasher.MakeSaltedHash(source);
            
            // Act
            bool isVerified = hasher.VerifySaltedHash(source, hash);
            
            // Assert
            Assert.IsTrue(isVerified);
        }
        
        [Test]
        public void VerifySaltedHash_ComputedMismatching_NotVerified()
        {
            // Arrange
            IHasher hasher = new Sha256Hasher();
            string source = "s3cr37p455w0rd!7ru57n01";
            string hash = hasher.MakeSaltedHash(source);
            hash = (hash[1] == 'A' ? 'B' : 'A') + hash.Substring(1, hash.Length - 1); // Corrupt the hash

            // Act
            bool isVerified = hasher.VerifySaltedHash(source, hash);
            
            // Assert
            Assert.IsFalse(isVerified);
        }
    }
}