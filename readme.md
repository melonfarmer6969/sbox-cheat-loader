# s&box cheat and cheat loader



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

# to reverse games
this method will most likely be patched in the future but you can use
https://github.com/chrisspieler/sbox-cll-extractor
to get the code of a game, then u can use reflection to get access to the classes


## fix ideas:
- so first off, they probably updated the .net core version so u need to change the target to .net core 10 or whatever is current
- i had it set up to stream the DLL from an api, so u need to change it to read the local file instead

https://learn.microsoft.com/en-us/dotnet/core/tutorials/netcore-hosting 
this API is made for devs to use in their own program, but since we're injecting a DLL we can just yoinky yoinky it.
the docs are incomplete though but luckily this rust library already sorted it (if the library is outdated you need to go to https://github.com/dotnet/runtime/blob/main/src/native/corehost/hostfxr.h and then reverse the error codes (its not that hard)
