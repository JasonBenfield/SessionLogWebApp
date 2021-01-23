Import-Module PowershellForXti -Force

$script:sessionLogConfig = [PSCustomObject]@{
    RepoOwner = "JasonBenfield"
    RepoName = "SessionLogWebApp"
    AppName = "SessionLog"
    AppType = "WebApp"
    ProjectDir = "Apps\SessionLogWebApp"
}

function SessionLog-New-XtiIssue {
    param(
        [Parameter(Mandatory, Position=0)]
        [string] $IssueTitle,
        $Labels = @(),
        [string] $Body = "",
        [switch] $Start
    )
    $script:sessionLogConfig | New-XtiIssue @PsBoundParameters
}

function SessionLog-Xti-StartIssue {
    param(
        [Parameter(Position=0)]
        [long]$IssueNumber = 0,
        $IssueBranchTitle = "",
        $AssignTo = ""
    )
    $script:sessionLogConfig | Xti-StartIssue @PsBoundParameters
}

function SessionLog-New-XtiVersion {
    param(
        [Parameter(Mandatory, Position=0)]
        [ValidateSet("major", "minor", "patch")]
        $VersionType,
        [ValidateSet("Development", "Production", "Staging", "Test")]
        $EnvName = "Production"
    )
    $script:sessionLogConfig | New-XtiVersion @PsBoundParameters
}

function SessionLog-Xti-Merge {
    param(
        [Parameter(Position=0)]
        [string] $CommitMessage
    )
    $script:sessionLogConfig | Xti-Merge @PsBoundParameters
}

function SessionLog-New-XtiPullRequest {
    param(
        [Parameter(Position=0)]
        [string] $CommitMessage
    )
    $script:sessionLogConfig | New-XtiPullRequest @PsBoundParameters
}

function SessionLog-Xti-PostMerge {
    param(
    )
    $script:sessionLogConfig | Xti-PostMerge @PsBoundParameters
}

function SessionLog-Publish {
    param(
        [ValidateSet("Production", “Development", "Staging", "Test")]
        [string] $EnvName="Development",
        [switch] $ExcludePackage
    )
    
    $ErrorActionPreference = "Stop"

    Write-Output "Publishing to $($EnvName)"
    $timestamp = Get-Date -Format "yyMMdd_HHmmssfff"
    $backupFilePath = "$($env:XTI_AppData)\$EnvName\Backups\app_$timestamp.bak"
    if($EnvName -eq "Production" -or $EnvName -eq "Staging") {
        Write-Output "Backuping up the app database"
	    Xti-BackupMainDb -envName "Production" -BackupFilePath $backupFilePath
        $env:DOTNET_ENVIRONMENT=$EnvName
        $env:ASPNETCORE_ENVIRONMENT=$EnvName
    }
    if($EnvName -eq "Staging") { 
        Write-Output "Restoring the app database"
	    Xti-RestoreMainDb -EnvName $EnvName -BackupFilePath $backupFilePath
    }

    Write-Output "Updating the app database"
    Xti-UpdateMainDb -EnvName $EnvName

    if ($EnvName -eq "Test"){
        Write-Output "Resetting the app database"
	    Xti-ResetMainDb -EnvName $EnvName
    }
    if($EnvName -eq "Production") {
        SessionLog-ImportWeb -Prod
    }
    else {
        SessionLog-ImportWeb
    }
    
    $defaultVersion = ""
    if($EnvName -eq "Production") {
        $branch = Get-CurrentBranchname
        Xti-BeginPublish -BranchName $branch
        $releaseBranch = Parse-ReleaseBranch -BranchName $branch
        $defaultVersion = $releaseBranch.VersionKey
    }
    
    Write-Output "Generating the api"
    SessionLog-GenerateApi -EnvName $EnvName -DefaultVersion $defaultVersion

    Write-Output "Running web pack"
    $script:sessionLogConfig | SessionLog-Webpack

    Write-Output "Building solution"
    dotnet build 

    SessionLog-Setup -EnvName $EnvName

    if ($EnvName -eq "Test") {
        Invoke-WebRequest -Uri https://test.guinevere.com/Authenticator/Current/StopApp
        Write-Output "Creating user"
        $password = Xti-GeneratePassword
        New-XtiUser -EnvName $EnvName -UserName SessionLogAdmin -Password $password
        $script:sessionLogConfig | New-XtiUserRoles -EnvName $EnvName -UserName SessionLogAdmin -RoleNames Admin
        New-XtiCredentials -EnvName $EnvName -CredentialKey SessionLogAdmin -UserName SessionLogAdmin -Password $password
    }

    Write-Output "Publishing website"
    
    $script:sessionLogConfig | Xti-PublishWebApp -EnvName $EnvName

    if($EnvName -eq "Production") {
        if(-not $ExcludePackage) {
            $script:sessionLogConfig | Xti-PublishPackage -DisableUpdateVersion -Prod
        }
        Xti-EndPublish -BranchName $branch
        $script:sessionLogConfig | Xti-Merge
    }
    elseif(-not $ExcludePackage) {
        $script:sessionLogConfig | Xti-PublishPackage -DisableUpdateVersion
    }
}

function SessionLog-PublishWebApp {
    param(
        [ValidateSet("Production", "Development", "Staging", "Test")]
        [string] $EnvName="Development",
        [switch] $ExcludePackage
    )
    $script:sessionLogConfig | Xti-PublishWebApp -EnvName $EnvName
}

function SessionLog-GenerateApi {
    param (
        [ValidateSet("Development", "Production", "Staging", "Test")]
        [string] $EnvName,
        [string] $DefaultVersion
    )
    dotnet build Apps/SessionLogApiGeneratorApp
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Session Log api generator build failed with exit code $LASTEXITCODE"
    }
    dotnet run --project Apps/SessionLogApiGeneratorApp --envrionment $EnvName --Output:DefaultVersion "`"$DefaultVersion`""
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Session Log api generator failed with exit code $LASTEXITCODE"
    }
    tsc -p Apps/SessionLogWebApp/Scripts/SessionLog/tsconfig.json
}

function SessionLog-Setup {
    param (
        [ValidateSet("Production", "Development", "Staging", "Test")]
        [string] $EnvName="Development"
    )
    
    dotnet build Apps/SessionLogSetupApp
    if( $LASTEXITCODE -ne 0 ) {
        Throw "SessionLog setup build failed with exit code $LASTEXITCODE"
    }
    dotnet run --environment=$EnvName --project=Apps/SessionLogSetupApp

    if( $LASTEXITCODE -ne 0 ) {
        Throw "SessionLog setup failed with exit code $LASTEXITCODE"
    }
}

function SessionLog-Webpack {
    param(
        [Parameter(Mandatory, ValueFromPipelineByPropertyName = $true)]
        [string] $ProjectDir
    )
    $currentDir = (Get-Item .).FullName
    Set-Location $ProjectDir
    webpack
    Set-Location $currentDir
}

function SessionLog-ImportWeb {
    param(
        [switch] $Prod
    )
    $script:sessionLogConfig | Xti-ImportWeb -Prod:$Prod -AppToImport Shared
    $script:sessionLogConfig | Xti-ImportWeb -Prod:$Prod -AppToImport Authenticator
}
