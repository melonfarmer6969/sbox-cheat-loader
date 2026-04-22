.PHONY: all netpayload nativenetloader sboxmod setup clean

MODE := debug
ifdef RELEASE
    MODE := release
endif

all: setup netpayload nativenetloader sboxmod

setup:
	mkdir -p bin/$(MODE)

netpayload: setup
	cd NETPayload && dotnet build -c $(MODE)
	cp NETPayload/bin/$(MODE)/net*/NETPayload.dll bin/$(MODE)/
	cp NETPayload/bin/$(MODE)/net*/NETPayload.runtimeconfig.json bin/$(MODE)/

nativenetloader: setup
	cd NativeNETLoader && cargo build --target x86_64-pc-windows-gnu  $(if $(filter release,$(MODE)),--release,)
	cp NativeNETLoader/target/$(MODE)/native_net_loader.dll bin/$(MODE)/

sboxmod: setup
	cd SBOXMod && dotnet build -c $(MODE)
	cp SBOXMod/bin/$(MODE)/net*/SBOXMod.dll bin/$(MODE)/

clean:
	rm -rf bin
	cd NETPayload && dotnet clean
	cd NativeNETLoader && cargo clean
	cd SBOXMod && dotnet clean