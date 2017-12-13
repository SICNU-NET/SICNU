@echo off

start /wait WebCrawl.exe
start /wait DownLoadProject.exe
start /wait Compile.exe

@echo on
echo finished!

