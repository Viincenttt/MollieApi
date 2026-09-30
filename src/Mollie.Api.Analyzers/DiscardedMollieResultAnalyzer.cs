using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Mollie.Api.Analyzers {
    /// <summary>
    /// Reports calls that return a <c>MollieResult</c> of which the result is never used. Because client methods
    /// no longer throw when the Mollie API returns an error, ignoring the result means ignoring the error.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DiscardedMollieResultAnalyzer : DiagnosticAnalyzer {
        public const string DiagnosticId = "MOLLIE001";

        private const string MollieResultMetadataName = "Mollie.Api.Models.MollieResult";

        private static readonly DiagnosticDescriptor Rule = new(
            id: DiagnosticId,
            title: "MollieResult is not used",
            messageFormat: "The result of '{0}' is not used, so an error returned by the Mollie API goes unnoticed. Check 'Success', call 'EnsureSuccess()' or discard the result explicitly with '_ ='.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Client methods do not throw when the Mollie API returns an error, they return a MollieResult instead. A result that is never inspected hides failed requests.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(compilationContext => {
                INamedTypeSymbol? mollieResultType = compilationContext.Compilation.GetTypeByMetadataName(MollieResultMetadataName);
                if (mollieResultType == null) {
                    return;
                }

                compilationContext.RegisterOperationAction(
                    operationContext => AnalyzeExpressionStatement(operationContext, mollieResultType),
                    OperationKind.ExpressionStatement);
            });
        }

        private static void AnalyzeExpressionStatement(OperationAnalysisContext context, INamedTypeSymbol mollieResultType) {
            IOperation expression = Unwrap(((IExpressionStatementOperation)context.Operation).Operation);

            // Only a call or an awaited call can be discarded by accident. Assignments, including the
            // explicit discard '_ = ...', are also expression statements and mean the result is used
            IInvocationOperation? invocation = expression switch {
                IAwaitOperation awaitOperation => FindInvocation(awaitOperation.Operation),
                IInvocationOperation invocationOperation => invocationOperation,
                _ => null
            };
            if (expression is not IAwaitOperation && invocation == null) {
                return;
            }

            if (!IsMollieResult(expression.Type, mollieResultType)) {
                return;
            }

            // Methods on the result itself, such as EnsureSuccess(), already inspect the result
            if (expression is IInvocationOperation && IsMollieResult(invocation!.TargetMethod.ContainingType, mollieResultType)) {
                return;
            }

            string name = invocation?.TargetMethod.Name ?? expression.Type!.Name;
            context.ReportDiagnostic(Diagnostic.Create(Rule, expression.Syntax.GetLocation(), name));
        }

        /// <summary>
        /// Finds the call that produced the awaited task, skipping over calls such as ConfigureAwait(false)
        /// </summary>
        private static IInvocationOperation? FindInvocation(IOperation operation) {
            IInvocationOperation? invocation = Unwrap(operation) as IInvocationOperation;
            while (invocation is { TargetMethod.Name: "ConfigureAwait", Instance: not null }) {
                if (Unwrap(invocation.Instance) is IInvocationOperation inner) {
                    invocation = inner;
                }
                else {
                    break;
                }
            }

            return invocation;
        }

        private static IOperation Unwrap(IOperation operation) {
            while (true) {
                switch (operation) {
                    case IParenthesizedOperation parenthesized:
                        operation = parenthesized.Operand;
                        break;
                    case IConversionOperation { IsImplicit: true } conversion:
                        operation = conversion.Operand;
                        break;
                    default:
                        return operation;
                }
            }
        }

        private static bool IsMollieResult(ITypeSymbol? type, INamedTypeSymbol mollieResultType) {
            for (ITypeSymbol? current = type; current != null; current = current.BaseType) {
                if (SymbolEqualityComparer.Default.Equals(current, mollieResultType)) {
                    return true;
                }
            }

            return false;
        }
    }
}
