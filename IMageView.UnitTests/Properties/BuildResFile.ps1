
function buildResources() {

    resgen  /useSourcePath /compile Resources.resx

    if (Test-Path -Path 'Resources.resources') {
        mv Resources.resources unitTest.res    
    }
    else {
        Write-Host 'Failed to generate unitTest.res file'  -ForegroundColor Red
    }
}

function ValidateEnvironment(){
    # Asuming get-cmdPath.bat Exists
    
    $path = Get-CmdPath resgen
    if($path -eq "") {
        Write-Host 'resgen was not found, please add it to the environment Path'
        return $FALSE
    }
    return $TRUE    
}

$res = ValidateEnvironment
if ( $res) {
    buildResources    
}