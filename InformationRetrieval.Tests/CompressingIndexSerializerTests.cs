using System.Collections.Generic;
using System.IO;
using Lab6.IndexCompression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InformationRetrieval.Tests
{
    [TestClass]
    public class CompressingIndexSerializerTests
    {
        private Mock<IStreamFactory> _factoryMock;
        private Mock<INumberEncoder> _encoderMock;
        private Mock<TextWriter> _dictionaryStreamMock;
        private Mock<Stream> _idsStreamMock;
        private Mock<INumberLengthReducer> _numberLengthReducerMock;
        private const string Filepath = "some/file/path";
        private CompressingIndexSerializer _target;

        [TestInitialize]
        public void Initialize()
        {
            _factoryMock = new Mock<IStreamFactory>();
            _dictionaryStreamMock = new Mock<TextWriter>();
            _idsStreamMock = new Mock<Stream>();

            _factoryMock.Setup(
                f => f.CreateDictionaryStreamWriter($"{Filepath}.{CompressingIndexSerializer.DictionaryFileSuffix}"))
                .Returns(_dictionaryStreamMock.Object);
            _factoryMock.Setup(
                f => f.CreateDocumentIdsStream($"{Filepath}.{CompressingIndexSerializer.PostingsFileSuffix}", FileMode.Create))
                .Returns(_idsStreamMock.Object);

            _encoderMock = new Mock<INumberEncoder>();
            _numberLengthReducerMock = new Mock<INumberLengthReducer>();
            _target = new CompressingIndexSerializer(_factoryMock.Object, _encoderMock.Object,
                _numberLengthReducerMock.Object);
        }

        [TestMethod]
        public void
            SerializeToFile_should_write_to_dictionaryStreamWriter_all_keys_from_dictionary_and_write_to_documentIdsStream_all_ids_from_hashSet
            ()
        {
            //Arrange
            var ids = new[] {213, 1, 45};
            var bytes = new byte[] {21, 24, 66};
            _numberLengthReducerMock.Setup(o => o.GetNumbersForEncoding(ids)).Returns(ids);
            _encoderMock.Setup(e => e.EncodeNumbers(ids)).Returns(bytes);
            var testWord = "testString";

            //Act 
            _target.SerializeToFile(Filepath, new Dictionary<string, HashSet<int>>
            {
                [testWord] = new HashSet<int>(ids)
            });

            //Assert
            _dictionaryStreamMock.Verify(o => o.WriteLine(testWord), Times.Once);
            _idsStreamMock.Verify(o => o.Write(bytes, 0, bytes.Length), Times.Once);
        }
    }
}
