@echo off
set xsdExe="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\xsd.exe"

%xsdExe% XmlFiles\MyOptions.xml
%xsdExe% /c MyOptions.xsd