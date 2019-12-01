function CreateTestResultMessage($file) {
    $filename = $file.name
    $xml = [XML](Get-Content $file)

    $start = $xml.TestRun.Times.start
    $finish = $xml.TestRun.Times.finish
    $executed = $xml.TestRun.ResultSummary.Counters.executed
    $passed = $xml.TestRun.ResultSummary.Counters.passed
    $failed = $xml.TestRun.ResultSummary.Counters.failed
    
    return "テスト[" + $filename + "]の実行結果だよー" +  `
    "  `n" + "> 開始時刻: " + ([DateTime]$start).ToString("yyyy/MM/dd HH:mm:ss") + `
    "  `n" + "終了時刻: " + ([DateTime]$finish).ToString("yyyy/MM/dd HH:mm:ss") + `
    "  `n" + "実行件数: " + $executed + `
    "  `n" + "成功件数: " + $passed + `
    "  `n" + "失敗件数: " + $failed
}

function CreateCoverageMessage($file) {
    $filename = $file.name
    $xml = [XML](Get-Content $file)

    $sequenceCoverage = $XML.CoverageSession.Summary.sequenceCoverage
    $branchCoverage = $XML.CoverageSession.Summary.branchCoverage
    $numSequencePoints = $XML.CoverageSession.Summary.numSequencePoints
    $visitedSequencePoints = $XML.CoverageSession.Summary.visitedSequencePoints
    $numBranchPoints = $XML.CoverageSession.Summary.numBranchPoints
    $visitedBranchPoints = $XML.CoverageSession.Summary.visitedBranchPoints

    return "テストカバレッジ[" + $filename + "]の実行結果だよー" +  `
    "  `n" + "> Sequence Coverage: " + $sequenceCoverage + "% " + "(" + $visitedSequencePoints + "/" + $numSequencePoints + ")" + `
    "  `n" + "Branch Coverage: " + $branchCoverage + "% " + "(" + $visitedBranchPoints + "/" + $numBranchPoints + ")"
}

function PostTeams($message) {
    $encode = [System.Text.Encoding]::GetEncoding('ISO-8859-1')
    $utf8Bytes = [System.Text.Encoding]::UTF8.GetBytes($message)
    
    $notificationPayload = @{ 
        text = $encode.GetString($utf8Bytes);
    }
    
    $postUri = 'xxx'
    
    Invoke-RestMethod -Method POST -Uri $postUri -Body  (ConvertTo-Json $notificationPayload) -ContentType application/json
}

$files = Get-ChildItem -Recurse -File -Include *.trx
Foreach ($file in $files) {
    $message = CreateTestResultMessage($file)
    PostTeams($message)
}

$files = Get-ChildItem -Recurse -File -Include coverage.xml
Foreach ($file in $files) {
    $message = CreateCoverageMessage($file)
    PostTeams($message)
}
