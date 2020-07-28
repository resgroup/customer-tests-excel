namespace SampleTests.Setup
{
    // This folder / namespace contains all the generated specification specific setup files

    // Files are generated unless there is already a matching '<ClassName>.cs' file in the
    // IgnoreUnderGeneration folder. This allows you to override the generated files if you 
    // want to.

    // A file is generated for each root obect being tested. Only the setup parts of the
    // class is generated, but the classes are `partial`, so you can add the rest of the 
    // implementation in the IgnoreUnderGeneration folder. The annoyance is that you have
    // to use this namespace, even though the file is in a different folder.
    // An example of this is:
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/GeneratedSpecificationSpecific/Calculator.cs

    // The `/assembliesUnderTest` specified on the command line are searched to find matching
    // interfaces, and suitable files generated for them. 
    // An example of this is:
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/GeneratedSpecificationSpecific/Cargo.cs
    // If there is no matching interface then a dummy file is created. This will compile, but 
    // won't set anything up in your code. These files can be useful for showing how things 
    // work, and sometimes you might just to just use them, and map values from them in to your 
    // system under test manually in the root object files. 
    // An example of this is:
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/GeneratedSpecificationSpecific/A_Table.cs
}