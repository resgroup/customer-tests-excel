rem The assemblies under test have to be an absolute path, which is annoying
rem  and means you will need to make sure they are correct if using this file
rem  locally
rem Normally I just run GenerateCodeFromExcelTest directly from visual studio
rem  when developing, instead of using this script
rem This file is used during the appveyor build, so the paths need to work there
"..\..\Builtdlls\Release\GenerateCodeFromExcelTest.exe" /folder "..\..\SampleTests" /project SampleTests.csproj /namespace SampleTests /usings "SampleSystemUnderTest SampleSystemUnderTest.AnovaCalculator SampleSystemUnderTest.Routing SampleSystemUnderTest.VermeulenNearWakeLength SampleSystemUnderTest.Calculator" /assertionClassPrefix "I" /assembliesUnderTest "C:\projects\customer-tests-excel\Builtdlls\Release\SampleSystemUnderTest.dll"