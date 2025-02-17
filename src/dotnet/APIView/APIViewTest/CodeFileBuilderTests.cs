﻿using System;
using ApiView;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace APIViewTest
{
    public class CodeFileBuilderTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CodeFileBuilderTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private Regex _stripRegex = new Regex(@"/\*-\*/(.*?)/\*-\*/", RegexOptions.Singleline);

        public static IEnumerable<object[]> ExactFormattingFiles
        {
            get
            {
                var assembly = typeof(CodeFileBuilderTests).Assembly;
                return assembly.GetManifestResourceNames()
                    .Where(r => r.Contains("ExactFormatting"))
                    .Select(r => new object[] { r })
                    .ToArray();
            }
        }

        [Theory]
        [MemberData(nameof(ExactFormattingFiles))]
        public async Task VerifyFormatted(string name)
        {
            var manifestResourceStream = typeof(CodeFileBuilderTests).Assembly.GetManifestResourceStream(name);
            var streamReader = new StreamReader(manifestResourceStream);
            var code = streamReader.ReadToEnd();
            code = code.Trim(' ', '\t', '\r', '\n');
            var formatted = _stripRegex.Replace(code, string.Empty);
            formatted = RemoveEmptyLines(formatted);
            formatted = formatted.Trim(' ', '\t', '\r', '\n');
            await AssertFormattingAsync(code, formatted);
        }

        private async Task AssertFormattingAsync(string code, string formatted)
        {
            var project = DiagnosticProject.Create(typeof(CodeFileBuilderTests).Assembly, LanguageVersion.Latest, new[] { code });
            project = project.WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var compilation = await project.GetCompilationAsync();
            Assert.Empty(compilation.GetDiagnostics().Where(d => d.Severity > DiagnosticSeverity.Warning));

            var memoryStream = new MemoryStream();
            compilation.Emit(memoryStream);
            memoryStream.Position = 0;

            var compilationFromDll = CompilationFactory.GetCompilation(memoryStream, null);
            var codeModel = new CodeFileBuilder()
            {
                SymbolOrderProvider = new NameSymbolOrderProvider()
            }.Build(compilationFromDll, false, null);
            var formattedModel = new CodeFileRenderer().Render(codeModel);
            var formattedString = string.Join(Environment.NewLine, formattedModel.Select(l => l.DisplayString));
            _testOutputHelper.WriteLine(formattedString);
            Assert.Equal(formatted, formattedString);
        }

        private string RemoveEmptyLines(string content)
        {
            var lines = content
                .Split(Environment.NewLine)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();

            return String.Join(Environment.NewLine, lines);
        }

        public class NameSymbolOrderProvider : ICodeFileBuilderSymbolOrderProvider
        {
            public IEnumerable<T> OrderTypes<T>(IEnumerable<T> symbols) where T : ITypeSymbol
            {
                return symbols.OrderBy(s => s.Name);
            }

            public IEnumerable<ISymbol> OrderMembers(IEnumerable<ISymbol> members)
            {
                return members.OrderBy(s => s.Name);
            }

            public IEnumerable<INamespaceSymbol> OrderNamespaces(IEnumerable<INamespaceSymbol> namespaces)
            {
                return namespaces.OrderBy(s => s.ToDisplayString());
            }
        }
    }
}
