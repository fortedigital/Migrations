using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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
        public void CompareByDependencyDoesNotSortAlphabeticallyByGuid()
        {
            var notDependent1 = new DummyMigrationDescriptor("1AB15879-C7AA-441F-B816-6B74A80A4CE4", "Not dependent (first)");
            var dependent1 = new DummyMigrationDescriptor("DA17C20C-AF6B-4788-AB2B-41C46A57A5EC", "Dependent (first)", null, new []{"93FB0A46-1767-433B-9653-5E37C3CB897E"});
            var otherDependsOnThis = new DummyMigrationDescriptor("93FB0A46-1767-433B-9653-5E37C3CB897E", "Other depends on this");
            var notDependent2 = new DummyMigrationDescriptor("60914067-6FCE-4C3F-ACD9-F78D297E04C0", "Not dependent (second)");
            var dependent2 = new DummyMigrationDescriptor("4C8682E1-DDEA-4BB6-918F-D054D954AFEC", "Dependent (second)", null, new []{"93FB0A46-1767-433B-9653-5E37C3CB897E"});

            var notOrderedInput = new[] { notDependent1, dependent1, otherDependsOnThis, notDependent2, dependent2 };
            var ordered = Sort(notOrderedInput).ToList();

            Assert.That(ordered.IndexOf(dependent1), Is.GreaterThan(ordered.IndexOf(otherDependsOnThis)));
            Assert.That(ordered.IndexOf(dependent2), Is.GreaterThan(ordered.IndexOf(otherDependsOnThis)));
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
