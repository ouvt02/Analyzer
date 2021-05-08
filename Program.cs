using System;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

int length_class_name = 3;
int length_field_name = 2;
int length_property_name = 5;
int length_function_name = 1;
int length_local_var_name = 3;



void Visit(SyntaxNode node)
{
    //here something doing with the node

    IEnumerable<SyntaxToken> tokens = node.ChildTokens();
    if (node.Kind().ToString() == "ClassDeclaration")
    {
        foreach (SyntaxToken token in tokens)
        {
            if(token.Kind().ToString() == "IdentifierToken")
            {
                if (token.Text.Length > length_class_name)
                    Console.WriteLine($"Length of name of class \"{token.Text}\" more than acceptable");
            }
        }
    }

    if (node.Kind().ToString() == "PropertyDeclaration")
    {
        foreach (SyntaxToken token in tokens)
        {
            if (token.Kind().ToString() == "IdentifierToken")
            {
                if (token.Text.Length > length_property_name)
                    Console.WriteLine($"Length of name of property \"{token.Text}\" more than acceptable");
            }
        }
    }

    if (node.Kind().ToString() == "MethodDeclaration")
    {
        foreach (SyntaxToken token in tokens)
        {
            if (token.Kind().ToString() == "IdentifierToken")
            {
                if (token.Text.Length > length_function_name)
                    Console.WriteLine($"Length of name of method \"{token.Text}\" more than acceptable");
            }
        }
    }

    if (node.Kind().ToString() == "FieldDeclaration")
    {
        foreach (SyntaxNode child_node in node.ChildNodes())
        {
            if (node.Kind().ToString() == "VariableDeclaration")
            {

                foreach (SyntaxNode new_child_node in node.ChildNodes())
                {
                    if (node.Kind().ToString() == "VariableDeclaratior")
                    {
                        foreach (SyntaxToken token in tokens)
                        {
                            if (token.Kind().ToString() == "IdentifierToken")
                            {
                                if (token.Text.Length > length_field_name)
                                    Console.WriteLine($"Length of name of field \"{token.Text}\" more than acceptable");
                            }
                        }
                    }
                }
            }
        }
    }


    if (node.Kind().ToString() == "LocalDeclarationStatement")
    {
        foreach (SyntaxNode child_node in node.ChildNodes())
        {
            if (node.Kind().ToString() == "VariableDeclaration")
            {

                foreach (SyntaxNode new_child_node in node.ChildNodes())
                {
                    if (node.Kind().ToString() == "VariableDeclaratior") 
                    {
                        foreach (SyntaxToken token in tokens)
                        {
                            if (token.Kind().ToString() == "IdentifierToken")
                            {
                                if (token.Text.Length > length_local_var_name)
                                    Console.WriteLine($"Length of name of local variable \"{token.Text}\" more than acceptable");
                            }
                        }
                    }
                }
            }
        }
    }


    //foreach (SyntaxToken token in tokens)

    IEnumerable < SyntaxNode > nodes = node.ChildNodes();
    foreach (SyntaxNode child_node in nodes)
        Visit(child_node);
}

VisualStudioInstance[] test = MSBuildLocator.QueryVisualStudioInstances().ToArray();
VisualStudioInstance inst = test[0];
MSBuildLocator.RegisterInstance(inst);

MSBuildWorkspace workspace = MSBuildWorkspace.Create();
Project project = workspace.OpenProjectAsync(@"C:\Users\Ульяна\source\repos\ForTesting\ForTesting.csproj").Result;
IEnumerable<Document> documents = project.Documents;

Document[] Documents = documents.ToArray();

foreach (Document document in documents)
    Visit(document.GetSyntaxTreeAsync().Result.GetRoot());





