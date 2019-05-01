﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Wyam.Common.Configuration;
using Wyam.Common.Execution;
using Wyam.Core.Execution;
using Wyam.Core.Modules.Control;
using Wyam.Testing;
using Wyam.Testing.Execution;
using Wyam.Testing.Modules;

namespace Wyam.Core.Tests.Modules.Control
{
    [TestFixture]
    [NonParallelizable]
    public class SwitchFixture : BaseFixture
    {
        public class ExecuteTests : SwitchFixture
        {
            [Test]
            public async Task SwitchResultsInCorrectCounts()
            {
                // Given
                CountModule a = new CountModule("A") { AdditionalOutputs = 2 };
                CountModule b = new CountModule("B");
                CountModule c = new CountModule("C");
                CountModule d = new CountModule("D");
                Switch switchModule = new Switch(Config.FromDocument(x => (object)x.Content)).Case("1", b).Case("2", c).Default(d);

                // When
                await ExecuteAsync(a, switchModule);

                // Then
                Assert.AreEqual(1, a.ExecuteCount);
                Assert.AreEqual(1, b.ExecuteCount);
                Assert.AreEqual(1, c.ExecuteCount);
                Assert.AreEqual(1, d.ExecuteCount);
            }

            [Test]
            public async Task SwitchNoCasesResultsInCorrectCounts()
            {
                // Given
                CountModule a = new CountModule("A") { AdditionalOutputs = 2 };
                CountModule b = new CountModule("B");
                CountModule c = new CountModule("C");
                Switch switchModule = new Switch(Config.FromDocument(x => (object)x.Content)).Default(b);

                // When
                await ExecuteAsync(a, switchModule, b);

                // Then
                Assert.AreEqual(1, a.ExecuteCount);
                Assert.AreEqual(1, b.ExecuteCount);
                Assert.AreEqual(3, b.InputCount);
                Assert.AreEqual(3, b.OutputCount);
                Assert.AreEqual(3, c.InputCount);
            }

            [Test]
            public async Task MissingDefaultResultsInCorrectCounts()
            {
                // Given
                CountModule a = new CountModule("A") { AdditionalOutputs = 2 };
                CountModule b = new CountModule("B");
                CountModule c = new CountModule("C");
                Switch switchModule = new Switch(Config.FromDocument(x => (object)x.Content)).Case("1", b);

                // When
                await ExecuteAsync(a, switchModule, c);

                // Then
                Assert.AreEqual(1, a.ExecuteCount);
                Assert.AreEqual(1, b.ExecuteCount);
                Assert.AreEqual(1, b.InputCount);
                Assert.AreEqual(1, b.OutputCount);
                Assert.AreEqual(3, c.InputCount);
            }

            [Test]
            public async Task ArrayInCaseResultsInCorrectCounts()
            {
                // Given
                CountModule a = new CountModule("A") { AdditionalOutputs = 2 };
                CountModule b = new CountModule("B");
                CountModule c = new CountModule("C");
                Switch switchModule = new Switch(Config.FromDocument(x => (object)x.Content)).Case(new string[] { "1", "2" }, b);

                // When
                await ExecuteAsync(a, switchModule, c);

                // Then
                Assert.AreEqual(1, a.ExecuteCount);
                Assert.AreEqual(1, b.ExecuteCount);
                Assert.AreEqual(2, b.InputCount);
                Assert.AreEqual(2, b.OutputCount);
                Assert.AreEqual(3, c.InputCount);
            }

            [Test]
            public async Task OmittingCasesAndDefaultResultsInCorrectCounts()
            {
                // Given
                CountModule a = new CountModule("A") { AdditionalOutputs = 2 };
                CountModule b = new CountModule("B");
                Switch switchModule = new Switch(Config.FromDocument(x => (object)x.Content));

                // When
                await ExecuteAsync(a, switchModule, b);

                // Then
                Assert.AreEqual(3, b.InputCount);
            }
        }
    }
}
