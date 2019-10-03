Task Publish -Depends Pack {
    Exec { docker login docker.io --username=tiksn }
    foreach ($VersionTag in $script:VersionTags) {
        $localTag = ($script:imageName + ":" + $VersionTag)
        $remoteTag = ("docker.io/" + $localTag)
        Exec { docker tag $localTag $remoteTag }
        Exec { docker push $remoteTag }

        try {
            Exec { keybase chat send --nonblock --private lionize "BUILD: Published $remoteTag" }
        }
        catch {
            Write-Warning "Failed to send notification"
        }
    }
}

Task Pack -Depends Build, EstimateVersions {
    $tagsArguments = @()
    foreach ($VersionTag in $script:VersionTags) {
        $tagsArguments += "-t"
        $tagsArguments += ($script:imageName + ":" + $VersionTag)
    }

    Exec { docker build -f Dockerfile $script:publishFolder $tagsArguments }
}

Task EstimateVersions {
    $script:VersionTags = @()

    if ($Latest) {
        $script:VersionTags += 'latest'
    }

    if (!!($Version)) {
        $Version = [Version]$Version

        Assert ($Version.Revision -eq -1) "Version should be formatted as Major.Minor.Patch like 1.2.3"
        Assert ($Version.Build -ne -1) "Version should be formatted as Major.Minor.Patch like 1.2.3"

        $Version = $Version.ToString()
        $script:VersionTags += $Version
    }

    Assert $script:VersionTags "No version parameter (latest or specific version) is passed."
}

Task Build -Depends TranspileModels {
    $script:publishFolder = Join-Path -Path $script:trashFolder -ChildPath "publish"

    New-Item -Path $script:publishFolder -ItemType Directory | Out-Null
    $project = Resolve-Path ".\src\WebAPI\WebAPI.csproj"
    $project = $project.Path
    Exec { dotnet publish $project --configuration Release --output $script:publishFolder }
}

Task TranspileModels -Depends Init, Clean {
    $apiModelYaml = (Resolve-Path ".\src\ApiModels.yml").Path
    $apiModelOutput = Join-Path -Path ".\src\WebAPI" -ChildPath "Models"
    Exec { smite --input-file $apiModelYaml --lang csharp --field property --output-folder $apiModelOutput }
}

Task Clean -Depends Init {
    Get-ChildItem .\ -Include bin, obj -Recurse | ForEach-Object ($_) { 
        Remove-Item $_.fullname -Force -Recurse
    }
}

Task Init {
    $date = Get-Date
    $ticks = $date.Ticks
    $script:imageName = "tiksn/lionize-habitica-task-provider-service"
    $script:trashFolder = Join-Path -Path . -ChildPath ".trash"
    $script:trashFolder = Join-Path -Path $script:trashFolder -ChildPath $ticks.ToString("D19")
    New-Item -Path $script:trashFolder -ItemType Directory | Out-Null
    $script:trashFolder = Resolve-Path -Path $script:trashFolder
}