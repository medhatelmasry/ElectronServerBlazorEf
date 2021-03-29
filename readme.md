### In case of runtime error

In SaveAs.razor remove ? in @page "/saveas/{filepath?}

In Startup.cs remove ? in {filepath?}

These changes are due to breaking changes in newer versions of .NET
