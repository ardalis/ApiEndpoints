using Ardalis.ApiEndpoints.CodeAnalyzers.Test.Helpers;
using Ardalis.ApiEndpoints.CodeAnalyzers.Test.Verifiers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ardalis.ApiEndpoints.CodeAnalyzers.Test
{
    /// <summary>
    /// Validates the behavior of <see cref="EndpointHasExtraPublicMethodAnalyzer"/>
    /// </summary>
    [TestClass]
    public class EndpointHasPublicActionMethodTests : CodeFixVerifier
    {
        private const string ValidEndpoint = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : BaseAsyncEndpoint<object, object>
                {
                    public override async Task<ActionResult<object>> HandleAsync([FromBody]object request)
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string ValidEndpointWithExtraNonPublicMethods = @"
            using System;            
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                class Program
                {
                    static void Main(string[] args)
                    {
                        const int i = 0;
                        Console.WriteLine(i);
                    }
                }
            }";

        private const string InvalidEndpointWithExtraPublicMethod = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : BaseEndpoint<object, object>
                {
                    public override ActionResult<object> Handle([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointWithExtraPublicMethod_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : BaseEndpoint<object, object>
                {
                    public override ActionResult<object> Handle([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointAsyncWithExtraPublicMethod = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : BaseAsyncEndpoint<object, object>
                {
                    public override async Task<ActionResult<object>> HandleAsync([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointAsyncWithExtraPublicMethod_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : BaseAsyncEndpoint<object, object>
                {
                    public override async Task<ActionResult<object>> HandleAsync([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointWithCustomBaseClass = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public abstract class CustomBaseClass : BaseAsyncEndpoint<object, object>
                {
                }

                public class TestEndpoint : CustomBaseClass
                {
                    public override async Task<ActionResult<object>> HandleAsync([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointWithCustomBaseClass_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public abstract class CustomBaseClass : BaseAsyncEndpoint<object, object>
                {
                }

                public class TestEndpoint : CustomBaseClass
                {
                    public override async Task<ActionResult<object>> HandleAsync([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointWithBaseEndpointAliased = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;
            using AliasBaseClass = Ardalis.ApiEndpoints.BaseEndpoint<object, object>;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : AliasBaseClass
                {
                    public override ActionResult<object> Handle([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointWithBaseEndpointAliased_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;
            using AliasBaseClass = Ardalis.ApiEndpoints.BaseEndpoint<object, object>;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : AliasBaseClass
                {
                    public override ActionResult<object> Handle([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointIsNonPublic = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                internal class TestEndpoint : BaseEndpoint<object, object>
                {
                    public override ActionResult<object> Handle([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    public virtual void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        private const string InvalidEndpointIsNonPublic_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                internal class TestEndpoint : BaseEndpoint<object, object>
                {
                    public override ActionResult<object> Handle([FromBody]object request)
                    {
                        throw new Exception();
                    }

                    internal virtual void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }";

        [DataTestMethod]
        [DataRow(""),
         DataRow(ValidEndpoint),
         DataRow(ValidEndpointWithExtraNonPublicMethods)]
        public void WhenTestCodeIsValidNoDiagnosticIsTriggered(string testCode)
        {
            VerifyCSharpDiagnostic(testCode);
        }

        [DataTestMethod]
        
        [
        DataRow(
            InvalidEndpointWithExtraPublicMethod, 
            InvalidEndpointWithExtraPublicMethod_Fixed,
            "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
            16, 33),
         DataRow(
             InvalidEndpointAsyncWithExtraPublicMethod,
             InvalidEndpointAsyncWithExtraPublicMethod_Fixed,
             "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
             16, 33),
         DataRow(
             InvalidEndpointWithCustomBaseClass,
             InvalidEndpointWithCustomBaseClass_Fixed,
             "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
             20, 33),
        DataRow(
             InvalidEndpointWithBaseEndpointAliased,
             InvalidEndpointWithBaseEndpointAliased_Fixed,
             "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
             17, 33),
        DataRow(
            InvalidEndpointIsNonPublic,
            InvalidEndpointIsNonPublic_Fixed,
            "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
            16, 41)
        ]
        public void WhenDiagnosticIsRaisedFixUpdatesCode(
            string test,
            string fixTest,
            string diagnosticMessage,
            int line,
            int column)
        {
            var expected = new DiagnosticResult
            {
                Id = EndpointHasExtraPublicMethodAnalyzer.DiagnosticId,
                Message = diagnosticMessage,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", line, column)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);

            VerifyCSharpFix(test, fixTest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new EndpointHasExtraPublicMethodCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new EndpointHasExtraPublicMethodAnalyzer();
        }

    }
}