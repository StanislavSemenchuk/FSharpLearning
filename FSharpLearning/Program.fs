open System
open System.Runtime.InteropServices
open System.Globalization


type Language = {
    LanguageIdentifier : int;
    Language : string;
    Locale : string;
}


[<DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern uint GetKeyboardLayoutList(int nBuff, [<Out>] IntPtr[] lpList)

let GetOS : OSPlatform =
    match int Environment.OSVersion.Platform with
        | 4 | 128 -> OSPlatform.Linux
        | 6       -> OSPlatform.OSX
        | _       -> OSPlatform.Windows

let GetInstalledLanguagesWindows = 
    let size : uint = GetKeyboardLayoutList(0, null)
    let ids : IntPtr[] = Array.zeroCreate<IntPtr> (int size)
    GetKeyboardLayoutList(ids.Length , ids) |> ignore
    [for id in ids do
        let bla = int id &&& 0xFFFF
        [yield new CultureInfo(bla)]]

GetInstalledLanguagesWindows |> printf "%A"