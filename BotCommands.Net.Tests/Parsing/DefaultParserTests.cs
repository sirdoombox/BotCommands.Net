using BotCommands.Parsing;
using BotCommands.Tests.Contexts;
using Xunit;

namespace BotCommands.Tests.Parsing
{
    public class ParserTests
    {
        private static readonly DefaultParser<TestContext> _parser = new DefaultParser<TestContext>();

        [Theory]
        [InlineData("!test 1 true 1.2345 stringTest")]
        [InlineData("!someOtherTest 10000 false -24.32 otherStringTest")]
        private void ParserShouldCorrectlyParseCorrectStrings(string data)
        {
            var context = new TestContext{Message = data};
            var parserResult = _parser.ParseContext(context, 1);
            var currArg = parserResult.FullArgsStart;
            Assert.Equal(typeof(string), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(int), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(bool), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(double), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(string), currArg.ArgType);
        }
        
        [Theory]
        [InlineData("!test false  1  string    1.3234")]
        [InlineData("! test true 1000 somestring        234.2341")]
        private void ParserShouldCorrectlyParseWhitespaceMangledStrings(string data)
        {
            var context = new TestContext {Message = data};
            var parserResult = _parser.ParseContext(context, 1);
            var currArg = parserResult.FullArgsStart;
            Assert.Equal(typeof(string), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(bool), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(int), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(string), currArg.ArgType);
            currArg = currArg.Next;
            Assert.Equal(typeof(double), currArg.ArgType);
        }
    }
}