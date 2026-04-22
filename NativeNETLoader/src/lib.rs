use std::ptr;
use std::thread;
use std::fs;
use std::io::{self, Write};


use netcorehost::{
    nethost,
    pdcstring::PdCString,
    hostfxr::AssemblyDelegateLoader,
    error::{HostingError, HostingResult, HostingSuccess},
    pdcstr
};

use std::path::Path;
use std::path::PathBuf;

use windows::{
    Win32::Foundation::*,
    Win32::System::SystemServices::*,
    Win32::System::LibraryLoader::*,
    Win32::System::Console::*,
};

use windows_sys::{
    Win32::System::Threading::CreateThread,
};

use reqwest::blocking::Client;
use reqwest::header::{HeaderMap, HeaderValue};
use reqwest::StatusCode;

static mut DLL_HANDLE: HMODULE = HMODULE(std::ptr::null_mut());


unsafe extern "system" fn thread_proc(_param: *mut std::ffi::c_void) -> u32 {
    unsafe {
        AllocConsole();
    }

    main_thread();
    println!("Press enter to close the console (fart)");
    let _ = std::io::stdin().read_line(&mut String::new());
        unsafe {
        FreeConsole();
    }

    0
}

#[unsafe(no_mangle)]
pub extern "system" fn DllMain(
    dll_module: HMODULE,
    call_reason: u32,
    _: *mut ())
    -> bool
{
    match call_reason {
        DLL_PROCESS_ATTACH => {
            unsafe {
                DLL_HANDLE = dll_module;

                let handle = CreateThread(
                    ptr::null(),
                    0,
                    Some(thread_proc),
                    ptr::null_mut(),
                    0,
                    ptr::null_mut(),
                );

                CloseHandle(HANDLE(handle));
            }
        },
        _ => ()
    }
    true
}

fn main_thread() {
    unsafe {
        AllocConsole();
    }

        let dll_dir = match get_dll_directory() {
        Ok(dir) => dir,
        Err(e) => {
            return;
        },
    };


    let assembly_path = dll_dir.join("NETPayload.dll");
    let runtime_config_path = dll_dir.join("NETPayload.runtimeconfig.json");

    let assembly_path_pdcstr = match PdCString::from_os_str(&assembly_path) {
        Ok(s) => s,
        Err(e) => {
            return;
        }
    };

    let runtime_config_path_pdcstr = match PdCString::from_os_str(&runtime_config_path) {
        Ok(s) => s,
        Err(e) => {
            return;
        }
    };

    let hostfxr = match nethost::load_hostfxr() {
        Ok(h) => h,
        Err(e) => {
            return;
        }
    };

    let context = match hostfxr.initialize_for_runtime_config(runtime_config_path_pdcstr) {
        Ok(c) => c,
        Err(e) => {
            return;
        }
    };

    let fn_loader = match context.get_delegate_loader_for_assembly(assembly_path_pdcstr) {
        Ok(l) => l,
        Err(e) => {
            return;
        }
    };

    let load_assembly_from_bytes_func = match fn_loader.get_function_with_default_signature(
        pdcstr!("NETPayload.Loader, NETPayload"),
        pdcstr!("LoadAssemblyFromBytes"),
    ) {
        Ok(f) => f,
        Err(e) => {
            return;
        }
    };

    let token = match load_token_from_dll_dir() {
    Ok(t) => t,
    Err(e) => {
        eprintln!("Failed to load token file");
        return;
    }
};

    let sbox_loader_bytes = match fetch_dll_from_api(&token) {
        Ok(bytes) => bytes,
        Err(e) => {
            eprintln!("failed to auth");
            return; // or return Err(e) if inside a function returning Result
        }
    };

    let result = unsafe {
        load_assembly_from_bytes_func(
            sbox_loader_bytes.as_ptr() as *const std::ffi::c_void,
            sbox_loader_bytes.len() as i32
        )
    };
    println!("Success :P");

}

fn get_dll_path() -> Result<String, Box<dyn std::error::Error>> {
    unsafe {
        let mut buffer = [0u16; 260];
        // Use None to get current module's path
        let len = GetModuleFileNameW(Some(DLL_HANDLE), &mut buffer);

        if len == 0 {
            return Err("GetModuleFileNameW failed".into());
        }

        let path = String::from_utf16_lossy(&buffer[..len as usize]);
        Ok(path)
    }
}

fn get_dll_directory() -> Result<PathBuf, Box<dyn std::error::Error>> {
    let dll_path = get_dll_path()?;
    let dll_dir = Path::new(&dll_path)
        .parent()
        .ok_or("Could not get parent directory")?
        .to_path_buf(); // Convert to PathBuf instead of String
    Ok(dll_dir)
}

fn load_token_from_dll_dir() -> Result<String, Box<dyn std::error::Error>> {
    let dll_dir = get_dll_directory()?;
    let token_path = dll_dir.join("token.txt");

    let token = fs::read_to_string(token_path)?
        .trim()
        .to_string();

    Ok(token)
}



fn fetch_dll_from_api(token: &str) -> Result<Vec<u8>, String> {
    let mut headers = HeaderMap::new();
    headers
        .insert("token", HeaderValue::from_str(token).map_err(|e| e.to_string())?);

    let client = Client::new();
    let resp = client
        .get("ummmmmm hahaha u can stream it if you want to keep some parts secret ")
        .headers(headers)
        .send()
        .map_err(|e| format!("HTTP request failed: {}", e))?;

    let status = resp.status();
    if status == StatusCode::UNAUTHORIZED {
        return Err("Your token is either incorrect, revoked or has hit its usage limit"
            .to_string());
    }
    if status != StatusCode::OK {
        return Err(format!("Unexpected status code: {}", status.as_u16()));
    }

    let bytes = resp
        .bytes()
        .map_err(|e| format!("Failed to read response body: {}", e))?;
    Ok(bytes.to_vec())
}
