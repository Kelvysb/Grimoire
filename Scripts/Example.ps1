Write-Host "Begin"
write-host "Example script:"
write-host "Loop:"
for ($i = 0; $i -lt 10; $i++) {
    Write-Host $"Iteration: $($i)"
}
write-host "-"
Write-Host "Interpolation: {{ExampleKey}}"
Write-Warning "Example Warning"
Write-Error "Example Error"
Write-Host "End"