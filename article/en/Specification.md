# Natasha  Packaging Specification

<br/>

## A complete Operator

The complete Operator consists of three parts：

-  Builer
-  Extension (For the convenience of developers, you can write some extension methods.)
-  Function (Customize according to your own needs.)

<br/>


## Script builder（Builder）  

As the core part of Operator, Builder is divided into two parts: template and compiler：
 
<br/>  

   - Template (for building):  
        
        Using Template templates to build runtime script strings, Natasha has built-in templates that make it easy for users to build scripts.          
        
       - Oop(class/struct/interface) Script templates that integrate classes, structures, and interfaces can be used to dynamically build object-oriented structures.
         
       - OnceMethod Using this template to quickly build a method that inherits the Oop template can solve the problem of common one-time delegate builds.  
       
       - Member In order to provide members with a common level of protection, modifiers, and other necessary build information.
         
       - Method/Ctor/Field Inheriting the Member template, the result is a single function / field body, which can be combined with the Oop template to build an object-oriented structure.。
         
<br/>  
     
   - Complier (for Compiling)
     
        To dock the compilation engine and collect exceptions, Natasha uses the abstract class IComplier for basic compilation. 
        These include choosing how to compile / get assemblies / get types / get method metadata / get delegates. 
        Beyond the engine is the compilation layer, which implements OopComplier/MethodComplier
        
      - OopComplier : On the basis of abstract compiler, the methods of obtaining classes, structures and interfaces are distinguished.  
      
      - MethodComplier : On the basis of abstract compiler, the method of obtaining delegate is wrapped.
        
        

<br/>


## Operator

Operator is packaged on the basis of Builder, and Builder provides most of the functionality of script construction and compilation, so the encapsulation of Operator needs to focus more on the development of functions and extensions.    
For extensions, after Operator or Builder is written, you can encapsulate an extension method as needed.  
The functionality of Operator is customized to your own needs.

#### Case  

For example, FastMethodOperator is wrapped and simplified on the basis of OnceMethodBuilder, and FastMethodBuilder's initialization function customizes its own script build process, as followings:

```C#
HiddenNameSpace()
 .OopAccess(AccessTypes.Public)
 .OopModifier(Modifiers.Static)
 .MethodAccess(AccessTypes.Public)
 .MethodModifier(Modifiers.Static);

```  

Hide namespaces; classes use Public protection levels and static modifiers; methods use Public protection levels and static modifiers; the above construction process generates the following code:

```C#

using XXXX;
public static class XXXXX{
 public static X Invoke(){
   xxxxxx
 }
}

```
After compiling, we can get the Invoke delegate function and use it directly.
