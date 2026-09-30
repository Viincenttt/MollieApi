using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Mollie.Api.Analyzers;
using Mollie.Api.Models;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Analyzers {
    public class DiscardedMollieResultAnalyzerTests {
        [Theory]
        [InlineData("await client.CancelPaymentAsync(\"tr_123\");")]
        [InlineData("await client.GetPaymentAsync(\"tr_123\");")]
        [InlineData("await client.CancelPaymentAsync(\"tr_123\").ConfigureAwait(false);")]
        [InlineData("client.CancelPaymentAsync(\"tr_123\").GetAwaiter().GetResult();")]
        [InlineData("await task;")]
        public async Task Analyze_ResultIsNotUsed_ReportsDiagnostic(string statement) {
            // Arrange
            string source = CreateSource(statement);

            // Act
            ImmutableArray<Diagnostic> diagnostics = await GetDiagnosticsAsync(source);

            // Assert
            Diagnostic diagnostic = diagnostics.ShouldHaveSingleItem();
            diagnostic.Id.ShouldBe(DiscardedMollieResultAnalyzer.DiagnosticId);
            diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        }

        [Theory]
        [InlineData("_ = await client.CancelPaymentAsync(\"tr_123\");")]
        [InlineData("var result = await client.CancelPaymentAsync(\"tr_123\");")]
        [InlineData("MollieResult result; result = await client.CancelPaymentAsync(\"tr_123\");")]
        [InlineData("(await client.CancelPaymentAsync(\"tr_123\")).EnsureSuccess();")]
        [InlineData("(await client.GetPaymentAsync(\"tr_123\")).EnsureSuccess();")]
        [InlineData("if ((await client.CancelPaymentAsync(\"tr_123\")).Success) { }")]
        [InlineData("Use(await client.CancelPaymentAsync(\"tr_123\"));")]
        [InlineData("await Task.Delay(1);")]
        [InlineData("await client.DoSomethingElseAsync();")]
        public async Task Analyze_ResultIsUsedOrNotAMollieResult_ReportsNoDiagnostic(string statement) {
            // Arrange
            string source = CreateSource(statement);

            // Act
            ImmutableArray<Diagnostic> diagnostics = await GetDiagnosticsAsync(source);

            // Assert
            diagnostics.ShouldBeEmpty();
        }

        [Fact]
        public async Task Analyze_ResultIsNotUsed_DiagnosticMessageContainsMethodName() {
            // Arrange
            string source = CreateSource("await client.CancelPaymentAsync(\"tr_123\").ConfigureAwait(false);");

            // Act
            ImmutableArray<Diagnostic> diagnostics = await GetDiagnosticsAsync(source);

            // Assert
            diagnostics.ShouldHaveSingleItem().GetMessage().ShouldContain("'CancelPaymentAsync'");
        }

        private static string CreateSource(string statement) {
            return $$"""
                using System.Threading.Tasks;
                using Mollie.Api.Models;

                public interface ITestClient {
                    Task<MollieResult> CancelPaymentAsync(string paymentId);
                    Task<MollieResult<string>> GetPaymentAsync(string paymentId);
                    Task<int> DoSomethingElseAsync();
                }

                public class TestClass {
                    public async Task TestMethod(ITestClient client, Task<MollieResult> task) {
                        {{statement}}
                    }

                    private static void Use(MollieResult result) { }
                }
                """;
        }

        private static async Task<ImmutableArray<Diagnostic>> GetDiagnosticsAsync(string source) {
            string trustedPlatformAssemblies = (string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!;
            var references = trustedPlatformAssemblies
                .Split(Path.PathSeparator)
                .Append(typeof(MollieResult).Assembly.Location)
                .Distinct()
                .Select(path => MetadataReference.CreateFromFile(path));

            var compilation = CSharpCompilation.Create(
                "AnalyzerTests",
                [CSharpSyntaxTree.ParseText(source)],
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            compilation.GetDiagnostics()
                .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                .ShouldBeEmpty();

            return await compilation
                .WithAnalyzers([new DiscardedMollieResultAnalyzer()])
                .GetAnalyzerDiagnosticsAsync();
        }
    }
}
