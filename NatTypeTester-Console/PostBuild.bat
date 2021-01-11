if %Configuration%==Release (
:: Merge dlls
%ILMergeConsolePath% /wildcards /out:%TargetDir%NTTMerged.exe ^
/lib:"C:\Windows\Microsoft.NET\Framework64\v4.0.30319" ^
/targetplatform:v4,"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8" ^
%TargetDir%NatTypeTester.Console.exe ^
%TargetDir%*.dll

DEL /f %TargetDir%*.dll >NUL 2>&1
MOVE /Y %TargetDir%NTTMerged.exe %TargetDir%NTT.exe >NUL
)

DEL /f %TargetDir%*.config >NUL 2>&1
DEL /f %TargetDir%*.pdb >NUL 2>&1
DEL /f %TargetDir%NatTypeTester.Console.exe >NUL 2>&1

