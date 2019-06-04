# JSON_Joy Project ReadMe

This project demonstrates an issue that arises frequently when working with JSON
data sets and its solution. The problem arises when some REST APIs return
strings of text that are valid JSON, but are incompatible with programming
languages and data bases for one or more reasons.

1) The key names are invalid as variable names in most, if not all, programming
languages, such as C, C++, C#, Visual Basic, Perl, PHP, Python, etc. For
example, they may contain embedded spaces, begin with numberals, or violate
other syntax rules of the chosen programming language.

2) Elements that are most easily treated as arrays are presented as discrete
named entities. Moreover, the entity names frequently cannot be used directly as
program variable names.

A companion CodeProject article delves more deeply into the issues addressed by
this solution, all of which arose in the course of routine software development
activities.

A side topic arose during development, which is that there appears to be no way
to coerce the Git clone engine into downloading a text file without replacing
its Unix line breaks with Windows line breaks. This gave rise to another one of
my state-machine-drive string parsing exercises, which I incorporated into
version 1.4 of this assembly.

Archive <https://github.com/txwizard/JSON_Jam/blob/master/Binaries.zip> contains
the biaries. When extracted into the project root directory, it deposits them in
the locations where they would exist if you built the project, which is the
location, in relation to the `Test_Data` directory, where the program expects
to find its input file and create its three output files. This configuration is
designed so that you can run the program by double-clicking `JSON_Jam.exe` in
the File Explorer.

Though the program consumes eight custom DLLs, all are restored from the NuGet
repository the first time you build, unless you disable automatic restore.

The PDB folder in the project root contains the PDB files that match the DLLs
that are included with the package, all of which are also included in the
`Binaries.zip` archive. They are here because the NuGet restore isn't putting
them into the local package repository. Until that gets sorted, this is the
most direct way to make them accessible. The conspicuous absence of the PDB that
should accompany `Newtonsoft.Json.dll` is because it is omitted from the
package, and I haven't searched for it on the symbol servers where it might
live.