﻿using FluentAssertions;

using NUnit.Framework;

namespace ICSharpCode.ILSpy.Tests
{
	[TestFixture]
	public class CommandLineArgumentsTests
	{
		[Test]
		public void VerifyEmptyArgumentsArray()
		{
			var cmdLineArgs = new CommandLineArguments(new string[] { });

			cmdLineArgs.AssembliesToLoad.Should().BeEmpty();
			cmdLineArgs.SingleInstance.Should().BeNull();
			cmdLineArgs.NavigateTo.Should().BeNull();
			cmdLineArgs.Search.Should().BeNull();
			cmdLineArgs.Language.Should().BeNull();
			cmdLineArgs.NoActivate.Should().BeFalse();
			cmdLineArgs.ConfigFile.Should().BeNull();
		}

		[Test]
		public void VerifySeparateOption()
		{
			var cmdLineArgs = new CommandLineArguments(new string[] { "--instancing", "Multi" });
			cmdLineArgs.SingleInstance.Should().BeFalse();
		}

		[Test]
		public void VerifySingleInstanceOption()
		{
			var cmdLineArgs = new CommandLineArguments(new string[] { "--instancing", "Single" });
			cmdLineArgs.SingleInstance.Should().BeTrue();
		}

		[Test]
		public void VerifyNavigateToOption()
		{
			const string navigateTo = "MyNamespace.MyClass";
			var cmdLineArgs = new CommandLineArguments(new string[] { "--navigateto", navigateTo });
			cmdLineArgs.NavigateTo.Should().BeEquivalentTo(navigateTo);
		}

		[Test]
		public void VerifySearchOption()
		{
			const string searchWord = "TestContainers";
			var cmdLineArgs = new CommandLineArguments(new string[] { "--search", searchWord });
			cmdLineArgs.Search.Should().BeEquivalentTo(searchWord);
		}

		[Test]
		public void VerifyLanguageOption()
		{
			const string language = "csharp";
			var cmdLineArgs = new CommandLineArguments(new string[] { "--language", language });
			cmdLineArgs.Language.Should().BeEquivalentTo(language);
		}

		[Test]
		public void VerifyConfigOption()
		{
			const string configFile = "myilspyoptions.xml";
			var cmdLineArgs = new CommandLineArguments(new string[] { "--config", configFile });
			cmdLineArgs.ConfigFile.Should().BeEquivalentTo(configFile);
		}

		[Test]
		public void VerifyNoActivateOption()
		{
			var cmdLineArgs = new CommandLineArguments(new string[] { "--noactivate" });
			cmdLineArgs.NoActivate.Should().BeTrue();
		}

		[Test]
		public void MultipleAssembliesAsArguments()
		{
			var cmdLineArgs = new CommandLineArguments(new string[] { "assembly1", "assembly2", "assembly3" });
			cmdLineArgs.AssembliesToLoad.Should().HaveCount(3);
		}

		[Test]
		public void PassAtFileArgumentsSpaceSeparated()
		{
			string filepath = System.IO.Path.GetTempFileName();

			System.IO.File.WriteAllText(filepath, "assembly1 assembly2 assembly3 --instancing multi --noactivate");

			var cmdLineArgs = new CommandLineArguments(new string[] { $"@{filepath}" });

			cmdLineArgs.SingleInstance.Should().BeFalse();
			cmdLineArgs.NoActivate.Should().BeTrue();
			cmdLineArgs.AssembliesToLoad.Should().HaveCount(3);
		}
	}
}
