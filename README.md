# FutuCalc

### A simple API for solving equations.

This project was created as part of an application to a developer role at Futurice, 
and features an API that exposes a single endpoint for solving simple equations.

## Table of Contents

1. [How To Use](#how-to-use)
2. [Decisions & Assumptions](#decisions--assumptions)
    * [Implementation Language](#implementation-language)
    * [Hosting Solution](#hosting-solution)
    * [Architecture](#architecture)
    * [Calculator Implementation](#calculator-implementation)
    * [Assumptions](#assumptions)
4. [Architecture](#architecture)
3. [Known Issues/Areas of Improvement](#known-issues--areas-of-improvement)

## How to Use

The API is currently hosted at [https://futucalc.herokuapp.com].
 
Note: as the application is running on a free Heroku instance, it may take some time for the first
requests to resolve as the application spins up.

Documentation can be located [here](https://app.swaggerhub.com/apis/DanMcElroy/FutuCalc). This should
provide a description of all API endpoints, how to use them and what response data to expect.

## Decisions & Assumptions

### Implementation Language

I chose to implement this API using C# via .NET Core Web API, as it is one of the languages most familiar to me
and allowed me to spin up a basic API project in a short amount of time, with a relatively small amount of code
and external dependencies.

### Hosting Solution

The project is hosted as a Docker instance on the Heroku platform, deployed via the Heroku Command Line tool.

The application was developed as a Docker-based microservice, as after some initial setup work, it makes
continuous integration and deployment incredibly simple, and allowed me to spend the majority of development
time focusing on solving the core problem.

Also, in a hypothetical scenario where this API is used in a production system, building the application as a
microservice allows for a high degree of flexibility in scale, deployment platforms, and so on.

Heroku was chosen as a platform primarily for its ease-of-use in getting started and configuration, while
offering good logging/continuous deployment solutions.

### Architecture

The project is implemented in 3 libraries: 
* `FutuCalc.API`, the Web API that is deployed to Heroku
* `FutuCalc.Core`, a class library containing the calculation logic
* `FutuCalc.Tests`, a unit-testing library 

This allows the calculation functionality to be exposed to both unit testing and the API, without exposing
the latter two to each other.

### Calculator Implementation

The calculator is implemented by converting the equation into postfix notation (also known as Reverse Polish
notation), as it (lends itself) to fairly simple and extensible implementation.

The equation is converted into postfix notation in two steps:

1. The raw string is converted into symbols, i.e. discrete tokens in the equation such as "23", "+" or "("
(see the class `SymbolParser`).
2. Dijkstra's "Shunting Yard" algorithm is used to arrange this list of symbols into postfix order (see the
class `PostfixShuntingYardConverter`). 

From there, the result is calculated by processing this list of symbols sequentially. When a number 
(or operand) is reached, it's added to a stack of values, and when an operator (such as "+" or "/")
is reached, it pops off the topmost items on the stack, operates on them, and pushes the result back on to
the stack.

By the end of the list of symbols, only one element should remain on the stack - this is the result of the
calculation.

### Assumptions

* If the provided input isn't the correct length for a base-64 string (i.e. is not divisible by 4),
the API should still accept the input and add padding characters (`=`) as required.
* Despite not being included in the example, floating-point numbers (i.e. 23.5) are valid within an
equation.
* The only valid whitespace character is a space (and not a tab, new line or carriage return).

## Known Issues / Areas of Improvement

Below are a number of issues which in a more fully fleshed-out project would be addressed, but were
left unresolved in the interest of time and complexity.

1. Unit testing coverage is currently quite low: the calculator is tested as a whole, but individual
components (such as the symbol parser and postfix processor) are not tested independently. The classes
were structured in such a way that such testing would be easy to introduce in future.
2. Extending the current calculator to support additional operators (such as `%` or `^`) is _mostly_
simple - it would involve creating a new implementation of `UnaryOperator` or `BinaryOperator` and
adding one new case to the switch statement in `SymbolParser.ParseSymbol` - however, the regular
expression for valid symbols defined in `SymbolParser.TokenFormat` also needs to be extended 
to support the new character for this operator. This isn't very intuitive, and for developers with
less experience of regular expressions could lead to otherwise-avoidable errors.