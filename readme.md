# sbox cheat and cheat loader

## setup (i recommend using linux or wsl2)
- install dotnet 9 sdk
- install rust and cargo
- install make
- install the cross compiler binaries (linux -> windows)
- run make command
- inject native_net_loader.dll into sbox


i have no idea if this injection method has been patched.

basicallllyyyyy it hijacks the hostfxr thread in the process.
some parts are undocumented by microsoft so if something breaks u might need to
read into the source code of the .net runtime

then it just injects a little payload to load the actual dll.


- native net loader (the first step, hijacks the thread then injects the payload)
- net payload (loads the actual cheat (sbox mod))
