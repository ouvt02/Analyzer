using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace CodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeAnalyzerAnalyzer : DiagnosticAnalyzer
    {

        int length_class_name = 3;
        int length_field_name = 2;
        int length_property_name = 5;
        int length_function_name = 1;
        int length_local_var_name = 3;

        public const string DiagnosticId = "CodeAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeClassNode, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMethodNode, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzePropertyNode, SyntaxKind.PropertyDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeVariableNode, SyntaxKind.PropertyDeclaration);

        }



        private void AnalyzeClassNode(SyntaxNodeAnalysisContext context)
        {
                var classDeclaration = (ClassDeclarationSyntax)context.Node;

                if (classDeclaration.Identifier.ValueText != null && classDeclaration.Identifier.ValueText.Length > length_class_name)
                {
                    var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.ValueText);
                    context.ReportDiagnostic(diagnostic);
                }

        }


        private void AnalyzeMethodNode(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;

            if (methodDeclaration.Identifier.ValueText != null && methodDeclaration.Identifier.ValueText.Length > length_function_name)
            {
                var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.ValueText);
                context.ReportDiagnostic(diagnostic);
            }

        }


        private void AnalyzePropertyNode(SyntaxNodeAnalysisContext context)
        {
            var propertyDeclaration = (PropertyDeclarationSyntax)context.Node;

            if (propertyDeclaration.Identifier.ValueText != null && propertyDeclaration.Identifier.ValueText.Length > length_property_name)
            {
                var diagnostic = Diagnostic.Create(Rule, propertyDeclaration.Identifier.GetLocation(), propertyDeclaration.Identifier.ValueText);
                context.ReportDiagnostic(diagnostic);
            }

        }


        private void AnalyzeVariableNode(SyntaxNodeAnalysisContext context)
        {
            var variableDeclaration = (VariableDeclaratorSyntax)context.Node;

            if (variableDeclaration.Parent is VariableDeclarationSyntax)
            {
                if (variableDeclaration.Parent.Parent is FieldDeclarationSyntax)
                {
                    if (variableDeclaration.Identifier.ValueText != null && variableDeclaration.Identifier.ValueText.Length > length_field_name)
                    {
                        var diagnostic = Diagnostic.Create(Rule, variableDeclaration.Identifier.GetLocation(), variableDeclaration.Identifier.ValueText);
                        context.ReportDiagnostic(diagnostic);
                    }
                }


                else
                {
                    if (variableDeclaration.Identifier.ValueText != null && variableDeclaration.Identifier.ValueText.Length > length_local_var_name)
                    {
                        var diagnostic = Diagnostic.Create(Rule, variableDeclaration.Identifier.GetLocation(), variableDeclaration.Identifier.ValueText);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }


    }
}
