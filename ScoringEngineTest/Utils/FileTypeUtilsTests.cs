using FluentAssertions;
using NUnit.Framework;
using ScoringEngine.Utils;

namespace ScoringEngineTest.Utils
{
    [TestFixture]
    public class FileTypeUtilsTests
    {
        private IFileTypeUtils _fileTypeUtils;


        [SetUp]
        public void Setup()
        {
            _fileTypeUtils = new FileTypeUtils();
        }

        [Test]
        public void ShouldReturnTrueForCsvExtensions()
        {
            _fileTypeUtils.ContainsCsvExtension("C:\\somepath\\sometest.csv").ShouldBeEquivalentTo(true);
            _fileTypeUtils.ContainsCsvExtension("C:\\somepath\\sometest.someother.csv").ShouldBeEquivalentTo(true);
        }

        [Test]
        public void ShouldReturnFalseFornonCsvExtensions()
        {
            _fileTypeUtils.ContainsCsvExtension("C:\\somepath\\sometest.tsv").ShouldBeEquivalentTo(false);
            _fileTypeUtils.ContainsCsvExtension("C:\\somepath\\sometest.csv.txt").ShouldBeEquivalentTo(false);
            _fileTypeUtils.ContainsCsvExtension("C:\\somepath\\sometest").ShouldBeEquivalentTo(false);
            _fileTypeUtils.ContainsCsvExtension("C:\\somepath\\csvfile.exe").ShouldBeEquivalentTo(false);
        }
    }
}