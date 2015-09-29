:run.bat
:
:runs parser code

.\Analyzer\bin\Debug\Analyzer.exe .\test *.cs
pause

.\Analyzer\bin\Debug\Analyzer.exe .\test *.cs" /S
pause

.\Analyzer\bin\Debug\Analyzer.exe .\test *.cs" /R
pause

.\Analyzer\bin\Debug\Analyzer.exe .\test *.cs" /X
pause

.\Analyzer\bin\Debug\Analyzer.exe .\test *.cs" /S /R /X 
pause