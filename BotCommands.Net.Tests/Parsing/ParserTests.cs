using BotCommands.Parsing;
using BotCommands.Tests.Contexts;
using Xunit;

namespace BotCommands.Tests.Parsing
{
    public class ParserTests
    {
        private static readonly Parser<TestContext> _parser = new Parser<TestContext>();

        [Theory]
        [InlineData("!test 1 true 1.2345 stringTest")]
        [InlineData("!someOtherTest 10000 false -24.32 otherStringTest")]
        private void ParserShouldCorrectlyParseCorrectStrings(string data)
        {
            var context = new TestContext{Message = data};
            var parserResult = _parser.ParseContext(context, 1);
            var currArg = parserResult.FullArgsStart;
            Assert.Equal(currArg.ArgType, typeof(string));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(int));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(bool));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(double));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(string));
        }
        
        [Theory]
        [InlineData("!test false  1  string    1.3234")]
        [InlineData("! test true 1000 somestring        234.2341")]
        private void ParserShouldCorrectlyParseWhitespaceMangledStrings(string data)
        {
            var context = new TestContext {Message = data};
            var parserResult = _parser.ParseContext(context, 1);
            var currArg = parserResult.FullArgsStart;
            Assert.Equal(currArg.ArgType, typeof(string));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(bool));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(int));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(string));
            currArg = currArg.Next;
            Assert.Equal(currArg.ArgType, typeof(double));
        }
    }
}