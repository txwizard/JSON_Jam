# JSON_Joy Project ReadMe

This project demonstrates an issue that arises frequently when working with JSON
data sets and its solution. The problem is that some REST APIs return strings of
text that are valid JSON, but are incompatible with programming languages and
data bases for one or more reasons.

This project demonstrates a solution to two issues that arise frequently.

1) The key names are invalid as variable names in most, if not all, programming
languages, such as C, C++, C#, Visual Basic, Perl, PHP, Python, etc.

2) Elements that are most easily treated as arrays are presented as discrete
named entities. Moreover, the entity names frequently cannot be uses directly as
program variable names.

A companion CodeProject article delves more deeply into the issues addressed by
this solution, all of which arose in the course of routine software development
activities.

Archive <https://github.com/txwizard/JSON_Jam/blob/master/Binaries.zip> contains
the biaries. When extracted into the project root directory, it deposits them in
the locations where they would exist if you built the project yourself, which is
the location, in relation to the Test_Data directory, where the program expects
to find its input file and create its three output files. This configuration is
designed so that you can run the program by double-clicking JSON_Jam.exe in the
File Explorer.