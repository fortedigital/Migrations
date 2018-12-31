using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

            var ordered = Sort(new[] { m1, m2 });
            Assert.That(ordered, Is.EqualTo(new[] { m1, m2 }));
        }

        [Test]
        public void CompareByDependency()
        {
            var m1 = new DummyMigrationDescriptor("1", "1");
            var m2 = new DummyMigrationDescriptor("2", "2", null, new[] { "1" });

            var ordered = Sort(new[] { m1, m2 });
            Assert.That(ordered, Is.EqualTo(new[] { m1, m2 }));
        }

        [Test]
        public void Compare3ByDependency()
        {
            var m3 = new DummyMigrationDescriptor("3", "3");
            var m2 = new DummyMigrationDescriptor("2", "2", null, new[] { "1" });
            var m1 = new DummyMigrationDescriptor("1", "1", null, new[] { "3" });

            var input = new[] { m3, m2, m1 };
            var ordered = Sort(input);
            Assert.That(ordered, Is.EqualTo(new[] { m3, m1, m2 }));
        }

        private static IEnumerable<MigrationDescriptor> Sort(DummyMigrationDescriptor[] input)
        {
            return new MigrationSorter(input).Sort();
        }
    }
}