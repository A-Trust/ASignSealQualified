#!/bin/sh

CLASSPATH='./:./lib/*'

javac -classpath "$CLASSPATH" PdfSignDemo.java
java -classpath "$CLASSPATH" PdfSignDemo
