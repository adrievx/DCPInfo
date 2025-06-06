# DCPInfo
A tool & C# library to parse DCPs (Digital Cinema Package)

## Using the tool
1. Load your DCP

![](.github/attachments/opendcp.png)

Select the DCP you want to load

![](.github/attachments/selectdcp.png)

3. Check if you need a KDM

If a KDM is required, this message will be displayed

If a KDM is not required, this process is already complete, see #5

![](.github/attachments/checkkdm.png)

4. Load your KDM

![](.github/attachments/openkdm.png)

Select the KDM you want to load

![](.github/attachments/selectkdm.png)

If the KDM is correct, it will show as **Unlocked** in the Rights Management panel

![](.github/attachments/checkkdmloaded.png)

5. Done

Once a DCP (and optionally KDM) is loaded, you will then be able to see information about your DCP/KDM in their respective panels

![](.github/attachments/screenshot.png)

## Examples - Using the `DCPUtils` library on its own
### Load a DCP and read it's metadata
```CSharp
string dcpPath = "F:\\REALD_BUMPER-3D_F_US-XX_51-XX_1920_20080414_LP_i3D";
var dcp = DCP.Read(dcpPath);

if(dcp != null) {
    Console.WriteLine(dcp.Metadata.AnnotationText);
}
```

### Check if a KDM is for a specific DCP
```CSharp
string dcpPath = "F:\\DIEncryptTest_TST-1-Temp-Pre_F-178_EN-XX_INT-TL_20_2K_SYN_20250421_SYN_SMPTE_OV";
string kdmPath = "F:\\KDM_DIEncryptTest_TST-1-Temp-Pre_F-178_EN-XX_INT-TL_20_2K_SYN_20250421_SYN_SMPTE_OV_SYNDEXTEST_DCPInfo.xml";
var dcp = DCP.Read(dcpPath);

if(dcp.FindKDM(kdmPath)) {
    Console.WriteLine("Specified KDM is for the loaded DCP");
}
else {
    Console.WriteLine("Specified KDM is not for the specified DCP");
}
```

### Load a KDM and verify it against a TMS certificate
```CSharp
string dcpPath = "F:\\DIEncryptTest_TST-1-Temp-Pre_F-178_EN-XX_INT-TL_20_2K_SYN_20250421_SYN_SMPTE_OV";
string kdmPath = "F:\\KDM_DIEncryptTest_TST-1-Temp-Pre_F-178_EN-XX_INT-TL_20_2K_SYN_20250421_SYN_SMPTE_OV_SYNDEXTEST_DCPInfo.xml";
string certificatePath = "C:\\TMS\\key.crt";
string privateKeyPath = "C:\\TMS\\key.pem";

var dcp = DCP.Read(dcpPath);

if(dcp.FindKDM(kdmPath)) {
    var kdm = KDM.Read(kdmPath);

    if(kdm != null) {
        if(kdm.Verify(dcp, certificatePath, privateKeyPath)) {
            Console.WriteLine("KDM is valid");
        }
        else {
            Console.WriteLine("The specified KDM's digital signature could not be validated");
        }
    }
    else {
        Console.WriteLine("Specified KDM is not valid");
    }
}
else {
    Console.WriteLine("Specified KDM is not for the specified DCP");
}
```

## Contribution
All contributions are welcome! If you wish to contribute, please open a pull request.

## Attributions
- [NTDesigns - Projector Icons](https://www.iconarchive.com/show/projector-icons-by-ntdesigns/projector-violet-icon.html)

## NuGet
The NuGet package can be found at https://www.nuget.org/packages/DCPUtils/0.1.0
