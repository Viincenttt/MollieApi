using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using System;

namespace Mollie.Tests.Integration.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RetryOnFailureAttribute : PropertyAttribute, IWrapSetUpTearDown
    {
        private int _count;

        /// <summary>
        /// Construct a RepeatAttribute
        /// </summary>
        /// <param name="count">The number of times to run the test</param>
        public RetryOnFailureAttribute(int count) : base(count) {
            _count = count;
        }

        /// <summary>
        /// Wrap a command and return the result.
        /// </summary>
        /// <param name="command">The command to be wrapped</param>
        /// <returns>The wrapped command</returns>
        public TestCommand Wrap(TestCommand command) {
            return new CustomRetryCommand(command, _count);
        }

        /// <summary>
        /// The test command for the RetryAttribute
        /// </summary>
        public class CustomRetryCommand : DelegatingTestCommand
        {
            private int _retryCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomRetryCommand"/> class.
            /// </summary>
            /// <param name="innerCommand">The inner command.</param>
            /// <param name="retryCount">The number of repetitions</param>
            public CustomRetryCommand(TestCommand innerCommand, int retryCount)
                : base(innerCommand) {
                _retryCount = retryCount;
            }

            /// <summary>
            /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
            /// </summary>
            /// <param name="context">The context in which the test should run.</param>
            /// <returns>A TestResult</returns>
            public override TestResult Execute(TestExecutionContext context) {
                int count = _retryCount;

                while (count-- > 0) {
                    context.CurrentResult = innerCommand.Execute(context);
                    var results = context.CurrentResult.ResultState;

                    if (results != ResultState.Error
                        && results != ResultState.Failure
                        && results != ResultState.SetUpError
                        && results != ResultState.SetUpFailure
                        && results != ResultState.TearDownError
                        && results != ResultState.ChildFailure) {
                        break;
                    }
                }

                return context.CurrentResult;
            }
        }
    }
}