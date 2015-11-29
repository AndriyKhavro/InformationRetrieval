using System.Collections.Generic;
using System.IO;
using Lab6.IndexCompression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace InformationRetrieval.Tests
{
    [TestClass]
    public class CompressingIndexSerializerTestsRhino
    {
        private IStreamFactory _factoryStub;
        private INumberEncoder _encoderStub;
        private TextWriter _textWriterMock;
        private Stream _streamMock;
        private INumberLengthReducer _numberLengthReducerStub;
        private const string Filepath = "some/file/path";
        private CompressingIndexSerializer _target;

        [TestInitialize]
        public void Initialize()
        {
            _factoryStub = MockRepository.GenerateStub<IStreamFactory>();
            _textWriterMock = MockRepository.GenerateMock<TextWriter>();
            _streamMock = MockRepository.GenerateMock<Stream>();

            _factoryStub.Stub(f => f.CreateDictionaryStreamWriter($"{Filepath}.{CompressingIndexSerializer.DictionaryFileSuffix}"))
                .Return(_textWriterMock);
            _factoryStub.Stub(f => f.CreateDocumentIdsStream($"{Filepath}.{CompressingIndexSerializer.PostingsFileSuffix}", FileMode.Create)).Return(_streamMock);

            _encoderStub = MockRepository.GenerateStub<INumberEncoder>();
            _numberLengthReducerStub = MockRepository.GenerateStub<INumberLengthReducer>();
            _target = new CompressingIndexSerializer(_factoryStub, _encoderStub, _numberLengthReducerStub);
        }

        [TestMethod]
        public void
            RhinoMocks_SerializeToFile_should_write_to_streamWriter_all_keys_from_dictionary_and_write_to_fileStream_all_postings_from_hashSet
            ()
        {
            //Arrange
            var numbers = new[] {213, 1, 45};
            var bytes = new byte[] {21, 24, 66};
            _numberLengthReducerStub.Stub(o => o.GetNumbersForEncoding(Arg<IEnumerable<int>>.Is.Anything)).Return(numbers);
            _encoderStub.Stub(e => e.EncodeNumbers(numbers)).Return(bytes);

            //Act 
            var testString = "testString";
            _target.SerializeToFile(Filepath, new Dictionary<string, HashSet<int>>
            {
                [testString] = new HashSet<int>(numbers)
            });

            //Assert
            _textWriterMock.Expect(o => o.WriteLine(testString));
            _streamMock.Expect(o => o.Write(bytes, 0, bytes.Length));
        }
    }
}
