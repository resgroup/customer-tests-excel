rem calls the real generatetest file, but from the directory it is expecting to be in, this makes the appveyor build a bit easier
pushd .\SampleTests\IgnoreOnGeneration
GenerateTests.bat
popd