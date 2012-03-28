# Gel.js Runtime Environment

###### *A native interface kit for Javascript, based on Node.js*

## Introduction

Gel.js (or just Gel) is meant to provide a quick and easy way to create software using Javascript and native software components.

Gel implements the Node.js module loading system and some of the core Node.js modules and globals such as `process` and `console` and combines them with some new modules that allow the programmer to create and interact with native software components and GUI elements.

Gel should be usable independently, but could also coordinate with an external Javascript engine/process such as Node or Rhino via IPC.

## Status

*Currently, an alpha grade Windows desktop implementation exists.*

Support for desktop environments such as Gnome, OS X and Windows is planned. Mobile device support may be added in the future, but is not being actively planned at this time.

### Node.js Compatibility Status

The following sections indicate the elements of the Node.js runtime environment that are available to Javascript code that is executing under the Gel.js runtime environment.

#### Globals

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

