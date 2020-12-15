Import-Module PowershellForXti -Force

$script:sessionLogConfig = [PSCustomObject]@{
    RepoOwner = "JasonBenfield"
    RepoName = "SessionLogWebApp"
    AppName = "SessionLog"
    AppType = "WebApp"
    ProjectDir = "C:\XTI\src\SessionLogWebApp\Apps\SessionLogWebApp"
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
        [Parameter(Position=0)]
        [ValidateSet("major", "minor", "patch")]
        $VersionType = "minor",
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

function Xti-CopyShared {
    $source = "..\SharedWebApp\Apps\SharedWebApp"
    $target = ".\Apps\SessionLogWebApp"
    robocopy "$source\Scripts\Shared\" "$target\Scripts\Shared\" *.ts /e /purge /njh /njs /np /ns /nc /nfl /ndl /a+:R
    robocopy "$source\Scripts\Shared\" "$target\Scripts\Shared\" /xf *.ts /e /purge /njh /njs /np /ns /nc /nfl /ndl /a-:R
    robocopy "$source\Views\Exports\Shared\" "$target\Views\Exports\Shared\" /e /purge /njh /njs /np /ns /nc /nfl /ndl /a+:R
}

function Xti-CopyAuthenticator {
    $source = "..\AuthenticatorWebApp\Apps\AuthenticatorWebApp"
    $target = ".\Apps\SessionLogWebApp"
    robocopy "$source\Scripts\Authenticator\" "$target\Scripts\Authenticator\" *.ts /e /purge /njh /njs /np /ns /nc /nfl /ndl /a+:R
    robocopy "$source\Scripts\Authenticator\" "$target\Scripts\Authenticator\" /xf *.ts /e /purge /njh /njs /np /ns /nc /nfl /ndl /a-:R
}

function SessionLog-Publish {
    param(
        [ValidateSet("Production", “Development", "Staging", "Test")]
        [string] $EnvName="Production",
        [switch] $ExcludePackage
    )
    
    $ErrorActionPreference = "Stop"

    $activity = "Publishing to $EnvName"
    
    $timestamp = Get-Date -Format "yyMMdd_HHmmssfff"
    $backupFilePath = "$($env:XTI_AppData)\$EnvName\Backups\app_$timestamp.bak"
    if($EnvName -eq "Production" -or $EnvName -eq "Staging") {
        Write-Progress -Activity $activity -Status "Backuping up the app database" -PercentComplete 10
	    Xti-BackupMainDb -envName "Production" -BackupFilePath $backupFilePath
        $env:DOTNET_ENVIRONMENT=$EnvName
        $env:ASPNETCORE_ENVIRONMENT=$EnvName
    }
    if($EnvName -eq "Staging") { 
        Write-Progress -Activity $activity -Status "Restoring the app database" -PercentComplete 15
	    Xti-RestoreMainDb -EnvName $EnvName -BackupFilePath $backupFilePath
    }

    Write-Progress -Activity $activity -Status "Updating the app database" -PercentComplete 18
    Xti-UpdateMainDb -EnvName $EnvName

    if ($EnvName -eq "Test"){
        Write-Progress -Activity $activity -Status "Resetting the app database" -PercentComplete 20
	    Xti-ResetMainDb -EnvName $EnvName
    }

    Xti-CopyShared
    Xti-CopyAuthenticator
    
    $defaultVersion = ""
    if($EnvName -eq "Production") {
        $branch = Get-CurrentBranchname
        Xti-BeginPublish -BranchName $branch
        $releaseBranch = Parse-ReleaseBranch -BranchName $branch
        $defaultVersion = $releaseBranch.VersionKey
    }
    
    Write-Progress -Activity $activity -Status "Generating the api" -PercentComplete 30
    SessionLog-GenerateApi -EnvName $EnvName -DefaultVersion $defaultVersion

    Write-Progress -Activity $activity -Status "Running web pack" -PercentComplete 40
    $script:sessionLogConfig | SessionLog-Webpack

    Write-Progress -Activity $activity -Status "Building solution" -PercentComplete 50
    dotnet build 

    SessionLog-Setup -EnvName $EnvName

    if ($EnvName -eq "Test") {
        Invoke-WebRequest -Uri https://test.guinevere.com/Authenticator/Current/StopApp
        Write-Progress -Activity $activity -Status "Creating user" -PercentComplete 70
        $password = Xti-GeneratePassword
        New-XtiUser -EnvName $EnvName -UserName SessionLogAdmin -Password $password
        $script:sessionLogConfig | New-XtiUserRoles -EnvName $EnvName -UserName SessionLogAdmin -RoleNames Admin
        New-XtiCredentials -EnvName $EnvName -CredentialKey SessionLogAdmin -UserName SessionLogAdmin -Password $password
    }

    Write-Progress -Activity $activity -Status "Publishing website" -PercentComplete 80
    
    $script:sessionLogConfig | Xti-PublishWebApp -EnvName $EnvName

    if($EnvName -eq "Production") {
        if(-not $ExcludePackage) {
            $script:sessionLogConfig | Xti-PublishPackage -DisableUpdateVersion -Prod
        }
        Xti-EndPublish -BranchName $branch
    }
    elseif(-not $ExcludePackage) {
        $script:sessionLogConfig | Xti-PublishPackage -DisableUpdateVersion
    }
}

function SessionLog-GenerateApi {
    param (
        [ValidateSet("Development", "Production", "Staging", "Test")]
        [string] $EnvName,
        [string] $DefaultVersion
    )
    dotnet run --project Apps/SessionLogApiGeneratorApp --envrionment $EnvName --Output:DefaultVersion "`"$DefaultVersion`""
}

function SessionLog-Setup {
    param (
        [ValidateSet("Production", "Development", "Staging", "Test")]
        [string] $EnvName="Development"
    )
    
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