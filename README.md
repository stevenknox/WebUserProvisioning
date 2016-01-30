# Web User Provisioning Tool

This is a simple tool that will enable you to bulk create users where you don't have access to import directly to a database or via API. It has been configured to work against the Asp.Net Identity Samples (https://www.nuget.org/packages/Microsoft.AspNet.Identity.Samples) however can be fully configured from the config file to work with other web sites and applications.

It can enable you to

 * Login as an user with User Management access rights
 * Navigate authenticated pages
 * Handle RequestVerificationToken in Asp.Net MVC applications
 * Read user data from a CSV file
 * Automatically populate input fields and post the data
 * Set a delay period between adding each user


It will try to capture any validation errors and output them to a log file.

This can be modified to work for many use cases where you need to authenticate into a website and perform batch processes such as creating users or products etc


### Stuff used to make this:

 * [CsvHelper](joshclose.github.io/CsvHelper) A .NET library for reading and writing CSV files
 * [HtmlAgilityPack](https://htmlagilitypack.codeplex.com) Agile HTML parser that builds a read/write DOM
 * [BrowserSession](http://refactoringaspnet.blogspot.de/2010/04/using-htmlagilitypack-to-get-and-post.html) Implementation of HtmlAgilityPack to handle sessions and cookies
 