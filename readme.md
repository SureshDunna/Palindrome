# Palindrome
Palindrome is a special string which gives same string even if it is reversed.

This solution contains Palindrome website implemented in Asp.Net Core with MVC and contains back API in .Net core and it uses SQL as local db with Entity framework.

## Features
- It has a page and web api route to list out all the persisted palindromes
- It has a page and web api route to find out if the entered string is palindrome or not.

Api documentation or test harness can be accessed from
[Swagger doc and test harness](http://localhost:5002/swagger)

Api health check can be accessed from
[Palindrome api health check](http://localhost:5002/healthcheck/ping)

Palindrome website health check can be accessed from
[Palindrome api health check](http://localhost:5001/healthcheck/ping)

Palindrome website can be accessed from
[Palindrome api health check](http://localhost:5001)

## Pre-requisites
- Windows & Mac
    - Visual Studio Code with C# extensions
    - [.Net Core SDK version](https://www.microsoft.com/net/download/core ".Net Core SDK")

## Build
The project can be built by running the built-in build script `.\Build.ps1` from the Powershell command line and script executes all the tests in the solution.