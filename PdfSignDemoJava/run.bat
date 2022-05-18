cls

set CLASSPATH="./;./lib/*;"

javac -classpath %CLASSPATH% DemoClient.java
java -classpath %CLASSPATH% DemoClient
