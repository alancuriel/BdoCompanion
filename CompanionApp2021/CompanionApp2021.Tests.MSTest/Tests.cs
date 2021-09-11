using System;

using CompanionApp2021.ViewModels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanionApp2021.Tests.MSTest
{
    // TODO WTS: Add appropriate tests
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        // TODO WTS: Add tests for functionality you add to SettingsViewModel.
        [TestMethod]
        public void TestSettingsViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new SettingsViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to World_BossesViewModel.
        [TestMethod]
        public void TestWorld_BossesViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new World_BossesViewModel();
            Assert.IsNotNull(vm);
        }
    }
}
