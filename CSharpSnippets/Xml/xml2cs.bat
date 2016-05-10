@echo off
set xsdExe="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\xsd.exe"

%xsdExe% XmlFiles\MyOptions.xml

rem Run this to generate (crap) C# class
rem %xsdExe% /c MyOptions.xsd