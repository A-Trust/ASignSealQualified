# Amtssignatur with A-Trust Qualified Signature service

This demo application builds on top of [DemoJava](../DemoJava) and uses [DSS](https://github.com/esig/dss)
to sign a [sample PDF (1.7)](https://svn.apache.org/repos/asf/tika/trunk/tika-parsers/src/test/resources/test-documents/testPDF_Version.8.x.pdf) 
using ECDSA encrypted signature returned from A-Trust qualified signature service.

## Prerequisites

### Download dependencies

To download dependencies used by this example, Maven can be used to copy dependencies into the
[lib](lib) directory:

```shell
mvn dependency:copy-dependencies -DoutputDirectory=lib
```

### JDK

A Java compiler is expected to be available on the `PATH` 

## Run the demo

Use either [run.bat](run.bat) for Windows or [run.sh](run.sh) for Unix/Linux based systems. At the end the signed
document is written to [output.pdf](output.pdf) file.

## Credits

* Thanks to [A-Trust](https://github.com/A-Trust) for providing open-source examples for their signing service
* Thanks to [DSS](https://github.com/esig/dss) project for their implementation and extensive documentation and 
examples
* Thanks to [BMF-RKSV-Technik](https://github.com/BMF-RKSV-Technik) project for their utility method to convert 
raw ECDSA R+S values to DER encoded signature.
