using ServerLibrary.Handlers;
using ServerLibrary.Parser;
using ServerLibrary.Statements;
using ServerLibrary.Storage.Write;
using ServerLibrary.Storage.Write.ToDisk;

namespace ServerLibrary.Tests
{
    [TestFixture]
    internal class HandlerTests
    {
        IBinaryWriter BinaryWriter { get; set; }


        [SetUp]
        public void SetUp() 
        {
            BinaryWriter = new ToDiskBinaryWriter();
        }

        //TODO: rename after these tests become meaningful
        [Test]
        public async Task HandlerTest1()
        {
            string CreatTableQuery = File.ReadAllText("CreateTableTests.txt");
            var statement = QueryTypeCalculater.DetermineQueryType(CreatTableQuery);

     
            if (statement.TryPickT4(out var createTableSQL, out _))
            {
                CreateTableHandler createTableHandler = new CreateTableHandler(createTableSQL, BinaryWriter);
                await createTableHandler.ExecuteStatement();
            }
        }

        [TearDown]
        public void TearDown() 
        {
            BinaryWriter.Dispose();
        }
    }
}
