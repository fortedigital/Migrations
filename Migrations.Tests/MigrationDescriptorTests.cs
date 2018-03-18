using NUnit.Framework;

namespace Forte.Migrations.Tests
{
    [TestFixture]
    public class MigrationDescriptorTests
    {
        [Test]
        public void CompareBySequenceNo()
        {
            var m1 = new DummyMigrationDescriptor("1", "1", 1);
            var m2 = new DummyMigrationDescriptor("2", "2", 2);

            Assert.That(m1.CompareTo(m2), Is.LessThan(0));
            Assert.That(m2.CompareTo(m1), Is.GreaterThan(0));
        }

        [Test]
        public void CompareByDependency()
        {
            var m1 = new DummyMigrationDescriptor("1", "1");
            var m2 = new DummyMigrationDescriptor("2", "2", null, new [] { "1" });

            Assert.That(m1.CompareTo(m2), Is.LessThan(0));
            Assert.That(m2.CompareTo(m1), Is.GreaterThan(0));
        }        
    }
}