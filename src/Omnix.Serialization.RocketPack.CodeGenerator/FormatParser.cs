using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Omnix.Serialization.RocketPack.CodeGenerator
{
    public static class Extensions
    {
        // 1行コメントを無視するためのパーサー
        private static Parser<string> SingleLineCommentParser { get; } =
            from leading in Parse.String("//")
            from item in Parse.AnyChar.Except(Parse.LineEnd.Return('\n')).Many().Text()
            select item;

        // 複数行コメントを無視するためのパーサー
        private static Parser<string> MultiLineCommentParser { get; } =
            from leading in Parse.String("/*")
            from item in Parse.AnyChar.Except(Parse.String("*/")).Or(Parse.LineEnd.Return('\n')).Many().Text()
            from trailing in Parse.String("*/")
            select item;

        private static Parser<string> CommentParser { get; } = SingleLineCommentParser.Or(MultiLineCommentParser);

        // コメントと空白行を無視する
        public static Parser<T> TokenWithSkipComment<T>(this Parser<T> parser)
        {
            return from leading in CommentParser.Or(Parse.WhiteSpace.Return("")).Many()
                   from item in parser
                   from trailing in CommentParser.Or(Parse.WhiteSpace.Return("")).Many()
                   select item;
        }
    }

    public static class FormatParser
    {
        public static RocketPackDefinition ParseV1_0(string text)
        {
            var notWhiteSpace = Sprache.Parse.Char(x => !char.IsWhiteSpace(x), "not whitespace");

            // 「"」で囲まれた文字列を抽出するパーサー
            var stringLiteralParser =
                from leading in Parse.WhiteSpace.Many()
                from openQuote in Parse.Char('\"')
                from fragments in Parse.Char('\\').Then(_ => Parse.AnyChar.Select(c => $"\\{c}")).Or(Parse.CharExcept("\\\"").Many().Text()).Many()
                from closeQuote in Parse.Char('\"')
                from trailing in Parse.WhiteSpace.Many()
                select $"{string.Join(string.Empty, fragments)}";

            // 英数字と'_'の文字列を抽出するパーサー
            var nameParser =
                from name in Parse.Char(x => ('0' <= x && x <= '9') || ('A' <= x && x <= 'Z') || ('a' <= x && x <= 'z') || x == '_', "Name").AtLeastOnce().Text()
                select name;

            // example: syntax = v1.0
            var syntaxParser =
                from keyword in Parse.String("syntax").TokenWithSkipComment()
                from equal in Parse.Char('=').TokenWithSkipComment()
                from type in Parse.String("v1.0").TokenWithSkipComment()
                from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
                select type;

            // example: option csharp_namespace = "RocketPack.Messages";
            var optionParser =
                from keyword in Parse.String("option").TokenWithSkipComment()
                from name in nameParser.TokenWithSkipComment().Text()
                from equal in Parse.Char('=').TokenWithSkipComment()
                from value in stringLiteralParser.TokenWithSkipComment()
                from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
                select new OptionDefinition(name, value);

            // example: using "RocketPack.Messages";
            var usingParser =
                from keyword in Parse.String("using").TokenWithSkipComment()
                from value in stringLiteralParser.TokenWithSkipComment()
                from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
                select new UsingDefinition(value);

            // example: namespace "RocketPack.Messages";
            var namespaceParser =
                from keyword in Parse.String("namespace").TokenWithSkipComment()
                from value in stringLiteralParser.TokenWithSkipComment()
                from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
                select new NamespaceDefinition(value);

            // example: [csharp_recyclable]
            var attributeParser =
                from beginTag in Parse.Char('[').TokenWithSkipComment()
                from name in Parse.CharExcept(']').AtLeastOnce().TokenWithSkipComment().Text()
                from endTag in Parse.Char(']').TokenWithSkipComment()
                select name;

            // example: capacity: 1024
            var parametersElementParser =
                from key in nameParser.TokenWithSkipComment().Text()
                from colon in Parse.Char(':').TokenWithSkipComment()
                from value in nameParser.TokenWithSkipComment().Text()
                select (key, value);

            // example: (capacity: 1024, recyclable: true)
            var parametersParser =
                from beginTag in Parse.Char('(').TokenWithSkipComment()
                from elements in parametersElementParser.Many().TokenWithSkipComment()
                from endTag in Parse.Char(')').TokenWithSkipComment()
                select new Dictionary<string, string>(elements.Select(n => new KeyValuePair<string, string>(n.key, n.value)));

            var intTypeParser =
                from isSigned in Parse.Char('u').Then(n => Parse.Return(false)).Or(Parse.Return(true))
                from type in Parse.String("int")
                from size in Parse.Decimal
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new IntType(isSigned, int.Parse(size), isOptional, parameters);

            var boolTypeParser =
                from type in Parse.String("bool").TokenWithSkipComment()
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new BoolType(isOptional, parameters);

            var floatTypeParser =
                from type in Parse.String("float").TokenWithSkipComment()
                from size in Parse.Decimal.TokenWithSkipComment()
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new FloatType(int.Parse(size), isOptional, parameters);

            var stringTypeParser =
                from type in Parse.String("string").TokenWithSkipComment()
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new StringType(isOptional, parameters);

            var timestampTypeParser =
                from type in Parse.String("timestamp")
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new TimestampType(isOptional, parameters);

            var memoryTypeParser =
                from type in Parse.String("memory").TokenWithSkipComment()
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new BytesType(isOptional, parameters);

            var customTypeParser =
                from type in nameParser.Text()
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new CustomType(type, isOptional, parameters);

            var vectorTypeParser =
                from type in Parse.String("vector").TokenWithSkipComment()
                from beginType in Parse.String("<").TokenWithSkipComment()
                from elementType in boolTypeParser
                    .Or<TypeBase>(intTypeParser)
                    .Or(floatTypeParser)
                    .Or(stringTypeParser)
                    .Or(timestampTypeParser)
                    .Or(memoryTypeParser)
                    .Or(customTypeParser).TokenWithSkipComment()
                from endType in Parse.String(">").TokenWithSkipComment()
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new VectorType(elementType, isOptional, parameters);

            var mapTypeParser =
                from type in Parse.String("map").TokenWithSkipComment()
                from beginType in Parse.String("<").TokenWithSkipComment()
                from keyType in boolTypeParser
                    .Or<TypeBase>(intTypeParser)
                    .Or(floatTypeParser)
                    .Or(stringTypeParser)
                    .Or(timestampTypeParser)
                    .Or(memoryTypeParser)
                    .Or(customTypeParser).TokenWithSkipComment()
                from comma_1 in Parse.Char(',').Or(Parse.Return(',')).TokenWithSkipComment()
                from valueType in boolTypeParser
                    .Or<TypeBase>(intTypeParser)
                    .Or(floatTypeParser)
                    .Or(stringTypeParser)
                    .Or(timestampTypeParser)
                    .Or(memoryTypeParser)
                    .Or(customTypeParser).TokenWithSkipComment()
                from endType in Parse.String(">").TokenWithSkipComment()
                from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
                from parameters in parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
                select new MapType(keyType, valueType, isOptional, parameters);

            var enumElementParser =
                from attributes in attributeParser.Many().TokenWithSkipComment()
                from name in nameParser.TokenWithSkipComment().Text()
                from equal in Parse.Char('=').TokenWithSkipComment()
                from id in Parse.Decimal.TokenWithSkipComment()
                from comma in Parse.Char(',').TokenWithSkipComment()
                select new EnumElement(attributes.ToList(), name, int.Parse(id));

            var enumParser =
                from attributes in attributeParser.Many().TokenWithSkipComment()
                from keyword in Parse.String("enum").TokenWithSkipComment()
                from name in nameParser.TokenWithSkipComment().Text()
                from colon in Parse.Char(':').TokenWithSkipComment()
                from type in intTypeParser
                from beginTag in Parse.Char('{').TokenWithSkipComment()
                from enumProperties in enumElementParser.Except(Parse.Char('}')).Many().TokenWithSkipComment()
                from endTag in Parse.Char('}').TokenWithSkipComment()
                select new EnumDefinition(attributes.ToList(), name, type, enumProperties.ToList());

            var smallMessageElementParser =
                from attributes in attributeParser.Many().TokenWithSkipComment()
                from name in nameParser.TokenWithSkipComment().Text()
                from colon in Parse.Char(':').TokenWithSkipComment()
                from type in boolTypeParser
                    .Or<TypeBase>(intTypeParser)
                    .Or(floatTypeParser)
                    .Or(stringTypeParser)
                    .Or(timestampTypeParser)
                    .Or(memoryTypeParser)
                    .Or(vectorTypeParser)
                    .Or(mapTypeParser)
                    .Or(customTypeParser).TokenWithSkipComment()
                from comma in Parse.Char(',').TokenWithSkipComment()
                select new MessageElement(attributes.ToList(), name, type, null);

            var smallMessageParser =
                from attributes in attributeParser.Many().TokenWithSkipComment()
                from keyword in Parse.String("message").TokenWithSkipComment()
                from name in nameParser.TokenWithSkipComment().Text()
                from colon in Parse.Char(':').TokenWithSkipComment()
                from formatType in Parse.String("small").Then(n => Parse.Return(MessageFormatType.Small)).TokenWithSkipComment()
                from beginTag in Parse.Char('{').TokenWithSkipComment()
                from elements in smallMessageElementParser.Except(Parse.Char('}')).Many().TokenWithSkipComment()
                from endTag in Parse.Char('}').TokenWithSkipComment()
                select new MessageDefinition(attributes.ToList(), name, formatType, elements.ToList());

            var mediumMessageElementParser =
                from attributes in attributeParser.Many().TokenWithSkipComment()
                from name in nameParser.TokenWithSkipComment().Text()
                from colon in Parse.Char(':').TokenWithSkipComment()
                from type in boolTypeParser
                    .Or<TypeBase>(intTypeParser)
                    .Or(floatTypeParser)
                    .Or(stringTypeParser)
                    .Or(timestampTypeParser)
                    .Or(memoryTypeParser)
                    .Or(vectorTypeParser)
                    .Or(mapTypeParser)
                    .Or(customTypeParser).TokenWithSkipComment()
                from equal in Parse.Char('=').Or(Parse.Return('=')).TokenWithSkipComment()
                from id in Parse.Decimal.TokenWithSkipComment()
                from comma in Parse.Char(',').TokenWithSkipComment()
                select new MessageElement(attributes.ToList(), name, type, int.Parse(id));

            var mediumMessageParser =
                from attributes in attributeParser.Many().TokenWithSkipComment()
                from keyword in Parse.String("message").TokenWithSkipComment()
                from name in nameParser.TokenWithSkipComment().Text()
                from colon in Parse.Char(':').Or(Parse.Return(':')).TokenWithSkipComment()
                from formatType in Parse.String("medium").Then(n => Parse.Return(MessageFormatType.Medium)).Or(Parse.Return(MessageFormatType.Medium))
                from beginTag in Parse.Char('{').TokenWithSkipComment()
                from elements in mediumMessageElementParser.Except(Parse.Char('}')).Many().TokenWithSkipComment()
                from endTag in Parse.Char('}').TokenWithSkipComment()
                select new MessageDefinition(attributes.ToList(), name, formatType, elements.ToList());

            var formatParser =
                from syntax in syntaxParser.Once().TokenWithSkipComment()
                from usings in usingParser.Many().TokenWithSkipComment()
                from @namespace in namespaceParser.Once().TokenWithSkipComment()
                from options in optionParser.TokenWithSkipComment().Many()
                from contents in enumParser.Or<object>(smallMessageParser).Or(mediumMessageParser).TokenWithSkipComment().Many()
                select new RocketPackDefinition(
                    usings,
                    @namespace.First(),
                    options,
                    contents.OfType<EnumDefinition>().ToList(),
                    contents.OfType<MessageDefinition>().ToList()
                );

            var result = formatParser.Parse(text);

            // Small形式のメッセージはOptional型は認めない。
            foreach (var messageInfo in result.Messages.Where(n => n.FormatType == MessageFormatType.Small))
            {
                if (messageInfo.Elements.Any(n => n.Type.IsOptional))
                {
                    throw new Exception();
                }
            }

            return result;
        }
    }
}
