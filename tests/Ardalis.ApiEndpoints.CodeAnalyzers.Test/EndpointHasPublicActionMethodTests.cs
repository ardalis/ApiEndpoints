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
        private const string EndpointBase = @"
            namespace Ardalis.ApiEndpoints
            {
                public abstract class EndpointBase
                {
                }
            }";

        private const string ValidEndpoint = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public async Task<ActionResult<object>> HandleAsync([FromBody] object request)
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string ValidEndpointCustomMethodName = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public async Task<ActionResult<object>> HandleTest([FromBody] object request)
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string ValidEndpointWithExtraStaticMethod = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public async Task<ActionResult<object>> HandleAsync([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    public static void ExtraMethod(){}
                }
            }" + EndpointBase;

        private const string ValidEndpointWithPublicConstructor = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public TestEndpoint(object o){}

                    public async Task<ActionResult<object>> HandleAsync([FromBody] object request)
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string ValidEndpointUsingCustomBaseClass = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public abstract class CustomBase : EndpointBase
                {
                }

                public class TestEndpoint : CustomBase
                {
                    public async Task<ActionResult<object>> HandleAsync([FromBody] object request)
                    {
                        return base.FooBar();
                    }
                }
            }" + EndpointBase;

        private const string ValidEndpointWithCustomEndpointBaseDefined = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            
            namespace ApiEndpointsAnalyzersTest
            {
                public abstract class EndpointBase { }

                public class TestEndpoint : EndpointBase
                {
                    public async Task<ActionResult<object>> HandleAsync([FromBody] object request)
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

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
            }" + EndpointBase;

        private const string InvalidEndpointWithExtraPublicMethod = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithExtraPublicMethod_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithExtraPublicMethodFirst = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }

                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithExtraPublicMethodFirst_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }

                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithExtraPublicMethodAndCustomMethodName = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public ActionResult<object> HandleTest([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithExtraPublicMethodAndCustomMethodName_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : EndpointBase
                {
                    public ActionResult<object> HandleTest([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithCustomBaseClass = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public abstract class CustomBaseClass : EndpointBase
                {
                }

                public class TestEndpoint : CustomBaseClass
                {
                    public async Task<ActionResult<object>> HandleAsync([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithCustomBaseClass_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                public abstract class CustomBaseClass : EndpointBase
                {
                }

                public class TestEndpoint : CustomBaseClass
                {
                    public async Task<ActionResult<object>> HandleAsync([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithEndpointBaseAliased = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;
            using AliasBaseClass = Ardalis.ApiEndpoints.EndpointBase;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : AliasBaseClass
                {
                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    public void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointWithEndpointBaseAliased_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;
            using AliasBaseClass = Ardalis.ApiEndpoints.EndpointBase;

            namespace ApiEndpointsAnalyzersTest
            {
                public class TestEndpoint : AliasBaseClass
                {
                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    internal void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointIsNonPublic = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                internal class TestEndpoint : EndpointBase
                {
                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    public virtual void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        private const string InvalidEndpointIsNonPublic_Fixed = @"
            using System;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Mvc;
            using Ardalis.ApiEndpoints;

            namespace ApiEndpointsAnalyzersTest
            {
                internal class TestEndpoint : EndpointBase
                {
                    public ActionResult<object> Handle([FromBody] object request)
                    {
                        throw new Exception();
                    }

                    internal virtual void ExtraPublicMethod()
                    {
                        throw new Exception();
                    }
                }
            }" + EndpointBase;

        [DataTestMethod]
        [DataRow(""),
         DataRow(ValidEndpoint),
         DataRow(ValidEndpointCustomMethodName),
         DataRow(ValidEndpointWithExtraStaticMethod),
         DataRow(ValidEndpointWithExtraNonPublicMethods),
         DataRow(ValidEndpointWithPublicConstructor),
         DataRow(ValidEndpointUsingCustomBaseClass),
         DataRow(ValidEndpointWithCustomEndpointBaseDefined)]
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
            InvalidEndpointWithExtraPublicMethodFirst,
            InvalidEndpointWithExtraPublicMethodFirst_Fixed,
            "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
            11, 33),
        DataRow(
            InvalidEndpointWithExtraPublicMethodAndCustomMethodName,
            InvalidEndpointWithExtraPublicMethodAndCustomMethodName_Fixed,
            "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
            16, 33),
         DataRow(
             InvalidEndpointWithCustomBaseClass,
             InvalidEndpointWithCustomBaseClass_Fixed,
             "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
             20, 33),
        DataRow(
             InvalidEndpointWithEndpointBaseAliased,
             InvalidEndpointWithEndpointBaseAliased_Fixed,
             "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
             17, 33),
        DataRow(
            InvalidEndpointIsNonPublic,
            InvalidEndpointIsNonPublic_Fixed,
            "Endpoint TestEndpoint has additional public method ExtraPublicMethod. Endpoints must have only one public method.",
            16, 41),
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

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new EndpointHasExtraPublicMethodCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new EndpointHasExtraPublicMethodAnalyzer();
    }
}
