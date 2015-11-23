#!/bin/sh
swig -csharp xwiimote.i
gcc -c -fpic xwiimote_wrap.c
gcc -shared /usr/lib/libxwiimote.so xwiimote_wrap.o -o libxwiimote.so
dmcs -out:Test *.cs
