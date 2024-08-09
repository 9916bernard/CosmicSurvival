chcp 65001

echo off

echo ===== 버전 확인
C:\Unity\Editor\2022.3.24f1\Editor\Unity.exe -version
echo.

echo ===== 날짜 확인
echo 날짜 : %date%
echo 시각 : %time%

set HOUR=%time:~0,2%
set MINUTE=%time:~3,2%
set SECOND=%time:~6,2%

set date2=%date:-=%
echo 빌드 : %date2%_%HOUR%_%MINUTE%_%SECOND%

rem echo ===== 프로젝트 실행
rem C:\Unity\Editor\2022.3.24f1\Editor\Unity.exe -projectPath D:\GIT\VSLike\SampleProject\SampleClient

rem -quit 모든 명령을 마친 후 자동으로 유니티 종료
rem -batchmode 콘솔 버전으로 실행
rem -logFile 빌드 로그 파일 생성 위치
rem "C:\Unity\Editor\2022.3.24f1\Editor\Unity.exe" -quit -batchmode -logFile "D:\GIT\VSLike\SampleProject\testBuild\build_log.log" -buildTarget Android -projectPath D:\GIT\VSLike\SampleProject\SampleClient
"C:\Unity\Editor\2022.3.24f1\Editor\Unity.exe" -quit -batchmode -logFile "D:\GIT\VSLike\SampleProject\testBuild\build_log.log" -projectPath D:\GIT\VSLike\SampleProject\SampleClient -executeMethod BuildFunction.TestBuild