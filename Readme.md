# Gel.js Runtime Environment

###### *A native user interface kit for Javascript*

## Status

*Currently only an early, alpha grade, Windows protoype built with .NET 3.5 and WinForms exists.*

See [win/gel.exe/Readme.md](gel/tree/develop/win/gel.exe/Readme.md) for notes on running the prototype.

## Introduction

Gel.js (or just Gel) is meant to provide a quick and easy way to create software using Javascript and native software components.

Gel is based on Node.js. It is an implementation of the Node.js module loading system and some of the core Node.js modules and globals such as `process` and `console`. With that basic system in place, other modules can be built to allow Javascript to create and interact with native software components and GUI elements.

Gel will not be bound to a single Javascript runtime engine. Ideally, it will run on the native Javascript engine of whatever OS it targets. It will be usable independently, but will also be able to coordinate with an external Javascript engine/child-process such as Node or Rhino.

## Roadmap

Current focus:

- Defining the basic scripting environment and APIs.
- Developing APIs for common native desktop OS components.
- Researching eventual mobile API developments.
- Developing the Windows prototype.

Future:

- Refine the initial Windows implementation.
- Begin Mac prototype implementation with Objective-C and JavaScriptCore.
- Package manager (possibly by forking NPM or just using it).
- Developing mobile (phone/tablet) prototypes.

## Status Detail

Gel has barely been started and the first and only implementation is nothing more than a prototype. The module loading system and many of the modules listed below where copied wholesale from Node.js. Nothing has been tested very rigorously, but since I was able to copy much of the Javascript code from Node.js, I believe that what has been done so far is fairly solid.

Development of User Interface APIs has not even begun. Only the most basic foundation of the project has been laid as of this writing.

### Node.js Compatibility Status

The following sections indicate the elements of the Node.js runtime environment that are available to Javascript code that is executing under the Gel.js runtime environment.

#### Globals

*Everything listed here works just like it does in Node.js, except for process which is very similar to it's Node counterpart, but missing a few methods and child objects.*

- global
- process
- console
- require
 - require.resolve
 - require.cache
- __filename
- __dirname
- module
- exports
- timer methods (setTimeout, clearTimeout, setInterval, clearInterval).

#### Libs

*Since buffer, stream, much of fs and others are not yet implemented, any public methods of the following modules that are associated with those have been omitted or will simply not work as of now.*

- _linklist
- assert
- events
- freelist
- module
- native_module
- os
- path
- punycode
- querystring
- string_decoder
- timers
- url
- util

#### Libs with Limited Functionality

- fs
- evals
- vm

## A note on licensing.

If developers are forced to put a three paragraph license at the top of every code file, then the lawyers have won. Please don't do this. If a file has more than a few lines of copyright and licensing comments, that content should be placed in a separate file and a reference to that separate file should be placed in the source code file.

Here is an example:

// COPYRIGHT AND (MIT) LICENSE APPLY. SEE FILE: ../lic/gel.txt

Thank You.

