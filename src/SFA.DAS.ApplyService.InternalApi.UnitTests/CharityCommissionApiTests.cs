namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    using System;
    using CharityCommissionService;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class CharityCommissionApiTests
    {
        [Test]
        public void Charity_has_a_single_entry_in_registration_history_for_registration()
        {
            var charity = new Charity();
            RegistrationHistory[] history = new RegistrationHistory[1];
            history[0] = new RegistrationHistory
            {
                RegistrationDate = "01/03/2012",
                RegistrationRemovalDate = null,
                RemovalReason = null        
            };
            charity.RegistrationHistory = history;

            charity.RegistrationDate.Should().Be(new DateTime(2012, 3, 1));
            charity.RegistrationRemovalDate.Should().BeNull();
        }

        [Test]
        public void Charity_has_a_single_entry_in_registration_history_for_registration_and_deregistration()
        {
            var charity = new Charity();
            RegistrationHistory[] history = new RegistrationHistory[1];
            history[0] = new RegistrationHistory
            {
                RegistrationDate = "01/03/2012",
                RegistrationRemovalDate = "04/05/2016",
                RemovalReason = null
            };
            charity.RegistrationHistory = history;

            charity.RegistrationDate.Should().Be(new DateTime(2012, 3, 1));
            charity.RegistrationRemovalDate.Should().Be(new DateTime(2016, 5, 4));
        }

        [Test]
        public void Charity_has_entries_for_registration_deregistration_and_another_registration()
        {
            var charity = new Charity();
            RegistrationHistory[] history = new RegistrationHistory[2];
            history[0] = new RegistrationHistory
            {
                RegistrationDate = "01/03/2012",
                RegistrationRemovalDate = "04/05/2016",
                RemovalReason = null
            };
            history[1] = new RegistrationHistory
            {
                RegistrationDate = "01/06/2017",
                RegistrationRemovalDate = null,
                RemovalReason = null
            };
            charity.RegistrationHistory = history;

            charity.RegistrationDate.Should().Be(new DateTime(2017, 6, 1));
            charity.RegistrationRemovalDate.Should().BeNull();
        }
    }
}
