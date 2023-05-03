cls

set JAVAPATH=c:\Program Files\Java\jdk-15.0.2\bin
rem set JAVAPATH=c:\Program Files\Java\openjdk-12.0.1\bin
set CLASSPATH="./;./lib/*;"

"%JAVAPATH%\javac.exe" -classpath %CLASSPATH% DemoClient.java
"%JAVAPATH%\java.exe" -classpath %CLASSPATH% DemoClient